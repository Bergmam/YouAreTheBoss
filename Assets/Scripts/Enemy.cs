﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float MovementSpeed = 1.0f;
    private float angularSpeed = 0f;

    private float Damage = 20.0f;

    private float Range = 1.0f;

    private float Health = 100.0f;

    private float MaxHealth = 100.0f;

    public bool SetForDeath = false;

    private float Scale = 1.0f;
    private float circlingSpeed;

    private bool selfDestruct;
    private bool invunerable;
    private float attackFrequency;

    BossHealth bossHealth;
    private ColorModifier colorModifier;
    private EnemySpawner enemySpawner;

    private GameObject hitParticle;
    private GameObject healthPickup;
    private GameObject freezePickup;
    private GameObject shieldPickup;
    private StatsHolder projectile;
    private float zigZagAngleLow;
    private float zigZagAngleHigh;
    private bool zigZag;
    private WaveHandler waveHandler;
    private bool frozen;
    private Transform sprite;
    public EnemyType EnemyType { get; private set; }
    private IEnumerator unfreezeCoroutine;
    public float turnBackDistance;
    public float turnForwardDistance;
    public int numberOfTurns; // Negative value means keep turning forever.

    // Handle camera shaking
    CameraShake camShake;

    void Awake()
    {
        this.enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
        this.sprite = transform.Find("Sprite");
        this.colorModifier = this.sprite.GetComponent<ColorModifier>();
        this.hitParticle = Resources.Load("Prefabs/HitParticleSystem", typeof(GameObject)) as GameObject;
        this.healthPickup = Resources.Load("Prefabs/HealthPickup", typeof(GameObject)) as GameObject;
        this.shieldPickup = Resources.Load("Prefabs/ShieldPickup", typeof(GameObject)) as GameObject;
        this.freezePickup = Resources.Load("Prefabs/FreezePickup", typeof(GameObject)) as GameObject;
        this.waveHandler = GameObject.FindObjectOfType<WaveHandler>();
    }

    void Start()
    {
        camShake = GameObject.Find("Handler").GetComponent<CameraShake>();
        bossHealth = GameObject.Find("Boss").GetComponent<BossHealth>();
        this.sprite.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.zero - transform.position);
    }

    void Update()
    {
        if (this.frozen && this.EnemyType != EnemyType.PROJECTILE)
        {
            return;
        }

        float distanceFromBoss = Vector3.Distance(Vector3.zero, transform.position);
        RadialPosition radialPosition = RotationUtils.XYToRadialPos(this.transform.position);

        // Move forward and back
        float step = MovementSpeed * Time.deltaTime;
        radialPosition.AddRadius((-1) * step);

        // Move sideways
        float angularStep = this.angularSpeed * Time.deltaTime;
        radialPosition.AddAngle(angularStep);
        float newPositionAngle = radialPosition.GetAngle();
        if (RotationUtils.InCounterClockwiseLimits(newPositionAngle, this.zigZagAngleHigh, this.zigZagAngleLow) && zigZag)
        {
            updateRadialPos(radialPosition, this.zigZagAngleLow, this.zigZagAngleHigh);
            angularSpeed = -1 * angularSpeed;
            circlingSpeed = -1 * circlingSpeed;
        }

        // Attack
        if (distanceFromBoss <= Range)
        {
            if (selfDestruct)
            {
                doDamageToBoss();
                KillSelf();
            }
            // Melee attack
            else if (Range <= Parameters.MELEE_RANGE && !IsInvoking("doDamageToBoss"))
            {
                InvokeRepeating("doDamageToBoss", 1.0f, this.attackFrequency);
            }
            // Ranged attack
            else if (Range > Parameters.MELEE_RANGE && !IsInvoking("spawnProjectile"))
            {
                InvokeRepeating("spawnProjectile", 1.0f, this.attackFrequency);
            }

            // Stop moving forward and back if no turns left when in range.
            if (this.numberOfTurns == 0)
            {
                this.MovementSpeed = 0;
                // Angular speed is stats.angularSpeed until no turns left and in range, then it becomes stats.circlingSpeed.
                this.angularSpeed = this.circlingSpeed;

                this.zigZag = false; // Circling units con't zigzag (yet).
            }
        }
        else
        {
            CancelInvoke(); // Stop attacking when out of range.
        }

        if (distanceFromBoss < this.turnBackDistance && this.MovementSpeed > 0
            || distanceFromBoss > this.turnForwardDistance && this.MovementSpeed < 0)
        {
            changeRadialDirection();
        }
        MoveTo(radialPosition);

        this.sprite.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.zero - transform.position);
    }

    private void changeRadialDirection()
    {
        if (this.numberOfTurns == 0)
        {
            return;
        }
        else if (this.numberOfTurns > 0 && this.MovementSpeed < 0) // 1 "turn" ends when enemy is moving forward again.
        {
            this.numberOfTurns--;
        }
        this.MovementSpeed = -this.MovementSpeed;
    }

    private void updateRadialPos(RadialPosition radialPos, float zigZagAngleLow, float zigZagAngleHigh)
    {
        float positionAngle = radialPos.GetAngle();
        float midAngle = RotationUtils.MiddleOfRotations(zigZagAngleHigh, zigZagAngleLow);
        // If exited over high limit ( [high, mid(high,low)] ), set to high.
        if (RotationUtils.InCounterClockwiseLimits(positionAngle, zigZagAngleHigh, midAngle))
        {
            radialPos.SetAngle(zigZagAngleHigh);
        }
        else
        {
            radialPos.SetAngle(zigZagAngleLow);
        }
    }

    void doDamageToBoss()
    {
        if (!SetForDeath)
        {
            bool killingBlow = bossHealth.bossTakeDamage(Damage);
            if (!killingBlow)
            {
                camShake.Shake(0.05f, 0.1f);
                this.colorModifier.FadeToDeselected(this.attackFrequency / 3.0f);
                GameObject hitParticle = Instantiate(this.hitParticle, transform.position / 2, transform.rotation);
                var main = hitParticle.GetComponent<ParticleSystem>().main;
                main.startColor = new Color(0.3f, 0.082f, 0.3945f, 0.6f);
                Destroy(hitParticle, hitParticle.GetComponent<ParticleSystem>().main.duration);
            }
        }
    }

    /// <summary>
    /// Spawns a projectile. Projectiles are fast, invunerable and self destructs on impact with the hero.
    /// </summary>
    void spawnProjectile()
    {
        this.colorModifier.FadeToDeselected(this.attackFrequency / 3.0f);
        RadialPosition thisRadialPos = RotationUtils.XYToRadialPos(transform.position);
        this.projectile.SetRadialSpawnPosition(thisRadialPos.GetAngle(), thisRadialPos.GetRadius());
        GameObject spawnedProjectile = this.enemySpawner.InstantiateEnemyPrefab(this.projectile);
    }

    public bool isInAttackArea(float lowAngle, float highAngle, float nearRadius, float farRadius)
    {

        float spriteRadius = this.sprite.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float distanceToBossActual = Mathf.Max(Vector3.Distance(Vector3.zero, transform.position), 0);
        float distanceToBossFar = distanceToBossActual + spriteRadius;
        float distanceToBossNear = distanceToBossActual - spriteRadius;

        RadialPosition radialPosition = RotationUtils.XYToRadialPos(this.transform.position);

        float enemyWidthAngle = Mathf.Rad2Deg * Mathf.Acos(1 - Mathf.Pow(spriteRadius / Mathf.Sqrt(2 * distanceToBossActual), 2));
        float enemyHighAngle = radialPosition.GetAngle() + enemyWidthAngle;
        float enemyLowAngle = radialPosition.GetAngle() - enemyWidthAngle;

        bool inHighAngle = RotationUtils.InCounterClockwiseLimits(enemyHighAngle, lowAngle, highAngle);
        bool inLowAngle = RotationUtils.InCounterClockwiseLimits(enemyLowAngle, lowAngle, highAngle);
        bool bossLargerThanRadius = RotationUtils.InCounterClockwiseLimits(lowAngle, enemyLowAngle, enemyHighAngle)
                                        && RotationUtils.InCounterClockwiseLimits(highAngle, enemyLowAngle, enemyHighAngle);
        bool inRadius = distanceToBossNear <= farRadius && distanceToBossFar >= nearRadius;

        return (inLowAngle || inHighAngle || bossLargerThanRadius) && inRadius;
    }

    public void applyDamageTo(float damage)
    {
        if (invunerable)
        {
            return;
        }

        GameObject hitParticle = Instantiate(this.hitParticle, transform.position, transform.rotation);
        var main = hitParticle.GetComponent<ParticleSystem>().main;
        main.startColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        Destroy(hitParticle, hitParticle.GetComponent<ParticleSystem>().main.duration);

        Health -= damage;
        UnityUtils.RecursiveFind(transform, "HealthBar").GetComponent<ProgressBarBehaviour>().UpdateFill(Health / MaxHealth);
        if (Health <= 0)
        {
            KillSelf();
            int itemRand = Random.Range(0, 12);
            if (itemRand < 3)
            {
                if (itemRand == 0)
                {
                    Instantiate(this.healthPickup, this.transform.position, Quaternion.identity);
                }
                else if (itemRand == 1)
                {
                    Instantiate(this.shieldPickup, this.transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(this.freezePickup, this.transform.position, Quaternion.identity);
                }
                this.waveHandler.ItemAdded();
            }
        }
        else
        {
            transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(UnityUtils.ChangeToDefaultColorAfterTime(colorModifier, 0.5f));
        }
    }

    public void SetStats(StatsHolder stats)
    {
        transform.name = stats.Name;
        this.EnemyType = stats.enemyType;
        this.selfDestruct = stats.selfDestruct;
        this.invunerable = stats.invunerable;
        if (invunerable) //Don't show healthbar for invunerable units (projectiles)
        {
            Destroy(UnityUtils.RecursiveFind(transform, "HealthBar").gameObject);
        }
        MoveTo(new RadialPosition(stats.spawnRadius, stats.spawnAngle));
        MovementSpeed = stats.MovementSpeed;
        this.angularSpeed = stats.angularSpeed;
        this.circlingSpeed = stats.circlingSpeed;
        Damage = stats.Damage;
        Range = stats.Range;
        Health = stats.Health;
        MaxHealth = stats.Health;
        Scale = stats.Scale;
        Transform sprite = transform.Find("Sprite");
        sprite.transform.localScale *= stats.Scale;
        if (stats.Scale > 1)
        {
            Transform canvas = transform.Find("Canvas");
            canvas.localPosition = new Vector3(canvas.localPosition.x, canvas.localPosition.y * stats.Scale * 0.8f, canvas.localPosition.z);
        }
        if (stats.projectile == null)
        {
            this.projectile = EnemyFactory.Projectile(this.Damage);
        }
        else
        {
            this.projectile = stats.projectile;
        }
        this.attackFrequency = stats.attackDelay;
        this.zigZag = stats.zigZag;
        if (stats.angularSpeed > 0)
        {
            this.zigZagAngleLow = stats.spawnAngle;
            this.zigZagAngleHigh = stats.spawnAngle + stats.zigZagAngle;
        }
        else
        {
            this.zigZagAngleHigh = stats.spawnAngle;
            this.zigZagAngleLow = stats.spawnAngle - stats.zigZagAngle;
        }
        this.turnBackDistance = stats.TurnBackDistance;
        this.turnForwardDistance = stats.TurnForwardDistance;
        this.numberOfTurns = stats.NumberOfTurns;
        colorModifier.SetDefaultColor(stats.Color);
        colorModifier.SetSelectedColor(Parameters.ENEMY_ATTACK_COLOR);
    }

    void OnDestroy()
    {
        CancelInvoke();
    }

    private void KillSelf()
    {
        //Invunerable units' helthbars are already removed so when they die, healthbar is null.
        Transform healthBarTransform = UnityUtils.RecursiveFind(transform, "HealthBar");
        if (healthBarTransform != null)
        {
            Destroy(healthBarTransform.gameObject);
        }

        SetForDeath = true;
        var renderer = this.sprite.gameObject.GetComponent<Renderer>();
        //renderer.material = Resources.Load("Materials/MAT_Dissolve", typeof(Material)) as Material;
        Invoke("DestroyGO", 0);
    }

    private void DestroyGO()
    {
        Destroy(gameObject);
    }

    public void MoveTo(RadialPosition radialPosition)
    {
        transform.position = RotationUtils.RadialPosToXY(radialPosition);
    }

    public void Freeze(int seconds)
    {
        if (seconds <= 0)
        {
            return;
        }

        if (this.unfreezeCoroutine != null)
        {
            StopCoroutine(this.unfreezeCoroutine);
        }
        this.frozen = true;
        CancelInvoke("doDamageToBoss");
        CancelInvoke("spawnProjectile");
        this.unfreezeCoroutine = ResetFrozenAfterTime(seconds);
        StartCoroutine(this.unfreezeCoroutine);
    }

    public IEnumerator ResetFrozenAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        this.frozen = false;
    }
}
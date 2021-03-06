﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum EnemyType
{
    ENEMY,
    PROJECTILE,
    MINION
}

public class Enemy : MonoBehaviour
{
    private float originalMovementSpeed = 1.0f;
    private float MovementSpeed = 1.0f;
    private float originalAngularSpeed = 0f;
    private float angularSpeed = 0f;

    private float Damage = 20.0f;

    private float Range = 1.0f;

    public float Health { get; private set; } = 100.0f;

    public float MaxHealth { get; private set; } = 100.0f;

    public bool SetForDeath = false;

    private float Scale = 1.0f;
    private float circlingSpeed;

    private bool selfDestruct;
    private bool invunerable;
    private float attackFrequency;
    private float initialAttackDelay;

    BossHealth bossHealth;
    private ColorModifier colorModifier;
    private EnemySpawner enemySpawner;
    private SpriteRenderer spriteRenderer;

    private GameObject hitParticle;
    private GameObject healthPickup;
    private GameObject freezePickup;
    private GameObject shieldPickup;
    private GameObject pushBackPickup;
    private EnemySettings projectile;
    private GameObject cooldownResetPickup;
    private float zigZagAngleLow;
    private float zigZagAngleHigh;
    private bool wasZigZag;
    private bool zigZag;
    private WaveHandler waveHandler;
    private bool frozen;
    private Transform sprite;
    public EnemyType EnemyType { get; private set; }
    private IEnumerator unfreezeCoroutine;
    public float turnBackDistance;
    public float turnForwardDistance;
    public int numberOfTurns; // Negative value means keep turning forever.
    private bool knockedBack;
    private Rigidbody2D enemyRigidbody;
    private ProgressBarBehaviour healthBar;
    private float timeSinceSpawned = 0.0f;

    private List<HealthInitiatedEnemyAction> healthInitiatedEnemyActions;
    private List<TimeInitiatedEnemyAction> timeInitiatedEnemyActions;

    void Awake()
    {
        this.enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
        this.sprite = transform.Find("Sprite");
        this.spriteRenderer = this.sprite.GetComponent<SpriteRenderer>();
        this.colorModifier = this.sprite.GetComponent<ColorModifier>();
        this.hitParticle = Resources.Load("Prefabs/HitParticleSystem", typeof(GameObject)) as GameObject;
        this.healthPickup = Resources.Load("Prefabs/HealthPickup", typeof(GameObject)) as GameObject;
        this.shieldPickup = Resources.Load("Prefabs/ShieldPickup", typeof(GameObject)) as GameObject;
        this.freezePickup = Resources.Load("Prefabs/FreezePickup", typeof(GameObject)) as GameObject;
        this.pushBackPickup = Resources.Load("Prefabs/PushBackPickup", typeof(GameObject)) as GameObject;
        this.cooldownResetPickup = Resources.Load("Prefabs/CooldownResetPickup", typeof(GameObject)) as GameObject;
        this.waveHandler = GameObject.FindObjectOfType<WaveHandler>();
        this.enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        this.healthBar = UnityUtils.RecursiveFind(transform, "HealthBar").GetComponent<ProgressBarBehaviour>();
    }

    void Start()
    {
        bossHealth = GameObject.Find("Boss").GetComponent<BossHealth>();
        this.sprite.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.zero - transform.position);
    }

    void Update()
    {
        this.timeSinceSpawned += Time.deltaTime;
        float distanceFromBoss = Vector3.Distance(Vector3.zero, transform.position);

        if (this.enemyRigidbody.velocity.magnitude != 0 && this.EnemyType != EnemyType.PROJECTILE)
        {
            this.knockedBack = true;
            if (distanceFromBoss > this.Range)
            {
                CancelInvoke();
            }
            return;
        }
        else if (knockedBack)
        {
            this.knockedBack = false;
            // Reset movement after knock back.
            this.MovementSpeed = (this.numberOfTurns % 2 == 0) ? this.originalMovementSpeed : -this.originalMovementSpeed;
            this.zigZag = this.wasZigZag;
            this.angularSpeed = this.originalAngularSpeed;
        }

        if (this.frozen && this.EnemyType != EnemyType.PROJECTILE)
        {
            return;
        }

            
        List<TimeInitiatedEnemyAction> actionsToExectue = this.timeInitiatedEnemyActions
            .Where(action => this.timeSinceSpawned > action.Time)
            .ToList();

        actionsToExectue.ForEach(action => action.Action.Execute(this.gameObject));
        this.timeInitiatedEnemyActions = this.timeInitiatedEnemyActions.Except(actionsToExectue).ToList();


        RadialPosition radialPosition = RotationUtils.XYToRadialPos(this.transform.position);
        
        // Move forward and back
        float radialStep = this.MovementSpeed * Time.deltaTime;
        radialPosition.AddRadius((-1) * radialStep);

        // Move sideways
        float angularStep = this.angularSpeed * Time.deltaTime;
        radialPosition.AddAngle(angularStep);
        float newPositionAngle = radialPosition.GetAngle();
        if (RotationUtils.InCounterClockwiseLimits(newPositionAngle, this.zigZagAngleHigh, this.zigZagAngleLow) && zigZag)
        {
            clampPositionAngle(radialPosition, this.zigZagAngleLow, this.zigZagAngleHigh);
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
                InvokeRepeating("doDamageToBoss", this.initialAttackDelay, this.attackFrequency);
            }
            // Ranged attack
            else if (Range > Parameters.MELEE_RANGE && !IsInvoking("spawnProjectile"))
            {
                InvokeRepeating("spawnProjectile", this.initialAttackDelay, this.attackFrequency);
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
            clampPositionRadius(radialPosition);
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

    private void clampPositionRadius(RadialPosition radialPos)
    {
        if (this.numberOfTurns == 0)
        {
            return;
        }
        if (this.MovementSpeed < 0)
        {
            radialPos.SetRadius(this.turnBackDistance);
        }
        else
        {
            radialPos.SetRadius(this.turnForwardDistance);
        }
    }

    private void clampPositionAngle(RadialPosition radialPos, float zigZagAngleLow, float zigZagAngleHigh)
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

        if (this.projectile == null)
        {
            this.projectile = Instantiate(EnemyFactory.Projectile());
        }

        EnemySettings instantiatedProjectile = Instantiate(this.projectile);
        instantiatedProjectile.Damage = this.Damage;
        instantiatedProjectile.predefinedPosition = true;
        instantiatedProjectile.spawnAngle = thisRadialPos.GetAngle();
        instantiatedProjectile.spawnRadius = thisRadialPos.GetRadius();

        GameObject spawnedProjectile = this.enemySpawner.InstantiateEnemyPrefab(instantiatedProjectile);
    }

    public bool isInAttackArea(float lowAngle, float highAngle, float nearRadius, float farRadius)
    {

        float spriteRadius = this.spriteRenderer.bounds.size.x / 2;
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
        float percentHealthLeft = Health / MaxHealth;
        this.healthBar.UpdateFill(percentHealthLeft);

        if (Health <= 0)
        {
            KillSelf();
            SpawnItem();
        }
        else
        {
            this.spriteRenderer.color = Color.red;
            StartCoroutine(UnityUtils.ChangeToDefaultColorAfterTime(colorModifier, 0.5f));
            
            List<HealthInitiatedEnemyAction> actionsToExectue = this.healthInitiatedEnemyActions
                .Where(action => percentHealthLeft < action.Health)
                .ToList();

            actionsToExectue.ForEach(action => action.Action.Execute(this.gameObject));
            this.healthInitiatedEnemyActions = this.healthInitiatedEnemyActions.Except(actionsToExectue).ToList();
        }
    }

    private void SpawnItem()
    {
        if (this.EnemyType != EnemyType.ENEMY)
        {
            return;
        }

        int itemRand = Random.Range(0, 55);
        if (itemRand < 5)
        {
            if (itemRand == 0)
            {
                Instantiate(this.healthPickup, this.transform.position, Quaternion.identity);
            }
            else if (itemRand == 1)
            {
                Instantiate(this.shieldPickup, this.transform.position, Quaternion.identity);
            }
            else if (itemRand == 2)
            {
                Instantiate(this.cooldownResetPickup, this.transform.position, Quaternion.identity);
            }
            else if (itemRand == 3)
            {
                Instantiate(this.pushBackPickup, this.transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(this.freezePickup, this.transform.position, Quaternion.identity);
            }
            this.waveHandler.ItemAdded();
        }
    }

    public void SetStats(EnemySettings enemySettings)
    {
        this.spriteRenderer.sprite = enemySettings.Sprite;
        this.spriteRenderer.material = enemySettings.Material;

        transform.name = enemySettings.Name;
        this.EnemyType = enemySettings.enemyType;
        this.selfDestruct = enemySettings.selfDestruct;
        this.invunerable = enemySettings.invunerable;
        if (invunerable) //Don't show healthbar for invunerable units (projectiles)
        {
            Destroy(UnityUtils.RecursiveFind(transform, "HealthBar").gameObject);
        }

        MoveTo(new RadialPosition(enemySettings.spawnRadius, enemySettings.spawnAngle));

        // Randomization variable takes precident over clockwise variable
        int angularDirectionMultiplier = 1;
        if (enemySettings.randomizeAngularDirection)
        {
            angularDirectionMultiplier = UnityEngine.Random.value >= 0.5 ? -1 : 1;
        }
        else if (enemySettings.angularMoveClockwise)
        {
            angularDirectionMultiplier = -1;
        }

        int circlingDirectionMultiplier = 1;
        if (enemySettings.randomizeCirclingDirection)
        {
            circlingDirectionMultiplier = UnityEngine.Random.value >= 0.5 ? -1 : 1;
        }
        else if (enemySettings.circlingMoveClockwise)
        {
            circlingDirectionMultiplier = -1;
        }

        this.MaxHealth = enemySettings.Health;
        if (enemySettings.SpawnHealth > 0)
        {
            this.Health = enemySettings.SpawnHealth;
            this.healthBar.UpdateFill(this.Health / this.MaxHealth);
        }
        else {
            this.Health = enemySettings.Health;
        }

        this.angularSpeed = enemySettings.angularSpeed * angularDirectionMultiplier;
        this.originalAngularSpeed = this.angularSpeed;
        this.circlingSpeed = enemySettings.circlingSpeed * circlingDirectionMultiplier;
        this.MovementSpeed = enemySettings.MovementSpeed;
        this.originalMovementSpeed = this.MovementSpeed;

        Damage = enemySettings.Damage;

        switch (enemySettings.Range)
        {
            case RangeLevel.SELF_DESTRUCT:
                Range = Parameters.SELF_DESTRUCT_RANGE;
                break;
            case RangeLevel.MELE:
                Range = Parameters.MELEE_RANGE;
                break;
            case RangeLevel.MID:
                Range = Parameters.MID_RANGE;
                break;
            case RangeLevel.LONG:
                Range = Parameters.LONG_RANGE;
                break;
            default:
                Range = Parameters.MELEE_RANGE;
                break;
        }

        Scale = enemySettings.Scale;
        Transform sprite = transform.Find("Sprite");
        sprite.transform.localScale *= enemySettings.Scale;

        if (enemySettings.Scale > 1)
        {
            Transform canvas = transform.Find("Canvas");
            canvas.localPosition = new Vector3(canvas.localPosition.x, canvas.localPosition.y * enemySettings.Scale * 0.8f, canvas.localPosition.z);
        }

        this.attackFrequency = enemySettings.attackDelay;
        this.initialAttackDelay = enemySettings.InitialAttackDelay;
        
        this.zigZag = enemySettings.zigZag;
        this.wasZigZag = this.zigZag;

        if (enemySettings.angularSpeed > 0)
        {
            this.zigZagAngleLow = enemySettings.spawnAngle;
            this.zigZagAngleHigh = enemySettings.spawnAngle + enemySettings.zigZagAngle;
        }
        else
        {
            this.zigZagAngleHigh = enemySettings.spawnAngle;
            this.zigZagAngleLow = enemySettings.spawnAngle - enemySettings.zigZagAngle;
        }

        // Make sure all health initiated actions where the limit has already been passed are not executed immediately on the first attack.
        this.healthInitiatedEnemyActions = enemySettings.HealthInitiatedEnemyActions
            .Where(action => action.Health < (this.Health / this.MaxHealth))
            .ToList();
        this.timeInitiatedEnemyActions = enemySettings.TimeInitiatedEnemyActions;

        this.projectile = enemySettings.projectile ? enemySettings.projectile : null;
        this.turnBackDistance = enemySettings.TurnBackDistance;
        this.turnForwardDistance = enemySettings.TurnForwardDistance;
        this.numberOfTurns = enemySettings.NumberOfTurns;
        colorModifier.SetDefaultColor(enemySettings.Color);
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

        CancelInvoke("doDamageToBoss");
        CancelInvoke("spawnProjectile");

        Destroy(this.gameObject);
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
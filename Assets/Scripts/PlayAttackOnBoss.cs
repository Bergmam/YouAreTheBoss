using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayAttackOnBoss : MonoBehaviour
{
    private ColorModifier aimColorModifier;
    BossAttack currentAttack;
    CooldownBehaviour currentCooldownBehaviour;
    private TextMeshProUGUI tapText;
    SpriteRenderer spriteRenderer;
    private GameObject projectile;
    private GameObject chargeSystem;
    private GameObject chargeSystemResource;
    private float activeAttackWait = 4.0f;
    private SelfShaker shaker;
    private bool started;
	private Material aimMaterial;

    void Awake()
    {
        this.projectile = Resources.Load<GameObject>("Prefabs/BossProjectile");
        Transform aim = UnityUtils.RecursiveFind(transform, "Aim");
		this.aimMaterial = aim.GetComponent<Renderer>().material;
        this.aimColorModifier = aim.GetComponent<ColorModifier>();
        this.tapText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        this.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        this.shaker = gameObject.AddComponent<SelfShaker>();
        this.chargeSystemResource = Resources.Load<GameObject>("Prefabs/ChargeUp");
    }

    void Start()
    {
        if (this.started)
        {
            return; // setAttack is sometimes called before Start. Since setAttack calls Start, we make sure Start is not run again after setAttack.
        }
        this.started = true;
        this.chargeSystem = Instantiate(this.chargeSystemResource, transform.position, Quaternion.identity);
        this.aimColorModifier.SetColor(Parameters.AIM_DEFAULT_COLOR);
        this.tapText.gameObject.SetActive(false);
        this.chargeSystem.SetActive(false);
    }

    public void setAttack(BossAttack attack)
    {
        Start();

        this.currentAttack = attack;
        
        this.aimMaterial.SetFloat("Angle", this.currentAttack.angle);
        this.aimMaterial.SetFloat("InnerRadius", this.currentAttack.closeRadius);
        this.aimMaterial.SetFloat("OuterRadius", this.currentAttack.farRadius);
        this.aimMaterial.SetFloat("Scale", this.transform.localScale.x);

        this.tapText.gameObject.SetActive(false);

        if (this.currentAttack.frequency > Parameters.SLOW_ATTACK_LIMIT)
        {
            this.aimColorModifier.SetSelectedColor(Parameters.ACTIVE_ATTACK_AIM_COLOR);
            InvokeRepeating("doActiveAttack", 0, this.currentAttack.frequency + this.activeAttackWait);
        }
        else
        {
            this.chargeSystem.SetActive(false);
            this.aimColorModifier.SetSelectedColor(Parameters.AIM_DAMAGE_COLOR);
            InvokeRepeating("doPassiveAttack", 0, this.currentAttack.frequency);
        }
    }

    void doActiveAttack()
    {
        if (this.currentAttack.isProjectile)
        {
            // TODO: Active attack projetiles
        }
        else
        {
            this.aimColorModifier.StopFade();
            this.aimColorModifier.Select();
            this.chargeSystem.SetActive(true);
            StartCoroutine(doActiveAttackAfterTime(this.aimColorModifier, this.activeAttackWait, this.currentAttack.frequency));
        }
    }

    private IEnumerator doActiveAttackAfterTime(ColorModifier colorModifier, float waitTime, float fadeTime)
    {
        yield return new WaitForSeconds(waitTime);
        this.spriteRenderer.color = Parameters.ACTIVE_ATTACK_AIM_COLOR;
        this.chargeSystem.SetActive(false);
        this.tapText.gameObject.SetActive(true);
        this.shaker.Shake(0.05f, 0.3f);
        StartCoroutine(UnityUtils.ChangeToColorAfterTime(this.spriteRenderer, Parameters.BOSS_COLOR, 0.5f));
        StartCoroutine(UnityUtils.DeactiveGameObjectAfterTime(this.tapText.gameObject, 1.5f));
        this.aimColorModifier.FadeToDeselected(fadeTime);
    }

    void doPassiveAttack()
    {
        if (this.currentAttack.isProjectile)
        {
            Vector3 projectilePos = this.transform.position;
            GameObject spawnedProjectile = Instantiate(this.projectile, projectilePos, Quaternion.identity);
            spawnedProjectile.GetComponent<BossProjectile>().Attack = this.currentAttack;
        }
        else
        {
            this.aimColorModifier.FadeToSelected(0.0f);
            this.aimColorModifier.FadeToDeselected(this.currentAttack.frequency);
        }

        this.spriteRenderer.color = Color.red;
        StartCoroutine(UnityUtils.ChangeToColorAfterTime(this.spriteRenderer, Parameters.BOSS_COLOR, 0.5f));
    }

    void OnDestroy()
    {
        StopAllCoroutines();
        CancelInvoke();
    }

    void OnDisable()
    {
        this?.aimColorModifier?.StopFade();
        this?.aimColorModifier?.DeSelect();
        this?.chargeSystem?.SetActive(false);

        StopAllCoroutines();
        CancelInvoke();
    }
}

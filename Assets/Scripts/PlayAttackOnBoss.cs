using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayAttackOnBoss : MonoBehaviour
{
    RadialFillControl radialFillControl;
    AttackMaskControl attackMaskControl;
    private ColorModifier aimColorModifier;
    BossAttack currentAttack;
    CooldownBehaviour currentCooldownBehaviour;
    private TextMeshProUGUI tapText;
    private Coroutine changeStuff;
    SpriteRenderer spriteRenderer;
    private GameObject projectile;

    void Awake()
    {
        this.projectile = Resources.Load<GameObject>("Prefabs/BossProjectile");
        this.radialFillControl = UnityUtils.RecursiveFind(transform, "Mask").GetComponent<RadialFillControl>();
        this.attackMaskControl = UnityUtils.RecursiveFind(transform, "Mask").GetComponent<AttackMaskControl>();
        Transform aim = UnityUtils.RecursiveFind(transform, "Image");
        this.aimColorModifier = aim.GetComponent<ColorModifier>();
        this.tapText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        this.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        this.aimColorModifier.SetDefaultColor(Parameters.AIM_DEFAULT_COLOR);
        UnityUtils.RecursiveFind(transform, "Image").GetComponent<Image>().color = Parameters.AIM_DEFAULT_COLOR;
        this.tapText.gameObject.SetActive(false);
    }

    public void setAttack(BossAttack attack)
    {
        this.currentAttack = attack;

        if (radialFillControl != null)
        {
            radialFillControl.SetMirroredFill(this.currentAttack.angle);
        }
        if (attackMaskControl != null)
        {
            attackMaskControl.SetSize(this.currentAttack.closeRadius, this.currentAttack.farRadius);
        }

        this.tapText.gameObject.SetActive(false);

        if (this.currentAttack.frequency > Parameters.SLOW_ATTACK_LIMIT)
        {
            this.aimColorModifier.SetSelectedColor(Parameters.ACTIVE_ATTACK_AIM_COLOR);
            this.aimColorModifier.Select();
            InvokeRepeating("doActiveAttack", 0.5f, this.currentAttack.frequency + 2.0f);
        }
        else
        {
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
            this.aimColorModifier.Select();
            this.changeStuff = StartCoroutine(DoActiveAttackAfterTime(this.aimColorModifier, 1.0f, this.currentAttack.frequency));
        }

        float resetAttacktiveAttackAfter = 1.3f;
        StartCoroutine(UnityUtils.ChangeToColorAfterTime(this.spriteRenderer, new Color(0, 0.89f, 1), resetAttacktiveAttackAfter));
        StartCoroutine(UnityUtils.DeactiveGameObjectAfterTime(this.tapText.gameObject, resetAttacktiveAttackAfter));
    }
    
    private IEnumerator DoActiveAttackAfterTime(ColorModifier colorModifier, float waitTime, float fadeTime) {
        yield return new WaitForSeconds(waitTime);
        this.spriteRenderer.color = Parameters.ACTIVE_ATTACK_AIM_COLOR;
        this.tapText.gameObject.SetActive(true);
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
        StartCoroutine(UnityUtils.ChangeToColorAfterTime(this.spriteRenderer, new Color(0, 0.89f, 1), 0.5f));
    }

    void OnDestroy()
    {
        StopAllCoroutines();
        CancelInvoke();
    }

    void OnDisable()
    {
        this.aimColorModifier.StopFade();
        this.aimColorModifier.DeSelect();
        StopAllCoroutines();
        CancelInvoke();
    }
}

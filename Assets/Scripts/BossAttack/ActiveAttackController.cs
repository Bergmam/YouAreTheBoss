using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActiveAttackController : MonoBehaviour
{

    private BossAttack currentAttack;
    private GameObject currentAttackButton;
    private Color currentAttackButtonOriginalColor;
    private ColorModifier aimColorModifier;
    private CameraShake camShake;
    private Sprite fireSprite;
    private Sprite defaultSprite;
    private static Color AIM_COLOR = Color.magenta;
    private GameObject backgroundFade;
    private GameObject activeAttackScreenButton;
    private bool active;

    void Awake()
    {
        AIM_COLOR.a = 0.6f;
        this.fireSprite = Resources.Load<Sprite>("Art/UI_Icon_FullScreenExit");
        this.defaultSprite = Resources.Load<Sprite>("Art/UI_Button_Standard_Sky_2");
        this.camShake = GameObject.Find("Handler").GetComponent<CameraShake>();
        Transform aim = UnityUtils.RecursiveFind(transform, "Image");
        this.aimColorModifier = aim.GetComponent<ColorModifier>();
        this.backgroundFade = GameObject.Find("BackgroundFade");
        this.activeAttackScreenButton = GameObject.Find("ActiveAttackScreenButton");
    }

    void Start()
    {
        this.activeAttackScreenButton.SetActive(false);
        this.backgroundFade.SetActive(false);
    }

    public void SetAttack(int attackNumber)
    {
        this.active = true;
        this.activeAttackScreenButton.SetActive(true);
        BossAttack newAttack = AttackLists.selectedAttacks[attackNumber - 1];
        this.currentAttack = newAttack;
        setColors(attackNumber);
        this.backgroundFade.SetActive(true);
    }

    private void setColors(int attackNumber)
    {
        this.currentAttackButton = GameObject.Find("Passive" + attackNumber + "Button");
        this.currentAttackButtonOriginalColor = currentAttackButton.transform.parent.GetComponent<Image>().color;
        this.currentAttackButton.transform.parent.GetComponent<Image>().color = new Color(1.0f, 0.3f, 1.0f, 1.0f);
        this.currentAttackButton.GetComponent<Image>().sprite = fireSprite;
        this.currentAttackButton.transform.Find("Image").gameObject.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
        this.aimColorModifier.FadeToSelected(0.0f);
        this.aimColorModifier.SetColor(AIM_COLOR);
        foreach (Transform child in GameObject.Find("BossButtons").transform)
        {
            if (!child.name.Contains(attackNumber.ToString()))
            {
                child.Find("Overlay").GetComponent<Image>().color = new Color(0, 0, 0, 0.2f);
            }
            else
            {
                child.Find("Highlight").GetComponent<Image>().color = Color.magenta;
            }
        }
    }

    public void DoAttack()
    {
        this.activeAttackScreenButton.SetActive(false);
        float unitCircleRotation = RotationUtils.MakePositiveAngle(transform.eulerAngles.z + 90);

        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (Enemy enemy in GameObject.FindObjectsOfType(typeof(Enemy)))
        {
            if (!enemy.SetForDeath && enemy.isInAttackArea(unitCircleRotation - this.currentAttack.angle,
                    unitCircleRotation + this.currentAttack.angle,
                    this.currentAttack.closeRadius,
                    this.currentAttack.farRadius))
            {
                enemy.applyDamageTo(this.currentAttack.damage);
            }
        }
        this.camShake.Shake(0.1f, 0.2f);

        StartCoroutine(UnityUtils.ChangeToColorAfterTime(gameObject.GetComponent<SpriteRenderer>(), new Color(0, 0.89f, 1), 0.5f));

        CooldownBehaviour cooldownBehaviour = this.currentAttackButton.GetComponentInChildren<CooldownBehaviour>();
        cooldownBehaviour.StartCooldown(this.currentAttack.frequency);
        this.backgroundFade.SetActive(false);
        this.activeAttackScreenButton.SetActive(false);
        aimColorModifier.SetDefaultColor(Parameters.AIM_DEFAULT_COLOR);
        aimColorModifier.SetSelectedColor(AIM_COLOR);
        aimColorModifier.FadeToSelected(this.currentAttack.frequency);
        currentAttackButton.transform.Find("Image").gameObject.SetActive(true);
        this.currentAttackButton.GetComponent<Image>().sprite = defaultSprite;
        StartCoroutine(this.ReactivateAttackAfterTime(this.currentAttack.frequency));
    }

    public IEnumerator ReactivateAttackAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (this.active)
        {
            this.backgroundFade.SetActive(true);
            this.activeAttackScreenButton.SetActive(true);
            currentAttackButton.transform.Find("Image").gameObject.SetActive(false);
            this.currentAttackButton.GetComponent<Image>().sprite = fireSprite;
        }
    }

    public void CancelReactivate()
    {
        this.active = false;
    }
}
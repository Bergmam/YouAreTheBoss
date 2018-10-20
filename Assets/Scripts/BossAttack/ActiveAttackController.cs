using UnityEngine;
using UnityEngine.UI;

public class ActiveAttackController : MonoBehaviour
{

    private BossAttack currentAttack;
    private GameObject activeAttackScreenButton;

    private Color currentAttackButtonOriginalColor;

    private ColorModifier aimColorModifier;

    private GameObject backgroundFade;
    private Sprite fireSprite;

    void Awake()
    {
        this.activeAttackScreenButton = GameObject.Find("ActiveAttackScreenButton");
        this.backgroundFade = GameObject.Find("BackgroundFade");
        this.backgroundFade.SetActive(false);
        this.fireSprite = Resources.Load<Sprite>("Art/UI_Icon_FullScreenExit");
    }

    public void SetAttack(int attackNumber)
    {
        BossAttack newAttack = AttackLists.selectedAttacks[attackNumber - 1];
        GameObject currentAttackButton = GameObject.Find("Passive" + attackNumber + "Button");
        this.currentAttackButtonOriginalColor = currentAttackButton.transform.parent.GetComponent<Image>().color;
        gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
        currentAttackButton.transform.parent.GetComponent<Image>().color = new Color(1.0f, 0.3f, 1.0f, 1.0f);
        currentAttackButton.GetComponent<Image>().sprite = fireSprite;
        currentAttackButton.transform.Find("Image").gameObject.SetActive(false);
        this.backgroundFade.SetActive(true);
        Color aimColor = Color.magenta;
        aimColor.a = 0.6f;
        this.aimColorModifier.SetColor(aimColor);
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
        this.activeAttackScreenButton.SetActive(true);
        this.activeAttackScreenButton.GetComponent<EventTimer>().AddTimedTrigger(() => SetAttack(attackNumber));
    }

    public void DoAttack()
    {
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
    }
}
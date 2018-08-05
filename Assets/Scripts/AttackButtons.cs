using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AttackButtons : MonoBehaviour
{

    List<Button> clickedButtons = new List<Button>();
    List<Color> usedColors = new List<Color>();
    Button playButton;
    GameObject attackPopUp;
    GameObject upgradePopUp;

    GameObject infoButton;
    private Dictionary<string, Button> attackButtons = new Dictionary<string, Button>();
    private static Color whiteNoAlpha = new Color(1, 1, 1, 0);
    GameObject attackButtonPrefab;

    void Awake()
    {
        this.attackPopUp = GameObject.Find("AttackPopUp");
        this.infoButton = GameObject.Find("InfoButton");
        this.playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        this.attackButtonPrefab = Resources.Load("Prefabs/AttackButton", typeof(GameObject)) as GameObject;
        this.upgradePopUp = GameObject.Find("UpgradePopUp");
    }

    void Start()
    {
        attackPopUp.SetActive(false);
        playButton.interactable = false;
        upgradePopUp.SetActive(false);

        for (int i = 0; i < AttackLists.GetNumberOfSelectableAttacks(); i++)
        {
            BossAttack attack = AttackLists.GetSelectableAttack(i);
            CreateAttackButton(i, attack);
        }

        // If we have previously chosen some attacks and entered the fighting scene
        AttackLists.selectedAttacks
            .Where(attack => attack != null)
            .ToList()
            .ForEach(attack => SelectAttackButton(attackButtons[attack.name]));

        // If we are on a certain wave number and have upgrade attacks left that we can choose from
        bool onUpgradeWave = WaveNumber.waveNumber % Parameters.ATTACK_UPGRADE_WAVE_NUMBER == 0 && WaveNumber.waveNumber != 0;
        bool attackUpgradesLeft = AttackLists.chooseableUpgradeAttacks.Count >= 2;
        if (onUpgradeWave && attackUpgradesLeft)
        {
            infoButton.GetComponent<Button>().interactable = false;
            upgradePopUp.SetActive(true);
            DisableButtons();

            // Get index of random attack from chooseableUpgradeAttacks
            int index = UnityEngine.Random.Range(0, AttackLists.chooseableUpgradeAttacks.Count);
            SetUpgradePopUpAttack(1, index);

            // Get another, different index
            int previousIndex = index;
            while (index == previousIndex)
            {
                index = UnityEngine.Random.Range(0, AttackLists.chooseableUpgradeAttacks.Count);
            }
            SetUpgradePopUpAttack(2, index);
            this.playButton.interactable = false;
        }
    }

    private void SetUpgradePopUpAttack(int upgradePopUpAttackNumber, int upgradeAttacksIndex)
    {
        GameObject attackButton = UnityUtils.RecursiveFind(upgradePopUp.transform, "Attack" + upgradePopUpAttackNumber + "Button").gameObject;
        GameObject attackText = UnityUtils.RecursiveFind(upgradePopUp.transform, "Attack" + upgradePopUpAttackNumber + "Name").gameObject;
        BossAttack attack = AttackLists.chooseableUpgradeAttacks[upgradeAttacksIndex];
        attackText.GetComponent<Text>().text = attack.name;
        attackButton.transform.Find("Text").GetComponent<Text>().text = "Damage: " + attack.damage;
        playAttackPreview(upgradePopUp.transform, attack, "BossAttackPreview" + upgradePopUpAttackNumber);
        attackButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            ChooseUpgrade(attack);
        });
    }

    void CreateAttackButton(int i, BossAttack attack)
    {
        GameObject instantiatedButtonPrefab = Instantiate(attackButtonPrefab);
        instantiatedButtonPrefab.transform.SetParent(transform, false);
        instantiatedButtonPrefab.name = "Attack" + (i) + " Object";
        Transform buttonObject = UnityUtils.RecursiveFind(instantiatedButtonPrefab.transform, "Attack0");
        buttonObject.name = "Attack" + (i);
        Button button = buttonObject.GetComponent<Button>();
        attackButtons.Add(attack.name, button);
        button.onClick.AddListener(() => this.AttackButtonPressed(button));
        Transform imageTransform = button.transform.Find("Image");
        Image image = imageTransform.GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>(AttackLists.GetAssetString(attack.name));
    }

    public void ChooseUpgrade(BossAttack attack)
    {
        AttackLists.ChooseUpgradeAttack(attack);
        CreateAttackButton(attackButtons.Count, attack);
        DisableUpgradePopUp();
        this.playButton.interactable = true;
    }

    public void DisableButtons()
    {
        attackButtons.ToList().ForEach(nameButtonPair => nameButtonPair.Value.interactable = false);
        this.playButton.interactable = false;
    }

    public void AttackButtonPressed(Button button)
    {
        if (!clickedButtons.Contains(button) && clickedButtons.Count < 3)
        {
            DisableButtons();
            infoButton.GetComponent<Button>().interactable = false;
            attackPopUp.SetActive(true);
            int buttonNumber = Int32.Parse(button.transform.name.Replace("Attack", ""));
            BossAttack attack = AttackLists.GetSelectableAttack(buttonNumber);
            UnityUtils.RecursiveFind(attackPopUp.transform, "AttackName").GetComponent<Text>().text = attack.name;
            Text text = UnityUtils.RecursiveFind(attackPopUp.transform, "AttackInfoText").GetComponent<Text>();
            text.text = "Damage: " + attack.damage + "\n";
            playAttackPreview(attackPopUp.transform, attack, "BossAttackPreview");
            Button yesButton = UnityUtils.RecursiveFind(attackPopUp.transform, "YesButton").GetComponent<Button>();
            yesButton.onClick.AddListener(() => { SelectAttackButton(button); });
        }
        else
        {
            DeSelectAttackButton(button);
        }

    }

    private void EnableAttackButtons()
    {
        attackButtons.ToList().ForEach(nameButtonPair => nameButtonPair.Value.interactable = true);
    }

    public void DisableUpgradePopUp()
    {
        UnityUtils.RecursiveFind(upgradePopUp.transform, "Attack1Button").GetComponent<Button>().onClick.RemoveAllListeners();
        UnityUtils.RecursiveFind(upgradePopUp.transform, "Attack2Button").GetComponent<Button>().onClick.RemoveAllListeners();
        EnableAttackButtons();
        DisableUnclickedButtons();
        upgradePopUp.SetActive(false);
        infoButton.GetComponent<Button>().interactable = false;
    }

    public void DisablePopUp()
    {
        UnityUtils.RecursiveFind(attackPopUp.transform, "YesButton").GetComponent<Button>().onClick.RemoveAllListeners();
        EnableAttackButtons();

        attackPopUp.SetActive(false);
        infoButton.GetComponent<Button>().interactable = true;
    }

    public void SelectAttackButton(Button button)
    {
        DisablePopUp();
        int buttonNumber = Int32.Parse(button.transform.name.Replace("Attack", ""));
        Image panelImage = button.transform.parent.Find("Panel").GetComponent<Image>();
        if (clickedButtons.Count < 3)
        {
            Color firstUnusedColor = Parameters.COLOR_LIST
                .Where(color => !usedColors.Contains(color))
                .ElementAt(0);
            panelImage.color = firstUnusedColor;
            usedColors.Add(firstUnusedColor);
            int index = Array.IndexOf(Parameters.COLOR_LIST, firstUnusedColor);
            AttackLists.selectedAttacks[index] = AttackLists.GetSelectableAttack(buttonNumber);
            clickedButtons.Add(button);
            if (!playButton.interactable && clickedButtons.Count >= 3)
            {
                playButton.interactable = true;
                DisableUnclickedButtons();
            }
        }
    }

    public void DisableUnclickedButtons()
    {
        attackButtons
        .Where(nameButtonPair => !clickedButtons.Contains(nameButtonPair.Value))
        .ToList()
        .ForEach(nameButtonPair => nameButtonPair.Value.interactable = false);
    }

    public void DeSelectAttackButton(Button button)
    {
        int buttonNumber = Int32.Parse(button.transform.name.Replace("Attack", ""));
        Image panelImage = button.transform.parent.Find("Panel").GetComponent<Image>();
        int index = Array.IndexOf(Parameters.COLOR_LIST, panelImage.color);
        AttackLists.selectedAttacks[index] = null;
        this.usedColors.Remove(panelImage.color);
        panelImage.color = whiteNoAlpha;
        clickedButtons.Remove(button);
        if (clickedButtons.Count < 3)
        {
            EnableAttackButtons();
            playButton.interactable = false;
        }
    }

    private void playAttackPreview(Transform popupTransform, BossAttack attack, string previewName)
    {
        Transform bossAttackPreview = UnityUtils.RecursiveFind(popupTransform, previewName);
        bossAttackPreview.GetComponent<PlayAttackOnBoss>().setAttack(attack);
    }
}

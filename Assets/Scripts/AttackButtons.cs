using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AttackButtons : MonoBehaviour
{

    List<Button> clickedButtons = new List<Button>();
    List<KeyValuePair<Color, bool>> colorTaken = new List<KeyValuePair<Color, bool>>();

    Button playButton;

    GameObject attackPopUp;
    GameObject upgradePopUp;
    List<Button> attackButtons = new List<Button>();

    Color whiteNoAlpha = new Color(1, 1, 1, 0);

    GameObject attackButtonPrefab;

    void Awake()
    {
        this.attackPopUp = GameObject.Find("AttackPopUp");
        this.playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        this.attackButtonPrefab = Resources.Load("Prefabs/AttackButton", typeof(GameObject)) as GameObject;
        this.upgradePopUp = GameObject.Find("UpgradePopUp");
    }

    void Start()
    {
        attackPopUp.SetActive(false);
        playButton.interactable = false;
        upgradePopUp.SetActive(false);

        for (int i = 0; i < AttackLists.allAttacks.Count; i++)
        {
            BossAttack attack = AttackLists.allAttacks[i];
            CreateAttackButton(i, attack);
        }

        // If we are on a certain wave number and have upgrade attacks left that we can choose from
        bool onUpgradeWave = WaveNumber.waveNumber % Parameters.ATTACK_UPGRADE_WAVE_NUMBER == 0 && WaveNumber.waveNumber != 0;
        bool attackUpgradesLeft = AttackLists.choseableUpgradeAttacks.Count >= 2;
        if (onUpgradeWave && attackUpgradesLeft)
        {
            upgradePopUp.SetActive(true);

            DisableButtons();

            GameObject attackButton1 = UnityUtils.RecursiveFind(upgradePopUp.transform, "Attack1Button").gameObject;
            GameObject attackButton2 = UnityUtils.RecursiveFind(upgradePopUp.transform, "Attack2Button").gameObject;
            GameObject attackText1 = UnityUtils.RecursiveFind(upgradePopUp.transform, "Attack1Name").gameObject;
            GameObject attackText2 = UnityUtils.RecursiveFind(upgradePopUp.transform, "Attack2Name").gameObject;

            // Get index of random attack from choseableUpgradeAttacks
            int index = UnityEngine.Random.Range(0, AttackLists.choseableUpgradeAttacks.Count);

            BossAttack attack = AttackLists.choseableUpgradeAttacks[index];
            attackText1.GetComponent<Text>().text = attack.name;
            attackButton1.transform.Find("Text").GetComponent<Text>().text = "Damage: " + attack.damage + "\n";
            playAttackPreview(upgradePopUp.transform, attack, "BossAttackPreview1");
            attackButton1.GetComponent<Button>().onClick.AddListener(() =>
            {
                ChooseUpgrade(attack);
            });

            // Get new index of attack,
            int previousIndex = index;
            while (index == previousIndex)
            {
                index = UnityEngine.Random.Range(0, AttackLists.choseableUpgradeAttacks.Count);
            }
            attack = AttackLists.choseableUpgradeAttacks[index];
            attackText2.GetComponent<Text>().text = attack.name;
            attackButton2.transform.Find("Text").GetComponent<Text>().text = "Damage: " + attack.damage + "\n";
            playAttackPreview(upgradePopUp.transform, attack, "BossAttackPreview2");
            attackButton2.GetComponent<Button>().onClick.AddListener(() =>
            {
                ChooseUpgrade(attack);
            });

        }

        foreach (Color color in Parameters.COLOR_LIST)
        {
            colorTaken.Add(new KeyValuePair<Color, bool>(color, false));
        }

        // If we have previously chosen some attacks and entered the fighting scene
        if (AttackLists.pressedButtonNameList.Count > 0 && !upgradePopUp.activeSelf)
        {
            playButton.interactable = true;
            foreach (string buttonName in AttackLists.pressedButtonNameList)
            {
                Button actualButton = GameObject.Find(buttonName).GetComponent<Button>();
                OnPressButtonHandling(actualButton);
            }

            foreach (Button attackButton in attackButtons)
            {
                if (!AttackLists.pressedButtonNameList.Contains(attackButton.name))
                {
                    attackButton.interactable = false;
                }
            }
        }
    }

    void CreateAttackButton(int i, BossAttack attack)
    {
        GameObject instantiatedButtonPrefab = Instantiate(attackButtonPrefab);
        instantiatedButtonPrefab.transform.SetParent(transform, false);
        instantiatedButtonPrefab.name = "Attack" + (i) + " Object";
        Transform buttonObject = UnityUtils.RecursiveFind(instantiatedButtonPrefab.transform, "Attack0");
        buttonObject.name = "Attack" + (i);
        Button button = buttonObject.GetComponent<Button>();
        attackButtons.Add(button);
        button.onClick.AddListener(() => this.EnablePopUp(button));
        Transform imageTransform = button.transform.Find("Image");
        Image image = imageTransform.GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>(AttackLists.GetAssetString(attack.name));
    }

    public void ChooseUpgrade(BossAttack attack)
    {
        if (!AttackLists.allAttacks.Contains(attack))
        {
            AttackLists.allAttacks.Add(attack);
        }
        CreateAttackButton(attackButtons.Count, attack);
        DisableUpgradePopUp();
    }

    public void DisableButtons()
    {
        foreach (Button attackButton in attackButtons)
        {
            attackButton.interactable = false;
        }
        this.playButton.interactable = false;
    }

    public void EnablePopUp(Button button)
    {
        Image panelImage = button.transform.parent.Find("Panel").GetComponent<Image>();
        if (panelImage.color == whiteNoAlpha && clickedButtons.Count < 3)
        {
            DisableButtons();

            attackPopUp.SetActive(true);

            int buttonNumber = Int32.Parse(button.transform.name.Replace("Attack", ""));
            BossAttack attack = AttackLists.allAttacks[buttonNumber];
            UnityUtils.RecursiveFind(attackPopUp.transform, "AttackName").GetComponent<Text>().text = attack.name;

            Text text = UnityUtils.RecursiveFind(attackPopUp.transform, "AttackInfoText").GetComponent<Text>();
            text.text = "Damage: " + attack.damage + "\n";
            playAttackPreview(attackPopUp.transform, attack, "BossAttackPreview");
            Button yesButton = UnityUtils.RecursiveFind(attackPopUp.transform, "YesButton").GetComponent<Button>();
            yesButton.onClick.AddListener(() => { OnPressButtonHandling(button); });
        }
        else
        {
            OnPressButtonHandling(button);
        }

    }

    private void enableAttackButtons()
    {
        foreach (Button attackButton in attackButtons)
        {
            attackButton.interactable = true;
        }
    }

    public void DisableUpgradePopUp()
    {
        UnityUtils.RecursiveFind(upgradePopUp.transform, "Attack1Button").GetComponent<Button>().onClick.RemoveAllListeners();
        UnityUtils.RecursiveFind(upgradePopUp.transform, "Attack2Button").GetComponent<Button>().onClick.RemoveAllListeners();
        enableAttackButtons();
        upgradePopUp.SetActive(false);
    }

    public void DisablePopUp()
    {
        UnityUtils.RecursiveFind(attackPopUp.transform, "YesButton").GetComponent<Button>().onClick.RemoveAllListeners();
        enableAttackButtons();

        attackPopUp.SetActive(false);
    }

    public void OnPressButtonHandling(Button button)
    {
        UnityUtils.RecursiveFind(attackPopUp.transform, "YesButton").GetComponent<Button>().onClick.RemoveAllListeners();
        attackPopUp.SetActive(false);

        int buttonNumber = Int32.Parse(button.transform.name.Replace("Attack", ""));

        Image panelImage = button.transform.parent.Find("Panel").GetComponent<Image>();

        if (panelImage.color == whiteNoAlpha && clickedButtons.Count < 3)
        {

            foreach (KeyValuePair<Color, bool> keyVal in colorTaken)
            {
                if (!keyVal.Value)
                {
                    panelImage.color = keyVal.Key;
                    int index = colorTaken.IndexOf(keyVal);
                    colorTaken.RemoveAt(index);
                    colorTaken.Insert(index, new KeyValuePair<Color, bool>(keyVal.Key, true));
                    AttackLists.chosenAttacksArray[index] = AttackLists.allAttacks[buttonNumber];
                    break;
                }
            }
            clickedButtons.Add(button);

            if (!upgradePopUp.activeSelf)
            {
                enableAttackButtons();
            }

            if (!playButton.interactable && clickedButtons.Count == 3)
            {
                playButton.interactable = true;
                foreach (Button attackButton in attackButtons)
                {
                    if (!clickedButtons.Contains(attackButton))
                    {
                        attackButton.interactable = false;
                    }
                }
            }

        }
        else if (panelImage.color != whiteNoAlpha)
        {

            foreach (KeyValuePair<Color, bool> keyVal in colorTaken)
            {
                if (keyVal.Key == panelImage.color)
                {
                    int index = colorTaken.IndexOf(keyVal);
                    colorTaken.RemoveAt(index);
                    colorTaken.Insert(index, new KeyValuePair<Color, bool>(keyVal.Key, false));
                    AttackLists.chosenAttacksArray[index] = null;
                    break;
                }
            }

            clickedButtons[clickedButtons.IndexOf(button)].transform.parent.Find("Panel").GetComponent<Image>().color = new Color(1, 1, 1, 0);
            clickedButtons.Remove(button);

            if (clickedButtons.Count < 3)
            {
                enableAttackButtons();
                playButton.interactable = false;
            }

        }
    }

    public void SaveButtonColors()
    {
        AttackLists.pressedButtonNameList = new List<string>();
        foreach (Button button in clickedButtons)
        {
            AttackLists.pressedButtonNameList.Add(button.transform.name);
        }
    }

    private void playAttackPreview(Transform popupTransform, BossAttack attack, string previewName)
    {
        Transform bossAttackPreview = UnityUtils.RecursiveFind(popupTransform, previewName);
        bossAttackPreview.GetComponent<PlayAttackOnBoss>().setAttack(attack);
    }
}

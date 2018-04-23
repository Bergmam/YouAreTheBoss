using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AttackButtons : MonoBehaviour {

	List<Button> clickedButtons = new List<Button>();
	List<KeyValuePair<Color, bool>> colorTaken = new List<KeyValuePair<Color, bool>>();

	Button playButton;

	GameObject attackPopUp;
	GameObject upgradePopUp;
	List<Button> relevantButtons = new List<Button>();

	Color whiteNoAlpha = new Color(1, 1, 1, 0);

	GameObject attackButtonPrefab;
	int highestButtonIndex = 0;

	void Start() {
		int buttonIndex = AttackLists.allAttacks.Count - 1;
		attackButtonPrefab = Resources.Load("Prefabs/AttackButton", typeof(GameObject)) as GameObject;
		for(int i = 1; i <= AttackLists.allAttacks.Count; i++){
			CreateAttackButton(i);
		}

		for(int i = 0; i < highestButtonIndex; i++) {
			relevantButtons.Add(GameObject.Find("Attack " + i).GetComponent<Button>());
		}

		// Handle pop-up for adding extra moves
		upgradePopUp = GameObject.Find("UpgradePopUp");

		// If we are on a certain wave number and have upgrade attacks left that we can choose from
		if (WaveNumber.waveNumber % Parameters.ATTACK_UPGRADE_WAVE_NUMBER == 0 && WaveNumber.waveNumber != 0 && AttackLists.choseableUpgradeAttacks.Count >= 2) {
			// Create list of all available indexes be able to remove indices instead of the actual attacks
			List<int> choseableIndices = new List<int>();
			int i = 0;
			foreach(BossAttack attack in AttackLists.choseableUpgradeAttacks){
				choseableIndices.Add(i);
				i++;
			}
			DisableButtons();

			GameObject attackButton1 = UnityUtils.RecursiveFind(upgradePopUp.transform, "Attack1Button").gameObject;
			GameObject attackButton2 = UnityUtils.RecursiveFind(upgradePopUp.transform, "Attack2Button").gameObject;
			GameObject attackText1 = UnityUtils.RecursiveFind(upgradePopUp.transform, "Attack1Name").gameObject;
			GameObject attackText2 = UnityUtils.RecursiveFind(upgradePopUp.transform, "Attack2Name").gameObject;

			// Get index of random attack from choseableUpgradeAttacks
			int randomNumber = UnityEngine.Random.Range(0, choseableIndices.Count);
			int index = choseableIndices[randomNumber];
			// Remove from indexarray so that we do not pick the same attack twice
			choseableIndices.Remove(randomNumber);
			attackText1.GetComponent<Text>().text = AttackLists.choseableUpgradeAttacks[index].name;
			SetButtonText(attackButton1.transform.Find("Text").GetComponent<Text>(), AttackLists.choseableUpgradeAttacks[index]);
			attackButton1.GetComponent<Button>().onClick.AddListener(() => {
				ChooseUpgrade(AttackLists.choseableUpgradeAttacks[index]);
			});

			// Get new index of attack,
			randomNumber = UnityEngine.Random.Range(0, choseableIndices.Count);
			index = choseableIndices[randomNumber];
			attackText2.GetComponent<Text>().text = AttackLists.choseableUpgradeAttacks[index].name;
			SetButtonText(attackButton2.transform.Find("Text").GetComponent<Text>(), AttackLists.choseableUpgradeAttacks[index]);
			attackButton2.GetComponent<Button>().onClick.AddListener(() => {
				ChooseUpgrade(AttackLists.choseableUpgradeAttacks[index]);
			});

		} else {
			upgradePopUp.SetActive(false);
		}

		attackPopUp = GameObject.Find("AttackPopUp");
		attackPopUp.SetActive(false);

		playButton = GameObject.Find("PlayButton").GetComponent<Button>();
		relevantButtons.Add(playButton);
		playButton.interactable = false;

		foreach(Color color in Parameters.COLOR_LIST){
			colorTaken.Add(new KeyValuePair<Color, bool>(color, false));
		}

		// If we have previously chosen some attacks and entered the fighting scene
		if (AttackLists.pressedButtonNameList.Count > 0 && !upgradePopUp.activeSelf){
			playButton.interactable = true;
			foreach(string buttonName in AttackLists.pressedButtonNameList)
			{
				Button actualButton = GameObject.Find(buttonName).GetComponent<Button>();
				OnPressButtonHandling(actualButton);
			}
		}
	}

	void CreateAttackButton(int i) {
		GameObject instantiatedButtonPrefab = Instantiate(attackButtonPrefab, transform);
		instantiatedButtonPrefab.name = "Attack " + (i-1) + " Object";
		GameObject buttonObject = UnityUtils.RecursiveContains(instantiatedButtonPrefab.transform, "Attack")[1];
		buttonObject.name = "Attack " + (i-1);
		Button button = buttonObject.GetComponent<Button>();
		button.onClick.AddListener(() => gameObject.GetComponent<AttackButtons>().EnablePopUp(button));
		button.transform.Find("Text").GetComponent<Text>().text = i.ToString();
		highestButtonIndex = i;
	}

	public void ChooseUpgrade(BossAttack attack){
		if (!AttackLists.allAttacks.Contains(attack)) {
			AttackLists.allAttacks.Add(attack);
		}
		CreateAttackButton(highestButtonIndex + 1);
		DisableUpgradePopUp();
	}

	public void DisableButtons() {
		foreach(Button relevantButton in relevantButtons) {
			relevantButton.interactable = false;
		}
	}

	public void EnablePopUp(Button button) {
		Image panelImage = button.transform.parent.Find("Panel").GetComponent<Image>();
		if (panelImage.color == whiteNoAlpha && clickedButtons.Count < 3) {
			DisableButtons();

			attackPopUp.SetActive(true);

			int buttonNumber = Int32.Parse(button.transform.name.Replace("Attack ", ""));
			BossAttack attack = AttackLists.allAttacks[buttonNumber];
			UnityUtils.RecursiveFind(attackPopUp.transform, "AttackName").GetComponent<Text>().text = attack.name;

			Text text = UnityUtils.RecursiveFind(attackPopUp.transform,  "AttackInfoText").GetComponent<Text>();
			SetButtonText(text, attack);

			Button yesButton = UnityUtils.RecursiveFind(attackPopUp.transform, "YesButton").GetComponent<Button>();
			yesButton.onClick.AddListener( () => { OnPressButtonHandling(button); });
		} else {
			OnPressButtonHandling(button);
		}
		
	}

	public void DisableUpgradePopUp() {
		UnityUtils.RecursiveFind(upgradePopUp.transform, "Attack1Button").GetComponent<Button>().onClick.RemoveAllListeners();
		UnityUtils.RecursiveFind(upgradePopUp.transform, "Attack2Button").GetComponent<Button>().onClick.RemoveAllListeners();
		foreach(Button relevantButton in relevantButtons) {
			if (relevantButton.name != "PlayButton") {
				relevantButton.interactable = true;
			}
		}
		upgradePopUp.SetActive(false);
	}

	public void DisablePopUp() {
		UnityUtils.RecursiveFind(attackPopUp.transform, "YesButton").GetComponent<Button>().onClick.RemoveAllListeners();
		foreach(Button relevantButton in relevantButtons) {
			if (relevantButton.name != "PlayButton") {
				relevantButton.interactable = true;
			}
		}

		attackPopUp.SetActive(false);
	}

	public void OnPressButtonHandling(Button button) {
		UnityUtils.RecursiveFind(attackPopUp.transform, "YesButton").GetComponent<Button>().onClick.RemoveAllListeners();
		attackPopUp.SetActive(false);

		int buttonNumber = Int32.Parse(button.transform.name.Replace("Attack ", ""));

		Image panelImage = button.transform.parent.Find("Panel").GetComponent<Image>();

		if (panelImage.color == whiteNoAlpha && clickedButtons.Count < 3) {
		
			foreach(KeyValuePair<Color, bool> keyVal in colorTaken){
				if(!keyVal.Value){
					panelImage.color = keyVal.Key;
					int index = colorTaken.IndexOf(keyVal);
					colorTaken.RemoveAt(index);
					colorTaken.Insert(index, new KeyValuePair<Color, bool>(keyVal.Key, true));
					AttackLists.chosenAttacksArray[index] = AttackLists.allAttacks[buttonNumber];
					break;
				}
			}
			clickedButtons.Add(button);

			if (!playButton.interactable && clickedButtons.Count == 3) {
				playButton.interactable = true;
			}
			
			if (!upgradePopUp.activeSelf) {
				foreach(Button relevantButton in relevantButtons) {
					if (relevantButton.name != "PlayButton") {
						relevantButton.interactable = true;
					}
				}
			}
			

		} else if (panelImage.color != whiteNoAlpha) {

			foreach(KeyValuePair<Color, bool> keyVal in colorTaken){
				if (keyVal.Key == panelImage.color) {
					int index = colorTaken.IndexOf(keyVal);
					colorTaken.RemoveAt(index);
					colorTaken.Insert(index, new KeyValuePair<Color, bool>(keyVal.Key, false));
					AttackLists.chosenAttacksArray[index] = null;
					break;
				}
			}

			clickedButtons[clickedButtons.IndexOf(button)].transform.parent.Find("Panel").GetComponent<Image>().color = new Color(1, 1, 1, 0);
			clickedButtons.Remove(button);

			if (clickedButtons.Count < 3) {
				playButton.interactable = false;
			}

		}
	}

	public void SaveButtonColors() {
		AttackLists.pressedButtonNameList = new List<string>();
		foreach(Button button in clickedButtons){
			AttackLists.pressedButtonNameList.Add(button.transform.name);
		}
	}

	public void SetButtonText(Text text, BossAttack attack) {
		if (attack.frequency >= Parameters.SLOW_ATTACK_LIMIT) {
				text.text = "Type: Active\n";
		} else {
			text.text = "Type: Passive\n";
		}
		text.text += 
			"Damage: " + attack.damage + "\n" +
			"Cooldown: " + attack.frequency + "\n" + 
			"Width: " + attack.angle * 2 + " degrees\n"; 

		if (attack.closeRadius == 0 && attack.farRadius < 2.5f) {
			text.text += "Reach: Melee";
		} else if (attack.closeRadius >= 2.0f) {
			text.text += "Reach: Only ranged";
		} else if (attack.closeRadius <= 1.0f && attack.farRadius >= 4.0f) {
			text.text += "Reach: Melee & Ranged";
		} else {
			text.text += "Reach: Medium";
		}
	}
}

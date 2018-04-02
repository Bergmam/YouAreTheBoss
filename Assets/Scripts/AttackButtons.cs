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

	void Start() {
		attackPopUp = GameObject.Find("AttackPopUp");
		attackPopUp.SetActive(false);


		playButton = GameObject.Find("PlayButton").GetComponent<Button>();
		playButton.interactable = false;

		foreach(Color color in Parameters.COLOR_LIST){
			colorTaken.Add(new KeyValuePair<Color, bool>(color, false));
		}

		// If we have previously chosen some attacks and entered the fighting scene
		if (AttackLists.pressedButtonNameList.Count > 0){
			playButton.interactable = true;
			foreach(string buttonName in AttackLists.pressedButtonNameList)
			{
				Button actualButton = GameObject.Find(buttonName).GetComponent<Button>();
				OnPressButtonHandling(actualButton);
			}
		}
	}

	public void EnablePopUp(Button button) {
		if (button.image.color == Color.white && clickedButtons.Count < 3) {
			attackPopUp.SetActive(true);

			int buttonNumber = Int32.Parse(button.transform.name.Replace("Attack ", ""));
			BossAttack attack = AttackLists.allAttacks[buttonNumber];
			UnityUtils.RecursiveFind(attackPopUp.transform, "AttackName").GetComponent<Text>().text = attack.name;

			Text text = UnityUtils.RecursiveFind(attackPopUp.transform,  "AttackInfoText").GetComponent<Text>();
			text.text = 
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

			Button yesButton = UnityUtils.RecursiveFind(attackPopUp.transform, "YesButton").GetComponent<Button>();
			yesButton.onClick.AddListener( () => { OnPressButtonHandling(button); });
		} else {
			OnPressButtonHandling(button);
		}
		
	}

	public void DisablePopUp() {
		UnityUtils.RecursiveFind(attackPopUp.transform, "YesButton").GetComponent<Button>().onClick.RemoveAllListeners();
		attackPopUp.SetActive(false);
	}

	public void OnPressButtonHandling(Button button) {
		UnityUtils.RecursiveFind(attackPopUp.transform, "YesButton").GetComponent<Button>().onClick.RemoveAllListeners();
		attackPopUp.SetActive(false);

		int buttonNumber = Int32.Parse(button.transform.name.Replace("Attack ", ""));

		if (button.image.color == Color.white && clickedButtons.Count < 3) {
		
			foreach(KeyValuePair<Color, bool> keyVal in colorTaken){
				if(!keyVal.Value){
					button.image.color = keyVal.Key;
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

		} else if (button.image.color != Color.white) {

			foreach(KeyValuePair<Color, bool> keyVal in colorTaken){
				if (keyVal.Key == button.image.color) {
					int index = colorTaken.IndexOf(keyVal);
					colorTaken.RemoveAt(index);
					colorTaken.Insert(index, new KeyValuePair<Color, bool>(keyVal.Key, false));
					AttackLists.chosenAttacksArray[index] = null;
					break;
				}
			}

			clickedButtons[clickedButtons.IndexOf(button)].image.color = Color.white;
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
}

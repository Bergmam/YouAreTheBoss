using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AttackButtons : MonoBehaviour {

	List<Button> clickedButtons = new List<Button>();
	List<KeyValuePair<Color, bool>> colorTaken = new List<KeyValuePair<Color, bool>>();

	Button playButton;

	void Start() {
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

	public void OnPressButtonHandling(Button button) {

		int buttonNumber = Int32.Parse(button.transform.name.Replace("Attack ", ""));

		if (button.image.color == Color.white && clickedButtons.Count < 3) {

			AttackLists.chosenAttacksArray[buttonNumber] = AttackLists.allAttacks[buttonNumber];

			foreach(KeyValuePair<Color, bool> keyVal in colorTaken){
				if(!keyVal.Value){
					button.image.color = keyVal.Key;
					int index = colorTaken.IndexOf(keyVal);
					colorTaken.RemoveAt(index);
					colorTaken.Insert(index, new KeyValuePair<Color, bool>(keyVal.Key, true));
					break;
				}
			}
			clickedButtons.Add(button);

			if (!playButton.interactable && clickedButtons.Count == 3) {
				playButton.interactable = true;
			}

		} else if (button.image.color != Color.white) {

			AttackLists.chosenAttacksArray[buttonNumber] = null;

			foreach(KeyValuePair<Color, bool> keyVal in colorTaken){
				if (keyVal.Key == button.image.color) {
					int index = colorTaken.IndexOf(keyVal);
					colorTaken.RemoveAt(index);
					colorTaken.Insert(index, new KeyValuePair<Color, bool>(keyVal.Key, false));
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

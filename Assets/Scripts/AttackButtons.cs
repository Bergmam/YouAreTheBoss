using System.Collections;
using System.Collections.Generic;
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
	}

	public void OnPressAttackLists(int attackIndex) {
		AttackLists.chosenAttacks.Add(AttackLists.allAttacks[attackIndex]);

		if (AttackLists.chosenAttacks.Count > 3) {
			AttackLists.chosenAttacks.RemoveAt(0);
		}
	}

	public void OnPressButtonHandling(Button button) {

		if (button.image.color == Color.white && clickedButtons.Count < 3) {

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
}

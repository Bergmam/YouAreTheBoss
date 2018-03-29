using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButtons : MonoBehaviour {

	List<Button> clickedButtons = new List<Button>();

	public void OnPressAttackLists(int attackIndex) {
		AttackLists.chosenAttacks.Add(AttackLists.allAttacks[attackIndex]);

		if (AttackLists.chosenAttacks.Count > 3) {
			AttackLists.chosenAttacks.RemoveAt(0);
		}
	}

	public void OnPressButtonHandling(Button button) {

		if (button.image.color == Color.white && clickedButtons.Count < 3) {
			
			button.image.color = Color.yellow;
			clickedButtons.Add(button);

		} else if (button.image.color == Color.yellow) {

			clickedButtons[clickedButtons.IndexOf(button)].image.color = Color.white;
			clickedButtons.Remove(button);

		}
	}
}

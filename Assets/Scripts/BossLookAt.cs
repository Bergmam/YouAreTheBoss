using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class BossLookAt : MonoBehaviour {

	void Update ()
	{
		if (PauseMenuController.gamePaused)
		{
			return;
		}
		
		Vector3 touchPos = Vector3.zero;
		if (Input.touchCount > 0)
		{
			/*
			TODO: Improve this script so that quick taps at specific spots don't rotate the boss.

			Right now the stuff below doesn't work very well unless you are already holding a finger
			on the screen when you pickup or use items.
			I don't think anything else than the fact that we only look at the first touch,
			`Input.GetTouch(0)`, really does anything. It is probably because a quick tap lasts
			for several updates.
			*/

			// Only the first finger can rotate the boss.
			Touch touch = Input.GetTouch(0);

			if (!(touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved))
			{
				return;
			}

			// Don't rotate boss when user is picking up an item.
			RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
			bool overItemPickup = hitInfo && hitInfo.transform.name.Contains("Pickup");

			// Don't rotate boss if the user is using an item.
			PointerEventData pointer = new PointerEventData(EventSystem.current);
			pointer.position = touch.position;
			List<RaycastResult> raycastResults = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointer, raycastResults);
			bool overItemButton = raycastResults.Any(res => res.gameObject.transform.name.Contains("Item"));

			if (!overItemButton && !overItemPickup)
			{
				touchPos = Camera.main.ScreenToWorldPoint(touch.position);
			}
		}
		else
		{
			touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}

		if (touchPos.y > Parameters.ARENA_BOTTOM && !touchPos.Equals (Vector3.zero))
		{
			transform.rotation = Quaternion.LookRotation (Vector3.forward, touchPos - transform.position);
		}
	}
}

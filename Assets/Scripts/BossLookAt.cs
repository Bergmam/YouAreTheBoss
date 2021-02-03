using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class BossLookAt : MonoBehaviour {

	private bool preventBossRotation;
	private List<GameObject> objectsForDeletion;

	void Awake()
	{
		this.objectsForDeletion = new List<GameObject>();
	}

	void Update ()
	{
		if (PauseMenuController.gamePaused)
		{
			return;
		}
		
		Vector3 touchPos = Vector3.zero;
		if (Input.touchCount > 0)
		{
			// Only the first finger can rotate the boss.
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Ended)
			{
				this.preventBossRotation = false;
				destroyObjectsMarkedForDeletion();
				return;
			}

			if (this.preventBossRotation)
			{
				destroyObjectsMarkedForDeletion();
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

			if (touch.phase == TouchPhase.Began && (overItemPickup || overItemButton))
			{
				this.preventBossRotation = true;
				destroyObjectsMarkedForDeletion();
				return;
			}

			if (touch.phase != TouchPhase.Began && touch.phase != TouchPhase.Moved)
			{
				return;
			}

			touchPos = Camera.main.ScreenToWorldPoint(touch.position);
		}
		else if (Input.mousePresent)
		{
			touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}

		if (touchPos.y > Parameters.ARENA_BOTTOM && !touchPos.Equals (Vector3.zero))
		{
			transform.rotation = Quaternion.LookRotation (Vector3.forward, touchPos - transform.position);
		}

		destroyObjectsMarkedForDeletion();
	}

	private void destroyObjectsMarkedForDeletion()
	{
		// If we destroy item pickups immediately when they are pressed, we won't be able to ignore
		// taps on them when rotating the boss.
		foreach (GameObject obj in this.objectsForDeletion)
		{
			Destroy(obj);
		}
		this.objectsForDeletion.Clear();
	}

	// Add a game object that will be destroyed at the end of the next update.
	public void MarkObjectForDeletion(GameObject obj)
	{
		this.objectsForDeletion.Add(obj);
	}
}

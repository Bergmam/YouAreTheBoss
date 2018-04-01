using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackListInit : MonoBehaviour {

	void Awake () {
		AttackLists.allAttacks.Add(new BossAttack("WideMelee", 30, 0, 2.0f, 50, 1.2f));
        AttackLists.allAttacks.Add(new BossAttack("NarrowMeleeAndRanged", 5, 0, 5.0f, 300, 5.0f));
        AttackLists.allAttacks.Add(new BossAttack("WideRanged", 30, 2.0f, 5.0f, 50, 1.2f));
        AttackLists.allAttacks.Add(new BossAttack("360NoScope", 180f, 0, 1.5f, 10, 0.3f));
	}
	
}
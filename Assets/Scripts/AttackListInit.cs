using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackListInit : MonoBehaviour {

	void Awake () {

        // Ranged: 2.0 -> 5.0
        // Melee 0 -> 2.0 

        // BossAttack Constructor:
        // Name, Angle, CloseRadius, FarRadius, Damage, Frequency
        if (!AttackLists.listsInitalized) {
            AttackLists.listsInitalized = true;
            
            AttackLists.allAttacks.Add(new BossAttack("Melee", 30, 0, 2.0f, 50, 1.2f));
            AttackLists.allAttacks.Add(new BossAttack("Sniper", 5, 0, 5.0f, 300, 5.0f));
            AttackLists.allAttacks.Add(new BossAttack("Ranged", 30, 2.0f, 5.0f, 50, 1.2f));
            AttackLists.allAttacks.Add(new BossAttack("Small Donut", 180, 0, 1.5f, 10, 0.3f));
        
            AttackLists.choseableUpgradeAttacks.Add(new BossAttack("Big Donut", 180, 2.0f, 5.0f, 10, 0.3f));
            AttackLists.choseableUpgradeAttacks.Add(new BossAttack("Small Donut Nuke", 180, 0, 1.5f, 400, 10.0f));
            AttackLists.choseableUpgradeAttacks.Add(new BossAttack("Big Donut Nuke", 180, 2.5f, 5.0f, 400, 10));
            AttackLists.choseableUpgradeAttacks.Add(new BossAttack("Thin Melee", 15, 0, 2.0f, 80, 1.2f));
        }
		
    }
	
}
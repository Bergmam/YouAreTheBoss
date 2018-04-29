using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackLists {

    public static bool listsInitalized = false;
    public static List<BossAttack> allAttacks = new List<BossAttack>();
    
    public static BossAttack[] chosenAttacksArray = new BossAttack[Parameters.NUMBER_OF_ATTACKS];

    public static List<string> pressedButtonNameList = new List<string>();

    public static List<BossAttack> choseableUpgradeAttacks = new List<BossAttack>();

    public static List<BossAttack> chosenUpgradeAttacks = new List<BossAttack>();

}

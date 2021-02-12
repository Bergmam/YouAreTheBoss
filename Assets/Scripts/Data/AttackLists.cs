using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackLists
{
    public static void ResetUpgradedAttacks()
    {
        chosenUpgradeAttacks.ForEach(attack => chooseableUpgradeAttacks.Add(attack));
        chosenUpgradeAttacks.Clear();
        selectedAttacks = new BossAttack[Parameters.NUMBER_OF_ATTACKS];
    }

    public static void ChooseUpgradeAttack(BossAttack attack)
    {
        chosenUpgradeAttacks.Add(attack);
        chooseableUpgradeAttacks.Remove(attack);
    }

    public static BossAttack GetSelectableAttack(int index)
    {
        // Make indexes larger than the number of default attacks start from zero but pick from upgrade attacks instead.
        if (index < defaultAttacks.Count)
        {
            return defaultAttacks[index];
        }
        else
        {
            return chosenUpgradeAttacks[index - defaultAttacks.Count];
        }
    }

    internal static int GetNumberOfSelectableAttacks()
    {
        return defaultAttacks.Count + chosenUpgradeAttacks.Count;
    }

    public static List<BossAttack> defaultAttacks = new List<BossAttack>()
    {
        new BossAttack("Melee", 30, RangeLevel.MELE, RangeLevel.MELE, 35, 1.0f, false),
        new BossAttack("Sniper", 5, RangeLevel.MELE, RangeLevel.LONG, 300, 4.8f, false),
        new BossAttack("Ranged", 30, RangeLevel.MID, RangeLevel.MID, 35, 1.0f, false)
        // new BossAttack("Small Donut", 120, RangeLevel.MELE, RangeLevel.MELE, 12, 0.4f, false)
    };

    public static BossAttack[] selectedAttacks = new BossAttack[Parameters.NUMBER_OF_ATTACKS];

    public static List<BossAttack> chooseableUpgradeAttacks = new List<BossAttack>()
    {
        new BossAttack("Shooter", 2, RangeLevel.MELE, RangeLevel.LONG, 100, 0.5f, true),
        new BossAttack("Lazer", 2, RangeLevel.MELE, RangeLevel.LONG, 7.5f, 0.1f, false),
        new BossAttack("Touch of Death", 2, RangeLevel.LONG, RangeLevel.LONG, 300, 0.08f, false),
        new BossAttack("Small Donut Nuke", 180, RangeLevel.MELE, RangeLevel.MELE, 400, 4.8f, false),
        // new BossAttack("Big Donut Nuke", 180, RangeLevel.MID, RangeLevel.MID, 400, 9, false),
        // new BossAttack("Thin Melee", 15, RangeLevel.MELE, RangeLevel.MELE, 70, 1.0f, false),
        // new BossAttack("Swipe", 75, RangeLevel.MELE, RangeLevel.MELE, 100, 1.8f, false),
        // new BossAttack("Ranged Donut", 120, RangeLevel.LONG, RangeLevel.LONG, 13, 0.3f, false),
        new BossAttack("Shotgun", 20, RangeLevel.MELE, RangeLevel.MID, 200, 2.51f, false),
        new BossAttack("Meka Nuke", 180, RangeLevel.MELE, RangeLevel.LONG, 400, 20.0f, false),
        // new BossAttack("Thin Range", 12.5f, RangeLevel.MID, RangeLevel.MID, 70, 1.0f, false),
        // new BossAttack("Ranged Swipe", 75, RangeLevel.MID, RangeLevel.MID, 100, 1.9f, false)
    };

    public static List<BossAttack> chosenUpgradeAttacks = new List<BossAttack>();
    private static Dictionary<string, string> nameAssetNameDictionary = new Dictionary<string, string>()
    {
        {"Melee", "Art/starsigns/aquarius"},
        {"Sniper", "Art/starsigns/aries"},
        {"Ranged", "Art/starsigns/capricorn"},
        {"Lazer", "Art/starsigns/crab"},
        {"Big Donut", "Art/starsigns/gemini"},
        {"Small Donut Nuke", "Art/starsigns/lion"},
        {"Big Donut Nuke", "Art/starsigns/libra"},
        {"Thin Melee", "Art/starsigns/pisces"},
        {"Touch of Death", "Art/starsigns/sagititarius"},
        {"Small Donut", "Art/starsigns/scorpio"},
        {"Ranged Donut", "Art/starsigns/taurus"},
        // {"Swipe", "Art/starsigns/virgo"},
        {"Shotgun", "Art/starsigns/akkh"},
        {"Meka Nuke", "Art/starsigns/cat"},
        {"Thin Range", "Art/starsigns/eye"},
        {"Ranged Swipe", "Art/starsigns/scarab"},
        {"Shooter", "Art/starsigns/virgo"}
    };

    public static string GetAssetString(string name)
    {
        return nameAssetNameDictionary[name];
    }

}

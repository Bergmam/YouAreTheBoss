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
        new BossAttack("Melee", 30, 0.0f, 0.4f, 35, 1.0f),
        new BossAttack("Sniper", 5, 0.0f, 1.0f, 300, 4.8f),
        new BossAttack("Ranged", 30, 0.4f, 1.0f, 35, 1.0f),
        new BossAttack("Small Donut", 120, 0.0f, 0.4f, 12, 0.4f),
        new BossAttack("Lazer", 2, 0.0f, 1.0f, 10, 0.1f),
        new BossAttack("Touch of Death", 2, 0.9f, 1.0f, 300, 0.08f),
        new BossAttack("Small Donut Nuke", 180, 0.0f, 0.3f, 400, 6.0f),
        new BossAttack("Big Donut Nuke", 180, 0.5f, 0.7f, 400, 9),
        new BossAttack("Thin Melee", 15, 0.0f, 0.4f, 70, 1.0f),
        new BossAttack("Swipe", 75, 0.0f, 0.45f, 100, 1.8f),
        new BossAttack("Ranged Donut", 120, 0.5f, 0.7f, 18, 0.3f)
    };

    public static BossAttack[] selectedAttacks = new BossAttack[Parameters.NUMBER_OF_ATTACKS];

    public static List<BossAttack> chooseableUpgradeAttacks = new List<BossAttack>()
    {
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
        {"Swipe", "Art/starsigns/virgo"}
    };

    public static string GetAssetString(string name)
    {
        return nameAssetNameDictionary[name];
    }

}

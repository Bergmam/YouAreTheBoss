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

    public static List<BossAttack> defaultAttacks = new List<BossAttack>(){
        new BossAttack("Melee", 30, 0.0f, 0.4f, 50, 1.2f),
        new BossAttack("Sniper", 5, 0.0f, 1.0f, 300, 5.0f),
        new BossAttack("Ranged", 30, 0.4f, 1.0f, 50, 1.2f),
        new BossAttack("Small Donut", 180, 0.0f, 0.3f, 10, 0.3f)
    };

    public static BossAttack[] selectedAttacks = new BossAttack[Parameters.NUMBER_OF_ATTACKS];

    public static List<BossAttack> chooseableUpgradeAttacks = new List<BossAttack>(){
        new BossAttack("Big Donut", 180, 0.4f, 1.0f, 10, 0.3f),
        new BossAttack("Small Donut Nuke", 180, 0.0f, 0.3f, 400, 10.0f),
        new BossAttack("Big Donut Nuke", 180, 0.5f, 1.0f, 400, 10),
        new BossAttack("Thin Melee", 15, 0.0f, 0.4f, 80, 1.2f)
    };

    public static List<BossAttack> chosenUpgradeAttacks = new List<BossAttack>();
    private static Dictionary<string, string> nameAssetNameDictionary = new Dictionary<string, string>()
    {
        {"Melee", "Art/zodiac/aquarius-water-container-symbol"},
        {"Sniper", "Art/zodiac/aries-symbol"},
        {"Ranged", "Art/zodiac/capricorn-1"},
        {"Small Donut", "Art/zodiac/crab-symbol-for-zodiac-cancer-sign"},
        {"Big Donut", "Art/zodiac/gemini-male-twins-zodiac-sign-symbol"},
        {"Small Donut Nuke", "Art/zodiac/leo-zodiac-symbol-of-lion-head-from-side-view"},
        {"Big Donut Nuke", "Art/zodiac/libra-balanced-scale-symbol"},
        {"Thin Melee", "Art/zodiac/pisces-astrological-sign-symbol-of-two-fishes"},
        {"sagittarius", "Art/zodiac/sagittarius-zodiac-symbol"},
        {"scorpio", "Art/zodiac/scorpio"},
        {"taurus", "Art/zodiac/taurus-astrological-sign-symbol"},
        {"virgo", "Art/zodiac/virgo-zodiac-symbol"}
    };

    public static string GetAssetString(string name)
    {
        return nameAssetNameDictionary[name];
    }

}

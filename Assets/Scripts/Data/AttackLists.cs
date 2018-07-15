using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AttackLists
{

    public static bool listsInitalized = false;
    public static List<BossAttack> allAttacks = new List<BossAttack>(){
        new BossAttack("Melee", 30, 0, 2.0f, 50, 1.2f),
        new BossAttack("Sniper", 5, 0, 5.0f, 300, 5.0f),
        new BossAttack("Ranged", 30, 2.0f, 5.0f, 50, 1.2f),
        new BossAttack("Small Donut", 180, 0, 1.5f, 10, 0.3f)
    };

    public static BossAttack[] chosenAttacksArray = new BossAttack[Parameters.NUMBER_OF_ATTACKS];

    public static List<BossAttack> choseableUpgradeAttacks = new List<BossAttack>(){
        new BossAttack("Big Donut", 180, 2.0f, 5.0f, 10, 0.3f),
        new BossAttack("Small Donut Nuke", 180, 0, 1.5f, 400, 10.0f),
        new BossAttack("Big Donut Nuke", 180, 2.5f, 5.0f, 400, 10),
        new BossAttack("Thin Melee", 15, 0, 2.0f, 80, 1.2f)
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

    public static string GetAssetString(int index)
    {
        return nameAssetNameDictionary.ToList()[index].Value;
    }

}

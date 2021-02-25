using UnityEngine;

public class ItemData
{
    public Sprite Sprite;
    public int PercentHealthToHeal;
    public int InvunerableSeconds;
    public int FreezeEnemiesSeconds;
    public bool ResetCooldowns;
    public float PushBackForce; // A negative value means pull enemies towards the boss, positive means push them away.
}

using UnityEngine;

public abstract class EnemyAction : ScriptableObject
{
    public abstract void Execute(GameObject enemyObject);
}

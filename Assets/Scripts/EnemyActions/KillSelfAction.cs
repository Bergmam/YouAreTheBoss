using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Action/Kill Self Action")]
public class KillSelfAction : EnemyAction
{
    public override void Execute(GameObject enemyObject)
    {
        Destroy(enemyObject);
    }
}

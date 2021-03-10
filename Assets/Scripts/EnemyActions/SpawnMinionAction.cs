using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Action/Spawn Minion Action")]
public class SpawnMinionAction : EnemyAction
{
    public EnemySettings Minon;

    public override void Execute(GameObject enemyObject)
    {
        EnemySettings minion = Instantiate(this.Minon);
        RadialPosition spawnPosition = RotationUtils.XYToRadialPos(enemyObject.transform.position);
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        minion.predefinedPosition = true;
        minion.spawnAngle = spawnPosition.GetAngle();
        minion.spawnRadius = spawnPosition.GetRadius();
        GameObject.FindObjectOfType<EnemySpawner>().InstantiateEnemyPrefab(minion);
    }
}

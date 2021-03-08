using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Action/Transform Action")]
public class TransformAction : EnemyAction
{
    public EnemySettings TransformTo;

    public override void Execute(GameObject enemyObject)
    {
        EnemySettings transformTo = Instantiate(this.TransformTo);
        RadialPosition spawnPosition = RotationUtils.XYToRadialPos(enemyObject.transform.position);
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        transformTo.Health = enemy.MaxHealth;
        transformTo.SpawnHealth = enemy.Health;
        transformTo.predefinedPosition = true;
        transformTo.spawnAngle = spawnPosition.GetAngle();
        transformTo.spawnRadius = spawnPosition.GetRadius();
        GameObject.FindObjectOfType<EnemySpawner>().InstantiateEnemyPrefab(transformTo);
        Destroy(enemyObject);
    }
}

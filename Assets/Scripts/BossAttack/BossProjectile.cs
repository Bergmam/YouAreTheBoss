using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float Speed;
    public BossAttack Attack;

    void Update()
    {
        RadialPosition radialPos = RotationUtils.XYToRadialPos(this.transform.position);

        // Destroy boss projectiles when they exit the screen.
        if (radialPos.GetRadius() > Parameters.DESTROY_BOSS_PROJECTILES_RADIUS)
        {
            Destroy(this.gameObject);
        }

        radialPos.SetRadius(radialPos.GetRadius() + this.Speed);
        this.transform.position = RotationUtils.RadialPosToXY(radialPos);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.transform.parent.gameObject.GetComponent<Enemy>();
        if (enemy?.EnemyType == EnemyType.ENEMY || enemy?.EnemyType == EnemyType.MINION)
        {
            enemy.applyDamageTo(this.Attack.damage);
            Destroy(this.gameObject);
        }
        else if (other.transform.tag == "enemy" && enemy?.EnemyType != EnemyType.PROJECTILE)
        {
            Destroy(this.gameObject);
        }
    }
}

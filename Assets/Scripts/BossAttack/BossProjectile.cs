using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float Speed;
    public BossAttack Attack;

    public Vector3 origin = Vector3.zero;

    void Update()
    {
        RadialPosition radialPos = RotationUtils.XYToRadialPos(this.transform.position);

        // Destroy boss projectiles when they exit the screen.
        if (radialPos.GetRadius() > Parameters.DESTROY_BOSS_PROJECTILES_RADIUS)
        {
            Destroy(this.gameObject);
        }

        // Make projectiles travel outwards from the origin.
        Vector3 positionRelativeToOrigin = this.transform.position - this.origin;
        RadialPosition radialPosRelativeToOrigin = RotationUtils.XYToRadialPos(positionRelativeToOrigin);
        radialPosRelativeToOrigin.SetRadius(radialPosRelativeToOrigin.GetRadius() + (this.Speed * Time.deltaTime));
        this.transform.position = this.origin + RotationUtils.RadialPosToXY(radialPosRelativeToOrigin);
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

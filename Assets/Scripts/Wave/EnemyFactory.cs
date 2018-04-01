using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory {

	public static StatsHolder SlowEnemy ()
	{
		return new StatsHolder("Slow Enemy", 0.3f, 6.0f, 1.0f, 300.0f, 2.0f, Color.black);
	}

    public static StatsHolder Rotator(bool clockwise)
    {
        StatsHolder stats = new StatsHolder("Rotator", 0.7f, 2.5f, 1.0f, 100.0f, 0.7f, Color.blue);
		if (clockwise)
        {
            stats.angularSpeed = -50f;
        }
        else
        {
            stats.angularSpeed = 50f;
        }
		return stats;
    }

    public static StatsHolder FastEnemy()
    {
        return new StatsHolder("Fast Enemy", 3.0f, 1.0f, 2.0f, 50.0f, 0.5f, Color.yellow);
    }

    public static StatsHolder RangedCirclingEnemy(bool clockwise)
    {
        StatsHolder stats = new StatsHolder("Shooting Shark", 3.0f, 1.0f, 2.5f, 50.0f, 1.2f, new Color(1.0f, 0.0f, 1.0f, 1.0f));
		if(clockwise)
		{
			stats.circlingSpeed = -30f;
		} 
		else 
		{
			stats.circlingSpeed = 30f;
		}
		return stats;
    }

    public static StatsHolder StandardEnemy()
    {
        return new StatsHolder("Standard Enemy", 1.0f, 2.5f, 1.0f, 100.0f, 1.0f, Color.white);
    }

    public static StatsHolder Projectile(float damage, float spawnRadius, float spawnAngle)
    {
        StatsHolder stats = new StatsHolder(
			"Projectile",
			Parameters.PROJECTILE_SPEED,
			damage,
			Parameters.PROJECTILE_RANGE,
			1.0f, //Health of projectile does not matter since they are invunerable.
			Parameters.PROJECTILE_SCALE,
			Parameters.PROJECTILE_COLOR
		);
		stats.selfDestruct = true;
		stats.invunerable = true;
		stats.SetRadialSpawnPosition(spawnAngle,spawnRadius);
		return stats;
    }

	public static StatsHolder Minion(){
		StatsHolder stats = new StatsHolder("Minion", 2.0f, 0.5f, 1.0f, 0.5f, 0.5f, Color.black);
		return stats;
	}
}

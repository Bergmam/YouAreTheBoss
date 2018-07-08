using System.Collections.Generic;
using System.Linq;

public class SubWave
{

    private List<StatsHolder> enemies;

    public SubWave()
    {
        this.enemies = new List<StatsHolder>();
    }

    public SubWave(List<StatsHolder> enemies)
    {
        this.enemies = enemies;
    }

    public List<StatsHolder> GetEnemies()
    {
        return this.enemies;
    }

    public void AddEnemy(StatsHolder enemy)
    {
        if (this.enemies != null)
        {
            this.enemies.Add(enemy);
        }
    }

    public int RequiredKillEnemyCount()
    {
        int count = 0;
        foreach (StatsHolder enemy in enemies)
        {
            if (enemy.requiredKill)
            {
                count++;
            }
        }
        return count;
    }

    public void ScaleSubWaveDamage(float scaleFactor)
    {
        foreach (StatsHolder enemy in this.enemies)
        {
            enemy.Damage *= scaleFactor;
        }
    }

    public void ScaleSubWaveHealth(float scaleFactor)
    {
        foreach (StatsHolder enemy in this.enemies)
        {
            enemy.Health *= scaleFactor;
        }
    }

    public void ScaleSubWaveSpeed(float scaleFactor)
    {
        foreach (StatsHolder enemy in this.enemies)
        {
            enemy.MovementSpeed *= scaleFactor;
        }
    }

    public void ScaleSubWaveAngularSpeed(float scaleFactor)
    {
        foreach (StatsHolder enemy in this.enemies)
        {
            enemy.angularSpeed *= scaleFactor;
        }
    }

    public void ScaleSubWaveSize(float scaleFactor)
    {
        foreach (StatsHolder enemy in this.enemies)
        {
            enemy.Scale *= scaleFactor;
        }
    }

    public void MultiplyNumberOfEnemies(int times)
    {
        List<StatsHolder> newEnemiesList = new List<StatsHolder>();
        foreach (StatsHolder enemy in this.enemies)
        {
            newEnemiesList.Add(enemy);

            for (int i = 1; i < times; i++)
            {
                StatsHolder clone = enemy.Clone();
                newEnemiesList.Add(enemy.Clone());
            }

        }
        this.enemies = newEnemiesList;
    }

    public void Merge(SubWave otherwave)
    {
        this.enemies.AddRange(otherwave.enemies);
    }

    public void SpreadOut()
    {
        float randomAngle = UnityEngine.Random.value * 360;
        foreach (StatsHolder enemy in this.enemies)
        {
            enemy.spawnAngle = randomAngle;
            enemy.predefinedPosition = true;
            randomAngle = (randomAngle + (360 / this.enemies.Count)) % 360;
        }
    }

    public void Shift(float degrees)
    {
        foreach (StatsHolder enemy in this.enemies)
        {
            enemy.predefinedPosition = true;
            enemy.spawnAngle = (enemy.spawnAngle + degrees) % 360;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        SubWave other = (SubWave)obj;
        return this.enemies.All(other.enemies.Contains) && this.enemies.Count == other.enemies.Count;
    }

    public override int GetHashCode()
    {
        int hashCode = 0;
        foreach (StatsHolder enemy in this.enemies)
        {
            hashCode += (enemy.GetHashCode()) % 153;
        }
        return hashCode;
    }

    public SubWave Clone()
    {
        SubWave clone = new SubWave();
        foreach (StatsHolder enemy in enemies)
        {
            clone.AddEnemy(enemy.Clone());
        }
        return clone;
    }
}

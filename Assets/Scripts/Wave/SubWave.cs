using System.Collections.Generic;


public class SubWave {

	private List<StatsHolder> enemies;
	private float duration;

	public SubWave(float duration){
		this.enemies = new List<StatsHolder> ();
		this.duration = duration;
	}

	public SubWave(List<StatsHolder> enemies, float duration){
		this.enemies = enemies;
		this.duration = duration;
	}

	public List<StatsHolder> GetEnemies(){
		return this.enemies;
	}

	public float GetDuration(){
		return this.duration;
	}

	public void AddEnemy(StatsHolder enemy){
		if (this.enemies != null) {
			this.enemies.Add (enemy);
		}
	}

	public int RequiredKillEnemyCount()
	{
		int count = 0;
		foreach(StatsHolder enemy in enemies){
			if(enemy.requiredKill){
				count++;
			}
		}
		return count;
	}

	public void ScaleSubWaveDamage(float scaleFactor) {
		foreach(StatsHolder enemy in this.enemies) {
			enemy.Damage *= scaleFactor;
		}
	}

	public void ScaleSubWaveHealth(float scaleFactor) {
		foreach(StatsHolder enemy in this.enemies) {
			enemy.Health *= scaleFactor;
		}
	}

	public void ScaleSubWaveSpeed(float scaleFactor) {
		foreach(StatsHolder enemy in this.enemies) {
			enemy.MovementSpeed *= scaleFactor;
		}
	}

	public void ScaleSubWaveAngularSpeed(float scaleFactor) {
		foreach(StatsHolder enemy in this.enemies) {
			enemy.angularSpeed *= scaleFactor;
		}
	}

	public void ScaleSubWaveSize(float scaleFactor) {
		foreach(StatsHolder enemy in this.enemies) {
			enemy.Scale *= scaleFactor;
		}
	}

	public void MultiplyNumberOfEnemies(int times){
		List<StatsHolder> newEnemiesList = new List<StatsHolder>();
		foreach(StatsHolder enemy in this.enemies) {
			newEnemiesList.Add(enemy);

			for(int i = 1; i < times; i++){
				StatsHolder clone = enemy.Clone();
				clone.spawnAngle = (clone.spawnAngle + 15) % 360;
				newEnemiesList.Add(enemy.Clone());
			}
			
		}
		this.enemies = newEnemiesList;
	}

	public void Merge(SubWave otherwave) {
		this.enemies.AddRange(otherwave.enemies);
	}

	public void SpreadOut() {
		float randomAngle = UnityEngine.Random.value * 360;
		foreach(StatsHolder enemy in this.enemies) {
			enemy.spawnAngle = randomAngle;
			UnityEngine.MonoBehaviour.print(enemy.spawnAngle);
			randomAngle = (randomAngle + (360 / this.enemies.Count)) % 360;
		}
	}
}

using System.Collections.Generic;


public class SubWave {

	private List<StatsHolder> enemies;
	private float duration;

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

}

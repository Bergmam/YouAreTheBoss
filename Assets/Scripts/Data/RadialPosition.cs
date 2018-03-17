using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialPosition {

	private float radius;
	private float angle;

	public RadialPosition (float radius, float angle) {
		this.radius = radius;
		this.angle = angle;
	}

	public float GetRadius() {
		return this.radius;
	}

	public float GetAngle () {
		return this.angle;
	}

}

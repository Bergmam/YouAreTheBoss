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

	public void SetRadius (float radius) {
		this.radius = radius;
	}

	public void SetAngle (float angle) {
		angle = RotationUtils.MakePositiveAngle (angle);
		this.angle = angle;
	}

	public void AddRadius (float addedRadius) {
		this.radius += addedRadius;
	}

	public void AddAngle (float addedAngle) {
		this.angle += addedAngle;
		this.angle = RotationUtils.MakePositiveAngle (angle);
	}

	public string ToString(){
		return "(angle = " + angle + ", radius = " + radius + ")";
	}
}

using UnityEngine;
using System.Collections;

public class LightIntensity : MonoBehaviour {

	public float duration = 2.0F;
	public Light lt;

	void Start() {
		lt = GetComponent<Light>();
		lt.intensity = 0.0f;
	}

	public void changeIntensity(float intensity) {
		if (intensity != 1.0f) {
			if (lt.intensity > intensity) {
				lt.intensity -= Time.deltaTime*duration;
				Debug.Log("LIGHT LOWERED");
			}
		} else {
			if (lt.intensity < intensity) {
				lt.intensity += Time.deltaTime*duration;
				Debug.Log("LIGHT HEIGHTENED");
			}
		}
	}

	public void changeIntensityHelpOnly(float intensity) {
		lt.intensity = intensity;
	}
}

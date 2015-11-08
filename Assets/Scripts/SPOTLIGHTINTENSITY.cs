using UnityEngine;
using System.Collections;

public class SPOTLIGHTINTENSITY : MonoBehaviour {
	public Light _lt;

	// Use this for initialization
	void Start () {
		_lt = GetComponent<Light>();
		_lt.intensity = 0;
	
	}
	
	public void changeSpotlightIntensity(float intensity) {
		_lt.intensity = intensity;
	}
}

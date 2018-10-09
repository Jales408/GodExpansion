using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour {
    public int splashNumber;

    // Use this for initialization
    public ParticleSystem syst;
	void Start () {
        syst.Emit(splashNumber);
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
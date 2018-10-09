using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Accroche : MonoBehaviour {
    public Transform positionCenter;
	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if (other.gameObject.tag != "PlayerBody") return;
        other.gameObject.GetComponentInParent<PlayerMovement>().hold(positionCenter);
    }
}

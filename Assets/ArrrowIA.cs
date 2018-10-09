using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrrowIA : MonoBehaviour {

    public float speed;
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.left * Time.deltaTime * speed;
	}
}

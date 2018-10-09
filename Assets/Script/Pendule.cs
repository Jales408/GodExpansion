using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendule : MonoBehaviour {

    public Transform positionPoids;

    public float vitesse;
    public float firstAngle;
    public float maxAngle;
    public float acceleration;
    private float delta =1f;
    private float angleUtile; //angle maximal en valeur absolue lorsque le pendule est au plus bas(a 180f) et minimal en haut (0f), il est negatif et positif selon le cote du balancement
	// Use this for initialization
	void Start () {
        transform.rotation = Quaternion.Euler(0f, 0f, firstAngle);
    }
	
	// Update is called once per frame
	void Update () {

        vitesse = -delta * Time.deltaTime * acceleration * Mathf.Pow(0.8f+((Mathf.Abs(angleUtile) - maxAngle) / (180f - maxAngle)),3);
        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z+vitesse*Time.deltaTime);
        angleUtile = transform.rotation.eulerAngles.z - 180f;
        if ((Mathf.Abs(angleUtile) < maxAngle) && Mathf.Sign(vitesse) != Mathf.Sign(angleUtile))
        {
            delta = -delta;
        }
    }
}

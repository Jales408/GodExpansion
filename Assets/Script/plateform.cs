using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plateform : MonoBehaviour {
    public int hauteur;
    public int direction;
    public float vitesse;
    public float ralenti;
    public float startAngle;
    public GameObject platform;
	// Use this for initialization
	void Start () {
        Vector3 rotationVector = transform.rotation.eulerAngles;
        rotationVector.z = startAngle;
        transform.rotation = Quaternion.Euler(rotationVector);
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 rotationVector = transform.rotation.eulerAngles;
        float rotationZ = rotationVector.z;

        float ralentissement = ((hauteur * ralenti - Mathf.Abs( 180f - rotationZ)));

        rotationZ += direction * ralentissement * vitesse * Time.deltaTime;
        rotationVector.z = rotationZ;
        transform.rotation = Quaternion.Euler(rotationVector);

        Vector3 pRotationVector = platform.transform.rotation.eulerAngles;
        float pRotationZ = pRotationVector.z;

        pRotationZ = 180f - rotationZ;
        if (pRotationZ > 0)
        {
            pRotationVector.z = pRotationZ/2;
        }
        else
        {
            pRotationVector.z = (360f+pRotationZ/2);
        }
        platform.transform.rotation = Quaternion.Euler( pRotationVector);

        if (direction>0 && rotationZ >hauteur+180 || direction<0 && rotationZ<180-hauteur) flip();

    }

    private void flip()
    {
        direction = -direction;
    }
}

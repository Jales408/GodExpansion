using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    public bool isPoisonous;
    public bool isoneTimeTrap;
    public bool isStatic;

    public Vector3 direction;
    public float speed;

    public int damage;

    public int poisonDamage;
    public int poisonTime;

    public int hit()
    {
        
        if (isoneTimeTrap)
        {
            Destroy(this.gameObject);
        }
        return damage;
    }

    public int[] poison()
    {
        if (isPoisonous)
        {
            int[] poison = new int[2];
            poison[0] = poisonDamage;
            poison[1] = poisonTime;
            return poison;
        }
        return null;
    }

    void Update () {

        transform.position += direction * Time.deltaTime * speed;
    }
}

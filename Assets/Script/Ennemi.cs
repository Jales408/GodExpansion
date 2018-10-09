using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemi : MonoBehaviour {

    public int health;
    public float speed;
    public GameObject bloodEffect;
    public GameObject bloodEffectPos;
    public bool poisonous;
    public int damage;
    public int poisonDamage;
    public int poisonTime;

    private Animator anim;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (health <= 0) return;
        if (other.gameObject.tag != "Player" || !other.gameObject.GetComponentInParent<Animator>().GetBool("Attacking")) return;
        int damage = other.gameObject.GetComponentInParent<PlayerMovement>().hit();
        Instantiate(bloodEffect, GetComponent<Collider2D>().Distance(other).pointA, Quaternion.identity);
        Vector3 directionForce = other.gameObject.transform.position - transform.position;
        //m_Rigidbody2D.AddForce(new Vector2(-direction.x * m_JumpSideForce, -direction.y * m_JumpForce));
        health -= damage;
        if (health <= 0)
        {
            anim.SetBool("die", true);
        }
    }

    public int hit()
    {
        return damage;
    }

    public void block()
    {
        //Instantiate effect
        anim.SetBool("Attacking", false);
    }

    public int[] poison()
    {
        if (poisonous)
        {
            int[] poison = new int[2];
            poison[0] = poisonDamage;
            poison[1] = poisonTime;
            return poison;
        }
        return null;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update () {

        //transform.Translate(Vector2.left * speed * Time.deltaTime);
	}
}

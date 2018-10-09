using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour {



    private PlayerMovement c_movement;  //Reference to PlayerMovement script
    private bool isJumping; //To determine if the player is jumping
    private bool isAttacking;
    private bool isHeavyAttacking;
    private bool isDefending;

    public int health;

    public GameObject PoisonEffect;
    public GameObject bloodEffect;

    

    void Awake()
    {
        //References
        c_movement = GetComponent<PlayerMovement>();
	}

    void Update()
    {
        //If he is not jumping...
        if (!isJumping)
        {
            //See if button is pressed...
            isJumping = CrossPlatformInputManager.GetButtonDown("Jump");
        }
        if (!isAttacking)
        {
            isAttacking = Input.GetKey(KeyCode.F);
            
            //=CrossPlatformInputManager.GetButtonDown("Fire1");
        }
        if (!isHeavyAttacking)
        {
            isHeavyAttacking = Input.GetKey(KeyCode.G);
            //=CrossPlatformInputManager.GetButtonDown("Fire1");
        }
        if (health <= 0)
        {
            die();
        }
           isDefending = Input.GetKey(KeyCode.E);
    }


    private void FixedUpdate()
    {
        if (isAttacking)
        {
            c_movement.Attack(0);
            isAttacking = false;
        }
        if (isHeavyAttacking)
        {
            c_movement.Attack(1);
            isHeavyAttacking = false;
        }
        //Get horizontal axis
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        //Call movement function in PlayerMovement
        c_movement.Move(h, isJumping,isDefending);
        //Reset
        isJumping = false;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (health <= 0) return;
        if (!(other.gameObject.GetComponentInParent<Animator>() == null && other.gameObject.tag=="Trap"))
        {
            if (other.gameObject.layer != 8 || !other.gameObject.GetComponentInParent<Animator>().GetBool("Attacking")) return;
        }
        if (!isDefending || other.gameObject.tag == "Trap")
        {
            int damage;
            int[] poison;
            Instantiate(bloodEffect, GetComponent<Collider2D>().Distance(other).pointA, Quaternion.identity);
            Vector3 directionForce = other.gameObject.transform.position - transform.position;
            c_movement.addHitForce(directionForce / directionForce.magnitude);

            if (other.gameObject.tag == "Trap")
            {
                damage = other.gameObject.GetComponentInParent<Trap>().hit();
                poison = other.gameObject.GetComponentInParent<Trap>().poison();
            }
            else
            {
                damage = other.gameObject.GetComponentInParent<Ennemi>().hit();
                poison = other.gameObject.GetComponentInParent<Ennemi>().poison();
            }
                health -= damage;
                
            
            if (poison != null)
            {
                TakePoison(poison[0], poison[1]);
            }
        }
        else
        {
                other.gameObject.GetComponentInParent<Ennemi>().block();
        }
        if (health <= 0)
        {
            //anim.SetBool("die", true);
        }
    }

    public void TakePoison(int poisonDamage, int poisonTime)
    {
        StartCoroutine(DelayPoison(poisonDamage, poisonTime - 1));
    }

    IEnumerator DelayPoison(int poisonDamage, int poisonTime)
    {

        yield return new WaitForSeconds(1);
        if (poisonTime > 0)
        {
            poisonTime--;
            StartCoroutine(DelayPoison(poisonDamage, poisonTime));
        }
        health -= poisonDamage;
        //EmitPoisonparticles
    }


    private void die()
    {
        Destroy(gameObject);
    }
}

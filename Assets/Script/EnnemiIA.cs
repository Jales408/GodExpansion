using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiIA : MonoBehaviour {

    private float timeBtwAttack;
    private float timeAttacking;
    public float starttimeAttacking;
    public float startTimeBtwAttack;

    public Transform detectPos;

    public float detectRange;
    public LayerMask LayerPlayer;
    public bool facingRight;

    public Ennemi ennemi;

    private Animator anim;

    void Awake()
    {
        //References
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ennemi.health <= 0) return;

        if (timeAttacking>0)
        { 
            timeAttacking -= Time.deltaTime; 
        }
        else if(timeBtwAttack > 0){
            anim.SetBool("Attacking", false);
            timeBtwAttack -= Time.deltaTime;
        }
        else if (timeBtwAttack <= 0)
        {
            this.Attack();
            timeBtwAttack = startTimeBtwAttack;
            timeAttacking = starttimeAttacking;
        }


    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        transform.Rotate(0f, 180f, 0f);
    }

    public void Attack()
    {
        Collider2D[] ennemiesInRange = Physics2D.OverlapCircleAll(detectPos.position, detectRange, LayerPlayer);
        if (ennemiesInRange.Length>0)
        {
            if (ennemiesInRange[0].transform.position.x > transform.position.x && facingRight)
                Flip();
            else if (ennemiesInRange[0].transform.position.x < transform.position.x && !facingRight)
                Flip();
            anim.SetBool("Attacking", true);
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(detectPos.position, detectRange);
    }
}

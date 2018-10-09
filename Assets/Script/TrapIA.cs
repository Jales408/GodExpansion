using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapIA : MonoBehaviour {

    private float timeBtwAttack;
    private float timeAttacking;
    public float starttimeAttacking;
    public float startTimeBtwAttack;
    public bool isAttackCycling;
    public bool hasRange;
    public bool isMoving;
    public float speed;

    public Transform detectPos;
    public float detectRange;
    public Vector3[] stepPoint;
    

    public LayerMask LayerPlayer;
    private Animator anim;
    private int currentTarget = 0;
    private float minimimumDistance = 0.2f;
    private Vector3 direction;


    void Awake()
    {
        //References
        anim = GetComponent<Animator>();
        if (stepPoint.Length == 0) {
            isMoving = false;
        }
        if (anim==null)
        {
            isAttackCycling = false;
        }

        for (int i = 0; i < stepPoint.Length; i++)
        {
            stepPoint[i] += this.transform.position;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAttackCycling)
        {
            if (timeAttacking > 0)
            {
                timeAttacking -= Time.deltaTime;
            }
            else if (timeBtwAttack > 0)
            {
                anim.SetBool("Attacking", false);
                timeBtwAttack -= Time.deltaTime;
            }
            else if (timeBtwAttack <= 0)
            {
                this.Attack();

            }
        }
        if (isMoving)
        {
            direction = transform.position - stepPoint[currentTarget];
            if (direction.magnitude < minimimumDistance)
            {
                currentTarget++;
                if (currentTarget > stepPoint.Length-1)
                {
                    currentTarget = 0;
                }
            }
            else
            {
                transform.position -= direction/direction.magnitude*speed*Time.deltaTime;
            }
        }

    }

    public void Attack()
    {
        if(hasRange){

            Collider2D[] ennemiesInRange = Physics2D.OverlapCircleAll(detectPos.position, detectRange, LayerPlayer);
            if (ennemiesInRange.Length > 0)
            {
                anim.SetBool("Attacking", true);
                timeBtwAttack = startTimeBtwAttack;
                timeAttacking = starttimeAttacking;
            }
        }
        else
        {
            anim.SetBool("Attacking", true);
            timeBtwAttack = startTimeBtwAttack;
            timeAttacking = starttimeAttacking;
        }
    }


    void OnDrawGizmosSelected()
    {
        if (hasRange)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(detectPos.position, detectRange);
        }
    }
}

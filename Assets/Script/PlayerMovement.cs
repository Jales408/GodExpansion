using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    private float m_MaxSpeed = 20f;                    // The fastest the player can travel in the x axis.
    [SerializeField]
    private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
    [SerializeField]
    private float m_JumpSideForce = 200f;                  // Amount of force added when the player jumps.
    [SerializeField]
    private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField]
    private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

                   // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = 1.5f;                // Radius of the overlap circle to determine if grounded
    private bool m_Grounded; // Whether or not the player is grounded.
    private bool m_Walled;
    private bool m_Climb;

    private bool isClimbing;
    private bool isAttacking;
    private bool isInputable = true;
    private bool isDefending;
    private bool isRushing;
    private bool canHold = true;
    private bool continueAttacking;
    private int numberofAttack = 0;
    private int typeAttack = 0;
    private Transform PositionObjetTenu;
    private bool isHolding = false;
    private Collider2D[] colliders;


    private Animator m_Anim;                           // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    
    public bool m_FacingLeft = true; // For determining which way the player is currently facing.
    

    public Transform m_GroundCheck;
    public Transform m_WallCheck;
    public float timeBtwInput;
    public float timeBtwAttack;
    public Transform swordPos;
    public int damage;
    public float attackSpeed;
    public float timeBtwRush;
    public float m_velocityWalled;
    public float m_velocityClimbing;
    public float m_acc;
    public float m_airControl;
    public float m_canHold;
    public GameObject BlockEffect;
    public float largeur;
    public float hauteur;
    public float hauteurClimb;
    public float climbingTime;



    void Awake () {
        // Setting up references.
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        colliders = new Collider2D[2];
    }

    public bool isGrounded()
    {
        return (m_Grounded);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        m_Grounded = false;
        m_Walled = false;
        m_Climb = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        int max = Physics2D.OverlapCircleNonAlloc(m_GroundCheck.position, k_GroundedRadius, colliders, m_WhatIsGround);
        for (int i = 0; i < max; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }
       if (!m_Grounded)
        {
            max = Physics2D.OverlapAreaNonAlloc(m_WallCheck.position+Vector3.left*largeur+Vector3.up*hauteur, m_WallCheck.position + Vector3.right * largeur + Vector3.down * hauteur, colliders, m_WhatIsGround);
            for (int i = 0; i < max; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Walled = true;
            }
            if (m_Walled)
            {
                m_Climb = true;
                max = Physics2D.OverlapAreaNonAlloc(m_WallCheck.position + Vector3.left * largeur + Vector3.up * (hauteur+hauteurClimb), m_WallCheck.position + Vector3.right * largeur + Vector3.up *hauteur, colliders, m_WhatIsGround);
                for (int i = 0; i < max; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                        m_Climb = false;
                    Debug.Log("canClimb");
                }
            }
        }

        m_Anim.SetBool("Ground", m_Grounded);

        //Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

    }

    public void Move(float _move, bool _jump, bool _defend)
    {
        //only control the player if grounded or airControl is turned on
        if (isHolding)
        {
            m_Rigidbody2D.velocity = new Vector2(0f, 0f);
            transform.position = PositionObjetTenu.position;

        }
        else if (isClimbing)
        {
            m_Rigidbody2D.velocity = new Vector2(m_velocityClimbing, m_velocityClimbing);
            Debug.Log(m_Rigidbody2D.velocity);
            return;
        }
        else if(m_Climb && m_Rigidbody2D.velocity.y <= -m_velocityWalled)
        {
            Debug.Log("startClimbing");
            isClimbing = true;
            StartCoroutine(DelayChangingBool(isClimbing,climbingTime));

        }
        else
        {
            m_Anim.SetBool("Holding", false);
            if (m_Walled && (_move < 0 && m_FacingLeft || _move > 0 && !m_FacingLeft))
            {
                if (m_Rigidbody2D.velocity.y <= -m_velocityWalled)
                {
                    m_Anim.SetBool("Walling", true);
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_velocityWalled);
                    Instantiate(BlockEffect, swordPos.position, Quaternion.identity);
                }
            }
            else if ((m_Grounded || m_AirControl) && !isAttacking)
            {
                m_Anim.SetBool("Walling", false);
                if (!(m_Grounded && _defend))
                {
                    // The Speed animator parameter is set to the absolute value of the horizontal input.

                    if (Mathf.Abs(m_Rigidbody2D.velocity.x) < m_MaxSpeed || Mathf.Sign(_move) != Mathf.Sign(m_Rigidbody2D.velocity.x))
                    {
                        m_Anim.SetFloat("Speed", Mathf.Abs(m_Rigidbody2D.velocity.x));
                        float acceleration = m_acc;
                        if (m_Grounded) acceleration =m_acc*2;
                        m_Rigidbody2D.AddForce(new Vector2(_move * acceleration, 0f), ForceMode2D.Impulse);
                    }
                }

            }

            //Verifier l'attaque
            if (isAttacking) 
            {
                //Au sol et Attaquant : Communiquez le type d'attaque a l'animator
                m_Anim.SetBool("Attacking", true);
                m_Anim.SetInteger("TypeAttack", typeAttack);
                m_Anim.SetInteger("numberofAttack", numberofAttack);
            }
            else
            {
                m_Anim.SetBool("Attacking", false);
            }
        }


            // If the input is moving the player right and the player is facing left...
            if (_move < 0 && !m_FacingLeft)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (_move > 0 && m_FacingLeft)
            {
                // ... flip the player.
                Flip();
            }
        

        

        //Verifier le saut
        if (_jump && !isAttacking)
        {
            if (m_Grounded)
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
            else if (isHolding)
            {
                
                m_Anim.SetBool("Holding", false);
                StartCoroutine(DelayHolding());
                StartCoroutine(DelayAirControl());
                isHolding = false;
                if (!m_FacingLeft)
                {
                    m_Rigidbody2D.AddForce(new Vector2(m_JumpSideForce, m_JumpForce));
                }
                else
                {
                    m_Rigidbody2D.AddForce(new Vector2(-m_JumpSideForce, m_JumpForce));
                }
            }
            else if(m_Walled && m_Rigidbody2D.velocity.y <= m_velocityWalled)
            {
                m_Anim.SetBool("Walling", false);
                StartCoroutine(DelayAirControl());
                if (m_FacingLeft)
                {
                    m_Rigidbody2D.AddForce(new Vector2(m_JumpSideForce, m_JumpForce));
                }
                else
                {
                    m_Rigidbody2D.AddForce(new Vector2(-m_JumpSideForce, m_JumpForce));
                }
                

            }
        }

        

        m_Anim.SetBool("Defending", _defend);

    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingLeft = !m_FacingLeft;

        // Multiply the player's x local scale by -1.
        transform.Rotate(0f, 180f, 0f);
    }

    public void Attack(int typeAttack)
    {
        if (!isInputable||!isGrounded()) { return; }
        if (!isAttacking)
        {
            isInputable = false;
            StartCoroutine(DelayNextInput());
            isAttacking = true;
            continueAttacking = false;
            numberofAttack = 0;
            this.typeAttack = typeAttack;
            if (typeAttack == 1)
            {
                
                numberofAttack = 2;
                StartCoroutine(DelayNextRush());

            }
            StartCoroutine(DelayNextAttack());
        }
        else
        {
            if (!(this.typeAttack == 1 && typeAttack == 0))
            {
                continueAttacking = true;
                this.typeAttack = typeAttack;
            }
        }
    }

    IEnumerator DelayNextInput()
    {
        if(isAttacking==false)
        {
            yield return new WaitForSeconds(timeBtwInput*2);
        }
        else
        {
            yield return new WaitForSeconds(timeBtwInput + typeAttack * 0.1f);
        }
        
        isInputable = true;
    }

    IEnumerator DelayNextRush()
    {
        yield return new WaitForSeconds(timeBtwRush);
        isRushing = true;
        if (!m_FacingLeft)
        {
            m_Rigidbody2D.AddForce(new Vector2(m_JumpSideForce, 0f));
        }
        else
        {
            m_Rigidbody2D.AddForce(new Vector2(-m_JumpSideForce, 0f));
        }
    }

    IEnumerator DelayAirControl()
    {
        m_AirControl = false;
        yield return new WaitForSeconds(m_airControl);
        m_AirControl= true;
    }

    IEnumerator DelayHolding()
    {
        canHold = false;
        yield return new WaitForSeconds(m_canHold);
        canHold = true;
    }

    IEnumerator DelayChangingBool(bool toChange, float time)
    {
        yield return new WaitForSeconds(time);
        isClimbing = false;
    }

    IEnumerator DelayNextAttack()
    {
        yield return new WaitForSeconds(timeBtwAttack+typeAttack*0.5f);
        if (continueAttacking && !(numberofAttack == 3 || numberofAttack==2 && typeAttack==1))
        {
            if (typeAttack == 0)
            {
                Debug.Log("continue simple");
                numberofAttack = numberofAttack + 1;
                continueAttacking = false;
                StartCoroutine(DelayNextAttack());
            }
            else if (typeAttack == 1){
                Debug.Log("continue heavy");
                numberofAttack = numberofAttack + 1;
                continueAttacking = false;
                StartCoroutine(DelayNextAttack());
                StartCoroutine(DelayNextRush());
            }
            
        }
        else
        {
            Debug.Log("stop");
            numberofAttack = 0;
            isAttacking = false;
        }
        isRushing = false;
        isInputable = false;
        StartCoroutine(DelayNextInput());
        
    }

    public int hit()
    {
        return damage;
    }

    public void addHitForce(Vector3 direction)
    {
        m_Rigidbody2D.velocity = new Vector2(0f,0f);
        m_Rigidbody2D.AddForce(new Vector2(-direction.x * m_JumpSideForce, -direction.y * m_JumpForce/2));
    }

    public void hold(Transform positionObjet)
    {
        if (canHold)
        {
            PositionObjetTenu = positionObjet;
            isHolding = true;
        }
        Debug.Log("Hold");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(m_WallCheck.position, new Vector3(largeur*2f, hauteur*2f, 0f));
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(m_WallCheck.position+Vector3.up*(hauteur+hauteurClimb), new Vector3(largeur*2f, hauteurClimb*2f, 0f));
        
    }
}

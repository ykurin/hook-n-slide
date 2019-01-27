using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [SerializeField] private float wallJumpForce = 400f;
    [SerializeField] private float wallJumpPushForce = 400f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private LayerMask m_WhatIsWall;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_WallCheck;

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    const float k_WallTouchRadius = .2f;
    public bool m_Grounded;            // Whether or not the player is grounded.
    public bool m_TouchingWall;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = Physics2D.OverlapCircle(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        if (m_Grounded && !wasGrounded)
            OnLandEvent.Invoke();

        m_TouchingWall = m_Grounded ? false :
            Physics2D.OverlapCircle(m_WallCheck.position, k_GroundedRadius, m_WhatIsWall);
    }

    public void Move(float move, bool jump)
    {
        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = targetVelocity;//Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }

        if (m_TouchingWall && jump)
        {
            WallJump();
        }
    }

     private void WallJump()
     {
         m_Rigidbody2D.AddForce (new Vector2 (wallJumpPushForce, wallJumpForce));
     }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

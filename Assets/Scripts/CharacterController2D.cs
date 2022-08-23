using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    #region Private Member Variables
    //How much to smooth out the movement
    [Tooltip("This slider allows us to smooth the players movement")]
    [Range(0, 0.3f)] [SerializeField] private float m_movementSmoothing = 0.05f;
    //Whether or not a player can steer while jumping
    [SerializeField] private bool m_airControl = false;
    //Whether or not a player can stick to slopes
    [SerializeField] private bool m_stickToSlopes = true;
    //A mask determining what is ground to the character
    [SerializeField] private LayerMask m_whatIsGround;
    //A mask determining what is ladder to the character
    [SerializeField] private LayerMask m_whatIsLadder;
    //A position marking where to check if the player is grounded
    [SerializeField] private Transform m_groundCheck;
    //A position marking where the starting point of ladder ray is
    [SerializeField] private Transform m_ladderCheck;
    //A position marking where to check if the player is not hitting anything
    [SerializeField] private Transform m_frontCheck;
    //Radius of the overlap circle to determine if grounded
    [SerializeField] private float m_groundedRadius = 0.05f;
    //Radius of the overlap circle to determine if front is blocked
    [SerializeField] private float m_frontCheckRadius = 0.05f;
    //Length of the ray beneath controller
    [SerializeField] private float m_groundRayLength = 0.2f;
    //Length of the ray above controller
    [SerializeField] private float m_ladderRayLength = 0.5f;
    //Gravity
    private float m_originalGravityScale;
    private bool jumped = false;
    private bool dashed = false;
    private float x;
    #endregion
    #region Public Variables
    [Header("Events")]
    public UnityEvent OnLandEvent;
    [Header("In game Checks")]
    public bool isGrounded;
    public bool isClimbing, isFrontBlocked, isFacingRight;
    public Rigidbody2D rigid;
    public Animator anim;
    #endregion
    #region Public Properties

    #endregion

    #region Unity Event Functions
    // Start is called before the first frame update
    void Awake()
    {
        //The first moment and object with this script is active in the scene
        //regardless of whether or not the script is active
        //we can use Awake to grab save data and run setup commands prior to turning the script on
        //this is out start up

        //Set our Rigidbody2d reference to the component Rigidbody2d found on the object that this script is on 
        rigid = this.GetComponent<Rigidbody2D>();
        //Set our Animator reference to the component Animator found on the object that this script is on 
        anim = this.GetComponent<Animator>();
        m_originalGravityScale = rigid.gravityScale;
        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }
    }
    void Start()
    {
        //The first moment an object and script are active in the scene
        //the script also needs to be active
        //it happens after awake
    }
    // Update is called once per frame
    void Update()
    {
        //happens every frame
        //there can be inconsistant time between these frames
    }
    void FixedUpdate()
    {
        //happens on fixed time
        //great for physics
        //consistant run time
        AnimateDefault();
        bool wasGrounded = isGrounded;
        isGrounded = false;
        isFrontBlocked = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_groundCheck.position, m_groundedRadius, m_whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                }
            }
        }
        colliders = Physics2D.OverlapCircleAll(m_frontCheck.position, m_frontCheckRadius, m_whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isFrontBlocked = true;
            }
        }
        if (isGrounded == true)
        {
            //reset double jump & dash
            jumped = false;
            dashed = false;
        }
    }
    void LateUpdate()
    {
        //happens on the end half of every frame
        //there can be inconsistant times between these frames
        //but they run always with update
    }
    #endregion
    #region Our own little behaviours
    public void DoesntHaveReturnType()
    {
        //this doesnt require an input or an output of data
    }
    public int ReturnsAnInteger(int myInt)
    {
        return myInt;
        //requires input of an int
        //requires output other that int
    }
    public bool HasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
            {
                return true;
            }
        }
        return false;
    }

    public void AnimateDefault()
    {
        if (HasParameter("IsGrounded", anim))
        {
            anim.SetBool("IsGrounded", isGrounded);
        }
        if (HasParameter("IsClimbing", anim))
        {
            anim.SetBool("IsClimbing", isClimbing);
        }
        if (HasParameter("JumpY", anim))
        {
            anim.SetFloat("JumpY", rigid.velocity.y);
        }
    }
    public void Flip()
    {
        //switch the side we are facing to the side we arent
        isFacingRight = !isFacingRight;
        //multiply the players local x scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    public void Climb(float offsetY)
    {
        //do we have this parameter on our players animator?
        if (HasParameter("ClimbSpeed", anim))
        {
            //if yes set our climb speed value
            anim.SetFloat("ClimbSpeed", offsetY);
        }
        RaycastHit2D ladderHit = Physics2D.Raycast(m_ladderCheck.position, Vector2.up, m_ladderRayLength, m_whatIsLadder);
        if (ladderHit.collider != null)
        {
            if (offsetY != 0)
            {
                isClimbing = true;
            }
            //reset double jump & dash on ladder
            jumped = false;
            dashed = false;
        }
        else
        {
            isClimbing = false;
        }
        if (isClimbing)
        {
            rigid.gravityScale = 0;
            rigid.velocity = new Vector2(rigid.velocity.x, offsetY);
        }
        else
        {
            rigid.gravityScale = m_originalGravityScale;
        }
    }
    public void Jump(float height)
    {
        //if the player should be able to jump
        if (isGrounded)
        {
            //add a vertical force to the player
            isGrounded = false;
            rigid.AddForce(new Vector2(0f, height), ForceMode2D.Impulse);
        }
    }
    public void daveJump(float height)
    {
        //if the player should be able to jump
        if (isGrounded)
        {
            //walljump
            if (isFrontBlocked)
            {
                if (isFacingRight) // right = true
                {
                    x = -1;
                }
                else
                {
                    x = 1;
                }
                rigid.AddForce(new Vector2(height * x, 0f), ForceMode2D.Impulse);
                Flip();
            }
            //add a vertical force to the player
            //jumped = false;
            isGrounded = false;
            rigid.AddForce(new Vector2(0f, height), ForceMode2D.Impulse);
        }
        else if (jumped == false)
        {
            //doublejump
            rigid.velocity = new Vector2(rigid.velocity.x, height);
            jumped = true;
        }
    }
    public void Dash(float length)
    {
        //dash
        if (dashed == false)
        {
            if (isFacingRight)
            {
                x = 1;
            }
            else
            {
                x = -1;
            }
            rigid.velocity = new Vector2(0, rigid.velocity.x);
            rigid.AddForce(new Vector2(length*x, 0f), ForceMode2D.Impulse);
            dashed = true;
        }
    }
    public void Stop()
    {
        rigid.velocity = new Vector2(0, 0);
    }
    public void Move(float offsetX)
    {
        if (HasParameter("IsRunning", anim))
        {
            anim.SetBool("IsRunning", offsetX != 0);
        }
        if (isGrounded || m_airControl)
        {
            if (m_stickToSlopes)
            {
                Ray groundRay = new Ray(transform.position, Vector3.down);
                RaycastHit2D groundHit = Physics2D.Raycast(groundRay.origin, groundRay.direction, m_groundRayLength, m_whatIsGround);
                if (groundHit.collider != null)
                {
                    Vector3 slopeDirection = Vector3.Cross(Vector3.up, Vector3.Cross(Vector3.up, groundHit.normal));
                    float slope = Vector3.Dot(Vector3.right * offsetX, slopeDirection);
                    offsetX += offsetX * slope;
                    float angle = Vector2.Angle(Vector2.up, slopeDirection);
                    if (angle > 0)
                    {
                        rigid.gravityScale = 0;
                        rigid.velocity = new Vector2(rigid.velocity.x, 0f);
                    }
                }
            }
            //Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(offsetX, rigid.velocity.y);
            Vector3 velocity = Vector3.zero;
            //And then smoothing it out and applying it to the character
            rigid.velocity = Vector3.SmoothDamp(rigid.velocity, targetVelocity, ref velocity, m_movementSmoothing);

            //If the input is moving the player right and the player is facing left 
            if (offsetX > 0 && !isFacingRight)
            {
                Flip();
            }
            //Otherwise if the input is moving the player left and the player is facing right
            else if (offsetX < 0 && isFacingRight)
            {
                Flip();
            }
        }
    }
    #endregion
}

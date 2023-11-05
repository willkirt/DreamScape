using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScript : MonoBehaviour
{
    [Header("Character object references.")]
    [SerializeField] GameObject playerObj;
    [SerializeField] Transform GC;
    [SerializeField] CharacterController playerController;
    [SerializeField] Animator anim;
    [SerializeField] GameObject interactable;
    [SerializeField] Image projectionBarUI = null;

    [Header("Player variables")]
    [SerializeField] float speed = 5.0f;
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float jumpHeight = 5.0f;
    [SerializeField] float groundDist = .1f;
    [SerializeField] float projMaxDuration = 10f;
    [SerializeField] float projCurrDuration = 0f;
    Vector3 velocity;
    float playerX;
    float playerZ;

    [Header("Player statuses")]
    [SerializeField] bool isExhausted;
    [SerializeField] bool isSprinting;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isProjection;
    [SerializeField] bool readyToJump;
    [SerializeField] bool hasWon;
 
    public LayerMask groundMask;

    #region Variable Accessors
    public bool IsProjection { get => isProjection; set => isProjection = value; }
    public bool IsExhausted { get => isExhausted; set => isExhausted = value; }
    public bool IsSprinting { get => isSprinting; set => isSprinting = value; }
    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
    public float Speed { get => speed; set => speed = value; }
    public float Gravity { get => gravity; set => gravity = value; }
    public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }
    public bool ReadyToJump { get => readyToJump; set => readyToJump = value; }
    public bool HasWon { get => hasWon; set => hasWon = value; }
    public float ProjCurrDuration { get => projCurrDuration; set => projCurrDuration = value; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Get needed objects at start
        playerObj = this.gameObject;
        playerController = playerObj.GetComponent<CharacterController>();
        GC = GameObject.Find("PlayerOrientation").transform;
        anim = GetComponentInChildren<Animator>();
        projectionBarUI = GameObject.Find("ProjectionBar").GetComponent<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Checks to see if the player is using the projection before moving
        if (!isProjection)
        {
            // Checks to see if the player is on the ground or not.
            isGrounded = Physics.CheckSphere(GC.position, groundDist, groundMask);
            anim.SetBool("isGrounded", isGrounded);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            playerX = Input.GetAxis("Horizontal");
            playerZ = Input.GetAxis("Vertical");

            if (playerX != 0 || playerZ != 0)
            {
                anim.SetBool("isWalking", true);

                if (isExhausted)
                {
                    playerController.Move(transform.forward * (speed / 2) * Time.deltaTime);
                }
                else if (isSprinting)
                {
                    playerController.Move(transform.forward * (speed * 1.5f) * Time.deltaTime);
                }
                else
                {
                    playerController.Move(transform.forward * speed * Time.deltaTime);
                }
            }
            else
            {
                anim.SetBool("isWalking", false);
            }

            if (Input.GetKey(KeyCode.LeftShift) && !isExhausted)
            {
                anim.SetBool("isRunning", true);
                isSprinting = true;
            }
            else
            {
                anim.SetBool("isRunning", false);
                isSprinting = false;
            }

            if (Input.GetButton("Jump") && isGrounded && readyToJump)
            {
                anim.SetBool("isJumping", true);
                readyToJump = false;
                velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            }
     

            if (Input.GetMouseButton(1))
            {
                playerObj.transform.rotation = GameObject.Find("PlayerOrientation").transform.rotation;
            }

            velocity.y += gravity * Time.deltaTime;
            playerController.Move(velocity * Time.deltaTime);

            if (projCurrDuration > 0)
            {
                projCurrDuration -= Time.fixedDeltaTime
                    ;
            }    
            else if (projCurrDuration < 0)
            {
                projCurrDuration = 0;
            }
        }
        else
        {
            isSprinting = false;
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("isJumping", false);

            if (projCurrDuration < projMaxDuration)
            {
                projCurrDuration += Time.fixedDeltaTime;
            }
            else
            {
                projCurrDuration = projMaxDuration;
                GameManager.instance.DeActivateProjection();
            }
        }

        if (projectionBarUI != null)
        {
            projectionBarUI.fillAmount = (projMaxDuration - projCurrDuration) / projMaxDuration;
        }

        if (hasWon != anim.GetBool("hasWon"))
        {
            anim.SetBool("hasWon", hasWon);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !IsProjection && isGrounded)
        {
            GameManager.instance.ActivateProjection();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.instance.CurrentState == GameManager.GameStates.Play)
            {
                GameManager.instance.ChangeState("Pause");
            }
            else
            {
                GameManager.instance.ChangeState("Play");
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //GameManager.instance.ChangeSkyBox();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interactable != null)
            {
                anim.SetTrigger("interact");
            }
        }
    }

    #region Trigger functions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            other.gameObject.GetComponent<Light>().enabled = true;
            interactable = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            other.gameObject.GetComponent<Light>().enabled = false;
            interactable = null;
        }
    }
    #endregion

    public void Activate()
    {
        try
        {
            interactable.GetComponent<LeverScript>().Activate();
        }
        catch
        { }
    }

    public void Kill()
    {
        anim.SetTrigger("isDead");
        GameManager.instance.DropTarget();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionScript : MonoBehaviour 
{
    [Header("Projection object references")]
    [SerializeField] GameObject projectionObj;
    [SerializeField] Transform GC;
    [SerializeField] CharacterController playerController;
    [SerializeField] Animator anim;

    [Header("Movement variables")]
    [SerializeField] float speed = 7.5f;
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float jumpHeight = 4.0f;
    [SerializeField] float groundDist = .1f;
    [SerializeField] Vector3 velocity;
    float playerX;
    float playerZ;

    [Header("Player statuses")]
    [SerializeField] bool isGliding;
    [SerializeField] bool isGrounded;
    [SerializeField] bool canJump;
    [SerializeField] bool readyToJump;

    public LayerMask groundMask;

    public bool ReadyToJump { get => readyToJump; set => readyToJump = value; }

    // Start is called before the first frame update
    void Start()
    {
        // Get the game object and its rigidbody.
        projectionObj = this.gameObject;
        GC = GameObject.Find("ProjDir").transform;
        playerController = projectionObj.GetComponent<CharacterController>();
        anim = projectionObj.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
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
            anim.SetBool("isRunning", true);
            playerController.Move(transform.forward * speed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if (Input.GetKey(KeyCode.LeftShift) && !isGrounded && velocity.y < 0)
        {
            isGliding = true;
        }
        else
        {
            isGliding = false;
        }

        if (Input.GetButton("Jump") && isGrounded && readyToJump)
        {
            anim.SetBool("isJumping", true);
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }

        velocity.y = isGliding ? -1.5f : velocity.y + gravity * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().DeActivateProjection();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}

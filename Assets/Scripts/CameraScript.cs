using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    [Header("Target References")]
    [Tooltip("This is the game object the camera will look at.")]
    public Transform player;
    [Tooltip("Player orientation for smoother turns.")]
    public Transform orientation;
    [Tooltip("player model.")]
    public Transform model;

    [Header("Camera references")]
    [Tooltip("This is the freelook camera in the scene.")]
    public CinemachineFreeLook cm;
    [Tooltip("How fast the camera can rotate around the player.")]
    [SerializeField]
    [Range(1, 10)] private float rotationSpeed;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cm.LookAt = player;
        cm.Follow = player;
    }

    public void Update()
    {
        // Rotate target orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // Rotate the target
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * vInput + orientation.right * hInput;

        if (inputDir != Vector3.zero)
            player.forward = Vector3.Lerp(player.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
    }
}

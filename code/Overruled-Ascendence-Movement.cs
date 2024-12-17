using UnityEngine;
public class SimplePlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;             // Speed of movement
    public float mouseSensitivity = 2f;  // Mouse sensitivity for turning

    [Header("Camera Settings")]
    public Transform playerShip;
    public Transform playerCamera;       // Reference to the camera

    [Header("Rotation Limits")]
    public float minPitch = -45f;        // Minimum pitch (look down)
    public float maxPitch = 45f;         // Maximum pitch (look up)
    public float minYaw = -180f;         // Minimum horizontal rotation
    public float maxYaw = 180f;          // Maximum horizontal rotation

    private float yaw = 0f;              // Horizontal rotation (yaw)
    private float pitch = 0f;            // Vertical rotation (pitch)
    private Transform thingToRotate;
    private bool playerLook;

    private Vector3 mDir;

    private void Awake()
    {
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize yaw to current rotation
        yaw = transform.eulerAngles.y;
    }

    private void Update()
    {
        HandleMovement();
        HandleMouseLook();

        // Toggle cursor visibility for debugging or menus (optional)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorVisibility();
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            playerLook = !playerLook;
        }
    }

    private void HandleMovement()
    {
        // Get movement input
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down Arrow

        // Calculate movement relative to the player's forward and right
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        mDir = moveDirection * speed * Time.deltaTime;
        // Apply movement
        Invoke("ApplyMovement", 1.5f);
    }

    private void ApplyMovement()
    {
        transform.Translate(mDir, Space.World);
    }

    private void HandleMouseLook()
    {
        if (playerLook)
        {
            minYaw = -20f;
            maxYaw = 20f;
            minPitch = -25f;
            maxPitch = 25f;
            thingToRotate = playerCamera;
        }
        else
        {
            minYaw = -180f;
            maxYaw = 180f;
            minPitch = -45f;
            maxPitch = 45f;
            thingToRotate = playerShip;
        }
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Update yaw (left/right rotation) with limits
        yaw += mouseX;
        yaw = Mathf.Clamp(yaw, minYaw, maxYaw);

        // Update pitch (up/down look)
        pitch -= mouseY;

        // Clamp pitch to avoid extreme angles
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Apply vertical rotation to the camera
        if (thingToRotate != null)
        {
            thingToRotate.localRotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }

    private void ToggleCursorVisibility()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
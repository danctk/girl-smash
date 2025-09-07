using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target;
    public float distance = 5f;
    public float height = 2f;
    public float rotationSpeed = 2f;
    public float smoothSpeed = 5f;
    
    [Header("Mobile Touch")]
    public float touchSensitivity = 2f;
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;
    
    private float currentX = 0f;
    private float currentY = 0f;
    private Vector3 velocity = Vector3.zero;
    
    void Start()
    {
        if (target == null)
        {
            target = FindObjectOfType<PlayerController>().transform;
        }
        
        // Initialize camera position
        currentX = target.eulerAngles.y;
        currentY = 20f;
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        HandleInput();
        UpdateCameraPosition();
    }
    
    void HandleInput()
    {
        // Mobile touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                currentX += touch.deltaPosition.x * touchSensitivity * Time.deltaTime;
                currentY -= touch.deltaPosition.y * touchSensitivity * Time.deltaTime;
                currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);
            }
        }
        else
        {
            // Mouse input for testing
            if (Input.GetMouseButton(1)) // Right mouse button
            {
                currentX += Input.GetAxis("Mouse X") * rotationSpeed;
                currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
                currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);
            }
        }
    }
    
    void UpdateCameraPosition()
    {
        // Calculate desired position
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 direction = rotation * Vector3.back;
        Vector3 desiredPosition = target.position + direction * distance + Vector3.up * height;
        
        // Smooth camera movement
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 1f / smoothSpeed);
        transform.LookAt(target.position + Vector3.up * height);
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}

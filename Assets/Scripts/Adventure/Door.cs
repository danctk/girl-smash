using UnityEngine;

public class Door : Interactable
{
    [Header("Door Settings")]
    public bool isLocked = true;
    public string keyId = "door_key";
    public float openAngle = 90f;
    public float openSpeed = 2f;
    
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    
    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
        interactionText = isLocked ? "Door is locked" : "Open Door";
    }
    
    protected override void PerformInteraction()
    {
        if (isLocked)
        {
            // Check if player has the key
            InventorySystem inventory = FindObjectOfType<InventorySystem>();
            if (inventory != null && inventory.HasItem(keyId))
            {
                UnlockDoor();
            }
            else
            {
                Debug.Log("You need a key to open this door!");
                return;
            }
        }
        
        ToggleDoor();
    }
    
    void UnlockDoor()
    {
        isLocked = false;
        interactionText = "Open Door";
        Debug.Log("Door unlocked!");
    }
    
    void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }
    
    void OpenDoor()
    {
        isOpen = true;
        interactionText = "Close Door";
        StartCoroutine(RotateDoor(openRotation));
    }
    
    void CloseDoor()
    {
        isOpen = false;
        interactionText = "Open Door";
        StartCoroutine(RotateDoor(closedRotation));
    }
    
    System.Collections.IEnumerator RotateDoor(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, openSpeed * Time.deltaTime);
            yield return null;
        }
        transform.rotation = targetRotation;
    }
}

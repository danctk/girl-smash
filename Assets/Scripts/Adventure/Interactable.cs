using UnityEngine;
using System;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public string interactionText = "Press E to interact";
    public float interactionRange = 2f;
    public bool requiresKey = false;
    public string requiredKeyId = "";
    
    [Header("Visual Feedback")]
    public GameObject highlightObject;
    public Material highlightMaterial;
    private Material originalMaterial;
    private Renderer objectRenderer;
    
    // Events
    public static event Action<Interactable> OnInteractionStarted;
    public static event Action<Interactable> OnInteractionCompleted;
    
    // State
    private bool isPlayerInRange = false;
    private bool isInteracted = false;
    private PlayerController player;
    private InventorySystem inventory;
    
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        inventory = FindObjectOfType<InventorySystem>();
        
        // Set up visual feedback
        if (highlightObject == null)
            highlightObject = gameObject;
        
        objectRenderer = highlightObject.GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalMaterial = objectRenderer.material;
        }
    }
    
    void Update()
    {
        CheckPlayerDistance();
        HandleInput();
    }
    
    void CheckPlayerDistance()
    {
        if (player == null) return;
        
        float distance = Vector3.Distance(transform.position, player.transform.position);
        bool wasInRange = isPlayerInRange;
        isPlayerInRange = distance <= interactionRange;
        
        if (isPlayerInRange && !wasInRange)
        {
            OnPlayerEnterRange();
        }
        else if (!isPlayerInRange && wasInRange)
        {
            OnPlayerExitRange();
        }
    }
    
    void HandleInput()
    {
        if (!isPlayerInRange) return;
        
        // Check for interaction input
        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Interact"))
        {
            Interact();
        }
    }
    
    void OnPlayerEnterRange()
    {
        // Show interaction prompt
        ShowInteractionPrompt();
        
        // Highlight object
        if (objectRenderer != null && highlightMaterial != null)
        {
            objectRenderer.material = highlightMaterial;
        }
    }
    
    void OnPlayerExitRange()
    {
        // Hide interaction prompt
        HideInteractionPrompt();
        
        // Remove highlight
        if (objectRenderer != null && originalMaterial != null)
        {
            objectRenderer.material = originalMaterial;
        }
    }
    
    void ShowInteractionPrompt()
    {
        // This would typically show UI text
        Debug.Log(interactionText);
    }
    
    void HideInteractionPrompt()
    {
        // This would typically hide UI text
        Debug.Log("Hide interaction prompt");
    }
    
    public virtual void Interact()
    {
        if (isInteracted) return;
        
        // Check if key is required
        if (requiresKey && !string.IsNullOrEmpty(requiredKeyId))
        {
            if (inventory == null || !inventory.HasItem(requiredKeyId))
            {
                Debug.Log("You need a key to interact with this!");
                return;
            }
        }
        
        OnInteractionStarted?.Invoke(this);
        
        // Perform interaction
        PerformInteraction();
        
        OnInteractionCompleted?.Invoke(this);
        isInteracted = true;
    }
    
    protected virtual void PerformInteraction()
    {
        // Override this method in derived classes
        Debug.Log($"Interacted with {gameObject.name}");
    }
    
    public void ResetInteraction()
    {
        isInteracted = false;
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw interaction range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}

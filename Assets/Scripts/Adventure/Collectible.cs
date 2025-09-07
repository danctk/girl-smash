using UnityEngine;

public class Collectible : Interactable
{
    [Header("Collectible Settings")]
    public string itemId;
    public string itemName;
    public int quantity = 1;
    public bool autoCollect = false;
    
    private InventorySystem inventory;
    
    void Start()
    {
        inventory = FindObjectOfType<InventorySystem>();
        interactionText = $"Collect {itemName}";
    }
    
    protected override void PerformInteraction()
    {
        if (inventory != null)
        {
            // Create inventory item
            InventoryItem item = new InventoryItem(
                itemId,
                itemName,
                $"A {itemName}",
                null, // Icon would be set in Unity Editor
                quantity,
                ItemType.Misc
            );
            
            if (inventory.AddItem(item))
            {
                Debug.Log($"Collected {itemName}!");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory is full!");
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (autoCollect && other.CompareTag("Player"))
        {
            PerformInteraction();
        }
    }
}

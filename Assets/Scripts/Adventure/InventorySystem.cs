using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class InventoryItem
{
    public string itemId;
    public string itemName;
    public string description;
    public Sprite icon;
    public int quantity;
    public ItemType itemType;
    
    public InventoryItem(string id, string name, string desc, Sprite iconSprite, int qty, ItemType type)
    {
        itemId = id;
        itemName = name;
        description = desc;
        icon = iconSprite;
        quantity = qty;
        itemType = type;
    }
}

public enum ItemType
{
    Consumable,
    Key,
    Tool,
    Quest,
    Misc
}

public class InventorySystem : MonoBehaviour
{
    [Header("Inventory Settings")]
    public int maxInventorySlots = 20;
    
    [Header("UI References")]
    public GameObject inventoryPanel;
    public Transform inventoryContent;
    public GameObject inventoryItemPrefab;
    
    // Inventory data
    private List<InventoryItem> inventory = new List<InventoryItem>();
    
    // Events
    public static event Action<InventoryItem> OnItemAdded;
    public static event Action<InventoryItem> OnItemRemoved;
    public static event Action<InventoryItem> OnItemUsed;
    
    void Start()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
    }
    
    public bool AddItem(InventoryItem item)
    {
        if (inventory.Count >= maxInventorySlots)
        {
            Debug.Log("Inventory is full!");
            return false;
        }
        
        // Check if item already exists
        InventoryItem existingItem = inventory.Find(i => i.itemId == item.itemId);
        if (existingItem != null)
        {
            existingItem.quantity += item.quantity;
        }
        else
        {
            inventory.Add(item);
        }
        
        OnItemAdded?.Invoke(item);
        UpdateInventoryUI();
        return true;
    }
    
    public bool RemoveItem(string itemId, int quantity = 1)
    {
        InventoryItem item = inventory.Find(i => i.itemId == itemId);
        if (item != null)
        {
            if (item.quantity >= quantity)
            {
                item.quantity -= quantity;
                if (item.quantity <= 0)
                {
                    inventory.Remove(item);
                }
                
                OnItemRemoved?.Invoke(item);
                UpdateInventoryUI();
                return true;
            }
        }
        return false;
    }
    
    public bool UseItem(string itemId)
    {
        InventoryItem item = inventory.Find(i => i.itemId == itemId);
        if (item != null)
        {
            // Handle different item types
            switch (item.itemType)
            {
                case ItemType.Consumable:
                    // Apply consumable effect
                    Debug.Log($"Used {item.itemName}");
                    RemoveItem(itemId, 1);
                    OnItemUsed?.Invoke(item);
                    return true;
                    
                case ItemType.Key:
                    // Keys are used automatically when interacting with locked objects
                    Debug.Log($"Used key: {item.itemName}");
                    return true;
                    
                case ItemType.Tool:
                    // Tools might have special effects
                    Debug.Log($"Used tool: {item.itemName}");
                    return true;
                    
                default:
                    Debug.Log($"Cannot use {item.itemName}");
                    return false;
            }
        }
        return false;
    }
    
    public bool HasItem(string itemId, int quantity = 1)
    {
        InventoryItem item = inventory.Find(i => i.itemId == itemId);
        return item != null && item.quantity >= quantity;
    }
    
    public InventoryItem GetItem(string itemId)
    {
        return inventory.Find(i => i.itemId == itemId);
    }
    
    public List<InventoryItem> GetAllItems()
    {
        return new List<InventoryItem>(inventory);
    }
    
    void UpdateInventoryUI()
    {
        if (inventoryContent == null || inventoryItemPrefab == null) return;
        
        // Clear existing UI items
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }
        
        // Create UI items for each inventory item
        foreach (InventoryItem item in inventory)
        {
            GameObject itemUI = Instantiate(inventoryItemPrefab, inventoryContent);
            // Set up the UI item with item data
            // This would typically be handled by a separate UI component
        }
    }
    
    public void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
        }
    }
}

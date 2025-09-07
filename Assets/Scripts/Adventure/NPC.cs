using UnityEngine;

public class NPC : Interactable
{
    [Header("NPC Settings")]
    public string npcName = "NPC";
    public string[] dialogueIds;
    public bool hasQuest = false;
    public string questId;
    
    private DialogueSystem dialogueSystem;
    private QuestSystem questSystem;
    
    void Start()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
        questSystem = FindObjectOfType<QuestSystem>();
        interactionText = $"Talk to {npcName}";
    }
    
    protected override void PerformInteraction()
    {
        if (dialogueSystem != null && dialogueIds.Length > 0)
        {
            // Start dialogue
            string dialogueId = GetAppropriateDialogue();
            dialogueSystem.StartDialogue(dialogueId);
        }
        
        if (hasQuest && questSystem != null && !string.IsNullOrEmpty(questId))
        {
            // Offer quest
            questSystem.StartQuest(questId);
        }
    }
    
    string GetAppropriateDialogue()
    {
        // Simple logic to choose dialogue based on game state
        // This could be expanded based on quest progress, player choices, etc.
        return dialogueIds[0];
    }
}

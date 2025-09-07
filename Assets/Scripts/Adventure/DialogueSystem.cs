using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    public string dialogueText;
    public Sprite speakerPortrait;
    public float displayTime = 3f;
}

[System.Serializable]
public class Dialogue
{
    public string dialogueId;
    public List<DialogueLine> lines;
    public bool isCompleted;
}

public class DialogueSystem : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public Text speakerNameText;
    public Text dialogueText;
    public Image speakerPortrait;
    public Button continueButton;
    public Button skipButton;
    
    [Header("Settings")]
    public float textSpeed = 0.05f;
    public bool autoAdvance = false;
    
    // Dialogue state
    private List<Dialogue> dialogues = new List<Dialogue>();
    private Dialogue currentDialogue;
    private int currentLineIndex;
    private bool isDialogueActive;
    private bool isTyping;
    
    // Events
    public static event Action<Dialogue> OnDialogueStarted;
    public static event Action<Dialogue> OnDialogueCompleted;
    public static event Action<DialogueLine> OnLineCompleted;
    
    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
        
        if (continueButton != null)
            continueButton.onClick.AddListener(ContinueDialogue);
        
        if (skipButton != null)
            skipButton.onClick.AddListener(SkipDialogue);
    }
    
    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            ContinueDialogue();
        }
    }
    
    public void StartDialogue(string dialogueId)
    {
        Dialogue dialogue = dialogues.Find(d => d.dialogueId == dialogueId);
        if (dialogue != null)
        {
            StartDialogue(dialogue);
        }
        else
        {
            Debug.LogWarning($"Dialogue with ID '{dialogueId}' not found!");
        }
    }
    
    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null || dialogue.lines.Count == 0) return;
        
        currentDialogue = dialogue;
        currentLineIndex = 0;
        isDialogueActive = true;
        
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);
        
        OnDialogueStarted?.Invoke(dialogue);
        DisplayCurrentLine();
    }
    
    void DisplayCurrentLine()
    {
        if (currentDialogue == null || currentLineIndex >= currentDialogue.lines.Count) return;
        
        DialogueLine line = currentDialogue.lines[currentLineIndex];
        
        // Set speaker name
        if (speakerNameText != null)
            speakerNameText.text = line.speakerName;
        
        // Set speaker portrait
        if (speakerPortrait != null && line.speakerPortrait != null)
            speakerPortrait.sprite = line.speakerPortrait;
        
        // Start typing effect
        StartCoroutine(TypeText(line.dialogueText));
    }
    
    System.Collections.IEnumerator TypeText(string text)
    {
        isTyping = true;
        if (dialogueText != null)
            dialogueText.text = "";
        
        foreach (char letter in text)
        {
            if (dialogueText != null)
                dialogueText.text += letter;
            
            yield return new WaitForSeconds(textSpeed);
        }
        
        isTyping = false;
        OnLineCompleted?.Invoke(currentDialogue.lines[currentLineIndex]);
        
        if (autoAdvance)
        {
            yield return new WaitForSeconds(currentDialogue.lines[currentLineIndex].displayTime);
            ContinueDialogue();
        }
    }
    
    public void ContinueDialogue()
    {
        if (!isDialogueActive) return;
        
        if (isTyping)
        {
            // Skip typing animation
            StopAllCoroutines();
            if (dialogueText != null)
                dialogueText.text = currentDialogue.lines[currentLineIndex].dialogueText;
            isTyping = false;
            return;
        }
        
        currentLineIndex++;
        
        if (currentLineIndex >= currentDialogue.lines.Count)
        {
            EndDialogue();
        }
        else
        {
            DisplayCurrentLine();
        }
    }
    
    public void SkipDialogue()
    {
        if (!isDialogueActive) return;
        
        EndDialogue();
    }
    
    void EndDialogue()
    {
        isDialogueActive = false;
        currentDialogue.isCompleted = true;
        
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
        
        OnDialogueCompleted?.Invoke(currentDialogue);
        
        currentDialogue = null;
        currentLineIndex = 0;
    }
    
    public void AddDialogue(Dialogue dialogue)
    {
        dialogues.Add(dialogue);
    }
    
    public void RemoveDialogue(string dialogueId)
    {
        dialogues.RemoveAll(d => d.dialogueId == dialogueId);
    }
    
    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
    
    public Dialogue GetCurrentDialogue()
    {
        return currentDialogue;
    }
}

using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Quest
{
    public string questId;
    public string questTitle;
    public string questDescription;
    public QuestStatus status;
    public List<QuestObjective> objectives;
    public List<string> rewards;
}

[System.Serializable]
public class QuestObjective
{
    public string objectiveId;
    public string description;
    public bool isCompleted;
    public int requiredAmount;
    public int currentAmount;
}

public enum QuestStatus
{
    NotStarted,
    InProgress,
    Completed,
    Failed
}

public class QuestSystem : MonoBehaviour
{
    private List<Quest> activeQuests = new List<Quest>();
    private List<Quest> completedQuests = new List<Quest>();
    
    public static event Action<Quest> OnQuestStarted;
    public static event Action<Quest> OnQuestCompleted;
    public static event Action<QuestObjective> OnObjectiveCompleted;
    
    public void StartQuest(string questId)
    {
        Quest quest = GetQuest(questId);
        if (quest != null && quest.status == QuestStatus.NotStarted)
        {
            quest.status = QuestStatus.InProgress;
            activeQuests.Add(quest);
            OnQuestStarted?.Invoke(quest);
        }
    }
    
    public void CompleteObjective(string questId, string objectiveId)
    {
        Quest quest = GetQuest(questId);
        if (quest != null)
        {
            QuestObjective objective = quest.objectives.Find(o => o.objectiveId == objectiveId);
            if (objective != null && !objective.isCompleted)
            {
                objective.currentAmount++;
                if (objective.currentAmount >= objective.requiredAmount)
                {
                    objective.isCompleted = true;
                    OnObjectiveCompleted?.Invoke(objective);
                    
                    // Check if all objectives are completed
                    if (AreAllObjectivesCompleted(quest))
                    {
                        CompleteQuest(quest);
                    }
                }
            }
        }
    }
    
    private bool AreAllObjectivesCompleted(Quest quest)
    {
        return quest.objectives.TrueForAll(o => o.isCompleted);
    }
    
    private void CompleteQuest(Quest quest)
    {
        quest.status = QuestStatus.Completed;
        activeQuests.Remove(quest);
        completedQuests.Add(quest);
        OnQuestCompleted?.Invoke(quest);
    }
    
    private Quest GetQuest(string questId)
    {
        return activeQuests.Find(q => q.questId == questId) ?? 
               completedQuests.Find(q => q.questId == questId);
    }
}

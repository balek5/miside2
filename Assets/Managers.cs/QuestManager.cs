using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private Dictionary<string, bool> questStatus = new Dictionary<string, bool>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize all quests as incomplete
        questStatus["WhoAreYou"] = false;
        questStatus["WhatIsThisPlace"] = false;
        questStatus["HowDoIGetOut"] = false;
    }

    public bool IsQuestCompleted(string questName)
    {
        return questStatus.ContainsKey(questName) && questStatus[questName];
    }

    public void CompleteQuest(string questName)
    {
        if (questStatus.ContainsKey(questName))
        {
            questStatus[questName] = true;
            Debug.Log($"Quest {questName} completed!");
        }
    }
}
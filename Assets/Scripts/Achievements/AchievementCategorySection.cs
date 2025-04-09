using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class AchievementCategorySection : MonoBehaviour
{
    [SerializeField] private GameObject achievementEntryPrefab;
    [SerializeField] private string categoryName;
    private AchievementEntry currentEntry;

    private void Awake()
    {
        if (achievementEntryPrefab != null && 
            achievementEntryPrefab.GetComponent<AchievementEntry>() == null)
        {
            Debug.LogError($"Prefab for {categoryName} is missing AchievementEntry component!");
        }
    }
    public void DisplayCurrentTier(List<Achievement> achievements)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        if (achievements == null || achievements.Count == 0)
        {
            Debug.LogWarning($"No achievements found for category: {categoryName}");
            return;
        }

        achievements.Sort((a, b) => a.tier.CompareTo(b.tier));

        Achievement highestUnlocked = null;
        for (int i = achievements.Count - 1; i >= 0; i--)
        {
            if (achievements[i].isUnlocked)
            {
                highestUnlocked = achievements[i];
                break;
            }
        }

        Achievement nextTier = highestUnlocked != null 
            ? achievements[achievements.IndexOf(highestUnlocked) + 1] 
            : achievements[0];

        Achievement achievementToShow = highestUnlocked ?? achievements[0];

        var entryObj = Instantiate(achievementEntryPrefab, transform);
        currentEntry = entryObj.GetComponent<AchievementEntry>();
        entryObj.SetActive(true);

        if (currentEntry == null)
        {
            Debug.LogError("Missing AchievementEntry component!", entryObj);
            return;
        }

        currentEntry.DisplayAchievement(achievementToShow, nextTier, categoryName);
    }
}
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AchievementCategorySection : MonoBehaviour
{
    [SerializeField] private GameObject achievementEntryPrefab;
    [SerializeField] private string categoryName; // Add this field to store the category name
    
    public void DisplayCurrentTier(List<Achievement> achievements, AchievementDisplay display)
    {
        // Clear existing entries
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Sort achievements by tier
        var sortedAchievements = achievements
            .OrderBy(a => a.tier)
            .ToList();

        Achievement highestUnlocked = sortedAchievements
            .LastOrDefault(a => a.isUnlocked);

        Achievement achievementToShow;
        Achievement nextTier = null;

        if (highestUnlocked != null)
        {
            achievementToShow = highestUnlocked;
            int currentIndex = sortedAchievements.IndexOf(highestUnlocked);
            if (currentIndex < sortedAchievements.Count - 1)
            {
                nextTier = sortedAchievements[currentIndex + 1];
            }
        }
        else
        {
            achievementToShow = sortedAchievements[0];
            nextTier = achievementToShow;
        }

        var entry = Instantiate(achievementEntryPrefab, transform);
        var entryScript = entry.GetComponent<AchievementEntry>();
        entryScript.DisplayAchievement(achievementToShow, nextTier, categoryName);
    }
}
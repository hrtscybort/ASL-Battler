using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AchievementDisplay : MonoBehaviour
{
    [System.Serializable]
    public class CategorySection
    {
        public Achievement.Category category;
        public AchievementCategorySection section;
    }

    [SerializeField] private List<CategorySection> categorySections;
    [SerializeField] private GameObject entryPrefab;

    private void OnEnable() => RefreshAllAchievements();

    public void RefreshAllAchievements()
    {
        if (!AchievementManager.Instance) return;

        var allAchievements = AchievementManager.Instance.GetAllAchievements();
        
        foreach (var categorySection in categorySections)
        {
            if (!categorySection.section) continue;
            
            var achievements = allAchievements
                .Where(a => a.category == categorySection.category)
                .ToList();

            DisplayHighestAchievement(achievements, categorySection.section);
        }
    }

    private void DisplayHighestAchievement(List<Achievement> achievements, AchievementCategorySection section)
    {
        section.ClearEntries();
        if (achievements.Count == 0) return;

        // Your original exact logic
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

        Achievement nextTier = null;
        if (highestUnlocked != null)
        {
            int currentIndex = achievements.IndexOf(highestUnlocked);
            if (currentIndex < achievements.Count - 1)
            {
                nextTier = achievements[currentIndex + 1];
            }
        }
        else
        {
            nextTier = achievements[0];
        }

        Achievement achievementToShow = highestUnlocked ?? achievements[0];

        var entry = Instantiate(entryPrefab, section.GetEntriesContainer())
            .GetComponent<AchievementEntry>();
            
        entry?.DisplayAchievement(achievementToShow, nextTier, section.GetCategoryName());
    }
}
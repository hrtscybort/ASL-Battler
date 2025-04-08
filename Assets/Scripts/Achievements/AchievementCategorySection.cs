using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class AchievementCategorySection : MonoBehaviour
{
    [SerializeField] private AchievementEntry achievementEntryPrefab;
    [SerializeField] private string categoryName;
    [SerializeField] private Sprite bronzeSprite;
    [SerializeField] private Sprite silverSprite;
    [SerializeField] private Sprite goldSprite;
    [SerializeField] private Sprite secretSprite;
    [SerializeField] private Sprite lockedSprite;
    private AchievementEntry currentEntry;

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

        currentEntry = Instantiate(achievementEntryPrefab, transform);
        currentEntry.DisplayAchievement(achievementToShow, nextTier, categoryName);
    }

    private Sprite GetTierSprite(Achievement.Tier tier)
    {
        return tier switch
        {
            Achievement.Tier.Bronze => bronzeSprite,
            Achievement.Tier.Silver => silverSprite,
            Achievement.Tier.Gold => goldSprite,
            Achievement.Tier.Secret => secretSprite,
            _ => lockedSprite
        };
    }
}
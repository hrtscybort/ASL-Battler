using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AchievementDisplay : MonoBehaviour
{
    [SerializeField] private AchievementCategorySection monsterHunterSection;
    [SerializeField] private AchievementCategorySection bossSlayerSection;
    [SerializeField] private AchievementCategorySection signWizardSection;
    [SerializeField] private AchievementCategorySection comboSignerSection;
    [SerializeField] private AchievementCategorySection allAchievementsSection;

    private Dictionary<Achievement.Category, AchievementCategorySection> categorySections;

    private void Start()
    {
        categorySections = new Dictionary<Achievement.Category, AchievementCategorySection>()
        {
            { Achievement.Category.MonsterHunter, monsterHunterSection },
            { Achievement.Category.BossSlayer, bossSlayerSection },
            { Achievement.Category.SignWizard, signWizardSection },
            { Achievement.Category.ComboSigner, comboSignerSection },
            { Achievement.Category.AllAchievements, allAchievementsSection }
        };

        RefreshAllAchievements();
    }

    private void OnEnable()
    {
        if (categorySections != null)
            RefreshAllAchievements();
    }
    public void RefreshAllAchievements()
    {
        if (AchievementManager.Instance == null)
        {
            Debug.LogWarning("AchievementManager not initialized");
            return;
        }

        var sections = new (Achievement.Category, AchievementCategorySection)[]
        {
            (Achievement.Category.MonsterHunter, monsterHunterSection),
            (Achievement.Category.BossSlayer, bossSlayerSection),
            (Achievement.Category.SignWizard, signWizardSection),
            (Achievement.Category.ComboSigner, comboSignerSection),
            (Achievement.Category.AllAchievements, allAchievementsSection)
        };

        var allAchievements = AchievementManager.Instance.GetAllAchievements();
        
        foreach (var (category, section) in sections)
        {
            if (section == null) continue;
            
            var categoryAchievements = new List<Achievement>();
            foreach (var achievement in allAchievements)
            {
                if (achievement.category == category)
                    categoryAchievements.Add(achievement);
            }
            
            section.DisplayCurrentTier(categoryAchievements);
        }
    }
}
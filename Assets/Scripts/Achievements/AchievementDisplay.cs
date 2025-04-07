using UnityEngine;
using System.Linq;

public class AchievementDisplay : MonoBehaviour
{
    [Header("Category Sections")]
    [SerializeField] private AchievementCategorySection monsterHunterSection;
    [SerializeField] private AchievementCategorySection bossSlayerSection;
    [SerializeField] private AchievementCategorySection signWizardSection;
    [SerializeField] private AchievementCategorySection comboSignerSection;
    [SerializeField] private AchievementCategorySection allAchievementsSection;

    [Header("Tier Icons")]
    [SerializeField] private Sprite bronzeIcon;
    [SerializeField] private Sprite silverIcon;
    [SerializeField] private Sprite goldIcon;
    [SerializeField] private Sprite secretIcon;
    [SerializeField] private Sprite lockedIcon;

    private void OnEnable()
    {
        RefreshAllAchievements();
    }

    public void RefreshAllAchievements()
    {
        var allAchievements = AchievementManager.Instance.GetAllAchievements();
        
        monsterHunterSection.DisplayCurrentTier(
            allAchievements.Where(a => a.category == Achievement.Category.MonsterHunter).ToList(),
            this
        );
        
        bossSlayerSection.DisplayCurrentTier(
            allAchievements.Where(a => a.category == Achievement.Category.BossSlayer).ToList(),
            this
        );
        
        signWizardSection.DisplayCurrentTier(
            allAchievements.Where(a => a.category == Achievement.Category.SignWizard).ToList(),
            this
        );
        
        comboSignerSection.DisplayCurrentTier(
            allAchievements.Where(a => a.category == Achievement.Category.ComboSigner).ToList(),
            this
        );
        
        allAchievementsSection.DisplayCurrentTier(
            allAchievements.Where(a => a.category == Achievement.Category.AllAchievements).ToList(),
            this
        );
    }

    public Sprite GetTierIcon(Achievement.Tier tier, bool isUnlocked)
    {
        if (!isUnlocked) return lockedIcon;
        
        return tier switch
        {
            Achievement.Tier.Bronze => bronzeIcon,
            Achievement.Tier.Silver => silverIcon,
            Achievement.Tier.Gold => goldIcon,
            Achievement.Tier.Secret => secretIcon,
            _ => lockedIcon
        };
    }
}
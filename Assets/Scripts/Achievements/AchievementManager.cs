// AchievementManager.cs
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    [SerializeField] private List<Achievement> allAchievements;
    private Dictionary<string, Achievement> achievementsDict = new Dictionary<string, Achievement>();

    // Player progress tracking
    private int totalMonstersKilled = 0;
    private int totalBossesKilled = 0;
    // private int totalPerfectSigns = 0;
    // private int currentPerfectCombo = 0;
    // private int maxPerfectCombo = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAchievements();
    }

    private void InitializeAchievements()
    {
        foreach (var achievement in allAchievements)
        {
            achievementsDict.Add(achievement.achievementID, achievement);
        }
    }

    public void RegisterEnemyDefeated(bool isBoss)
    {
        if (isBoss)
        {
            totalBossesKilled++;
            CheckBossSlayerAchievements();
        }
        else
        {
            totalMonstersKilled++;
            CheckMonsterHunterAchievements();
        }

        CheckAllAchievements();
    }
/*
    public void RegisterPerfectSign(bool isPerfect)
    {
        if (isPerfect)
        {
            totalPerfectSigns++;
            currentPerfectCombo++;
            if (currentPerfectCombo > maxPerfectCombo)
            {
                maxPerfectCombo = currentPerfectCombo;
            }
            CheckSignWizardAchievements();
            CheckComboSignerAchievements();
        }
        else
        {
            currentPerfectCombo = 0;
        }

        CheckAllAchievements();
    }
*/
    private void CheckMonsterHunterAchievements()
    {
        CheckAchievementProgress("MH_5", totalMonstersKilled);
        CheckAchievementProgress("MH_10", totalMonstersKilled);
        CheckAchievementProgress("MH_20", totalMonstersKilled);
        CheckAchievementProgress("MH_40", totalMonstersKilled);
    }

    private void CheckBossSlayerAchievements()
    {
        CheckAchievementProgress("BS_3", totalBossesKilled);
        CheckAchievementProgress("BS_7", totalBossesKilled);
        CheckAchievementProgress("BS_15", totalBossesKilled);
        CheckAchievementProgress("BS_25", totalBossesKilled);
    }
/*
    private void CheckSignWizardAchievements()
    {
        CheckAchievementProgress("SW_5", totalPerfectSigns);
        CheckAchievementProgress("SW_10", totalPerfectSigns);
        CheckAchievementProgress("SW_20", totalPerfectSigns);
        CheckAchievementProgress("SW_35", totalPerfectSigns);
    }

    private void CheckComboSignerAchievements()
    {
        CheckAchievementProgress("CS_3", maxPerfectCombo);
        CheckAchievementProgress("CS_5", maxPerfectCombo);
        CheckAchievementProgress("CS_10", maxPerfectCombo);
        CheckAchievementProgress("CS_20", maxPerfectCombo);
    }
*/
    private void CheckAllAchievements()
    {
        CheckTierAchievements(Achievement.Tier.Bronze);
        CheckTierAchievements(Achievement.Tier.Silver);
        CheckTierAchievements(Achievement.Tier.Gold);
        CheckTierAchievements(Achievement.Tier.Secret);
    }

    private void CheckTierAchievements(Achievement.Tier tier)
    {
        int unlockedCount = 0;
        int totalCount = 0;

        foreach (var achievement in allAchievements)
        {
            if (achievement.tier == tier)
            {
                totalCount++;
                if (achievement.isUnlocked) unlockedCount++;
            }
        }

        if (unlockedCount == totalCount && totalCount > 0)
        {
            string achievementID = $"ALL_{tier.ToString().ToUpper()}";
            if (achievementsDict.ContainsKey(achievementID))
            {
                UnlockAchievement(achievementID);
            }
        }
    }

    private void CheckAchievementProgress(string achievementID, int currentValue)
    {
        if (achievementsDict.TryGetValue(achievementID, out Achievement achievement))
        {
            if (!achievement.isUnlocked && currentValue >= achievement.targetValue)
            {
                UnlockAchievement(achievementID);
            }
        }
    }

    private void UnlockAchievement(string achievementID)
    {
        if (achievementsDict.TryGetValue(achievementID, out Achievement achievement))
        {
            achievement.isUnlocked = true;
            ShowAchievementPopup(achievement);
            SaveAchievements();
        }
    }

    private void ShowAchievementPopup(Achievement achievement)
    {
        Debug.Log($"Achievement Unlocked: {achievement.title} - {achievement.description}");
        
        // Example: Instantiate a popup prefab with achievement details
        // AchievementPopup.Show(achievement);
    }

    private void SaveAchievements()
    {
        // Implement saving to PlayerPrefs or your save system
        foreach (var achievement in allAchievements)
        {
            PlayerPrefs.SetInt(achievement.achievementID, achievement.isUnlocked ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    private void LoadAchievements()
    {
        foreach (var achievement in allAchievements)
        {
            achievement.isUnlocked = PlayerPrefs.GetInt(achievement.achievementID, 0) == 1;
        }
    }

    public List<Achievement> GetAllAchievements()
    {
        return allAchievements;
    }
}

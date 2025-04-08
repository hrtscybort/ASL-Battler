using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    #region Variables
    public static AchievementManager Instance { get; private set; }

    [SerializeField] private List<Achievement> allAchievements;
    [SerializeField] private AchievementPopup popupPrefab;
    [SerializeField] private Sprite bronzeIcon;
    [SerializeField] private Sprite silverIcon;
    [SerializeField] private Sprite goldIcon;
    [SerializeField] private Sprite secretIcon;

    private AchievementPopup activePopup;
    private Dictionary<string, Achievement> achievementsDict = new Dictionary<string, Achievement>();
    private const string SAVE_PREFIX = "ACH_";
    private const string PROGRESS_PREFIX = "PROG_";

    // Player progress tracking
    private int totalMonstersKilled = 0;
    private int totalBossesKilled = 0;
    private int totalPerfectSigns = 0;
    private int currentPerfectCombo = 0;
    private int maxPerfectCombo = 0;

    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        if (popupPrefab != null && activePopup == null)
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();
            if (canvas != null)
            {
                activePopup = Instantiate(popupPrefab, canvas.transform);
                activePopup.gameObject.SetActive(false);
            }
        }

        InitializeAchievements();
        LoadAchievements();
    }

    private void InitializeAchievements()
    {
        foreach (var achievement in allAchievements)
        {
            if (string.IsNullOrEmpty(achievement.achievementID))
            {
                Debug.LogError("Achievement missing ID!");
                continue;
            }
            
            if (achievementsDict.ContainsKey(achievement.achievementID))
            {
                Debug.LogError($"Duplicate achievement ID: {achievement.achievementID}");
                continue;
            }
            
            achievementsDict.Add(achievement.achievementID, achievement);
            achievement.OnUnlocked += HandleAchievementUnlocked;
        }
    }

    private void OnDestroy()
    {
        foreach (var achievement in allAchievements)
        {
            achievement.OnUnlocked -= HandleAchievementUnlocked;
        }
    }

    #region Achievement Logic

    private void HandleAchievementUnlocked(Achievement achievement)
    {
        SaveAchievements(); // Immediate save on unlock
        ShowAchievementPopup(achievement);
        UpdateAchievementsUI();
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
            string achievementID = $"ALL_{tier.ToString()}";
            if (achievementsDict.ContainsKey(achievementID))
            {
                UnlockAchievement(achievementID);
            }
        }
    }

    public void UpdateAchievementsUI()
    {
        AchievementDisplay display = FindFirstObjectByType<AchievementDisplay>();
        
        if (display != null)
        {
            display.RefreshAllAchievements();
        }
        else
        {
            Debug.LogWarning("AchievementDisplay not found in scene");
        }
    }

    #endregion

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
    public List<Achievement> GetAllAchievements()
    {
        return allAchievements;
    }

    private void ShowAchievementPopup(Achievement achievement)
    {
        Debug.Log($"Achievement Unlocked: {achievement.title} - {achievement.description}");
        
        if (activePopup == null) return;
    
        Sprite tierSprite = achievement.tier switch
        {
            Achievement.Tier.Bronze => bronzeIcon,
            Achievement.Tier.Silver => silverIcon,
            Achievement.Tier.Gold => goldIcon,
            Achievement.Tier.Secret => secretIcon,
                _ => bronzeIcon
        };

        activePopup.Show(achievement, tierSprite);
    }

    #region Player Prefs
    private void LoadAchievements()
    {
        foreach (Achievement achievement in allAchievements)
        {
            achievement.isUnlocked = 
                PlayerPrefs.GetInt(SAVE_PREFIX + achievement.achievementID, 0) == 1;
        }
        
        totalMonstersKilled = PlayerPrefs.GetInt(PROGRESS_PREFIX + "MONSTER_KILLS", 0);
        totalBossesKilled = PlayerPrefs.GetInt(PROGRESS_PREFIX + "BOSS_KILLS", 0);

        Debug.Log(totalBossesKilled);
        Debug.Log(totalMonstersKilled);
    }

    private void SaveAchievements()
    {
        foreach (Achievement achievement in allAchievements)
        {
            PlayerPrefs.SetInt(SAVE_PREFIX + achievement.achievementID, 
                            achievement.isUnlocked ? 1 : 0);
        }
        
        PlayerPrefs.SetInt(PROGRESS_PREFIX + "MONSTER_KILLS", totalMonstersKilled);
        PlayerPrefs.SetInt(PROGRESS_PREFIX + "BOSS_KILLS", totalBossesKilled);
        
        PlayerPrefs.Save();
    }

    private void UnlockAchievement(string achievementID)
    {
        if (achievementsDict.TryGetValue(achievementID, out Achievement achievement))
        {
            if (!achievement.isUnlocked)
            {
                achievement.Unlock();
                PlayerPrefs.SetInt(SAVE_PREFIX + achievementID, 1);
                PlayerPrefs.Save();
                ShowAchievementPopup(achievement);
            }
        }
    }

    // save triggers
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) SaveAchievements();
    }

    private void OnApplicationQuit()
    {
        SaveAchievements();
    }

    #endregion
}

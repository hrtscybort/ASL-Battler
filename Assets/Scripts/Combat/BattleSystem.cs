using System.Collections;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Combat
{
    public class BattleSystem : StateMachine
    {
        #region Fields and Properties

        [SerializeField] private BattleUI ui;
        [SerializeField] private Fighter player;
        [SerializeField] private Fighter[] minionTypes;
        [SerializeField] private Fighter bossType;
        [SerializeField] private Button startBattleButton;

        private Fighter enemy;
        private int currentWave = 0;
        private int currentEnemyIndex = 0;
        private Fighter[] currentWaveEnemies;

        public Fighter Player => player;
        public Fighter Enemy => enemy;
        public Fighter[] MinionTypes => minionTypes;
        public Fighter BossType => bossType;
        public BattleUI Interface => ui;
        public int CurrentWave => currentWave;

        #endregion

        #region Execution

        private void Start()
        {
            player.Reset();
            currentWave = 1;
            SetupWave();

            Interface.InitializePlayer(Player);

            Interface.UpdateWaveText(currentWave);
            SetState(new Begin(this));

            if (startBattleButton != null)
            {
                startBattleButton.onClick.AddListener(OnStartButton);
            }
            else
            {
                Debug.LogError("Start Battle Button is not assigned in the Inspector!");
            }
        }

        private void SetupWave()
        {
            currentEnemyIndex = 0;
            currentWaveEnemies = new Fighter[3];

            for (int i = 0; i < 2; i++)
            {
                int index = Random.Range(0, MinionTypes.Length);
                currentWaveEnemies[i] = MinionTypes[index];
            }

            currentWaveEnemies[2] = BossType;
        }

        public void InitializeEnemy()
        {
            if (currentEnemyIndex >= 0 && currentEnemyIndex < currentWaveEnemies.Length)
            {
                enemy = currentWaveEnemies[currentEnemyIndex];
                Enemy.Reset();
            }
        }

        public void OnEnemyDefeated()
        {
            currentEnemyIndex++;
            if (currentEnemyIndex >= currentWaveEnemies.Length)
            {
                currentWave++;
                SetupWave();
                Interface.UpdateWaveText(currentWave);
            }
        }

        public void OnAttackButton()
        {
            StartCoroutine(State.Attack());
        }

        public void OnHealButton()
        {
            StartCoroutine(State.Heal());
        }

        public void OnSpecialButton()
        {
            StartCoroutine(State.Special());
        }

        public void OnDefendButton()
        {
            StartCoroutine(State.Defend());
        }

        public void OnPauseButton()
        {
            Interface.ShowPauseMenu();
        }

        public void OnResumeButton()
        {
            Interface.HidePauseMenu();
        }

        public void OnMainMenuButton()
        {
            SceneManager.LoadScene("Main Menu");
        }

        public void OnRestartButton()
        {
            SceneManager.LoadScene("Main");
        }

        public void OnAchievementButton()
        {
            Interface.ShowAchievementMenu();
        }

        public void OnMenuButton()
        {
            Interface.MainMenu();
        }

        public void OnStartButton()
        {
            Interface.HideTutorial();
            Debug.Log("Battle Started! Transition to the main battle screen.");
        }

        public void OnBackButton()
        {
            if (Interface.IsAchievementScreenActive())
            {
                Interface.HideAchievementMenu();
            }
            else
            {
                Interface.HidePauseMenu();
            }
        }

        #endregion
    }
}
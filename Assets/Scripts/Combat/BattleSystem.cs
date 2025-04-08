using System.Collections;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Combat
{
    public class BattleSystem : StateMachine
    {
        #region Fields and Properties

        [SerializeField] private BattleUI ui;
        [SerializeField] private Fighter player;
        [SerializeField] private Fighter[] enemyTypes;
        private Fighter enemy;

        public Fighter Player => player;
        public Fighter Enemy => enemy;
        public Fighter[] EnemyTypes => enemyTypes;
        public BattleUI Interface => ui;

        #endregion

        #region Execution

        private void Start()
        {
            player.Reset();

            Interface.InitializePlayer(Player);

            SetState(new Begin(this));
        }

        public void InitializeEnemy()
        {
            if (EnemyTypes != null && EnemyTypes.Length > 0)
            {
                int index = Random.Range(0, EnemyTypes.Length);
                enemy = EnemyTypes[index];
                Enemy.Reset();
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

        #endregion
    }
}
﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public enum GamePhase { Tutorial, Monster, Boss }

    public class BattleUI : MonoBehaviour
    {
        [SerializeField] private GameObject fighterPrefab;
        [SerializeField] private Transform playerParent;
        [SerializeField] private Transform enemyParent;
        [SerializeField] private GameObject playerParentGO;
        [SerializeField] private GameObject enemyParentGO;
        [SerializeField] private StatusDisplay playerStatusDisplay;
        [SerializeField] private StatusDisplay enemyStatusDisplay;
        [SerializeField] private GameObject PauseScreen;
        [SerializeField] private GameObject DefeatScreen;
        [SerializeField] private GameObject ActionsAndStatus;
        [SerializeField] private Button SpecialButton;
        [SerializeField] private GameObject pauseButton;
        [SerializeField] private GameObject AchievementScreen;
        [SerializeField] private GameObject TutorialScreen;
        [SerializeField] private Text WaveText;

        private bool isInTutorial = true;

        public void Initialize(Fighter player, Fighter enemy)
        {
            InitializePlayer(player);
            InitializeEnemy(enemy);
        }

        public void UpdateWaveText(int wave)
        {
            WaveText.text = $"WAVE {wave}";
        }

        public void EnableSpecialButton()
        {
            SpecialButton.interactable = true;
        }

        public void DisableSpecialButton()
        {
            SpecialButton.interactable = false;
        }

        public void ShowDefeatScreen()
        {
            DefeatScreen.SetActive(true);
            ActionsAndStatus.SetActive(false);
        }

        public void ShowPauseMenu()
        {
            PauseScreen.SetActive(true);
            playerParentGO.SetActive(false);
            enemyParentGO.SetActive(false);
            AchievementScreen.SetActive(false);
            ActionsAndStatus.SetActive(false);
            pauseButton.SetActive(false);
        }

        public void HidePauseMenu()
        {
            if (isInTutorial) {
                TutorialScreen.SetActive(true);
                PauseScreen.SetActive(false);
                pauseButton.SetActive(true);
            } else {
                PauseScreen.SetActive(false);
                playerParentGO.SetActive(true);
                enemyParentGO.SetActive(true);
                ActionsAndStatus.SetActive(true);
                pauseButton.SetActive(true);
            }
        }

        public void HideTutorial()
        {
            playerParentGO.SetActive(true);
            enemyParentGO.SetActive(true);
            ActionsAndStatus.SetActive(true);
            TutorialScreen.SetActive(false);
        }

        public void ShowTutorial()
        {
            playerParentGO.SetActive(false);
            enemyParentGO.SetActive(false);
            ActionsAndStatus.SetActive(false);
            TutorialScreen.SetActive(true);
        }

        public bool IsAchievementScreenActive()
        {
            return AchievementScreen != null && AchievementScreen.activeSelf;
        }

        public void ShowAchievementMenu()
        {
            isInTutorial = false;
            AchievementScreen.SetActive(true);
            PauseScreen.SetActive(false);
            ActionsAndStatus.SetActive(false);
            pauseButton.SetActive(false);
        }

        public void HideAchievementMenu()
        {
            AchievementScreen.SetActive(false);
            PauseScreen.SetActive(true);
            ActionsAndStatus.SetActive(false);
            pauseButton.SetActive(false);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }

        public void InitializePlayer(Fighter fighter)
        {
            foreach (Transform child in playerParent)
            {
                Destroy(child.gameObject);
            }

            playerStatusDisplay.Initialize(fighter);

            var go = Instantiate(fighterPrefab, playerParent, false);

            go.transform.localScale = Vector3.one * 0.4f;
            var image = go.GetComponent<Image>();
            image.color = fighter.Color;
            image.preserveAspect = true;

            var animator = go.AddComponent<AnimationController>();
            animator.Initialize(fighter.IdleAnimationFrames, fighter.AnimationSpeed);
        }

        public void InitializeEnemy(Fighter fighter)
        {
            foreach (Transform child in enemyParent)
            {
                Destroy(child.gameObject);
            }

            enemyStatusDisplay.Initialize(fighter);

            var go = Instantiate(fighterPrefab, enemyParent, false);
            if (fighter.Name.Contains("Boss"))
            {
                RectTransform rect = go.GetComponent<RectTransform>();
                rect.anchoredPosition += Vector2.up * 45f;
                rect.anchoredPosition += Vector2.right * 30f;
            }
            else
            {
                go.transform.localScale = Vector3.one * 0.4f;
            }

            var image = go.GetComponent<Image>();
            image.color = fighter.Color;
            image.preserveAspect = true;

            var animator = go.AddComponent<AnimationController>();
            animator.Initialize(fighter.IdleAnimationFrames, fighter.AnimationSpeed);
        }
    }
}

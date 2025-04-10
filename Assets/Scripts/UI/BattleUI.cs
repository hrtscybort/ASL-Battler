﻿﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class BattleUI : MonoBehaviour
    {
        [SerializeField] private GameObject fighterPrefab;
        [SerializeField] private Transform playerParent;
        [SerializeField] private Transform enemyParent;
        [SerializeField] private StatusDisplay playerStatusDisplay;
        [SerializeField] private StatusDisplay enemyStatusDisplay;
        [SerializeField] private GameObject PauseScreen;
        [SerializeField] private GameObject ActionsAndStatus;
        [SerializeField] private Button SpecialButton;
        [SerializeField] private GameObject AchievementScreen;

        public void Initialize(Fighter player, Fighter enemy)
        {
            InitializePlayer(player);
            InitializeEnemy(enemy);
        }

        public void EnableSpecialButton()
        {
            SpecialButton.interactable = true;
        }

        public void DisableSpecialButton()
        {
            SpecialButton.interactable = false;
        }

        public void ShowPauseMenu()
        {
            PauseScreen.SetActive(true);
            AchievementScreen.SetActive(false);
            ActionsAndStatus.SetActive(false);
        }

        public void HidePauseMenu()
        {
            PauseScreen.SetActive(false);
            ActionsAndStatus.SetActive(true);
        }

        public bool IsAchievementScreenActive()
        {
            return AchievementScreen != null && AchievementScreen.activeSelf;
        }

        public void ShowAchievementMenu()
        {
            AchievementScreen.SetActive(true);
            PauseScreen.SetActive(false);
            ActionsAndStatus.SetActive(false);
        }

        public void HideAchievementMenu()
        {
            AchievementScreen.SetActive(false);
            PauseScreen.SetActive(true);
            ActionsAndStatus.SetActive(false);
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

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class StatusDisplay : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private Image manaBar;
        [SerializeField] private Image shieldBar;

        private Fighter _fighter;

        public void Initialize(Fighter fighter)
        {
            _fighter = fighter;
        }

        public void Update()
        {
            UpdateHealthBar();
            UpdateManaBar();
            UpdateShieldBar();
        }

        private void UpdateHealthBar()
        {
            if (_fighter.CurrentHealth == 0)
            {
                healthBar.fillAmount = 0;
            }
            else
            {
                healthBar.fillAmount = (float)_fighter.CurrentHealth / _fighter.MaxHealth;
            }
        }

        private void UpdateManaBar()
        {
            if (_fighter.MaxMana == 0)
            {
                manaBar.fillAmount = 0;
            }
            else
            {
                manaBar.fillAmount = (float)_fighter.CurrentMana / _fighter.MaxMana;
            }
        }

        private void UpdateShieldBar()
        {
            if (_fighter.MaxShield == 0)
            {
                shieldBar.fillAmount = 0;
            }
            else
            {
                shieldBar.fillAmount = (float)_fighter.CurrentShield / _fighter.MaxShield;
            }
        }
    }
}
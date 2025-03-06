using System;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu]
    public class Fighter : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private int _level;
        [SerializeField] private Color _color;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private int _currentHealth;
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentMana;
        [SerializeField] private int _maxMana;
        [SerializeField] private int _currentShield;
        [SerializeField] private int _maxShield;
        [SerializeField] private int _attack;
        [SerializeField] private int _healing;
        [SerializeField] private int _charging;
        [SerializeField] private int _special;
        [SerializeField] private int _defending;

        public string Name => _name;
        public int Level => _level;
        public Color Color => _color;
        public Sprite Sprite => _sprite;
        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;
        public int CurrentMana => _currentMana;
        public int MaxMana => _maxMana;
        public int CurrentShield => _currentShield;
        public int MaxShield => _maxShield;
        public int Attack => _attack;
        public int Healing => _healing;
        public int Charging => _charging;
        public int Defending => _defending;
        public int Special => _special;


        public bool Damage(int amount)
        {
            var currentShield = _currentShield;

            _currentShield = Math.Max(0, _currentShield - amount);
            amount = Math.Max(0, amount - currentShield);

            _currentHealth = Math.Max(0, _currentHealth - amount);
            return _currentHealth == 0;
        }

        public void Heal(int amount)
        {
            _currentHealth += amount;
        }

        public void Charge(int amount)
        {
            _currentMana += amount;
        }

        public void Discharge()
        {
            _currentMana = 0;
        }

        public void Defend(int amount)
        {
            _currentShield += amount;
        }

        public void Reset()
        {
            _currentHealth = _maxHealth;
            _currentMana = 0;
            _currentShield = 0;
        }

        private void OnValidate()
        {
            _currentHealth = Math.Min(_currentHealth, _maxHealth);
            _currentMana = Math.Min(_currentMana, _maxMana);
            _currentShield = Math.Min(_currentShield, _maxShield);
        }
    }
}
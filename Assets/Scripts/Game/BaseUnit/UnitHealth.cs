using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using System;

namespace Game.BaseUnit
{
    public class UnitHealth : EnablerMonoBehaviour
    {
        private int _maxHealthPoints;
        private int _currentHealthPoints;

        public Action<int,int,int> OnDamageReceived;
        public Action OnDeath;

        public void Initialize(int maxPoints)
        {
            _currentHealthPoints = maxPoints;
            _maxHealthPoints = maxPoints;
            Enable();
        }

        public void TakeDamage(int amount)
        {
            if (!_isEnabled)
            {
                return;
            }

            if (amount <= 0)
            {
                return;
            }

            if (amount >= _currentHealthPoints)
            {
                _currentHealthPoints = 0;
                OnDeath?.Invoke();
                Disable();
                return;
            }

            _currentHealthPoints -= amount;
            // int damage, int currentHealth, int maxHealth
            OnDamageReceived?.Invoke(amount, _currentHealthPoints,  _maxHealthPoints); 
        }
    }
}
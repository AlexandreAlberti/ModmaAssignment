using System;
using UnityEngine;

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
        
        public void Restart(int maxPoints)
        {
            _currentHealthPoints = maxPoints;
            _maxHealthPoints = maxPoints;
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
            OnDamageReceived?.Invoke(amount, _currentHealthPoints,  _maxHealthPoints); 
        }
    }
}
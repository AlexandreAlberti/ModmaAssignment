using Game.BaseUnit;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class HealthBar : Bar
    {
        [SerializeField] private UnitHealth _unitHealth;
        [SerializeField] private TextMeshProUGUI _currentHealth;

        public void Initialize(int currentHealth)
        {
            base.Initialize();
            _unitHealth.OnDamageReceived += PlayerHealth_OnDamaged;
            _unitHealth.OnDeath += PlayerHealth_OnDeath;
            _currentHealth.text = $"{currentHealth}";
        }

        public void Restart(int currentHealth)
        {
            base.Initialize();
            _currentHealth.text = $"{currentHealth}";
        }

        private void PlayerHealth_OnDamaged(int damage, int currentHealth, int maxHealth)
        {
            EmptyBarPercentage((float)currentHealth / maxHealth);
            _currentHealth.text = $"{currentHealth}";
        }
        
        private void PlayerHealth_OnDeath()
        {
            _bar.SetActive(false);
        }
    }
}
using System;
using UnityEngine;

namespace Game.BaseUnit
{
    [RequireComponent(typeof(UnitHealth))]
    [RequireComponent(typeof(UnitVisuals))]
    public class Unit : EnablerMonoBehaviour
    {
        private UnitHealth _unitHealth;
        private UnitVisuals _unitVisuals;

        public Action OnUnitDamage;
        public Action<Unit> OnUnitDeath;

        private void Awake()
        {
            _unitHealth = GetComponent<UnitHealth>();
            _unitVisuals = GetComponent<UnitVisuals>();
        }

        public void Initialize(int healthPoints)
        {
            _unitHealth.Initialize(healthPoints);
            _unitHealth.OnDamageReceived += UnitHealth_OnDamageReceived;
            _unitHealth.OnDeath += UnitHealth_OnDeath;
            _unitVisuals.Initialize(healthPoints);
            _unitVisuals.PlayIdleAnimation();
            Enable();
        }

        private void UnitHealth_OnDamageReceived(int damage, int currentHealth, int maxHealth)
        {
            _unitVisuals.EnableDamageFlash();
            _unitVisuals.PlayDamagedAnimation();
            OnUnitDamage?.Invoke();
        }

        private void UnitHealth_OnDeath()
        {
            Disable();
            _unitVisuals.EnableDamageFlash();
            _unitVisuals.PlayDieAnimation();
            OnUnitDeath.Invoke(this);
        }

        public void TakeDamage(int damage)
        {
            _unitVisuals.SpawnDamagedParticle(damage);
            _unitHealth.TakeDamage(damage);
        }

        public void PlayAttackAnimation(Vector3 directionToEnemy)
        {
            _unitVisuals.PlayAttackAnimation(transform.forward, directionToEnemy);
        }

        public void ResetFaceToEnemyRotation()
        {
            _unitVisuals.ResetLocalRotation();
        }

        public void PlayMoveAnimation()
        {
            _unitVisuals.PlayMoveAnimation();
        }

        public void PlayIdleAnimation()
        {
            _unitVisuals.PlayIdleAnimation();
        }

        public void ChangeAnimatorSpeed(float speedMultiplier)
        {
            _unitVisuals.ChangeAnimatorSpeed(speedMultiplier);
        }
    }
}
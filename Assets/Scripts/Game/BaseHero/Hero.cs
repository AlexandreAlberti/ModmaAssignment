using UnityEngine;
using Game.BaseUnit;

namespace Game.BaseHero
{
    [RequireComponent(typeof(HeroMeleeAttacker))]
    [RequireComponent(typeof(HeroMovement))]
    [RequireComponent(typeof(HeroVisuals))]
    [RequireComponent(typeof(Unit))]
    public class Hero : EnablerMonoBehaviour
    {
        private HeroMeleeAttacker _heroMeleeAttacker;
        private HeroMovement _heroMovement;
        private HeroVisuals _heroVisuals;
        private Unit _unit;

        private void Awake()
        {
            _heroMeleeAttacker = GetComponent<HeroMeleeAttacker>();
            _heroMovement = GetComponent<HeroMovement>();
            _heroVisuals = GetComponent<HeroVisuals>();
            _unit = GetComponent<Unit>();
        }

        public void Initialize(int healthPoints)
        {
            _unit.Initialize(healthPoints);
            _unit.OnUnitDamage += Unit_OnUnitDamage;
            _unit.OnUnitDeath += Unit_OnUnitDeath;
            _heroMovement.OnMoved += OnMoved;
            _heroMovement.OnMovementStart += OnMovementStart;
            _heroMovement.OnMovementEnd += OnMovementEnd;
            _heroMovement.Enable();
            _heroMeleeAttacker.OnAttackPerformed += PlayerMeleeAttacker_OnAttackPerformed;
            _heroMeleeAttacker.OnWeaponChanged += PlayerMeleeAttacker_OnWeaponChanged;
            _heroMeleeAttacker.Initialize();
            Enable();
        }
        
        private void OnMoved()
        {
            _unit.ResetFaceToEnemyRotation();
            _heroMeleeAttacker.ResetFaceToEnemyOrientation();
        }

        private void OnMovementStart()
        {
            _unit.PlayMoveAnimation();
        }

        private void OnMovementEnd()
        {
            _unit.PlayIdleAnimation();
        }

        private void PlayerMeleeAttacker_OnAttackPerformed(Vector3 directionToEnemy)
        {
            FaceToEnemyDirection(directionToEnemy);
        }

        private void PlayerMeleeAttacker_OnWeaponChanged(float newAttackRangeScaler, float speedMultiplier)
        {
            _heroMovement.ChangeSpeedMultiplier(speedMultiplier);
            _heroVisuals.IncrementAttackRange(newAttackRangeScaler);
            _unit.ChangeAnimatorSpeed(speedMultiplier);
        }

        private void Unit_OnUnitDeath(Unit unit)
        {
            _unit.OnUnitDeath += Unit_OnUnitDeath;
        }

        private void Unit_OnUnitDamage()
        {
            // Not Implementing as not expected to receive any event here
        }
        
        private void FaceToEnemyDirection(Vector3 directionToEnemy)
        {
            _unit.PlayAttackAnimation(directionToEnemy);
        }

        public void ChangeWeapon(int newWeapon)
        {
            _heroMeleeAttacker.ChangeWeapon(newWeapon);
        }
    }
}
using System;
using UnityEngine;
using Game.BaseUnit;

namespace Game.BaseEnemy
{
    [RequireComponent(typeof(Unit))]
    [RequireComponent(typeof(EnemyVisuals))]
    public class Enemy : EnablerMonoBehaviour
    {
        private Unit _unit;
        private EnemyVisuals _enemyVisuals;
        private Enemy _enemyPrefab;

        public Action<Enemy> OnDead;
        
        private void Awake()
        {
            _unit = GetComponent<Unit>();
            _enemyVisuals = GetComponent<EnemyVisuals>();
        }

        public void Initialize(int healthPoints, Enemy enemyPrefab)
        {
            _unit.Initialize(healthPoints);
            _unit.OnUnitDamage -= Unit_OnUnitDamage;
            _unit.OnUnitDamage += Unit_OnUnitDamage;
            _unit.OnUnitDeath -= Unit_OnUnitDeath;
            _unit.OnUnitDeath += Unit_OnUnitDeath;
            _enemyPrefab = enemyPrefab;
            UnTargetEnemy();
            Enable();
        }
        
        private void Unit_OnUnitDeath(Unit unit)
        {
            OnDead?.Invoke(this);
        }

        private void Unit_OnUnitDamage()
        {
            
        }

        public void TakeDamage(int damage)
        {
            _unit.TakeDamage(damage);
        }

        public Enemy GetEnemyPrefab()
        {
            return _enemyPrefab;
        }

        public Unit GetUnit()
        {
            return _unit;
        }
        
        public void TargetEnemy()
        {
            _enemyVisuals.EnableTargetedMark();
        }

        public void UnTargetEnemy()
        {
            _enemyVisuals.DisableTargetedMark();
        }
    }
}
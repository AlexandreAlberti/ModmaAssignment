using System.Collections.Generic;
using Game.BaseEnemy;
using Game.BaseUnit;
using UnityEngine;

namespace Game.BaseHero
{
    public class HeroAttackTrigger : UnitAttackTrigger
    {
        private List<Enemy> _damagedEnemies;
        
        public override void Enable()
        {
            base.Enable();
            _damagedEnemies = new List<Enemy>();
        }

        public override void Disable()
        {
            base.Disable();
            _damagedEnemies = new List<Enemy>();
        }

        protected override void OnTriggerStay(Collider other)
        {
            if (!_isEnabled)
            {
                return;
            }

            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy && !_damagedEnemies.Contains(enemy))
            {
                enemy.TakeDamage(_damage);
                _damagedEnemies.Add(enemy);
            }
        }
    }
}

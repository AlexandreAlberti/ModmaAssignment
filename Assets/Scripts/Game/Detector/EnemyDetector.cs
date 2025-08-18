using System.Collections.Generic;
using Game.BaseEnemy;
using Game.BaseUnit;
using UnityEngine;

namespace Game.Detector
{
    public class EnemyDetector : UnitDetector
    {
        private float _detectionDistance;

        public void Initialize(Transform detectorCenterPosition, float detectionDistance)
        {
            base.Initialize(detectorCenterPosition);
            _detectionDistance = detectionDistance;
        }
        
        protected override void TargetUnit()
        {
            if (!_closestUnit)
            {
                return;
            }

            _closestUnit.GetComponent<Enemy>().TargetEnemy();
        }

        public Enemy GetClosestEnemy()
        {
            if (!_closestUnit)
            {
                return null;
            }

            return _closestUnit.GetComponent<Enemy>();
        }

        public override float GetDetectionDistance()
        {
            return _detectionDistance;
        }

        protected override List<Unit> GetUnitsList()
        {
            List<Unit> units = new List<Unit>();

            foreach (Enemy enemy in EnemyManager.Instance.GetEnemies())
            {
                if (!enemy)
                {
                    continue;
                }
                
                units.Add(enemy.GetUnit());
            }

            return units;
        }
        
        protected override void ResetTargetUnit()
        {
            if (!_closestUnit)
            {
                return;
            }
            
            _closestUnit.GetComponent<Enemy>().UnTargetEnemy();
            base.ResetTargetUnit();
        }
    }}

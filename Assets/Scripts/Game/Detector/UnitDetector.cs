using System.Collections.Generic;
using Game.BaseUnit;
using UnityEngine;

namespace Game.Detector
{
    public abstract class UnitDetector : EnablerMonoBehaviour
    {
        [SerializeField] private float _detectionCooldown;
        
        private Transform _detectorCenterPosition;
        private float _detectionTimer;
        protected Unit _closestUnit;
        private RaycastHit[] _raycastHits;

        public void Initialize(Transform detectorCenterPosition)
        {
            _raycastHits = new RaycastHit[1];
            _closestUnit = null;
            _detectorCenterPosition = detectorCenterPosition;
            Enable();
        }
        
        private void Update()
        {
            if (!_isEnabled)
            {
                return;
            }

            _detectionTimer -= Time.deltaTime;

            if (_detectionTimer <= 0.0f)
            {
                _detectionTimer = _detectionCooldown;

                FindClosestUnit();
            }
        }

        public abstract float GetDetectionDistance();
        
        protected abstract List<Unit> GetUnitsList();

        public void FindClosestUnit(Unit unitToIgnore = null)
        {
            Unit newClosestUnit = null;
            float closestDistance = GetDetectionDistance();

            foreach (Unit unit in GetUnitsList())
            {
                if (!unit || unitToIgnore == unit)
                {
                    continue;
                }
                
                Vector3 direction = unit.transform.position - _detectorCenterPosition.position;
                float distance = direction.magnitude;

                if (distance < closestDistance)
                {
                        closestDistance = distance;
                        newClosestUnit = unit;
                }
            }

            bool differentUnitDetected = newClosestUnit != _closestUnit;
            
            if (!newClosestUnit || differentUnitDetected)
            {
                ResetTargetUnit();
            }
            
            if (newClosestUnit && differentUnitDetected)
            {
                _closestUnit = newClosestUnit;
                _closestUnit.OnUnitDeath += Unit_OnDead;
                TargetUnit();
            }
        }

        protected virtual void ResetTargetUnit()
        {
            if (!_closestUnit)
            {
                return;
            }
            
            _closestUnit.OnUnitDeath -= Unit_OnDead;
            _closestUnit = null;
        }
        
        protected virtual void TargetUnit()
        {
        }
        
        private void Unit_OnDead(Unit unit)
        {
            unit.OnUnitDeath -= Unit_OnDead;
            _closestUnit = null;
        }
    }
}
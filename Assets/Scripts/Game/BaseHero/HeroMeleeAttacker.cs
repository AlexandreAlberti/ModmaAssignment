using System;
using System.Collections;
using Game.BaseEnemy;
using Game.Detector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.BaseHero
{
    [RequireComponent(typeof(EnemyDetector))]
    [RequireComponent(typeof(HeroMovement))]
    public class HeroMeleeAttacker : EnablerMonoBehaviour
    {
        [SerializeField] private HeroAttackTrigger _heroAttackTrigger;
        [SerializeField] private float _attackingCooldown;
        [SerializeField] private int _attackDamage;
        [SerializeField] private float _attackStartTime;
        [SerializeField] private GameObject[] _weapons;
        [SerializeField] private int[] _weaponsDamages;
        [SerializeField] private float[] _weaponsScales;
        [SerializeField] private float[] _weaponsSpeeds;

        private const int AttackTriggerFramesToRemainActive = 10;
        
        private Hero _hero;
        private HeroMovement _heroMovement;
        private EnemyDetector _enemyDetector;
        private float _attackTimer;
        private float _attackRange;
        private float _speedMultiplier;

        public Action<Vector3> OnAttackPerformed;
        public Action<float, float> OnWeaponChanged;

        private void Awake()
        {
            _hero = GetComponent<Hero>();
            _enemyDetector = GetComponent<EnemyDetector>();
            _heroMovement = GetComponent<HeroMovement>();
        }

        public void Initialize()
        {
            foreach (GameObject weapon in _weapons)
            {
                weapon.SetActive(false);
            }

            _heroAttackTrigger.Disable();
            ChangeWeapon(Random.Range(0, _weapons.Length));
            _attackTimer = _attackingCooldown;
            Enable();
        }
        
        private void Update()
        {
            if (!_isEnabled)
            {
                return;
            }
            
            _attackTimer -= Time.deltaTime * _speedMultiplier;
            Enemy closestEnemy = _enemyDetector.GetClosestEnemy();
            
            if (_attackTimer <= 0.0f && !_heroMovement.IsMoving() && closestEnemy)
            {
                _attackTimer = _attackingCooldown;
                Vector3 vectorToEnemy = closestEnemy.transform.position - transform.position;
                Vector3 directionToEnemy = vectorToEnemy.normalized;
                OnAttackPerformed?.Invoke(directionToEnemy);
                StartCoroutine(AttackChoreography());
                float signedAngle = Vector3.SignedAngle(transform.forward, directionToEnemy, transform.up);
                _heroAttackTrigger.transform.localRotation = Quaternion.AngleAxis(signedAngle, Vector3.up);
            }
        }
        
        private IEnumerator AttackChoreography()
        {
            yield return new WaitForSeconds(_attackStartTime / _speedMultiplier);
            _heroAttackTrigger.Enable();
            
            for (int i = 0; i < AttackTriggerFramesToRemainActive; i++)
            {
                yield return new WaitForEndOfFrame();
            }
            
            _heroAttackTrigger.Disable();
        }
        
        public void ChangeWeapon(int index)
        {
            int indexToUse = index;
            
            if (index < 0)
            {
                indexToUse = 0;
            }

            if (index >= _weapons.Length)
            {
                indexToUse =  _weapons.Length - 1;
            }
            
            _weapons[indexToUse].SetActive(true);
            _attackRange = _weaponsScales[indexToUse];
            _speedMultiplier = _weaponsSpeeds[indexToUse];
            _heroAttackTrigger.Initialize(_weaponsDamages[indexToUse]);
            _heroAttackTrigger.SetScale(_attackRange);
            _enemyDetector.Initialize(_hero.transform, _attackRange);
            OnWeaponChanged?.Invoke(_attackRange * 2, _speedMultiplier);
        }
        
        public void ResetFaceToEnemyOrientation()
        {
            _heroAttackTrigger.transform.localRotation =  Quaternion.identity;
        }
    }
}

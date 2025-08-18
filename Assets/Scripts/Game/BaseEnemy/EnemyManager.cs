using System;
using System.Collections;
using System.Collections.Generic;
using Game.BaseHero;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.BaseEnemy
{
    public class EnemyManager : EnablerMonoBehaviour
    {
        private const float WAIT_TIME_BEFORE_ENEMY_APPEAR = 0.1f;
        
        [SerializeField] private PooledEnemyConfig _pooledEnemyConfig;
        [SerializeField] private float _enemyDistanceFromHeroToAppear;
        [SerializeField] private float _enemySpawnInterval;
        [SerializeField] private int _maxEnemySpawnPerRound;
        [SerializeField] private int _maxEnemyAmount;
        [SerializeField] private int _enemyDefaultHp;
        [SerializeField] private EnemyMarkSpawner _enemySpawnMarkVfxPrefab;
        [SerializeField] private EnemyRaySpawner _enemySpawnRayVfxPrefab;

        public static EnemyManager Instance { get; private set; }
        
        private List<Enemy> _enemyList;
        private float _spawnTimer;

        private void Awake()
        {
            Instance = this;
        }

        public void Initialize()
        {
            _enemyList = new List<Enemy>();
            ObjectPool.Instance.RegisterPrefab(_pooledEnemyConfig.EnemyPrefab.gameObject, _pooledEnemyConfig.EnemyPrewarmCount);
            ObjectPool.Instance.RegisterPrefab(_enemySpawnMarkVfxPrefab.gameObject, _pooledEnemyConfig.EnemyPrewarmCount);
            ObjectPool.Instance.RegisterPrefab(_enemySpawnRayVfxPrefab.gameObject, _pooledEnemyConfig.EnemyPrewarmCount);
            _spawnTimer = _enemySpawnInterval;
        }

        public override void Enable()
        {
            base.Enable();

            foreach (Enemy enemy in _enemyList)
            {
                enemy.Enable();
            }
        }
        public override void Disable()
        {
            base.Disable();

            foreach (Enemy enemy in _enemyList)
            {
                enemy.Disable();
            }
        }

        private void Update()
        {
            if (!_isEnabled)
            {
                return;
            }

            _spawnTimer -= Time.deltaTime;
            
            if (_spawnTimer <= 0)
            {
                _spawnTimer += _enemySpawnInterval;
                SpawnLogicSimple();
            }
        }

        private void SpawnLogicSimple()
        {
            int enemiesToSpawn = Math.Min(_maxEnemySpawnPerRound, _maxEnemyAmount - _enemyList.Count);
            StartCoroutine(SpawnRound(enemiesToSpawn));
        }
        
        private IEnumerator SpawnRound(int enemiesToSpawn)
        {
            Vector3 heroPosition = HeroManager.Instance.GetHeroPosition();
            
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Vector2 directionFromPlayer = Random.insideUnitCircle.normalized;
                Vector3 spawnPosition = heroPosition + new Vector3(directionFromPlayer.x, 0.0f, directionFromPlayer.y) * _enemyDistanceFromHeroToAppear;
                StartCoroutine(SpawnEnemyChoreography(spawnPosition));
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator SpawnEnemyChoreography(Vector3 spawnPosition)
        {
            EnemyMarkSpawner spawnMarkVfxInstance = ObjectPool.Instance
                .GetPooledObject(_enemySpawnMarkVfxPrefab.gameObject, Vector3.zero, Quaternion.identity)
                .GetComponent<EnemyMarkSpawner>();
            spawnMarkVfxInstance.transform.position = spawnPosition;
            spawnMarkVfxInstance.Initialize();
            EnemyRaySpawner spawnRayVfxInstance = ObjectPool.Instance
                .GetPooledObject(_enemySpawnRayVfxPrefab.gameObject, Vector3.zero, Quaternion.identity)
                .GetComponent<EnemyRaySpawner>();
            spawnRayVfxInstance.transform.position = spawnPosition;
            spawnRayVfxInstance.Initialize();
            float waitTime = spawnMarkVfxInstance.GetSpawnDuration();
            StartCoroutine(SpawnEnemyAndWaitToAddToDetectableList(spawnPosition, waitTime));
            yield return new WaitForSeconds(waitTime);
            ObjectPool.Instance.Release(_enemySpawnMarkVfxPrefab.gameObject, spawnMarkVfxInstance.gameObject);
            ObjectPool.Instance.Release(_enemySpawnRayVfxPrefab.gameObject, spawnRayVfxInstance.gameObject);
        }

        private IEnumerator SpawnEnemyAndWaitToAddToDetectableList(Vector3 spawnPosition, float waitTime)
        {
            yield return new WaitForSeconds(WAIT_TIME_BEFORE_ENEMY_APPEAR);
            Enemy enemyInstance = ObjectPool.Instance.GetPooledObject(_pooledEnemyConfig.EnemyPrefab.gameObject, spawnPosition, Quaternion.identity).GetComponent<Enemy>();
            enemyInstance.Initialize(_enemyDefaultHp, _pooledEnemyConfig.EnemyPrefab);
            enemyInstance.OnDead += Enemy_OnDead;
            yield return new WaitForSeconds(waitTime - WAIT_TIME_BEFORE_ENEMY_APPEAR);
            _enemyList?.Add(enemyInstance);
        }

        private void Enemy_OnDead(Enemy enemy)
        {
            enemy.OnDead -= Enemy_OnDead;
            _enemyList.Remove(enemy);
            StartCoroutine(WaitAndReleaseEnemy(enemy));
        }

        private IEnumerator WaitAndReleaseEnemy(Enemy enemy)
        {
            yield return new WaitForSeconds(1.0f);
            ObjectPool.Instance.Release(enemy.GetEnemyPrefab().gameObject, enemy.gameObject);
        }

        public List<Enemy> GetEnemies()
        {
            return _enemyList;
        }
    }
}
using System;

namespace Game.BaseEnemy
{
    [Serializable]
    public struct PooledEnemyConfig
    {
        public Enemy EnemyPrefab;
        public int EnemyPrewarmCount;
    }
}
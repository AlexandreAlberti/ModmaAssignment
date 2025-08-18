using UnityEngine;

namespace Game.BaseEnemy
{
    public class EnemyRaySpawner : MonoBehaviour
    {
        [SerializeField] private Animator _rayAnimator;
        [SerializeField] private AnimationClip _enemySpawnAnimation;

        public void Initialize()
        {
            _rayAnimator.Rebind();
        }

        public float GetSpawnDuration()
        {
            return _enemySpawnAnimation.length;
        }
    }
}

using UnityEngine;

namespace Game.BaseEnemy
{
    public class EnemyVisuals : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyTargetedMark;

        public void EnableTargetedMark()
        {
            _enemyTargetedMark.SetActive(true);
        }

        public void DisableTargetedMark()
        {
            _enemyTargetedMark.SetActive(false);
        }
    }
}
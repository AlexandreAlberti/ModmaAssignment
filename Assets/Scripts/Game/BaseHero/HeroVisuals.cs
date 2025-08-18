using UnityEngine;

namespace Game.BaseHero
{
    public class HeroVisuals : MonoBehaviour
    {
        [SerializeField] private Transform _detectionCircle;

        public void IncrementAttackRange(float newAttackRangeRadius)
        {
            _detectionCircle.localScale = new Vector3(newAttackRangeRadius, newAttackRangeRadius, newAttackRangeRadius);
        }
    }
}
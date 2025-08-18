using System;
using UnityEngine;

namespace Game.BaseUnit
{
    public abstract class UnitAttackTrigger : EnablerMonoBehaviour
    {
        private BoxCollider _boxCollider;
        protected int _damage;

        private void Awake()
        {
            _boxCollider =  GetComponent<BoxCollider>();
        }

        public void Initialize(int damage)
        {
            _damage = damage;
            Disable();
        }
        
        public void SetScale(float scale)
        {
            _boxCollider.center = new Vector3(0.0f, 0.0f, scale * 0.5f);
            _boxCollider.size = Vector3.one * scale;
        }

        protected abstract void OnTriggerStay(Collider other);
    }
}

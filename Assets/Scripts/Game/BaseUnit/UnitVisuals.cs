using System.Collections.Generic;
using DamageNumbersPro;
using Game.Manager;
using Game.UI;
using UnityEngine;

namespace Game.BaseUnit
{
    [RequireComponent(typeof(UnitDamageFlash))]
    public class UnitVisuals : MonoBehaviour
    {
        private static readonly int ATTACK_ANIMATION = Animator.StringToHash("Attack");
        private static readonly int DAMAGE_ANIMATION = Animator.StringToHash("Damage");
        private static readonly int DIE_ANIMATION = Animator.StringToHash("Die");
        private static readonly int IDLE_ANIMATION = Animator.StringToHash("Idle");
        private static readonly int MOVE_ANIMATION = Animator.StringToHash("Move");
        
        [SerializeField] private Renderer[] _unitRenderers;
        [SerializeField] private Transform _visuals;
        [SerializeField] private Animator _animator;
        [SerializeField] private DamageNumber _damageParticle;
        [SerializeField] private Transform _damageParticleSpawnPosition;
        [SerializeField] private HealthBar _healthBar;
        
        private UnitDamageFlash _unitDamageFlash;
        private List<Material> _originalMaterials;

        private void Awake()
        {
            _unitDamageFlash = GetComponent<UnitDamageFlash>();
            SaveOriginalRendererMaterials();
        }

        public void Initialize(int currentHealth)
        {
            _healthBar.Initialize(currentHealth);
            _unitDamageFlash.Initialize();
            EnableHealthBar();
            SaveOriginalRendererMaterials();
        }
        
        public void Restart(int currentHealth)
        {
            _healthBar.Restart(currentHealth);
        }
        
        private void SaveOriginalRendererMaterials()
        {
            _originalMaterials = new List<Material>();

            for (int i = 0; i < _unitRenderers.Length; i++)
            {
                for (int j = 0; j < _unitRenderers[i].materials.Length; j++)
                {
                    _originalMaterials.Add(_unitRenderers[i].materials[j]);
                }
            }
        }

        public void ChangeRenderersMaterial(Material material)
        {
            foreach (Renderer unitRenderer in _unitRenderers)
            {
                Material[] materials = unitRenderer.materials;

                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = material;
                }

                unitRenderer.materials = materials;
            }
        }

        public void RestoreOriginalRenderersMaterial()
        {
            int materialCount = 0;

            for (int i = 0; i < _unitRenderers.Length; i++)
            {
                Material[] materials = _unitRenderers[i].materials;

                for (int j = 0; j < materials.Length; j++)
                {
                    materials[j] = _originalMaterials[materialCount++];
                }

                _unitRenderers[i].materials = materials;
            }
        }
        
        private void EnableHealthBar()
        {
            _healthBar.gameObject.SetActive(true);
        }
        
        public void PlayDamagedAnimation()
        {
            _animator.Play(DAMAGE_ANIMATION);
        }
        
        public void SpawnDamagedParticle(int amount)
        {
            DamageNumber newParticle = _damageParticle.Spawn(_damageParticleSpawnPosition.position, amount);
            newParticle.cameraOverride = CameraManager.Instance.Camera().transform;
        }
        
        public void PlayDieAnimation()
        {
            _animator.Play(DIE_ANIMATION);
        }

        public void PlayIdleAnimation()
        {
            _animator.Play(IDLE_ANIMATION);
        }

        public void PlayMoveAnimation()
        {
            _animator.Play(MOVE_ANIMATION);
        }

        public void PlayAttackAnimation(Vector3 forwardDirection, Vector3 directionToEnemy)
        {
            float signedAngle = Vector3.SignedAngle(forwardDirection, directionToEnemy, Vector3.up);
            _visuals.localRotation = Quaternion.AngleAxis(signedAngle, Vector3.up);
            _animator.Play(ATTACK_ANIMATION);
        }

        public void ResetLocalRotation()
        {
            _visuals.localRotation = Quaternion.identity;
        }

        public void ChangeAnimatorSpeed(float speedMultiplier)
        {
            _animator.speed = speedMultiplier;
        }

        public void EnableDamageFlash()
        {
            _unitDamageFlash.Flash();
        }
    }
}

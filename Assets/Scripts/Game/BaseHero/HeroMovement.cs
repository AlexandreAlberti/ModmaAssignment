using System;
using Game.Input;
using UnityEngine;

namespace Game.BaseHero
{
    public class HeroMovement : EnablerMonoBehaviour
    {
        [SerializeField] private float _movementSpeed;
        
        private bool _isMoving;
        private float _speedMultiplier;

        public Action OnMoved;
        public Action OnMovementStart;
        public Action OnMovementEnd;

        public override void Enable()
        {
            base.Enable();
            TouchInputManager.Instance.OnDragVector += TouchInputManager_OnDragVector;
            TouchInputManager.Instance.OnDragStarted += TouchInputManager_OnDragStarted;
            TouchInputManager.Instance.OnDragStopped += TouchInputManager_OnDragStopped;
        }

        public override void Disable()
        {
            TouchInputManager.Instance.OnDragVector -= TouchInputManager_OnDragVector;
            TouchInputManager.Instance.OnDragStarted -= TouchInputManager_OnDragStarted;
            TouchInputManager.Instance.OnDragStopped -= TouchInputManager_OnDragStopped;
            base.Disable();
        }
        
        public bool IsMoving()
        {
            return _isMoving;
        }
        private void TouchInputManager_OnDragStarted()
        {
            _isMoving = true;
            OnMovementStart?.Invoke();
        }

        private void TouchInputManager_OnDragStopped()
        {
            _isMoving = false;
            OnMovementEnd?.Invoke();
        }

        private void TouchInputManager_OnDragVector(Vector2 input)
        {
            if (!_isEnabled)
            {
                return;
            }

            _isMoving = input != Vector2.zero;

            if (!_isMoving)
            {
                return;
            }

            Vector3 movementDirection = new Vector3(input.x, 0.0f, input.y);
            float moveAmount = _movementSpeed * _speedMultiplier * Time.deltaTime;
            transform.position += movementDirection * moveAmount;
            transform.forward = movementDirection;
            OnMoved?.Invoke();
        }

        public void ChangeSpeedMultiplier(float speedMultiplier)
        {
            _speedMultiplier = speedMultiplier;
        }
    }
}

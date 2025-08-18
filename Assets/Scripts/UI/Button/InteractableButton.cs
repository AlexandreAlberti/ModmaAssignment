using System;
using System.Collections;
using Game;
using UnityEngine;

namespace UI.Button
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    [RequireComponent(typeof(PointerListener))]
    public class InteractableButton : EnablerMonoBehaviour
    {
        [SerializeField] protected UnityEngine.UI.Button _button;
        [SerializeField] protected PointerListener _pointerListener;
        [Header("Animation")]
        [SerializeField] protected bool _animateClick = true;
        [SerializeField] protected float _normalScale;
        [SerializeField] protected float _scaleFactor;
        [SerializeField] protected float _scaleChangeDuration;
        
        public Action<InteractableButton> OnClicked;

        private void Awake()
        {
            Enable();
        }

        private void OnButtonUp()
        {
            if (!_isEnabled)
            {
                return;
            }

            if (_animateClick)
            {
                float scale = _normalScale - _normalScale * _scaleFactor;
                StartCoroutine(ApplyScale(scale, _normalScale, _scaleChangeDuration));
            }
        }

        private void OnButtonDown()
        {
            if (!_isEnabled)
            {
                return;
            }

            if (_animateClick)
            {
                float scale = _normalScale - _normalScale * _scaleFactor;
                StartCoroutine(ApplyScale(_normalScale, scale, _scaleChangeDuration));
            }
        }

        private void OnClick()
        {
            if (!_isEnabled)
            {
                return;
            }
            
            if (_animateClick)
            {
                float scale = _normalScale - _normalScale * _scaleFactor;
                StartCoroutine(ApplyScale(_normalScale, scale, _scaleChangeDuration, OnButtonUp));
            }

            OnClicked?.Invoke(this);
        }

        private IEnumerator ApplyScale(float initialScale, float endScale, float duration, Action onComplete = null)
        {
            float timer = 0.0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                transform.localScale = Vector3.one *  Mathf.Lerp(initialScale, endScale, Mathf.Min(1.0f, timer / duration));
                yield return new WaitForEndOfFrame();
            }

            transform.localScale = Vector3.one * endScale;
            onComplete?.Invoke();
        }
        
        public override void Enable()
        {
            if (_isEnabled)
            {
                return;
            }

            base.Enable();
            _button.interactable = true;
            _button.onClick.AddListener(OnClick);
            _pointerListener.OnButtonDown += OnButtonDown;
            _pointerListener.OnButtonUp += OnButtonUp;
        }

        public override void Disable()
        {
            if (!_isEnabled)
            {
                return;
            }

            base.Disable();
            _button.interactable = false;
            _button.onClick.RemoveListener(OnClick);
            _pointerListener.OnButtonDown -= OnButtonDown;
            _pointerListener.OnButtonUp -= OnButtonUp;
        }
    }
}
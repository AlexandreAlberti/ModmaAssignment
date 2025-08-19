using System;
using System.Collections;
using Game.BaseHero;
using UI.Button;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] protected InteractableButton _curvedSwordButton;
        [SerializeField] protected InteractableButton _greatSwordButton;
        [SerializeField] protected InteractableButton _longSwordButton;
        [SerializeField] protected int _curvedSwordIndex;
        [SerializeField] protected int _greatSwordIndex;
        [SerializeField] protected int _longSwordIndex;
        [SerializeField] private GameObject _startGamePanel;
        [SerializeField] private AnimationClip _startGamePanelAnimationClip;

        public static UIManager Instance  { get; private set; }

        public Action OnStartGameAnimationEnded;

        private void Awake()
        {
            Instance = this;
        }

        public void Initialize()
        {
            _startGamePanel.SetActive(false);
            _curvedSwordButton.OnClicked += CurvedSwordButton_OnClicked;
            _greatSwordButton.OnClicked += GreatSwordButton_OnClicked;
            _longSwordButton.OnClicked += LongSwordButton_OnClicked;
        }

        private void CurvedSwordButton_OnClicked(InteractableButton button)
        {
            HeroManager.Instance.ChangeHeroWeapon(_curvedSwordIndex);
        }
        private void GreatSwordButton_OnClicked(InteractableButton button)
        {
            HeroManager.Instance.ChangeHeroWeapon(_greatSwordIndex);
        }

        private void LongSwordButton_OnClicked(InteractableButton button)
        {
            HeroManager.Instance.ChangeHeroWeapon(_longSwordIndex);
        }

        public void ShowStartGameAnimation()
        {
            _startGamePanel.SetActive(true);

            StartCoroutine(WaitAndEndStartGameAnimation());
        }

        private IEnumerator WaitAndEndStartGameAnimation()
        {
            yield return new WaitForSeconds(_startGamePanelAnimationClip.length);
            
            _startGamePanel.SetActive(false);
            OnStartGameAnimationEnded?.Invoke();
        }
    }
}

using System;
using System.Collections;
using Game.BaseHero;
using TMPro;
using UI.Button;
using UnityEngine;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] protected InteractableButton _curvedSwordButton;
        [SerializeField] protected InteractableButton _greatSwordButton;
        [SerializeField] protected InteractableButton _longSwordButton;
        [SerializeField] protected InteractableButton _retryButton;
        [SerializeField] protected InteractableButton _resetButton;
        [SerializeField] protected int _curvedSwordIndex;
        [SerializeField] protected int _greatSwordIndex;
        [SerializeField] protected int _longSwordIndex;
        [SerializeField] private GameObject _startGamePanel;
        [SerializeField] private AnimationClip _startGamePanelAnimationClip;
        [SerializeField] private EndGamePanel _endGamePanel;
        [SerializeField] private TextMeshProUGUI _remainingEnemiesText;
        [SerializeField] private TextMeshProUGUI _timeLeftText;

        private const string REMAINING_ENEMIES = "Enemies Left: ";
        private const string REMAINING_SECONDS = "Time Left: ";
        
        public static UIManager Instance  { get; private set; }

        public Action OnStartGameAnimationEnded;
        public Action OnRetryButtonPressed;

        private void Awake()
        {
            Instance = this;
            _timeLeftText.gameObject.SetActive(false);
            _remainingEnemiesText.gameObject.SetActive(false);
            _startGamePanel.SetActive(false);
            _endGamePanel.gameObject.SetActive(false);
        }

        public void Initialize(int enemiesToKill, int remainingSeconds)
        {
            _timeLeftText.gameObject.SetActive(true);
            _remainingEnemiesText.gameObject.SetActive(true);
            UpdateRemainingKills(enemiesToKill);
            UpdateRemainingSeconds(remainingSeconds);
            _curvedSwordButton.OnClicked += CurvedSwordButton_OnClicked;
            _greatSwordButton.OnClicked += GreatSwordButton_OnClicked;
            _longSwordButton.OnClicked += LongSwordButton_OnClicked;
            _retryButton.OnClicked += RetryButton_OnClicked;
            _resetButton.OnClicked += RetryButton_OnClicked;
        }

        public void Restart(int enemiesToKill, int remainingSeconds)
        {
            UpdateRemainingKills(enemiesToKill);
            UpdateRemainingSeconds(remainingSeconds);
            _startGamePanel.SetActive(false);
            _endGamePanel.gameObject.SetActive(false);
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

        private void RetryButton_OnClicked(InteractableButton button)
        {
            OnRetryButtonPressed?.Invoke();
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

        public void ShowEndGame(bool win)
        {
            _endGamePanel.gameObject.SetActive(true);
            
            if (win)
            {
                _endGamePanel.ShowWinText();
            }
            else
            {
                _endGamePanel.ShowLoseText();
            }
        }

        public void UpdateRemainingKills(int enemiesToKill)
        {
            _remainingEnemiesText.SetText($"{REMAINING_ENEMIES}{enemiesToKill}");
        }
        
        public void UpdateRemainingSeconds(int remainingSeconds)
        {
            _timeLeftText.SetText($"{REMAINING_SECONDS}{remainingSeconds}");
        }
    }
}

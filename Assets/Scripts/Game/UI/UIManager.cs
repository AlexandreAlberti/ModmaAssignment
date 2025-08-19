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

        private void Awake()
        {
            Instance = this;
        }

        public void Initialize(int enemiesToKill, int remainingSeconds)
        {
            _startGamePanel.SetActive(false);
            _endGamePanel.Initialize();
            _endGamePanel.gameObject.SetActive(false);
            UpdateRemainingKills(enemiesToKill);
            UpdateRemainingSeconds(remainingSeconds);
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

        public void ShowEndGame(bool win)
        {
            if (win)
            {
                _endGamePanel.ShowWinText();
            }
            else
            {
                _endGamePanel.ShowLoseText();
            }
            
            _endGamePanel.gameObject.SetActive(true);
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

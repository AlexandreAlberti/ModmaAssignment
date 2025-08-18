using System;
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

        public static UIManager Instance  { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void Initialize()
        {
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
    }
}

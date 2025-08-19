using System.Collections;
using Game.BaseEnemy;
using Game.BaseHero;
using Game.Input;
using UI;
using UnityEngine;

namespace Game.Manager
{
    public class GameManager : MonoBehaviour
    {
        void Start()
        {
            ObjectPool.Instance.Initialize();
            HeroManager.Instance.Initialize();
            EnemyManager.Instance.Initialize();
            Joystick.Instance.Initialize();
            TouchInputManager.Instance.Initialize();
            ObjectPool.Instance.ActivatePooling();
            UIManager.Instance.Initialize();
            UIManager.Instance.ShowStartGameAnimation();
            UIManager.Instance.OnStartGameAnimationEnded += UIManager_OnStartGameAnimationEnded;
        }

        private void UIManager_OnStartGameAnimationEnded()
        {
            GameMatchStart();
        }

        private void GameMatchStart()
        {
            Joystick.Instance.Enable();
            TouchInputManager.Instance.Enable();
            EnemyManager.Instance.Enable();
        }
    }
}

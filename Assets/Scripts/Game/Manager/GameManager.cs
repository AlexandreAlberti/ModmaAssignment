using System;
using Game.BaseEnemy;
using Game.BaseHero;
using Game.Input;
using Game.UI;
using Game.Utils;
using UnityEngine;

namespace Game.Manager
{
    public class GameManager : MonoBehaviour
    {
        private CountDown _matchCountDown;
        private int _enemiesToKill;
        private int _enemiesKilled;
        private int _playTime;
        private int _secondsPassed;
        
        private async void Start()
        {
            ObjectPool.Instance.Initialize();
            HeroManager.Instance.Initialize();
            EnemyManager.Instance.Initialize();
            Joystick.Instance.Initialize();
            TouchInputManager.Instance.Initialize();
            ObjectPool.Instance.ActivatePooling();
            
            RemoteConfig.Instance.OnRemoteConfigLoaded += RemoteConfig_OnRemoteConfigLoaded;
            await RemoteConfig.Instance.Initialize();
        }

        private void RemoteConfig_OnRemoteConfigLoaded(int playTime, int enemiesToKill)
        {
            _playTime = playTime;
            _enemiesToKill = enemiesToKill;
            
            _matchCountDown = gameObject.AddComponent<CountDown>();
            _matchCountDown.SetTotalTime(_playTime);
            _matchCountDown.OnSecondPassed += MatchCountDown_OnSecondPassed;
            _matchCountDown.OnCountDownEnded += MatchCountDown_OnCountDownEnded;

            UIManager.Instance.Initialize(_enemiesToKill, _playTime);
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
            EnemyManager.Instance.OnEnemyKilled += EnemyManager_OnEnemyKilled;
            HeroManager.Instance.OnHeroDeath += HeroManager_OnHeroDeath;
            _enemiesKilled = 0;
            _secondsPassed = 0;
            _matchCountDown.StartCounting();
        }

        private void HeroManager_OnHeroDeath()
        {
            EndGame(false);
        }

        private void EnemyManager_OnEnemyKilled()
        {
            _enemiesKilled++;
            UIManager.Instance.UpdateRemainingKills(Math.Max(0, _enemiesToKill - _enemiesKilled));

            if (_enemiesKilled >= _enemiesToKill)
            {
                EndGame(true);
            }
        }

        private void EndGame(bool win)
        {
            _matchCountDown.StopCounting();
            Joystick.Instance.Disable();
            TouchInputManager.Instance.Disable();
            EnemyManager.Instance.Disable();
            UIManager.Instance.ShowEndGame(win);
        }
        
        private void MatchCountDown_OnCountDownEnded()
        {
            HeroManager.Instance.KillHero();
        }
        
        private void MatchCountDown_OnSecondPassed()
        {
            _secondsPassed++;
            UIManager.Instance.UpdateRemainingSeconds(_playTime - _secondsPassed);
        }
    }
}

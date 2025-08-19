using System;
using System.Collections;
using Game.BaseEnemy;
using Game.BaseHero;
using Game.Input;
using Game.UI;
using Game.Utils;
using UI.ScreenFader;
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
            ScreenFader.Instance.FadeOut();
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
            StartGameChoreography();
        }

        private void StartGameChoreography()
        {
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
            EnemyManager.Instance.OnEnemyKilled -= EnemyManager_OnEnemyKilled;
            EnemyManager.Instance.OnEnemyKilled += EnemyManager_OnEnemyKilled;
            HeroManager.Instance.OnHeroDeath += HeroManager_OnHeroDeath;
            _enemiesKilled = 0;
            _secondsPassed = 0;
            _matchCountDown.StartCounting();
            UIManager.Instance.OnRetryButtonPressed += OnRetryButtonPressed;
        }

        private void OnRetryButtonPressed()
        {
            StopGamePlay();
            UIManager.Instance.OnRetryButtonPressed -= OnRetryButtonPressed;
            StartCoroutine(ResetGameChoreography());
        }

        private IEnumerator ResetGameChoreography()
        {
            float fadeInDuration = ScreenFader.Instance.FadeIn(true);
            yield return new WaitForSeconds(fadeInDuration);
            _matchCountDown.SetTotalTime(_playTime);
            HeroManager.Instance.Restart();
            EnemyManager.Instance.Restart();
            UIManager.Instance.Restart(_enemiesToKill, _playTime);
            float fadeOutDuration = ScreenFader.Instance.FadeOut();
            yield return new WaitForSeconds(fadeOutDuration);
            StartGameChoreography();
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
            StopGamePlay();
            UIManager.Instance.ShowEndGame(win);
        }

        private void StopGamePlay()
        {
            EnemyManager.Instance.OnEnemyKilled -= EnemyManager_OnEnemyKilled;
            HeroManager.Instance.OnHeroDeath -= HeroManager_OnHeroDeath;
            _matchCountDown.StopCounting();
            Joystick.Instance.Disable();
            TouchInputManager.Instance.Disable();
            EnemyManager.Instance.Disable();
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

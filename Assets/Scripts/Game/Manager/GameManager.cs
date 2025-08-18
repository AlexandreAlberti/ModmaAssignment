using Game.BaseEnemy;
using Game.BaseHero;
using Game.Input;
using UI;
using UnityEngine;

namespace Game
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
            TouchInputManager.Instance.Enable();
            Joystick.Instance.Enable();
            ObjectPool.Instance.ActivatePooling();
            EnemyManager.Instance.Enable();
            UIManager.Instance.Initialize();
        }
    }
}

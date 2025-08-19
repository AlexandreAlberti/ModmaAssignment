using System;
using UnityEngine;

namespace Game.BaseHero
{
    public class HeroManager : EnablerMonoBehaviour
    {
        private const int UNIT_MAX_HEALTH = 50;
        public static HeroManager Instance  { get; private set; }

        public Action OnHeroDeath;
        private Hero _hero;

        private void Awake()
        {
            Instance = this;
        }

        public void Initialize()
        {
            _hero = FindObjectOfType<Hero>();
            _hero.Initialize(UNIT_MAX_HEALTH);
            _hero.OnHeroDeath += Hero_OnHeroDeath;
        }

        public Vector3 GetHeroPosition()
        {
            return _hero.transform.position;
        }

        public void ChangeHeroWeapon(int newWeapon)
        {
            _hero.ChangeWeapon(newWeapon);
        }

        private void Hero_OnHeroDeath()
        {
            OnHeroDeath?.Invoke();
        }

        public void KillHero()
        {
            _hero.KillHero(UNIT_MAX_HEALTH);
        }

        public void Restart()
        {
            _hero.Restart(UNIT_MAX_HEALTH);
            _hero.transform.position = Vector3.zero;
        }
    }
}

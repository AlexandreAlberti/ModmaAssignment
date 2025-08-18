using UnityEngine;

namespace Game.BaseHero
{
    public class HeroManager : EnablerMonoBehaviour
    {
        public static HeroManager Instance  { get; private set; }
        
        private Hero _hero;

        private void Awake()
        {
            Instance = this;
        }

        public void Initialize()
        {
            _hero = FindObjectOfType<Hero>();
            _hero.Initialize(50);
        }

        public Vector3 GetHeroPosition()
        {
            return _hero.transform.position;
        }

        public void ChangeHeroWeapon(int newWeapon)
        {
            _hero.ChangeWeapon(newWeapon);
        }
    }
}

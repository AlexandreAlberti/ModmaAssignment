using UnityEngine;

namespace Game.UI
{
    public class EndGamePanel : MonoBehaviour
    {
        [SerializeField] public GameObject _winText;
        [SerializeField] public GameObject _loseText;
        public void Initialize()
        {
            _winText.SetActive(false);
            _loseText.SetActive(false);
        }

        public void ShowWinText()
        {
            _winText.SetActive(true);
        }

        public void ShowLoseText()
        {
            _loseText.SetActive(true);
        }
    }
}

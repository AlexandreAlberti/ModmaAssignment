using Cinemachine;
using UnityEngine;

namespace Game.Manager
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;

        public static CameraManager Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }
        
        public CinemachineVirtualCamera Camera()
        {
            return _camera;
        }
    }
}

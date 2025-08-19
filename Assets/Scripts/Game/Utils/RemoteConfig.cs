using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Game.Utils
{
    public class RemoteConfig : MonoBehaviour
    {
        private const string PLAY_TIME_KEY = "PlayTime";
        private const string ENEMIES_TO_KILL_KEY = "EnemiesToKill";
     
        public static RemoteConfig Instance { get; private set; }
        
        public Action<int, int> OnRemoteConfigLoaded;

        private struct userAttributes {}
        private struct appAttributes {}

        private void Awake()
        {
            Instance = this;
        }

        public async Task Initialize()
        {
            if (Utilities.CheckForInternetConnection())
            {
                await InitializeRemoteConfigAsync();
            }

            RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
            RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
        }
        
        private async Task InitializeRemoteConfigAsync()
        {
            // initialize handlers for unity game services
            await UnityServices.InitializeAsync();

            // remote config requires authentication for managing environment information
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }


        private void ApplyRemoteSettings(ConfigResponse configResponse)
        {
            Debug.Log("RemoteConfigService.Instance.appConfig fetched: " + RemoteConfigService.Instance.appConfig.config);
            
            OnRemoteConfigLoaded?.Invoke(
                RemoteConfigService.Instance.appConfig.GetInt(PLAY_TIME_KEY), 
                RemoteConfigService.Instance.appConfig.GetInt(ENEMIES_TO_KILL_KEY)
            );
        }
    }
}

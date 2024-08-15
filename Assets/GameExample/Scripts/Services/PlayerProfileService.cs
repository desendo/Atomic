using System;
using Atomic.Elements;
using GameExample.Scripts.Data;
using GameExample.Scripts.Services.StorageHandler;
using UnityEngine;

namespace GameExample.Scripts.Services
{
    public interface IPlayerProfileService
    {
        void LoadProfile(Action<PlayerData> callback, bool reset = false);
        void SaveProfile(bool isLocalOnly = false);
        void ClearLocalStorages();
        ReactiveVariable<bool> IsPlayerDataLoaded { get; }
        ReactiveVariable<PlayerData> PlayerDataLoaded { get; }
        PlayerData CurrentPlayerData { get; }
    }

    public class PlayerProfileService : IPlayerProfileService
    {

        private readonly ReactiveVariable<bool> _isPlayerDataLoaded = new ReactiveVariable<bool>(false);
        private readonly ReactiveVariable<PlayerData> _playerDataLoaded = new ReactiveVariable<PlayerData>(null);
        private readonly PlayerPrefsStorageHandler<PlayerData> _playerPrefs;
        private readonly ScriptableObjectStorageHandler<PlayerData> _scriptableObjectStorageHandler;

        public PlayerProfileService()
        {
            _playerPrefs = Di.Instance.Get<PlayerPrefsStorageHandler<PlayerData>>();
            _scriptableObjectStorageHandler = Di.Instance.Get<ScriptableObjectStorageHandler<PlayerData>>();
        }
        public PlayerData CurrentPlayerData { get; private set; } = new PlayerData();
        private int CurrentVersion { get; set; } = -1;

        public ReactiveVariable<bool> IsPlayerDataLoaded => _isPlayerDataLoaded;
        public ReactiveVariable<PlayerData> PlayerDataLoaded => _playerDataLoaded;

        public void LoadProfile(Action<PlayerData> callback, bool reset = false)
        {
            _isPlayerDataLoaded.Value = false;
            if (reset)
            {
                _scriptableObjectStorageHandler.GetData(defaultData =>
                {
                    CurrentPlayerData = defaultData.Copy();
                    CurrentVersion = 0;

                    Log("Load player data: choose DEFAULT on reset");
                    callback.Invoke(CurrentPlayerData);
                    _isPlayerDataLoaded.Value = true;

                });
            }
            else
            {
                _playerPrefs.GetVersion(localVersion =>
                {
                    Log("LoadProfile prefs version "+localVersion);

                    if (localVersion >= 0)
                        _playerPrefs.GetData(localData =>
                        {
                            if (localData != null)
                            {
                                CurrentPlayerData = localData;
                                CurrentVersion = localVersion;
                                callback.Invoke(CurrentPlayerData);
                                _isPlayerDataLoaded.Value = true;
                            }
                            else
                                LoadProfile(callback, true);

                        });
                    else
                        _scriptableObjectStorageHandler.GetData(defaultData =>
                        {
                            CurrentPlayerData = defaultData.Copy();
                            CurrentVersion = 0;
                            _isPlayerDataLoaded.Value = true;

                            Log("Load player data: choose DEFAULT on start");
                            callback.Invoke(CurrentPlayerData);

                        });
                });


            }
        }

        public void SaveProfile(bool isLocalOnly = false)
        {
            var playerData = CurrentPlayerData;

            if (CurrentVersion < 0)
                CurrentVersion = 0;
            CurrentVersion++;
            playerData.Version = CurrentVersion;
            _playerPrefs.SaveData(playerData);
            Log("Save player data: local ok");
        }

        public void ClearLocalStorages()
        {
            _playerPrefs.Clear();
        }

        private void Log(object obj)
        {
            Debug.Log(obj);
        }

        private void LogWarning(object obj)
        {

            Debug.LogWarning(obj);
        }
    }
}
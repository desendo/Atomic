using Services.StorageHandler;
using UnityEngine;

namespace GameExample.Scripts.Data
{
    [System.Serializable]
    public class PlayerData : IData, IVersion
    {
        [SerializeField]
        private int _currentVersion = -1;

        public int CurrentPlayerIndex;

        public PlayerData Copy()
        {
            var serialized = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(serialized);
        }

        public int Version
        {
            get => _currentVersion ;
            set => _currentVersion = value;
        }
    }


}
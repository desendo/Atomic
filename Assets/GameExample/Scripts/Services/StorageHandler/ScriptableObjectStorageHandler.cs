using System;
using GameExample.Scripts.Data;
using Services.StorageHandler;
using UnityEditor;

namespace GameExample.Scripts.Services.StorageHandler
{
    public class ScriptableObjectStorageHandler<T> : IStorage, IStorageReadable<T>, IStorageWritable<T> where T : class, IData
    {
        private readonly DataContainer<T> _dataScriptableObject;


        public ScriptableObjectStorageHandler()
        {
            _dataScriptableObject = Di.Instance.Get<DataContainer<T>>();
        }

        public ConfigStorageType SourceType => ConfigStorageType.ScriptableObject;

        public void GetData(Action<T> onGetData, T defaultData = default(T))
        {
            onGetData.Invoke(_dataScriptableObject.Data);
        }

        public void SetData(T data, Action<bool> onComplete = null)
        {
            _dataScriptableObject.Data = data;
#if UNITY_EDITOR
            EditorUtility.SetDirty(_dataScriptableObject);
#endif
        }
    }
}
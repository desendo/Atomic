using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameExample.Scripts
{
    [CreateAssetMenu(fileName = "PrefabsConfig", menuName = "PrefabsConfig", order = 0)]
    public class PrefabsConfig : ScriptableObject
    {
        [SerializeField] private List<PrefabEntry> _list;

        private readonly Dictionary<Type, PrefabEntry> _entriesCache = new Dictionary<Type, PrefabEntry>();

        public GameObject Get(string id)
        {
            foreach (var prefabEntry in _list)
            {
                if (prefabEntry.Id == id)
                    return prefabEntry.prefab;
            }

            return null;
        }

        public T Get<T>() where T: MonoBehaviour
        {
            foreach (var prefabEntry in _list)
            {
                if (prefabEntry.prefab.TryGetComponent(out T component))
                {
                    _entriesCache.Add(typeof(T), prefabEntry);
                    return component;
                }
            }

            return null;
        }

        [Serializable]
        private class PrefabEntry
        {
            public string Id;
            public GameObject prefab;
        }
    }
}
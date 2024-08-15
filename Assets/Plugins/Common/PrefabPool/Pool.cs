using System.Collections.Generic;
using UnityEngine;

namespace Common.PrefabPool
{
    public class Pool
    {
        private Transform _parent;
        public static Pool Instance { get; private set; }

        private readonly Dictionary<int, GameObjectPool> _pools = new Dictionary<int, GameObjectPool>();
        public void Init(Transform poolsParent)
        {
            if(Instance !=null )
                return;

            Instance = this;

            _parent = poolsParent;
        }

        private class GameObjectPool
        {
            public GameObject Prefab;
            public readonly HashSet<int> InstancesHash = new HashSet<int>();
            public Transform Parent;

            public void Despawn(GameObject gameObject, int id)
            {
                if(gameObject.TryGetComponent(out IPooled pooled))
                {
                    pooled.OnDespawn();
                }

                gameObject.transform.SetParent(Parent);
                InstancesHash.Remove(id);

            }

            public GameObject Spawn(Transform parent = null)
            {
                if (Parent.childCount > 0)
                {
                    var instance = Parent.GetChild(0).gameObject;
                    InstancesHash.Add(instance.GetInstanceID());
                    instance.transform.SetParent(parent);
                    instance.GetComponent<IPooled>()?.OnSpawn();

                    return instance;
                }
                else
                {
                    var instance = Object.Instantiate(Prefab, null, true);
                    InstancesHash.Add(instance.GetInstanceID());
                    instance.GetComponent<IPooled>()?.OnSpawn();

                    return instance;
                }
            }
        }
        public GameObject Spawn(GameObject prefab)
        {
            var prefabId = prefab.GetInstanceID();
            if (!_pools.ContainsKey(prefabId))
            {
                InitGameObjectPool(prefab);
            }

            var instance = _pools[prefabId].Spawn();
            return instance;
        }

        private void InitGameObjectPool(GameObject prefab)
        {
            var prefabId = prefab.GetInstanceID();

            var ui = prefab.transform is RectTransform;

            var poolContainer = new GameObject();
            if (ui)
                poolContainer.AddComponent<Canvas>();

            poolContainer.transform.SetParent(_parent);
            poolContainer.transform.localPosition = Vector3.zero;
            poolContainer.SetActive(false);
            poolContainer.name = $"{prefab.name}_pool";

            var pool = new GameObjectPool();
            pool.Prefab = prefab;
            pool.Parent = poolContainer.transform;

            _pools.Add(prefabId, pool);

        }

        public void Despawn(GameObject gameObject)
        {
            var id = gameObject.GetInstanceID();
            foreach (var (key, value) in _pools)
            {
                if (value.InstancesHash.Contains(id))
                {
                    value.Despawn(gameObject, id);
                    return;
                }
            }
            Debug.LogError($"{gameObject.name} Does not have pool");
        }

    }

    public interface IPooled
    {
        public void OnSpawn();
        public void OnDespawn();
    }
}
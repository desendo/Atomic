using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



namespace Atomic.Entities
{
    [AddComponentMenu("Atomic/Entities/Entity World")]
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-1000)]
    public sealed class SceneEntityWorld : MonoBehaviour, IEntityWorld
    {
        #region Main

        private readonly EntityWorld _world = new();

        [SerializeField]
        private bool autoRefresh = true;

        [Space]
        [SerializeField]
        private bool unityName = true;

        [SerializeField]
        private bool scanEntities = true;


        [SerializeField]
        private bool includeInactiveOnScan = true;


        public string Name
        {
            get => _world.Name;
            set => _world.Name = value;
        }

        private void Awake()
        {
            if (this.unityName)
            {
                _world.Name = name;
            }
        }

        private void Start()
        {
            if (this.scanEntities)
            {
                this.AddAllEntitiesFromScene(this.includeInactiveOnScan);
            }
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (this.autoRefresh && !EditorApplication.isPlaying && !EditorApplication.isCompiling)
            {
                try
                {
                    this.RefreshInEditMode();
                }
                catch (Exception)
                {
                    // ignored
                }
            }
#endif
        }


        private void RefreshInEditMode()
        {
            _world.Name = name;
            this.AddAllEntitiesFromScene();
        }

        #endregion

        #region Entities

        public event Action OnStateChanged
        {
            add => _world.OnStateChanged += value;
            remove => _world.OnStateChanged -= value;
        }

        public event Action<IEntity> OnEntityAdded
        {
            add => _world.OnEntityAdded += value;
            remove => _world.OnEntityAdded -= value;
        }

        public event Action<IEntity> OnEntityDeleted
        {
            add => _world.OnEntityDeleted += value;
            remove => _world.OnEntityDeleted -= value;
        }


        public int EntityCount
        {
            get { return _world.EntityCount; }
        }


        public IReadOnlyList<IEntity> Entities
        {
            get { return _world.Entities; }
        }

        public IEntity GetEntityWithTag(int tag)
        {
            return _world.GetEntityWithTag(tag);
        }

        public IReadOnlyList<IEntity> GetEntitiesWithTag(int tag)
        {
            return _world.GetEntitiesWithTag(tag);
        }

        public int GetEntitiesWithTag(int tag, IEntity[] results)
        {
            return _world.GetEntitiesWithTag(tag, results);
        }

        public IEntity GetEntityWithValue(int valueId)
        {
            return _world.GetEntityWithValue(valueId);
        }

        public IReadOnlyList<IEntity> GetEntitiesWithValue(int valueId)
        {
            return _world.GetEntitiesWithValue(valueId);
        }

        public int GetEntitiesWithValue(int valueId, IEntity[] results)
        {
            return _world.GetEntitiesWithValue(valueId, results);
        }


        public bool HasEntity(IEntity entity)
        {
            return _world.HasEntity(entity);
        }

        public int CopyEntitiesTo(IEntity[] results)
        {
            return _world.CopyEntitiesTo(results);
        }

        public bool AddEntity(IEntity entity)
        {
            return _world.AddEntity(entity);
        }


        public bool DelEntity(IEntity entity)
        {
            return _world.DelEntity(entity);
        }


        public void ClearEntities()
        {
            _world.ClearEntities();
        }

        #endregion

        #region Lifecycle


        public void InitEntities()
        {
            _world.InitEntities();
        }

        public void EnableEntities()
        {
            _world.EnableEntities();
        }

        public void DisableEntities()
        {
            _world.DisableEntities();
        }

        public void DisposeEntities()
        {
            _world.DisposeEntities();
        }

        public void UpdateEntities(float deltaTime)
        {
            _world.UpdateEntities(deltaTime);
        }

        public void FixedUpdateEntities(float deltaTime)
        {
            _world.FixedUpdateEntities(deltaTime);
        }

        public void LateUpdateEntities(float deltaTime)
        {
            _world.LateUpdateEntities(deltaTime);
        }

        #endregion

        #region Static

        public static SceneEntityWorld Instantiate(
            string name = null,
            bool scanEntities = false,
            params IEntity[] entities
        )
        {
            GameObject gameObject = new GameObject(name);
            SceneEntityWorld world = gameObject.AddComponent<SceneEntityWorld>();
            world.Name = name;
            world.scanEntities = scanEntities;

            world.AddEntities(entities);
            return world;
        }

        #endregion
    }
}
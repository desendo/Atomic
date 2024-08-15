using System;
using Atomic.Entities;
using UnityEngine;

namespace GameExample.Scripts.EntityPresenters
{
    [Serializable]
    public class CharacterEntityViewInstaller : IEntityInstaller, IEntityUnInstaller
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Transform _transform;
        public void Install(IEntity entity)
        {
            entity.AddGameObject(_gameObject);
            entity.AddTransform(_transform);
        }

        public void Uninstall(IEntity entity)
        {
            entity.DelTransform();
            entity.DelGameObject();
        }
    }
}
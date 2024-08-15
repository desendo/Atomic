using System;
using System.Collections.Generic;
using Atomic.Entities;
using UnityEngine;

namespace GameExample.Scripts.EntityPresenters
{
    public class EntityPresenterBase : MonoBehaviour
    {
        [SerializeField] private string _waitForName;

        protected readonly List<IEntityInstaller> _installers = new List<IEntityInstaller>();
        protected readonly List<IEntityUnInstaller> _uninstallers = new List<IEntityUnInstaller>();


        public void Bind(IEntity entity)
        {
        }

        protected virtual void InstallEntity(IEntity entity)
        {
            foreach (var installer in _installers)
            {
                installer.Install(entity);
            }
        }
        protected virtual void UnInstallEntity(IEntity entity)
        {

            foreach (var entityUnInstaller in _uninstallers)
            {
                entityUnInstaller.Uninstall(entity);
            }
        }

    }
}

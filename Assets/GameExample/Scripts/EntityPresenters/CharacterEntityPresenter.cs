using GameExample.Content;
using UnityEngine;

namespace GameExample.Scripts.EntityPresenters
{
    public class CharacterEntityPresenter : EntityPresenterBase
    {
        [SerializeField] private CharacterEntityViewInstaller _characterEntityViewInstaller;
        private void Awake()
        {
            _installers.Add(_characterEntityViewInstaller);
            _uninstallers.Add(_characterEntityViewInstaller);
        }
    }
}
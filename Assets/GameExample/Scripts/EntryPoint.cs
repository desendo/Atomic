using Atomic.Entities;
using Common;
using Common.PrefabPool;
using Game.Services;
using GameExample.Scripts.Rules;
using GameExample.Scripts.Services;
using Services;
using UnityEngine;

namespace GameExample.Scripts
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private PrefabsConfig _prefabsConfig;
        
        public static readonly Pool Pool = new Pool();
        private UpdateProvider _updateProvider;

        private void Awake()
        {

            DontDestroyOnLoad(gameObject);

            Pool.Init(transform);
            var world = new EntityWorld();
            Di.Instance.Add(world);

            Di.Instance.Add(_prefabsConfig);
            _updateProvider = Di.Instance.Add<UpdateProvider>();
            Di.Instance.Add<SignalBus>();

            Di.Instance.CreateInject<JsonService>();
            Di.Instance.CreateInject<PlayerProfileService>();
            Di.Instance.CreateInject<LoadMapRule>();

            /*
            Di.Instance.AddInject<TestRule>();
            Di.Instance.AddInject<LoadMapRule>();*/
        }

        private void Update()
        {
            _updateProvider.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            _updateProvider.FixedUpdate(Time.fixedDeltaTime);
        }

    }
}

using UnityEngine;

namespace GameExample.Scripts.Data
{
    [CreateAssetMenu(menuName = "Create PlayerDataContainer", fileName = "PlayerDataContainer", order = 0)]

    public class PlayerDataContainer : DataContainer<PlayerData>
    {
    }
}
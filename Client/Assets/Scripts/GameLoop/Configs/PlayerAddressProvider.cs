using GameLoop.Entities;
using UnityEngine;
using Utils;

namespace GameLoop.Configs
{
    [CreateAssetMenu(fileName = "PlayerAddressProvider", menuName = "Game/PrefabConfigs/PlayerAddressProvider")]
    public class PlayerAddressProvider : GameObjectPoolAddressProvider<PlayerComponent> { }
}
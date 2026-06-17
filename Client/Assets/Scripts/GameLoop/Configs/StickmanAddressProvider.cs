using GameLoop.Entities;
using UnityEngine;
using Utils;

namespace GameLoop.Configs
{
    [CreateAssetMenu(fileName = "StickmanAddressProvider", menuName = "Game/PrefabConfigs/StickmanAddressProvider")]
    public class StickmanAddressProvider : GameObjectPoolAddressProvider<StickmanComponent> { }
}
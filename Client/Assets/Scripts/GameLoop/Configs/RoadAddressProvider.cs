using GameLoop.Entities;
using UnityEngine;
using Utils;

namespace GameLoop.Configs
{
    [CreateAssetMenu(fileName = "RoadAddressProvider", menuName = "Game/PrefabConfigs/RoadAddressProvider")]
    public class RoadAddressProvider : GameObjectPoolAddressProvider<RoadComponent> { }
}
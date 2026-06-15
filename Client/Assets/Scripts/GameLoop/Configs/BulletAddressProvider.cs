using GameLoop.Entities;
using UnityEngine;
using Utils;

namespace GameLoop.Configs
{
    // It is boilerplaty, but most efficient way, if we want to extend system easely
    // I generally use custom inspector window insted of create asset menu items for stuff like this,
    // as it will be mostly responsibility of tech staff, we can type in name of class, avoiding overcrowding menu
    [CreateAssetMenu(fileName = "BulletAddressProvider", menuName = "Game/Configs/BulletAddressProvider")]
    public class BulletAddressProvider : GameObjectPoolAddressProvider<BulletComponent> { }
}
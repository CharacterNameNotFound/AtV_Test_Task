using GameLoop.Entities;
using UnityEngine;
using Utils;

namespace GameLoop.Configs
{
    // It is boilerplaty, but most efficient way, if we want to extend system easily
    // I generally use custom inspector window instead of create asset menu items for stuff like this,
    // as it will be mostly responsibility of tech staff, we can type in name of class, avoiding overcrowding menu
    // On other hand, we can compress all providers into one, using interface instead of base class
    [CreateAssetMenu(fileName = "BulletAddressProvider", menuName = "Game/Configs/BulletAddressProvider")]
    public class BulletAddressProvider : GameObjectPoolAddressProvider<BulletComponent> { }
}
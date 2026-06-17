using UnityEngine;
using Utils;

namespace VFX.FlyingText
{
    [CreateAssetMenu(fileName = "FlyingTextConfigs", menuName = "Game/VFX configs/FlyingTextConfigs")]
    public class FlyingTextConfigs : GameObjectPoolAddressProvider<FlyingTextComponent>, IFlyingTextConfigs
    {
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public Vector3 FlightVector { get; private set; }
    }
}
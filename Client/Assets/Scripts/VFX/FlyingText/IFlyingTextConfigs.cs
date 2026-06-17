using UnityEngine;
using UnityEngine.AddressableAssets;

namespace VFX.FlyingText
{
    public interface IFlyingTextConfigs
    {
        public float Duration { get; }
        public Vector3 FlightVector { get; }
    }
}
using UnityEngine;

namespace VFX.ColorBlink
{
    public interface IColorBlinkAnimationConfigurationsProvider
    {
        public Material BlinkMaterial { get; }
        public float BlinkDuration { get; }
        public float RelaxationDuration { get; }
        public int BlinkCycles { get; }
    }
}
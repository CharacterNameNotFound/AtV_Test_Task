using UnityEngine;

namespace VFX.ColorBlink
{
    [CreateAssetMenu(fileName = "ColorBlinkAnimationConfigurationsProvider", menuName = "Game/VFX configs/ColorBlinkAnimationConfigurationsProvider")]
    public class ColorBlinkAnimationConfigurationsProvider : ScriptableObject, IColorBlinkAnimationConfigurationsProvider
    {
        [field: SerializeField] public Material BlinkMaterial { get; private set; }
        [field: SerializeField] public float BlinkDuration { get; private set; }
        [field: SerializeField] public float RelaxationDuration { get; private set; }
        [field: SerializeField] public int BlinkCycles { get; private set; }
    }
}
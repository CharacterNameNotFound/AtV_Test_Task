using UnityEngine;

namespace GameLoop.Configs
{
    [CreateAssetMenu(fileName = "LevelConfigsProvider", menuName = "Game/GameplayConfigs/LevelConfigsProvider")]
    public class LevelConfigsProvider : ScriptableObject, ILevelConfigsProvider
    {
        [field: SerializeField] public float LevelLength { get; private set; }
    }
}
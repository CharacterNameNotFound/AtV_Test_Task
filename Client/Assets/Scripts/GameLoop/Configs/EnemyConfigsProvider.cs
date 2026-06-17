using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameLoop.Configs
{
    [CreateAssetMenu(fileName = "EnemySpawningConfigs", menuName = "Game/GameplayConfigs/EnemySpawningConfigs")]
    public class EnemyConfigsProvider : ScriptableObject, IEnemyConfigsProvider
    {
        [field: Header("Spawning positions")]
        [field: SerializeField] public float BasicSpawningCountPerSection { get; private set; }
        [field: SerializeField] public float SpawningCountPerSectionLengthModifier { get; private set; }
        [field: SerializeField] public float SpawnStartOffset { get; private set; }
        [field: SerializeField] public float SpawnPeriod { get; private set; }
        [field: SerializeField] public float PreSpawnLength { get; private set; }
        [field: SerializeField] public float SpawnWidth { get; private set; }
        
        [field: Header("Stickman configs")]
        [field: SerializeField] public float Hp { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float ReactionRadius { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject EnemyDeathAssetReference { get; private set; }

    }
}
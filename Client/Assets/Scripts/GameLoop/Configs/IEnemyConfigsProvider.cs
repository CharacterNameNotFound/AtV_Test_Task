using UnityEngine.AddressableAssets;

namespace GameLoop.Configs
{
    public interface IEnemyConfigsProvider
    {
        public float BasicSpawningCountPerSection { get; }
        public float SpawningCountPerSectionLengthModifier { get; }
        public float SpawnStartOffset { get; }
        public float SpawnPeriod { get; }
        public float PreSpawnLength { get; }
        public float SpawnWidth { get; }
        public float Hp { get; }
        public float Speed { get; }
        public float Damage { get; }
        public float ReactionRadius { get; }
        public AssetReferenceGameObject EnemyDeathAssetReference { get; }
    }
}
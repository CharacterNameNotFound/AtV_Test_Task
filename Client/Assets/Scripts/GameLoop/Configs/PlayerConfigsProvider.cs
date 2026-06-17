using UnityEngine;

namespace GameLoop.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfigsProvider", menuName = "Game/GameplayConfigs/PlayerConfigsProvider")]
    public class PlayerConfigsProvider : ScriptableObject, IPlayerConfigsProvider
    {
        [field: Header("Player car configs")]
        [field: SerializeField] public float StartingHp { get; private set; }
        [field: SerializeField] public float Speed { get; private set;}
        
        [field: Header("Shooting configs")]
        [field: SerializeField] public float Damage { get; private set;}
        [field: SerializeField] public float ShootingCooldown { get; private set; }
        [field: SerializeField] public float BulletFlightSpeed { get; private set; }
        [field: SerializeField] public float TurretRotationConstraint { get; private set; }
        
        [field: Header("VisualConfigs")]
        [field: SerializeField] public int MaxParticleIntensity { get; private set; }
        [field: SerializeField] public float ParticleStartThreshold { get; private set; }
        [field: SerializeField] public float HorizontalMovementAmplitude { get; private set; }
        [field: SerializeField] public float HorizontalMovementPeriodMult { get; private set; }
    }
}
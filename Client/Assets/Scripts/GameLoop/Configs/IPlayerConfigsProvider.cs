namespace GameLoop.Configs
{
    public interface IPlayerConfigsProvider
    {
        public float StartingHp { get; }
        public float Speed { get; }
        public float Damage { get; }
        public float ShootingCooldown { get; }
        public float BulletFlightSpeed { get; }
        public float TurretRotationConstraint { get; }
        public int MaxParticleIntensity { get; }
        public float ParticleStartThreshold { get; }
    }
}
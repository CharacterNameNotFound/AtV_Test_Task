using System.Threading;
using Cysharp.Threading.Tasks;
using GameLoop.Configs;
using GameLoop.Entities;
using UnityEngine;

namespace GameLoop.Systems.Player
{
    public class PlayerHpUpdater : ILoopedSystem
    {
        private static readonly int FillValue = Shader.PropertyToID("_FillValue");
        private IPlayerConfigsProvider _playerConfigsProvider;

        public PlayerHpUpdater(IPlayerConfigsProvider playerConfigsProvider)
        {
            _playerConfigsProvider = playerConfigsProvider;
        }

        public UniTask Initialize(GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            gameRegistry.PlayerComponent.Hp = _playerConfigsProvider.StartingHp;
            gameRegistry.PlayerComponent.MaxHp = _playerConfigsProvider.StartingHp;
            
            UpdateParticleIntensity(gameRegistry.PlayerComponent);
            gameRegistry.PlayerComponent.HpBar.material.SetFloat(FillValue, 1);
            return UniTask.CompletedTask;
        }

        public UniTask Reset(GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            gameRegistry.PlayerComponent.Hp = _playerConfigsProvider.StartingHp;
            gameRegistry.PlayerComponent.MaxHp = _playerConfigsProvider.StartingHp;
            
            UpdateParticleIntensity(gameRegistry.PlayerComponent);
            gameRegistry.PlayerComponent.HpBar.material.SetFloat(FillValue, 1);
            return UniTask.CompletedTask;
        }

        public UniTask Loop(float deltaTime, GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            gameRegistry.PlayerComponent.HpBar.material.SetFloat(
                FillValue, 
                gameRegistry.PlayerComponent.Hp / gameRegistry.PlayerComponent.MaxHp);

            UpdateParticleIntensity(gameRegistry.PlayerComponent);
            
            return UniTask.CompletedTask;
        }

        private void UpdateParticleIntensity(PlayerComponent playerComponent)
        {
            ParticleSystem.EmissionModule emissionModule = playerComponent.FireParticleSystem.emission;

            float hpPercentile = playerComponent.Hp / playerComponent.MaxHp;
            bool inParticleRange = hpPercentile <= _playerConfigsProvider.ParticleStartThreshold;

            float particleIntensity = inParticleRange
                ? (_playerConfigsProvider.ParticleStartThreshold - hpPercentile) *
                (1 / _playerConfigsProvider.ParticleStartThreshold) * _playerConfigsProvider.MaxParticleIntensity
                : 0;

            emissionModule.rateOverTime = particleIntensity;
        }
        
    }
}
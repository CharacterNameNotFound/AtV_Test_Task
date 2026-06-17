using System.Threading;
using Cysharp.Threading.Tasks;
using GameLoop.Configs;
using GameLoop.Entities;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using Utils;

namespace GameLoop.Systems.Player
{
    public class PlayerMover : ILoopedSystem
    {
        private readonly PlayerAddressProvider _playerAddressProvider;
        private readonly GameRootTransformProvider _gameRootTransformProvider;
        private readonly IPlayerConfigsProvider _playerConfigsProvider;

        
        public PlayerMover(
            PlayerAddressProvider playerAddressProvider, 
            GameRootTransformProvider gameRootTransformProvider, 
            IPlayerConfigsProvider playerConfigsProvider)
        {
            _playerAddressProvider = playerAddressProvider;
            _gameRootTransformProvider = gameRootTransformProvider;
            _playerConfigsProvider = playerConfigsProvider;
        }

        // generally this should be out of work scope of the system, but squashing all as requested
        public async UniTask Initialize(GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            PlayerComponent playerComponent = await _playerAddressProvider.AssetReferenceGameObject.Instantiate<PlayerComponent>(
                new InstantiationParameters(_gameRootTransformProvider.GameRoot, false), 
                cancellationToken);

            playerComponent.Transform.forward = Vector3.right;
            
            gameRegistry.PlayerComponent = playerComponent;
        }

        public UniTask Reset(GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            gameRegistry.PlayerComponent.Transform.position = Vector3.zero;
            foreach (TrailRenderer item in gameRegistry.PlayerComponent.Trails)
            {
                item.Clear();
            }
            
            return UniTask.CompletedTask;
        }

        public UniTask Loop(float deltaTime, GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            float speed = _playerConfigsProvider.Speed; 
            Vector3 transformPosition = gameRegistry.PlayerComponent.transform.position + new Vector3(speed * deltaTime, 0, 0);

            float angle = transformPosition.x * Mathf.Deg2Rad * _playerConfigsProvider.HorizontalMovementPeriodMult;
            transformPosition.z = Mathf.Sin(angle) * _playerConfigsProvider.HorizontalMovementAmplitude;
            
            gameRegistry.PlayerComponent.Rigidbody.MovePosition(transformPosition);
            
            return UniTask.CompletedTask;
        }
        
    }
}
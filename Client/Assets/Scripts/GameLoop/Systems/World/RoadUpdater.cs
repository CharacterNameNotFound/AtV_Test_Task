using System.Threading;
using Cysharp.Threading.Tasks;
using GameLoop.Entities;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using Utils;

namespace GameLoop.Systems.World
{
    // generally it is required to repivot "world center" from time to time due to float precision, but taking into account level size, we do not need it
    public class RoadUpdater : ILoopedSystem
    {
        private const int PoolSize = 5;

        private readonly GameObjectPoolAddressProvider<RoadComponent> _addressProvider;
        private readonly GameRootTransformProvider _gameRootTransformProvider;
        
        private readonly Transform _poolHolder;
        private readonly RoadComponent[] _roadSections;
        
        private float _roadSectorLength;
        private int _lastSectionIndex;
        private int _playerCurrentSection;
        
        public RoadUpdater(
            GameObjectPoolAddressProvider<RoadComponent> addressProvider, 
            GameRootTransformProvider gameRootTransformProvider)
        {
            _addressProvider = addressProvider;
            _gameRootTransformProvider = gameRootTransformProvider;

            _roadSections = new RoadComponent[PoolSize];
            _poolHolder = new GameObject($"{nameof(RoadComponent)}").transform;
        }

        // generally this should be out of work scope of the system, but squashing all as requested
        public async UniTask Initialize(GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            _poolHolder.SetParent(_gameRootTransformProvider.GameRoot);
            
            for (int i = 0; i < PoolSize; i++)
            {
                _roadSections[i] = await _addressProvider.AssetReferenceGameObject.Instantiate<RoadComponent>(
                    new InstantiationParameters(_poolHolder, false), 
                    cancellationToken);
            }
            
            _roadSectorLength = _roadSections[0].Renderer.bounds.size.x;
            SetRoadsInitialPlacement(0);

            _lastSectionIndex = 0;
            _playerCurrentSection = 0;
        }

        public UniTask Reset(GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            SetRoadsInitialPlacement(0);
            
            _lastSectionIndex = 0;
            _playerCurrentSection = 0;
            
            return UniTask.CompletedTask;
        }

        public UniTask Loop(float deltaTime, GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            int playerSection = Mathf.FloorToInt(gameRegistry.PlayerComponent.Transform.position.x / _roadSectorLength);
            
            if (_playerCurrentSection == playerSection)
            {
                return UniTask.CompletedTask;
            }
            
            MoveLastSection((uint) playerSection);
            _playerCurrentSection++;
                
            return UniTask.CompletedTask;
        }

        private void SetRoadsInitialPlacement(uint startingIndex)
        {
            for (int i = 0; i < PoolSize; i++, startingIndex++)
            {
                // In case we have uniq sequence of roads, this way we will have almost no chance to break it.
                _roadSections[startingIndex % PoolSize].transform.position = new Vector3(_roadSectorLength * i, 0, 0);
            }
        }
        
        private void MoveLastSection(uint playerCurrentSection)
        {
            uint roadPlacementIndex = playerCurrentSection + PoolSize - 1;
            _roadSections[_lastSectionIndex % PoolSize].transform.position = new Vector3(_roadSectorLength * roadPlacementIndex, 0, 0);

            _lastSectionIndex++;
        }
        
    }
}
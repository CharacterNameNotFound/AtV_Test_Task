using System.Threading;
using Cysharp.Threading.Tasks;
using GameLoop.Entities;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using Utils;

namespace GameLoop.Systems.World
{
    // generally it is required to repivot world ceter from time to time due to float precision, but taking into account level size, we do not need it
    public class RoadUpdater : ILoopedSystem
    {
        private const int PoolSize = 5;

        private GameObjectPoolAddressProvider<RoadComponent> _addressProvider;

        private Transform _poolHolder;
        private RoadComponent[] _roadSections;
        private float _roadSectorLength;
        
        public RoadUpdater(GameObjectPoolAddressProvider<RoadComponent> addressProvider)
        {
            _addressProvider = addressProvider;

            _roadSections = new RoadComponent[PoolSize];
            _poolHolder = new GameObject($"{nameof(RoadComponent)} pool").transform;
        }

        public async UniTask Initialize(CancellationToken cancellationToken)
        {
            for (int i = 0; i < PoolSize; i++)
            {
                _roadSections[i] = await _addressProvider.AssetReferenceGameObject.Instantiate<RoadComponent>(
                    new InstantiationParameters(_poolHolder, false), 
                    cancellationToken);
            }
            
            _roadSectorLength = _roadSections[0].Renderer.bounds.size.x;
            PlaceRoads(0, Vector3.zero);
        }

        public UniTask Reset(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public UniTask Loop(float deltaTime, GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        private void PlaceRoads(uint startingIndex, Vector3 startingPosition)
        {
            for (int i = -1; i < PoolSize - 1; i++, startingIndex++)
            {
                // In case we have uniq sequence of roads, this way we will have almost no chance to breake it.
                _roadSections[startingIndex % PoolSize].transform.position = new Vector3(startingPosition.x + _roadSectorLength * i, 0, 0);
            }
        }
        
    }
}
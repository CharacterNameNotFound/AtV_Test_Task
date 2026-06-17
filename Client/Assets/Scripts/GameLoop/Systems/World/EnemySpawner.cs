using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameLoop.Configs;
using GameLoop.Entities;
using GameLoop.Services;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace GameLoop.Systems.World
{
    public class EnemySpawner : ILoopedSystem
    {
        private const int PoolSize = 100;
        
        private readonly GameObjectPool<StickmanComponent> _enemyPool;
        private readonly IEnemyConfigsProvider _enemyConfigs;
        private readonly IEntityResetService _entityResetService;
        private readonly GameRootTransformProvider _gameRootTransformProvider;

        private Transform _activeEnemiesHost;
        private float _nextSectionSpawnBorder;

        public EnemySpawner(
            GameObjectPool<StickmanComponent> enemyPool, 
            IEnemyConfigsProvider enemyConfigs, 
            IEntityResetService entityResetService, 
            GameRootTransformProvider gameRootTransformProvider)
        {
            _enemyPool = enemyPool;
            _enemyConfigs = enemyConfigs;
            _entityResetService = entityResetService;
            _gameRootTransformProvider = gameRootTransformProvider;
        }

        public async UniTask Initialize(GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            _activeEnemiesHost = new GameObject("Active Enemies host").transform;
            _activeEnemiesHost.SetParent(_gameRootTransformProvider.GameRoot);
            
            gameRegistry.Stickmans = new List<StickmanComponent>(PoolSize);
            await _enemyPool.Extend(PoolSize, cancellationToken);
            await SpawnInitialEnemies(gameRegistry, cancellationToken);
            _nextSectionSpawnBorder = _enemyConfigs.SpawnPeriod;
        }

        public async UniTask Reset(GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            for (int i = 0; i < gameRegistry.Stickmans.Count; i++)
            {
                StickmanComponent enemy = gameRegistry.Stickmans[i];
                _entityResetService.Reset(enemy);
                _enemyPool.Return(enemy);
            }
            
            gameRegistry.Stickmans.Clear();
            
            await SpawnInitialEnemies(gameRegistry, cancellationToken);
            _nextSectionSpawnBorder = _enemyConfigs.SpawnPeriod;
        }

        public async UniTask Loop(float deltaTime, GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            if (gameRegistry.PlayerComponent.Transform.position.x < _nextSectionSpawnBorder)
            {
                return;
            }

            float sectionStart = _nextSectionSpawnBorder + _enemyConfigs.PreSpawnLength - _enemyConfigs.SpawnPeriod;
            await SpawnForSection(sectionStart, gameRegistry, cancellationToken);
            _nextSectionSpawnBorder += _enemyConfigs.SpawnPeriod;
        }

        private async UniTask SpawnInitialEnemies(GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            float spawnCountFloat = (_enemyConfigs.PreSpawnLength - _enemyConfigs.SpawnStartOffset) /
                                    _enemyConfigs.SpawnPeriod;

            int spawnCount = Mathf.FloorToInt(spawnCountFloat);

            for (int i = 0; i < spawnCount; i++)
            {
                float sectionStart = _enemyConfigs.SpawnStartOffset + i * _enemyConfigs.SpawnPeriod;
                await SpawnForSection(sectionStart, gameRegistry, cancellationToken);
            }

        }

        private async UniTask SpawnForSection(float sectionStart, GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            float spawnCountFloat = _enemyConfigs.BasicSpawningCountPerSection +
                             _enemyConfigs.SpawningCountPerSectionLengthModifier * sectionStart;

            int spawnCount = Mathf.FloorToInt(spawnCountFloat);

            for (int i = 0; i < spawnCount; i++)
            {
                await SpawnOne(sectionStart, gameRegistry, cancellationToken);
            }

        }

        private async UniTask SpawnOne(float sectionStart, GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            StickmanComponent stickman = await _enemyPool.GetInstance(cancellationToken);

            stickman.MaxHp = _enemyConfigs.Hp;
            stickman.Hp = _enemyConfigs.Hp;
            stickman.Speed = _enemyConfigs.Speed;
            stickman.Damage = _enemyConfigs.Damage;
            stickman.ReactionRadius = _enemyConfigs.ReactionRadius;

            Vector3 position = Vector3.zero;
            position.x = sectionStart + Random.value * _enemyConfigs.SpawnPeriod;
            position.z = (Random.value - 0.5f) * _enemyConfigs.SpawnWidth;
            Quaternion rotation = Quaternion.Euler(0, Random.value * 360, 0);


            stickman.Transform.SetParent(_activeEnemiesHost, true);
            stickman.Transform.position = position;
            stickman.CharacterModelTransform.rotation = rotation;
            stickman.gameObject.SetActive(true);
            stickman.Rigidbody.MovePosition(position);
            
            gameRegistry.Stickmans.Add(stickman);
        }
        
        
        
    }
}
using System.Threading;
using Cysharp.Threading.Tasks;
using GameLoop.Configs;
using GameLoop.Entities;
using GameLoop.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Utils;

namespace GameLoop.Systems.Player
{
    public class PlayerShooter : ILoopedSystem
    {
        private const int PoolSize = 40;
        
        private readonly GameObjectPool<BulletComponent> _bulletPool;
        private readonly IPlayerConfigsProvider _playerConfigsProvider;
        private readonly IEntityResetService _entityResetService;
        private readonly GameRootTransformProvider _gameRootTransformProvider;

        private Transform _activeEnemiesHost;
        private float _lastShootTime;

        public PlayerShooter(
            GameObjectPool<BulletComponent> bulletPool, 
            IPlayerConfigsProvider playerConfigsProvider, 
            IEntityResetService entityResetService, 
            GameRootTransformProvider gameRootTransformProvider)
        {
            _bulletPool = bulletPool;
            _playerConfigsProvider = playerConfigsProvider;
            _entityResetService = entityResetService;
            _gameRootTransformProvider = gameRootTransformProvider;
        }

        public async UniTask Initialize(GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            _activeEnemiesHost = new GameObject("Active Enemies host").transform;
            _activeEnemiesHost.SetParent(_gameRootTransformProvider.GameRoot);
            
            gameRegistry.Bullets = new(PoolSize);
            await _bulletPool.Extend(PoolSize, cancellationToken);
        }

        public UniTask Reset(GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            for (int i = 0; i < gameRegistry.Bullets.Count; i++)
            {
                BulletComponent bullet = gameRegistry.Bullets[i];
                _entityResetService.Reset(bullet);
                _bulletPool.Return(bullet);
            }
            
            gameRegistry.Bullets.Clear();
            return UniTask.CompletedTask;
        }

        public async UniTask Loop(float deltaTime, GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            UpdateDirection(gameRegistry);

            if (Time.time - _lastShootTime < _playerConfigsProvider.ShootingCooldown)
            {
                return;
            }
            
            await Shoot(gameRegistry, cancellationToken);
        }

        private void UpdateDirection(GameRegistry gameRegistry)
        {
            TouchControl primaryTouch = Touchscreen.current.primaryTouch;

            if (!primaryTouch.press.isPressed)
            {
                return;
            }
            
            // initially I implemented relative rotation from point of touch,
            // but that felt a little clanky, having ability to snap in different direction feels much better for me,
            // plus code became somewhat simpler
            
            float currentPressPosition = primaryTouch.position.x.value;

            float inputShift = currentPressPosition / Screen.width;

            float angle = Mathf.Lerp(
                -_playerConfigsProvider.TurretRotationConstraint / 2,
                _playerConfigsProvider.TurretRotationConstraint / 2,
                inputShift);
                
            gameRegistry.PlayerComponent.Turret.localRotation = Quaternion.Euler(0, angle, 0);
        }

        private async UniTask Shoot(GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            BulletComponent bullet = await _bulletPool.GetInstance(cancellationToken);
            bullet.Transform.position = gameRegistry.PlayerComponent.BulletPivot.transform.position;
            bullet.Direction = gameRegistry.PlayerComponent.BulletPivot.transform.forward;
            bullet.Transform.forward = bullet.Direction;
            bullet.Speed = _playerConfigsProvider.BulletFlightSpeed;
            bullet.Damage = _playerConfigsProvider.Damage;
            
            bullet.Transform.SetParent(_activeEnemiesHost, true);
            bullet.gameObject.SetActive(true);
            
            gameRegistry.Bullets.Add(bullet);
            _lastShootTime = Time.time;
        }
        
    }
}
using System.Globalization;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameLoop.Entities;
using GameLoop.Services;
using UnityEngine;
using Utils;
using VFX.FlyingText;

namespace GameLoop.Systems.Player
{
    public class PlayerBulletsMover : ILoopedSystem
    {
        private readonly GameObjectPool<BulletComponent> _bulletPool;
        private readonly IEntityResetService _entityResetService;
        private readonly FlyingTextManager _flyingTextManager;

        public PlayerBulletsMover(
            GameObjectPool<BulletComponent> bulletPool, 
            IEntityResetService entityResetService, 
            FlyingTextManager flyingTextManager)
        {
            _bulletPool = bulletPool;
            _entityResetService = entityResetService;
            _flyingTextManager = flyingTextManager;
        }
        
        public UniTask Initialize(GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        public UniTask Reset(GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        public UniTask Loop(float deltaTime, GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            for (int i = 0; i < gameRegistry.Bullets.Count; i++)
            {
                BulletComponent bullet = gameRegistry.Bullets[i];

                if (TryUpdateCollision(bullet, ref i, gameRegistry, cancellationToken))
                {
                    continue;
                }

                if (TryUpdateByRenderer(bullet, ref i, gameRegistry))
                {
                    continue;
                }
                
                UpdatePosition(deltaTime, bullet);
            }
            
            return UniTask.CompletedTask;
        }

        private void UpdatePosition(float deltaTime, BulletComponent bullet)
        {
            Vector3 position = bullet.Transform.position + bullet.Speed * deltaTime * bullet.Direction;
            bullet.Rigidbody.MovePosition(position);
        }

        private bool TryUpdateByRenderer(BulletComponent bullet, ref int index, GameRegistry gameRegistry)
        {
            if (bullet.Renderer.isVisible)
            {
                return false;
            }
            
            _entityResetService.Reset(bullet);
            _bulletPool.Return(bullet);
            gameRegistry.Bullets.RemoveAt(index);
            index--;
            
            return true;
        }

        private bool TryUpdateCollision(BulletComponent bullet, ref int index, GameRegistry gameRegistry, CancellationToken cancellationToken)
        {
            if (!bullet.IsCollided)
            {
                return false;
            }

            bullet.IsCollided = false;
            
            StickmanComponent stickmanComponent = bullet.Collision.gameObject.GetComponentInParent<StickmanComponent>();
            if (stickmanComponent)
            {
                stickmanComponent.Hp -= bullet.Damage;
                stickmanComponent.IsHpUpdated = true;
                
                _flyingTextManager.Play(
                        stickmanComponent.HpBarRenderer.transform.position, 
                        bullet.Damage.ToString(CultureInfo.InvariantCulture), 
                        cancellationToken)
                    .Forget();
            }
            
            bullet.Collision = null;
            
            _entityResetService.Reset(bullet);
            _bulletPool.Return(bullet);
            gameRegistry.Bullets.RemoveAt(index);
            index--;
            
            return true;
        }
        
        
    }
}
using System.Threading;
using Cysharp.Threading.Tasks;
using GameLoop.Configs;
using GameLoop.Entities;
using GameLoop.Services;
using UnityEngine;
using Utils;
using VFX.ColorBlink;
using VFX.ParticlePlayer;

namespace GameLoop.Systems.Enemy
{
    public class EnemyHpUpdater : ILoopedSystem
    {
        private static readonly int FillValue = Shader.PropertyToID("_FillValue");
        
        private readonly GameObjectPool<StickmanComponent> _enemyPool;
        private readonly IEntityResetService _entityResetService;
        private readonly ColorBlinkAnimationManager _colorBlinkAnimationManager;
        private readonly IEnemyConfigsProvider _enemyConfigsProvider;

        public EnemyHpUpdater(
            GameObjectPool<StickmanComponent> enemyPool, 
            IEntityResetService entityResetService, 
            ColorBlinkAnimationManager colorBlinkAnimationManager, 
            IEnemyConfigsProvider enemyConfigsProvider)
        {
            _enemyPool = enemyPool;
            _entityResetService = entityResetService;
            _colorBlinkAnimationManager = colorBlinkAnimationManager;
            _enemyConfigsProvider = enemyConfigsProvider;
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
            for (var i = 0; i < gameRegistry.Stickmans.Count; i++)
            {
                StickmanComponent stickman = gameRegistry.Stickmans[i];
                
                if (!stickman.IsHpUpdated)
                {
                    continue;
                }

                if (stickman.Hp <= 0)
                {
                    _entityResetService.Reset(stickman);
                    gameRegistry.Stickmans.RemoveAt(i);
                    i--;

                    ParticleSystemUtils.PlayFromAssetReference(
                            stickman.Transform.position, 
                            _enemyConfigsProvider.EnemyDeathAssetReference, 
                            cancellationToken)
                        .Forget();
                    
                    _enemyPool.Return(stickman);
                    
                    continue;
                }

                stickman.IsHpUpdated = false;
                
                stickman.HpBarRenderer.gameObject.SetActive(true);
                stickman.HpBarRenderer.material.SetFloat(FillValue, stickman.Hp / stickman.MaxHp);
                stickman.Animator.SetTrigger(EnemyAnimationsUtils.GettingHit);
                _colorBlinkAnimationManager.PlayAnimation(stickman.CharacterModelRenderer, cancellationToken).Forget();
            }

            return UniTask.CompletedTask;
        }
    }
}
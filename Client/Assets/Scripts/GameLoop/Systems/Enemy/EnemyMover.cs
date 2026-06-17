using System.Globalization;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameLoop.Entities;
using GameLoop.Services;
using UnityEngine;
using Utils;
using VFX.ColorBlink;
using VFX.FlyingText;

namespace GameLoop.Systems.Enemy
{
    public class EnemyMover : ILoopedSystem
    {
        private readonly ColorBlinkAnimationManager _animationManager;
        private readonly FlyingTextManager _flyingTextManager;

        public EnemyMover(
            ColorBlinkAnimationManager animationManager, 
            FlyingTextManager flyingTextManager)
        {
            _animationManager = animationManager;
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
            for (int i = 0; i < gameRegistry.Stickmans.Count; i++)
            {
                StickmanComponent stickman = gameRegistry.Stickmans[i];

                if (CheckState(stickman, gameRegistry.PlayerComponent) || 
                    CheckCollision(stickman, gameRegistry.PlayerComponent, cancellationToken))
                {
                    stickman.IsHpUpdated = true;
                    stickman.Hp = -1;
                    continue;
                }
                
                if (stickman.IsEnraged)
                {
                    UpdateEnraged(deltaTime, stickman, gameRegistry.PlayerComponent);
                }
                else
                {
                    UpdatePassive(deltaTime, stickman);
                }
            }
            
            return UniTask.CompletedTask;
        }

        private bool CheckState(StickmanComponent stickman, PlayerComponent player)
        {
            if (stickman.IsEnraged)
            {
                return false;
            }

            if (stickman.Transform.position.x < player.Transform.position.x - stickman.ReactionRadius)
            {
                return true;
            }

            if (stickman.Transform.position.x > player.Transform.position.x + stickman.ReactionRadius)
            {
                return false;
            }

            stickman.IsEnraged = Vector3.SqrMagnitude(player.Transform.position - stickman.Transform.position) <=
                                      stickman.ReactionRadius * stickman.ReactionRadius;
            
            stickman.Animator.SetBool(EnemyAnimationsUtils.EnemyFound, true);
            
            return false;
        }

        private bool CheckCollision(StickmanComponent stickman, PlayerComponent playerComponent, CancellationToken cancellationToken)
        {
            if (!stickman.IsPlayerCollided)
            {
                return false;
            }

            playerComponent.Hp -= stickman.Damage;
            playerComponent.ModelAnimator.SetTrigger(PlayerAnimationsUtils.DamageTaken);
            
            _animationManager.PlayAnimation(playerComponent.ModelRenderer, cancellationToken).Forget();
            _flyingTextManager.Play(
                stickman.HpBarRenderer.transform.position, 
                stickman.Damage.ToString(CultureInfo.InvariantCulture), 
                cancellationToken)
                .Forget();
            
            return true;
        }

        private void UpdatePassive(float deltaTime, StickmanComponent stickman)
        {
            Vector3 position = stickman.Transform.position + deltaTime * 0.15f * stickman.CharacterModelTransform.forward;
            stickman.Rigidbody.MovePosition(position);
        }

        private void UpdateEnraged(float deltaTime, StickmanComponent stickman, PlayerComponent player)
        {
            Vector3 radiusVector = player.Transform.position - stickman.Transform.position;
            
            Vector3 position = stickman.Transform.position + deltaTime * stickman.Speed * stickman.CharacterModelTransform.forward;
            Quaternion lookRotation = Quaternion.LookRotation(radiusVector);

            stickman.CharacterModelTransform.rotation =
                Quaternion.Slerp(stickman.CharacterModelTransform.rotation, lookRotation, 0.2f);
            stickman.Rigidbody.MovePosition(position);
        }
        
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace VFX.ColorBlink
{
    public class ColorBlinkAnimationManager
    {
        private readonly IColorBlinkAnimationConfigurationsProvider _animationConfigs;

        private Dictionary<Renderer, (CancellationTokenSource cancellationTokenSource, Material originalMaterial)> _activeAnimations = new();

        public ColorBlinkAnimationManager(IColorBlinkAnimationConfigurationsProvider animationConfigs)
        {
            _animationConfigs = animationConfigs;
        }

        public async UniTask PlayAnimation(Renderer renderer, CancellationToken cancellationToken)
        {
            if (_activeAnimations.Remove(renderer, out (CancellationTokenSource cancellation, Material originalMaterial) item))
            {
                item.cancellation.Cancel();
                item.cancellation.Dispose();
                renderer.material = item.originalMaterial;
            }

            try
            {
                CancellationTokenSource cancellationTokenSource =
                    CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                _activeAnimations.Add(renderer, (cancellationTokenSource, renderer.material));

                await PlayAnimationInternal(
                    renderer,
                    _animationConfigs.BlinkMaterial,
                    _animationConfigs.BlinkCycles,
                    _animationConfigs.BlinkDuration,
                    _animationConfigs.RelaxationDuration,
                    cancellationTokenSource.Token);
                
                if (_activeAnimations.Remove(renderer, out item))
                {
                    item.cancellation.Dispose();
                    renderer.material = item.originalMaterial;
                }

            }
            catch (OperationCanceledException)
            {
                // ignored
            }
            
        }

        private async UniTask PlayAnimationInternal(
            Renderer renderer, 
            Material blinkMaterial, 
            int blinkCycles, 
            float blinkDuration, 
            float relaxationDuration, 
            CancellationToken cancellationToken)
        {
            Material originalMaterial = renderer.material; 
            
            for (int i = 0; i < blinkCycles; i++)
            {
                // waiting blink animation
                renderer.material = blinkMaterial;

                UniTask colorBlinkTask = UniTask.WaitForSeconds(blinkDuration, cancellationToken: cancellationToken);

                int result = await UniTask.WhenAny(colorBlinkTask, UniTask.WaitUntil(() => !renderer.gameObject.activeInHierarchy, cancellationToken: cancellationToken));
                
                if (result == 1 || i == blinkCycles - 1)
                {
                    return;
                }
                
                // waiting relaxation animation
                renderer.material = originalMaterial;

                colorBlinkTask = UniTask.WaitForSeconds(relaxationDuration, cancellationToken: cancellationToken);

                result = await UniTask.WhenAny(colorBlinkTask, UniTask.WaitUntil(() => !renderer.gameObject.activeInHierarchy, cancellationToken: cancellationToken));

                if (result == 1)
                {
                    return;
                }
                
            }
            
            
        }
        
        
    }
}
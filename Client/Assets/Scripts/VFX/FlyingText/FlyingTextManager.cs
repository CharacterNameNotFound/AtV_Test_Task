using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Utils;

namespace VFX.FlyingText
{
    public class FlyingTextManager
    {
        private const int PoolSize = 30; 
        
        private readonly IFlyingTextConfigs _flyingTextConfigs;
        private readonly GameObjectPoolAddressProvider<FlyingTextComponent> _flyingTextAddressProvider;
        
        private GameObjectPool<FlyingTextComponent> _textPool;
        
        public FlyingTextManager(
            IFlyingTextConfigs flyingTextConfigs, 
            GameObjectPoolAddressProvider<FlyingTextComponent> flyingTextAddressProvider)
        {
            _flyingTextConfigs = flyingTextConfigs;
            _flyingTextAddressProvider = flyingTextAddressProvider;
        }

        public async UniTask Initialize(CancellationToken cancellationToken)
        {
            _textPool = new GameObjectPool<FlyingTextComponent>(_flyingTextAddressProvider);
            await _textPool.Extend(PoolSize, cancellationToken);
        }
        
        public async UniTask Play(Vector3 position, string text, CancellationToken cancellationToken)
        {
            FlyingTextComponent item = null;

            try
            {
                item = await _textPool.GetInstance(cancellationToken);

                item.transform.rotation = Quaternion.Euler(0, 90, 0);
                
                await item.Play(
                    position,
                    text, 
                    _flyingTextConfigs.Duration, 
                    _flyingTextConfigs.FlightVector,
                    cancellationToken);
                
            }
            catch (OperationCanceledException) { }
            finally
            {
                if (item)
                {
                    _textPool.Return(item);
                }
            }
            
        }

        
        
        
    }
}
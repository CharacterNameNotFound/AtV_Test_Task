using System.Threading;
using Cysharp.Threading.Tasks;
using GameLoop;
using UnityEngine;
using Utils;
using Zenject;

namespace GameCore
{
    public class GameBootstrap : MonoBehaviour
    {
        private IGameRoot _gameRoot;
        private GameRootTransformProvider _gameRootTransformProvider;

        [Inject]
        private void Construct(IGameRoot gameRoot, GameRootTransformProvider gameRootTransformProvider)
        {
            _gameRoot = gameRoot;
            _gameRootTransformProvider = gameRootTransformProvider;
        }
        
        // yeah, there is awaitables and they compatible with start method, but because they described as having coroutine based it is scary to use them...,
        // basically they added "UniTask" out of box, but it is worse, at least for now
        private void Awake()
        {
            Application.targetFrameRate = 60;
            
            transform.position = Vector3.zero;
            _gameRootTransformProvider.Set(transform);
            
            AwakeAsync(Application.exitCancellationToken).Forget();
        }

        private async UniTask AwakeAsync(CancellationToken cancellationToken)
        {
            await _gameRoot.Initialize(cancellationToken);
        }
        
    }
}
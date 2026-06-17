using System.Threading;
using Cysharp.Threading.Tasks;
using GameLoop;
using UI;
using VFX.FlyingText;

namespace GameCore
{
    public class GameRoot : IGameRoot
    {
        private readonly IGameLooper _gameLooper;
        private readonly MainUIControllerHolder _mainUIControllerHolder;
        private readonly FlyingTextManager _flyingTextManager;

        public GameRoot(
            IGameLooper gameLooper, 
            MainUIControllerHolder mainUIControllerHolder, 
            FlyingTextManager flyingTextManager)
        {
            _gameLooper = gameLooper;
            _mainUIControllerHolder = mainUIControllerHolder;
            _flyingTextManager = flyingTextManager;
        }

        public async UniTask Initialize(CancellationToken cancellationToken)
        {
            await _flyingTextManager.Initialize(cancellationToken);
            await _gameLooper.Initialize(cancellationToken);
            await _mainUIControllerHolder.MainUI.Initialize(cancellationToken);
        }

        public async UniTask Reset(CancellationToken cancellationToken)
        {
            await _gameLooper.Reset(cancellationToken);
            await _mainUIControllerHolder.MainUI.Initialize(cancellationToken);
        }

        public async UniTask Loop(CancellationToken cancellationToken)
        {
            await _gameLooper.Loop(cancellationToken);
        }
        
    }
}
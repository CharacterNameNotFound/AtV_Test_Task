using System.Threading;
using Cysharp.Threading.Tasks;
using GameLoop;

namespace GameCore
{
    public class GameRoot : IGameRoot
    {
        private IGameLooper _gameLooper;

        public GameRoot(IGameLooper gameLooper)
        {
            _gameLooper = gameLooper;
        }

        public async UniTask Initialize(CancellationToken cancellationToken)
        {
            await _gameLooper.Initialize(cancellationToken);
        }

        public async UniTask Reset(CancellationToken cancellationToken)
        {
            await _gameLooper.Reset(cancellationToken);
        }

        public async UniTask Loop(CancellationToken cancellationToken)
        {
            await _gameLooper.Loop(cancellationToken);
        }
        
    }
}
using GameLoop;
using UnityEngine;
using Zenject;

namespace GameCore
{
    public class GameInitializer : MonoBehaviour
    {
        private IGameRoot _gameRoot;

        [Inject]
        private void Construct(IGameRoot gameRoot)
        {
            _gameRoot = gameRoot;
        }
        
        // yeah..., they added "UniTask" out of box, but it is worse, at least for now
        private async Awaitable Start()
        {
            await _gameRoot.Initialize(Application.exitCancellationToken);
            // show press overlay
        }
        
    }
}
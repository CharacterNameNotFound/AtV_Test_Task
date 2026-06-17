using System.Threading;
using Cysharp.Threading.Tasks;
using GameLoop;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace UI.States
{
    public class LevelStartScreenState : BaseScreenState
    {
        private IGameRoot _gameRoot;
        
        private bool _isActivated;
        
        [Inject]
        private void Construct(IGameRoot gameLooper)
        {
            _gameRoot = gameLooper;
        }
        
        public override UniTask OnStateEnter(MainUIController controller, CancellationToken cancellationToken)
        {
            _isActivated = false;
            return base.OnStateEnter(controller, cancellationToken);
        }

        private void Update()
        {
            if (_isActivated || !Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                return;
            }

            _isActivated = true;
            
            _gameRoot.Loop(Application.exitCancellationToken).Forget();
            Controller.SwapState<LevelScreenState>(Application.exitCancellationToken).Forget();
        }
    }
}
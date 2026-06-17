using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameLoop;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace UI.States
{
    public class LevelWonScreenState : BaseScreenState
    {
        [SerializeField] private CanvasGroup _canvasGroup; 
        
        private IGameRoot _gameRoot;
        
        private bool _isActivated;
        
        [Inject]
        private void Construct(IGameRoot gameLooper)
        {
            _gameRoot = gameLooper;
        }
        
        public override async UniTask OnStateEnter(MainUIController controller, CancellationToken cancellationToken)
        {
            _isActivated = true;
            Controller = controller;

            _canvasGroup.alpha = 0;
            gameObject.SetActive(true);
            
            await DOVirtual
                .Float(0, 1, 1f, value => _canvasGroup.alpha = value)
                .Play()
                .ToUniTask(cancellationToken: cancellationToken);
            
            _isActivated = false;
        }
        
        private void Update()
        {
            if (_isActivated || !Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                return;
            }

            _isActivated = true;
            
            _gameRoot.Reset(Application.exitCancellationToken).Forget();
        }
    }
}
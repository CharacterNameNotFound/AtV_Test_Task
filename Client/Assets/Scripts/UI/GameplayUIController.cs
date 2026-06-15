using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameLoop;
using UI.States;
using UnityEngine;
using Zenject;

namespace UI
{
    // injection into game objects on awake isn't exactly a good thing... but proper UI system is bulky for test task
    public class GameplayUIController : MonoBehaviour
    {
        [SerializeField] private BaseScreenState[] _screenStates;

        private IGameLooper _gameLooper;
        
        private Dictionary<Type, BaseScreenState> _states;
        private BaseScreenState _currentState;

        [Inject]
        private void Construct(IGameLooper gameLooper)
        {
            _gameLooper = gameLooper;
        }

        private void Start()
        {
            Initialize(Application.exitCancellationToken).Forget();
        }

        private async UniTask Initialize(CancellationToken cancellationToken)
        {
            await SwapState<LevelStartScreenState>(cancellationToken);
        }
        
        private async UniTask SwapState<T>(CancellationToken cancellationToken) where T : BaseScreenState
        {
            if (_currentState)
            {
                await _currentState.OnStateExit(cancellationToken);
            }

            _currentState = _states[typeof(T)];
            await _currentState.OnStateEnter(this, cancellationToken);
        }
        
        
        
    }
}
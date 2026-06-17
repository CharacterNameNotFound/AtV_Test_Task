using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameLoop;
using GameLoop.Services;
using GameLoop.Systems.Player;
using UI.States;
using UnityEngine;
using Zenject;

namespace UI
{
    // injection into game objects on awake isn't exactly a good thing... but proper UI system is bulky for test task
    public class MainUIController : MonoBehaviour
    {
        [SerializeField] private BaseScreenState[] _screenStates;

        private MainUIControllerHolder _mainUIControllerHolder;
            
        private Dictionary<Type, BaseScreenState> _states;
        private BaseScreenState _currentState;
        
        [Inject]
        private void Construct(MainUIControllerHolder mainUIControllerHolder)
        {
            _mainUIControllerHolder = mainUIControllerHolder;
        }

        private void Awake()
        {
            _states = new(_screenStates.Length);
            foreach (BaseScreenState baseScreenState in _screenStates)
            {
                baseScreenState.gameObject.SetActive(false);
                _states.Add(baseScreenState.GetType(), baseScreenState);
            }

            _mainUIControllerHolder.Set(this);
        }

        public async UniTask Initialize(CancellationToken cancellationToken)
        {
            await SwapState<LevelStartScreenState>(cancellationToken);
        }
        
        public async UniTask SwapState<T>(CancellationToken cancellationToken) where T : BaseScreenState
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
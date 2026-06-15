using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameLoop
{
    // Easies to extend and work with for game of this type will be ECS architecture,
    // I will not use packages to bring down complexity a notch
    // GameRegistry will play role of game world storage
    // This class will loop through systems
    // And instead of entity-component pair I will just use slightly questionable single component objects
    //      Questionability stems from me not separating components per role
    //      Basically I will reduce code complexity by using entities only, which of course will cause flexibility reduction
    //      On other hand it will remove requirement to manage "complex" process of cleanup of components from corresponding collections
    //      P.S.: Just in case, this process can be solved by creating dictionaries per component
    //      with signature Dictionary<InstanceId, Component> or just using library (as ultimate solution, and performance with upgrade too)
    public class GameLooper : IGameLooper
    {
        public event Action OnWin;
        public event Action OnLose;
        
        private GameRegistry _gameRegistry;
        private ILoopedSystem[] _systems;
        private IGameEndDecisionMaker _gameEndDecisionMaker;

        public GameLooper(GameRegistry gameRegistry, ILoopedSystem[] systems, IGameEndDecisionMaker gameEndDecisionMaker)
        {
            _gameRegistry = gameRegistry;
            _systems = systems;
            _gameEndDecisionMaker = gameEndDecisionMaker;
        }

        public async UniTask Initialize(CancellationToken cancellationToken)
        {
            for (int i = 0; i < _systems.Length; i++)
            {
                await _systems[i].Initialize(cancellationToken);
            }
            
        }

        public async UniTask Reset(CancellationToken cancellationToken)
        {
            for (int i = 0; i < _systems.Length; i++)
            {
                await _systems[i].Reset(cancellationToken);
            }
            
        }
        
        public async UniTask Loop(CancellationToken cancellationToken)
        {
            bool isWin = false;
            await UniTask.WaitForFixedUpdate();
            
            do
            {
                for (int i = 0; i < _systems.Length; i++)
                {
                    await _systems[i].Loop(Time.fixedDeltaTime, _gameRegistry, cancellationToken);
                }
                
                await UniTask.WaitForFixedUpdate();
            } while (!cancellationToken.IsCancellationRequested && 
                     !_gameEndDecisionMaker.IsGameEnd(_gameRegistry, out isWin));
            
            cancellationToken.ThrowIfCancellationRequested();

            if (isWin)
            {
                OnWin?.Invoke();
                return;
            }

            OnLose?.Invoke();
        }

        
        
        
    }
}
using Cysharp.Threading.Tasks;
using GameLoop;
using GameLoop.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.States
{
    public class LevelScreenState : BaseScreenState
    {
        [SerializeField] private Slider _fillBar;
        [SerializeField] private TMP_Text _distanceText;

        private IGameProgressProvider _gameProgressProvider;
        private IGameEndDecisionMaker _gameEndDecisionMaker;
        private GameRegistry _gameRegistry;
        
        [Inject]
        private void Construct(IGameProgressProvider gameProgressProvider, IGameEndDecisionMaker gameEndDecisionMaker)
        {
            _gameProgressProvider = gameProgressProvider;
            _gameEndDecisionMaker = gameEndDecisionMaker;
        }

        private void LateUpdate()
        {
            _fillBar.value = _gameProgressProvider.GetProgress();
            _distanceText.text = $"{(int)_gameProgressProvider.GetDistance()}m";

            if (!_gameEndDecisionMaker.IsGameEnd(out bool isWin))
            {
                return;
            }

            if (isWin)
            {
                Controller.SwapState<LevelWonScreenState>(Application.exitCancellationToken).Forget();
            }
            else
            {
                Controller.SwapState<LevelLostScreenState>(Application.exitCancellationToken).Forget();
            }
            
        }
        
    }
}
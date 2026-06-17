using GameLoop.Services;

namespace GameLoop
{
    public class GameEndDecisionMaker : IGameEndDecisionMaker
    {
        private readonly IGameProgressProvider _gameProgressProvider;
        private readonly GameRegistry _gameRegistry;

        public GameEndDecisionMaker(IGameProgressProvider gameProgressProvider, GameRegistry gameRegistry)
        {
            _gameProgressProvider = gameProgressProvider;
            _gameRegistry = gameRegistry;
        }

        public bool IsGameEnd(out bool isWin)
        {
            isWin = false;
                
            if (_gameRegistry.PlayerComponent.Hp <= 0)
            {
                return true;
            }

            if (_gameProgressProvider.GetProgress() >= 1)
            {
                isWin = true;
                return true;
            }
            
            return false;
        }
    }
}
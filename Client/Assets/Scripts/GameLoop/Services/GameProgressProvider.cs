using GameLoop.Configs;

namespace GameLoop.Services
{
    public class GameProgressProvider : IGameProgressProvider
    {
        private readonly ILevelConfigsProvider _levelConfigsProvider;
        private readonly GameRegistry _gameRegistry;

        public GameProgressProvider(ILevelConfigsProvider levelConfigsProvider, GameRegistry gameRegistry)
        {
            _levelConfigsProvider = levelConfigsProvider;
            _gameRegistry = gameRegistry;
        }

        public float GetProgress()
        {
            return _gameRegistry.PlayerComponent.Transform.position.x / _levelConfigsProvider.LevelLength;
        }

        public float GetDistance()
        {
            return _gameRegistry.PlayerComponent.Transform.position.x;
        }
    }
}
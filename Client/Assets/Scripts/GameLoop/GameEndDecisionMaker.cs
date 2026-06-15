namespace GameLoop
{
    public class GameEndDecisionMaker : IGameEndDecisionMaker
    {
        public bool IsGameEnd(GameRegistry registry, out bool isWin)
        {
            isWin = false;
                
            if (registry.PlayerComponent.Hp <= 0)
            {
                return true;
            }
            
            return false;
        }
    }
}
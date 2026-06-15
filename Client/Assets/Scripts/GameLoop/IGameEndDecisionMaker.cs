namespace GameLoop
{
    public interface IGameEndDecisionMaker
    {
        public bool IsGameEnd(GameRegistry registry, out bool isWin);
    }
}
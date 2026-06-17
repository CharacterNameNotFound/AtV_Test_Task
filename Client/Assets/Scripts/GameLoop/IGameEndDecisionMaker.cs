namespace GameLoop
{
    public interface IGameEndDecisionMaker
    {
        public bool IsGameEnd(out bool isWin);
    }
}
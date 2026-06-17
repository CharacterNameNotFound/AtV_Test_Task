namespace GameLoop.Services
{
    public interface IGameProgressProvider
    {
        public float GetProgress();
        public float GetDistance();
    }
}
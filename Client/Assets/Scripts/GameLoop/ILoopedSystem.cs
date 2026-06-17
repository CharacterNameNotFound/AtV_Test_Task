using System.Threading;
using Cysharp.Threading.Tasks;

namespace GameLoop
{
    public interface ILoopedSystem
    {
        public UniTask Initialize(GameRegistry gameRegistry, CancellationToken cancellationToken);
        public UniTask Reset(GameRegistry gameRegistry, CancellationToken cancellationToken);
        public UniTask Loop(float deltaTime, GameRegistry gameRegistry, CancellationToken cancellationToken);
    }
}
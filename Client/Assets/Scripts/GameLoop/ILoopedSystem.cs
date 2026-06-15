using System.Threading;
using Cysharp.Threading.Tasks;

namespace GameLoop
{
    public interface ILoopedSystem
    {
        public UniTask Initialize(CancellationToken cancellationToken);
        public UniTask Reset(CancellationToken cancellationToken);
        public UniTask Loop(float deltaTime, GameRegistry gameRegistry, CancellationToken cancellationToken);
    }
}
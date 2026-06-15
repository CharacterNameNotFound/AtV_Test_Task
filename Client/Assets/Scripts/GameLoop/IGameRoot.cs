using System.Threading;
using Cysharp.Threading.Tasks;

namespace GameLoop
{
    public interface IGameRoot
    {
        public UniTask Initialize(CancellationToken cancellationToken);
        public UniTask Reset(CancellationToken cancellationToken);
        public UniTask Loop(CancellationToken cancellationToken);
    }
}
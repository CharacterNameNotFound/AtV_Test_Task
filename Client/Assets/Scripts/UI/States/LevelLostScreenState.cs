using System.Threading;
using Cysharp.Threading.Tasks;

namespace UI.States
{
    public class LevelLostScreenState : BaseScreenState
    {
        public override UniTask OnStateEnter(GameplayUIController controller, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public override UniTask OnStateExit(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
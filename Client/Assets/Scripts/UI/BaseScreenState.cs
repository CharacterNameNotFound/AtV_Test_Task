using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI
{
    public abstract class BaseScreenState : MonoBehaviour
    {
        public abstract UniTask OnStateEnter(GameplayUIController controller, CancellationToken cancellationToken);
        public abstract UniTask OnStateExit(CancellationToken cancellationToken);
    }
}
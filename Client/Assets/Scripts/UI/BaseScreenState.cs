using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI
{
    public abstract class BaseScreenState : MonoBehaviour
    {
        protected MainUIController Controller;
        
        public virtual UniTask OnStateEnter(MainUIController controller, CancellationToken cancellationToken)
        {
            Controller = controller;
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }
        
        public virtual UniTask OnStateExit(CancellationToken cancellationToken)
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }
        
    }
}
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Utils
{
    // trying to stay a little close to ECS, so not going to create "IPoolableInstance" interface to implement OnPooled method
    public class GameObjectPool<T> where T : MonoBehaviour
    {
        private GameObjectPoolAddressProvider<T> _addressProvider;
        
        private Transform _poolHolder;
        private List<T> _instances;
        
        public GameObjectPool(GameObjectPoolAddressProvider<T> addressProvider)
        {
            _addressProvider = addressProvider;
            
            _poolHolder = new GameObject($"{nameof(T)} pool").transform;
            _instances = new List<T>(0);
        }

        public async UniTask Extend(int count, CancellationToken cancellationToken)
        {
            _instances.Capacity += count;
            
            for (int i = 0; i < count; i++)
            {
                T instance = await BuildOne(cancellationToken);
                _instances.Add(instance);
            }
        }

        public UniTask<T> GetInstance(CancellationToken cancellationToken)
        {
            if (_instances.Count == 0)
            {
                return BuildOne(cancellationToken);
            }

            T instance = _instances[^1];
            _instances.RemoveAt(_instances.Count - 1);
            return UniTask.FromResult(instance);
        }

        public void Return(T instance)
        {
            _instances.Add(instance);
        }

        private async UniTask<T> BuildOne(CancellationToken cancellationToken)
        {
            GameObject instanceGO = await _addressProvider.AssetReferenceGameObject.Instantiate(new InstantiationParameters(_poolHolder, true), cancellationToken);
            
            instanceGO.SetActive(false);
            T instance = instanceGO.GetComponent<T>();
            
            return instance;
        }
        
    }
}
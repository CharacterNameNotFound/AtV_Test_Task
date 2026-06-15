using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Utils
{
    public static class AssetReferenceUtilExtensions
    {
        public static async UniTask<T> Load<T>(this AssetReference assetReferenceDto, CancellationToken cancellationToken)
        {
            AsyncOperationHandle<T> operation = Addressables.LoadAssetAsync<T>(assetReferenceDto);
            await operation.ToUniTask(cancellationToken: cancellationToken);
            return operation.Result;
        }
        
        public static async UniTask<T> Load<T>(this object runtimeKey, CancellationToken cancellationToken)
        {
            AsyncOperationHandle<T> operation = Addressables.LoadAssetAsync<T>(runtimeKey);
            await operation.ToUniTask(cancellationToken: cancellationToken);
            return operation.Result;
        }

        public static async UniTask<T> Instantiate<T>(this AssetReference assetReferenceDto, InstantiationParameters instantiationParameters, CancellationToken cancellationToken)
        {
            GameObject gameObject = await Instantiate(assetReferenceDto, instantiationParameters, cancellationToken);
            return gameObject.GetComponent<T>();
        }
        
        public static async UniTask<GameObject> Instantiate(this AssetReference assetReferenceDto, InstantiationParameters instantiationParameters, CancellationToken cancellationToken)
        {
            AsyncOperationHandle operation = Addressables.InstantiateAsync(assetReferenceDto, instantiationParameters);
            await operation.WithCancellation(cancellationToken);
            return (GameObject) operation.Result;
        }

        public static async UniTask<SceneInstance> LoadScene(this AssetReference assetReferenceDto, LoadSceneMode loadSceneMode, SceneReleaseMode releaseMode = SceneReleaseMode.ReleaseSceneWhenSceneUnloaded, CancellationToken cancellationToken = default)
        {
            AsyncOperationHandle<SceneInstance> sceneLoading = Addressables.LoadSceneAsync(assetReferenceDto, loadSceneMode, releaseMode);
            
            await sceneLoading.Task.AsUniTask().AttachExternalCancellation(cancellationToken);
            return sceneLoading.Result;
        }

        public static async UniTask<T> Load<T>(this string address, CancellationToken cancellationToken)
        {
            AsyncOperationHandle<T> operation = Addressables.LoadAssetAsync<T>(address);
            await operation.ToUniTask(cancellationToken: cancellationToken);
            return operation.Result;
        }
        
    }
}
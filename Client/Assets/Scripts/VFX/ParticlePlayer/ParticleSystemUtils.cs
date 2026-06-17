using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using Utils;

namespace VFX.ParticlePlayer
{
    public static class ParticleSystemUtils
    {
        public static async UniTask PlayFromAssetReference(Vector3 position, AssetReferenceGameObject particleSystemReference, CancellationToken cancellationToken)
        {
            ParticleSystem particleSystem = null;
            try
            {
                particleSystem = await particleSystemReference.Instantiate<ParticleSystem>(
                    new InstantiationParameters(position, Quaternion.identity, null), cancellationToken);

                await UniTask.WaitForSeconds(particleSystem.main.duration, cancellationToken: cancellationToken);
                particleSystem.gameObject.SetActive(false);
            }
            catch (OperationCanceledException) { }
            finally
            {
                if (particleSystem)
                {
                    Addressables.ReleaseInstance(particleSystem.gameObject);
                }
            }
            
        }
        
    }
}
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Utils
{
    // I generally use custom inspector window insted of create asset menu items for stuff like this,
    // as it will be mostly responsibility of tech staff
    public class GameObjectPoolAddressProvider<T> : ScriptableObject where T : MonoBehaviour
    {
        // just in case, preventing override
        [field: SerializeField] public AssetReferenceGameObject AssetReferenceGameObject { get; protected set; }

        public void SetReference(AssetReferenceGameObject assetReferenceGameObject)
        {
            AssetReferenceGameObject = assetReferenceGameObject;
        }
    }
}
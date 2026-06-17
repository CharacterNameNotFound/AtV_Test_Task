using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "ClassScriptableListInstaller", menuName = "Installers/ClassScriptableListInstaller")]
    public class ClassScriptableListInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private List<ScriptableObject> _dataObject = new();
        [SerializeField] private bool _installParent;
        
        public override void InstallBindings()
        {
            if (_installParent)
            {
                foreach (var item in _dataObject)
                {
                    Container.Bind(item.GetType().BaseType).FromInstance(item).AsSingle();
                }
            }
            
            foreach (var item in _dataObject)
            {
                Container.Bind(item.GetType()).FromInstance(item).AsSingle();
            }
            
        }
    }
}
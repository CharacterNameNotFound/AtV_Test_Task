using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "ClassScriptableListInstaller", menuName = "Installers/ClassScriptableListInstaller")]
    public class ClassScriptableListInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private List<ScriptableObject> _dataObject = new ();

        public override void InstallBindings()
        {
            foreach (var item in _dataObject)
            {
                Container.Bind(item.GetType().BaseType).FromInstance(item).AsSingle();
            }
            
        }
    }
}
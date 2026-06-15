using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "InterfaceScriptableListInstaller", menuName = "Installers/InterfaceScriptableListInstaller")]
    public class InterfaceScriptableListInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private List<ScriptableObject> _dataObject = new ();

        public override void InstallBindings()
        {
            foreach (var item in _dataObject)
            {
                foreach (Type type in item.GetType().GetInterfaces())
                {
                    Container.Bind(type).FromInstance(item).AsSingle();
                }
            }
            
        }
    }
}
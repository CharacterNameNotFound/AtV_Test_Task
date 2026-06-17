using UnityEngine;

namespace Utils
{
    public class GameRootTransformProvider
    {
        public Transform GameRoot { get; private set; }

        public void Set(Transform transform)
        {
            GameRoot = transform;
        }
    }
}
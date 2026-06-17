using UnityEngine;

namespace GameLoop.Entities
{
    public class BulletComponent : MonoBehaviour
    {
        public Renderer Renderer;
        public Transform Transform;
        public Rigidbody Rigidbody;
        public TrailRenderer TrailRenderer;
        
        [HideInInspector] public float Damage;
        [HideInInspector] public Vector3 Direction;
        [HideInInspector] public float Speed;
        
        [HideInInspector] public bool IsCollided;
        [HideInInspector] public GameObject Collision;
        

        private void OnCollisionEnter(Collision collision)
        {
            IsCollided = true;
            Collision = collision.gameObject;
        }
        
    }
}
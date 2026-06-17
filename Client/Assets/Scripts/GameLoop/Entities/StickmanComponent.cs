using UnityEngine;

namespace GameLoop.Entities
{
    public class StickmanComponent : MonoBehaviour
    {
        public Renderer HpBarRenderer;
        public Transform CharacterModelTransform;
        public Transform Transform;
        public Rigidbody Rigidbody;
        public Animator Animator;
        public Renderer CharacterModelRenderer;
        
        [HideInInspector] public float MaxHp;
        [HideInInspector] public float Hp;
        [HideInInspector] public float Speed;
        [HideInInspector] public float Damage;
        [HideInInspector] public float ReactionRadius;
        [HideInInspector] public bool IsEnraged;

        [HideInInspector] public bool IsPlayerCollided;
        [HideInInspector] public bool IsHpUpdated;

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.GetComponent<PlayerComponent>()) 
                return;
            
            IsPlayerCollided = true;
        }
        
    }
}
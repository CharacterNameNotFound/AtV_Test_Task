using UnityEngine;

namespace GameLoop.Entities
{
    public class PlayerComponent : MonoBehaviour
    {
        public Transform Transform;
        public Rigidbody Rigidbody;
        public Transform Turret;
        public Transform BulletPivot;
        public Renderer HpBar;
        public ParticleSystem FireParticleSystem;
        public TrailRenderer[] Trails;
        public Animator ModelAnimator;
        public Renderer ModelRenderer;
        
        [HideInInspector] public float MaxHp;
        [HideInInspector] public float Hp;
    }
}
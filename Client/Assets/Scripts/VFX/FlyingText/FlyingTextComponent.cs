using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

namespace VFX.FlyingText
{
    public class FlyingTextComponent : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        
        public async UniTask Play(Vector3 position, string text, float duration, Vector3 displacement, CancellationToken cancellationToken)
        {
            _text.text = text;
            _text.alpha = 1;
            
            gameObject.SetActive(true);
            transform.position = position;

            float halfDuration = duration / 2;

            TweenerCore<Vector3, Vector3, VectorOptions> movementTween = 
                transform.DOMove(position + displacement, halfDuration)
                    .SetEase(Ease.InOutExpo);
            
            TweenerCore<Color, Color, ColorOptions> fadeTween = 
                _text.DOFade(0, halfDuration)
                    .SetEase(Ease.InOutExpo);
            
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(halfDuration);
            sequence.Append(movementTween);
            sequence.Join(fadeTween);

            await sequence.Play().ToUniTask(cancellationToken: cancellationToken);
                
            gameObject.SetActive(false);
        }
        
    }
}
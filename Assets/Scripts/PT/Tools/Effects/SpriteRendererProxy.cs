using UnityEngine;

namespace PT.Tools.Effects
{
    public class SpriteRendererProxy : MonoBehaviour, ISpriteRendererProxy
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public Sprite Sprite
        {
            get => spriteRenderer.sprite;
            set => spriteRenderer.sprite = value;
        }
    }
}
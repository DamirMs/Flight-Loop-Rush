using UnityEngine;
using UnityEngine.UI;

namespace PT.Tools.Effects
{
    public class ImageSpriteProxy : MonoBehaviour, ISpriteRendererProxy
    {
        [SerializeField] private Image image;

        public Sprite Sprite
        {
            get => image.sprite;
            set => image.sprite = value;
        }
    }
}
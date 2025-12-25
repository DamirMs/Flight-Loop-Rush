using System;
using UnityEngine;

namespace Gameplay.General.Game
{
    public class TargetTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask collisionLayer;

        public event Action<GameObject> OnTriggered;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & collisionLayer.value) != 0)
            {
                OnTriggered?.Invoke(collision.gameObject);
            }
        }
    }
}
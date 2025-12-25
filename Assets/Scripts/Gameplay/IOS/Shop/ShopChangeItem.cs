using UnityEngine;
using Zenject;

namespace Gameplay.IOS.Shop
{
    public class ShopChangeItem : MonoBehaviour
    {
        [SerializeField] private ShopItemEnum itemEnum;
        [SerializeField] private GameObject[] changingItems;

        [Inject] private ShopManager _shopManager;

        private void OnEnable()
        {
            UpdateItem();
        }

        private void UpdateItem()
        {
            var index = _shopManager.GetEquippedId(itemEnum);
            
            if (index < 0 || index >= changingItems.Length) index = 0;
            
            for (int i = 0; i < changingItems.Length; i++)
            {
                changingItems[i].SetActive(i == index);
            }
        }
    }
}
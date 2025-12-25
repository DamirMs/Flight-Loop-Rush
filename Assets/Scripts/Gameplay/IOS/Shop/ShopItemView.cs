using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.IOS.Shop
{
    public class ShopItemView : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Button buyButton;
        [SerializeField] private Button useButton;
        [SerializeField] private Button equippedButton;
        [SerializeField] private TextMeshProUGUI priceText;

        private ShopItemInfo _info;
        private Action<ShopItemView, ShopItemInfo> _onClick;

        public void Init(ShopItemInfo shopItemInfo, Action<ShopItemView, ShopItemInfo> onClick)
        {
            _info = shopItemInfo;
            _onClick = onClick;

            iconImage.sprite = shopItemInfo.Sprite;

            buyButton.onClick.AddListener(() => _onClick?.Invoke(this, _info));
            useButton.onClick.AddListener(() => _onClick?.Invoke(this, _info));
            equippedButton.onClick.AddListener(() => _onClick?.Invoke(this, _info));
        }

        public void UpdateInfo(ShopManager shopManager, int currency)
        {
            if (_info == null) return;
            
            buyButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(false);
            equippedButton.gameObject.SetActive(false);
            
            if (shopManager.IsEquipped(_info.ItemEnum, _info.Id))
            {
                equippedButton.gameObject.SetActive(true);
                equippedButton.interactable = false;
                priceText.SetActive(false);
            }
            else if (shopManager.IsOwned(_info.ItemEnum, _info.Id))
            {
                useButton.gameObject.SetActive(true);
                useButton.interactable = true;
                priceText.SetActive(false);
            }
            else
            {
                buyButton.gameObject.SetActive(true);
                buyButton.interactable = currency >= _info.Price;
                priceText.SetActive(true);
            }
            
            if (priceText) priceText.text = _info.Price.ToString();
        }
    }
}
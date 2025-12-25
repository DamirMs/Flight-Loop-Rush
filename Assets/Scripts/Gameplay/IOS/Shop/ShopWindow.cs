using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.IOS.CurrencyRelated;
using PT.Tools.Helper;
using PT.Tools.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.IOS.Shop
{
    public enum ShopItemEnum
    {
        ShopMainItem,
    }
    
    public class ShopWindow : WindowBase
    {
        [SerializeField] private Button[] closeButtons;
        
        [SerializeField] private SerializableKeyValue<ShopItemEnum, SerializableItemViews> shopItemsViews;

        [Serializable]
        class SerializableItemViews
        {
            [SerializeField] private ShopItemView[] shopItemView;
            
            public ShopItemView[] Value => shopItemView;
        }

        [Inject] private ShopItemsConfig _shopItemsConfig;
        [Inject] private ShopManager _shopManager;
        [Inject] private CurrencyManager _currencyManager;
        
        [Inject(Id = "Menu")] private WindowsManager _windowsManager; 

        private void Awake()
        {
            foreach (var button in closeButtons)
            {
                if (button) button.onClick.AddListener(() => _windowsManager.Close(WindowTypeEnum.MenuShop).Forget());
            }
        }
        
        private void Start()
        {
            foreach (var shopItemViews in shopItemsViews.Dictionary)
            {
                var infos = _shopItemsConfig.ShopItemInfos
                    .Where(x => x.ItemEnum == shopItemViews.Key)
                    .OrderBy(x => x.Id)
                    .ToArray();
                
                for (int i = 0; i < shopItemViews.Value.Value.Length; i++)
                {
                    shopItemViews.Value.Value[i].Init(infos[i], ItemPressed);
                    shopItemViews.Value.Value[i].UpdateInfo(_shopManager, _currencyManager.Get(CurrencyType.Gold));
                }
            }
        }

        private void OnEnable()
        {
            UpdateItems();
        }

        private void ItemPressed(ShopItemView shopItemView, ShopItemInfo info)
        {
            if (_shopManager.IsOwned(info.ItemEnum, info.Id))
            {
                _shopManager.Equip(info);
            }
            else if (_shopManager.TryBuyItem(info.ItemEnum, info.Id, CurrencyType.Gold))
            {
                _shopManager.Equip(info);
            }

            UpdateItems();
        }

        private void UpdateItems()
        {
            foreach (var shopItemViews in shopItemsViews.Dictionary)
            {
                for (int i = 0; i < shopItemViews.Value.Value.Length; i++)
                {
                    shopItemViews.Value.Value[i].UpdateInfo(_shopManager, _currencyManager.Get(CurrencyType.Gold));
                }
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Gameplay.IOS.CurrencyRelated;
using UnityEngine;
using Zenject;

namespace Gameplay.IOS.Shop
{
    public class ShopManager : MonoBehaviour
    {
        private const string OwnedKey = "Owned_";
        private const string EquippedKey = "Equipped_";

        [Inject] private ShopItemsConfig _shopItemsConfig;
        [Inject] private CurrencyManager _currencyManager;       
        
        private Dictionary<ShopItemEnum, HashSet<int>> _ownedItems = new();
        private Dictionary<ShopItemEnum, int> _equippedItems = new();

        private void Awake()
        {
            LoadInfo();
        }
        
        public bool IsOwned(ShopItemEnum type, int id) =>
            _ownedItems.ContainsKey(type) && _ownedItems[type].Contains(id);
        public bool IsEquipped(ShopItemEnum type, int id) =>
            _equippedItems.TryGetValue(type, out var current) && current == id;
        
        public bool TryBuyItem(ShopItemEnum type, int id, CurrencyType currencyType)
        {
            var info = _shopItemsConfig.ShopItemInfos
                .FirstOrDefault(x => x.ItemEnum == type && x.Id == id);

            if (info == null) return false;

            if (_currencyManager.Get(currencyType) < info.Price) return false;
            if (IsOwned(type, info.Id)) return false;
    
            if (!_currencyManager.TrySpend(currencyType, info.Price))
                return false;

            if (!_ownedItems.ContainsKey(type))
                _ownedItems[type] = new HashSet<int>();

            _ownedItems[type].Add(info.Id);
            PlayerPrefs.SetString(OwnedKey + type, string.Join(",", _ownedItems[type]));
            PlayerPrefs.Save();

            return true;
        }

        public void Equip(ShopItemInfo info)
        {
            if (!IsOwned(info.ItemEnum, info.Id)) return;
            _equippedItems[info.ItemEnum] = info.Id;
            
            PlayerPrefs.SetInt(EquippedKey + info.ItemEnum, info.Id);
            PlayerPrefs.Save();
        }

        public int GetEquippedId(ShopItemEnum type)
        {
            int id = _equippedItems.TryGetValue(type, out id) ? id : -1;

            if (id == -1) DebugManager.Log(DebugCategory.Errors, $"no equipped item for {type}", LogType.Error);
            
            return id;
        }

        
        private void LoadInfo()
        {
            foreach (var info in _shopItemsConfig.ShopItemInfos)
            {
                _ownedItems[info.ItemEnum] = new();
                _equippedItems[info.ItemEnum] = PlayerPrefs.GetInt(EquippedKey + info.ItemEnum, -1);
                
                var ownedIdsStr = PlayerPrefs.GetString(OwnedKey + info.ItemEnum, "");
                if (!string.IsNullOrEmpty(ownedIdsStr))
                {
                    var ownedIds = ownedIdsStr.Split(',');
                    foreach (var idStr in ownedIds)
                    {
                        if (int.TryParse(idStr, out int id))
                        {
                            _ownedItems[info.ItemEnum].Add(id);
                        }
                    }
                }
                
                if (info.IsDefaultOwned && !_ownedItems[info.ItemEnum].Contains(info.Id))
                {
                    _ownedItems[info.ItemEnum].Add(info.Id);
                    Equip(info); 
                }
            }
        }
    }
}
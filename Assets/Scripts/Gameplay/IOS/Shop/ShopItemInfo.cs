using System;
using UnityEngine;

namespace Gameplay.IOS.Shop
{
    [Serializable]
    public class ShopItemInfo
    {
        [SerializeField] private ShopItemEnum itemEnum;
        [SerializeField] private int id;
        [Space]
        [SerializeField] private Sprite sprite;
        [Space]
        [SerializeField] private int price;
        [Space]
        [SerializeField] private bool isDefaultOwned;
        
        public ShopItemEnum ItemEnum => itemEnum;
        public int Id => id;
        
        public Sprite Sprite => sprite;
        public int Price => price;
        
        public bool IsDefaultOwned => isDefaultOwned;
    }
}
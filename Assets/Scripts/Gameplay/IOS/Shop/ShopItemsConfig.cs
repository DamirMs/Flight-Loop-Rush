using UnityEngine;

namespace Gameplay.IOS.Shop
{
    [CreateAssetMenu(menuName = "Configs/ShopItems", fileName = "ShopItems")]
    public class ShopItemsConfig : ScriptableObject
    {
        [SerializeField] private ShopItemInfo[] shopItemInfos;
        
        public ShopItemInfo[] ShopItemInfos => shopItemInfos;
    }
}
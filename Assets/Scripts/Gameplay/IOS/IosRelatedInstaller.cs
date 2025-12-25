using Gameplay.IOS.CurrencyRelated;
using Gameplay.IOS.Shop;
using UnityEngine;
using Zenject;

namespace Gameplay.IOS
{
    public class IosRelatedInstaller : MonoInstaller
    {
        [SerializeField] private ShopItemsConfig shopItemsConfig;
        
        public override void InstallBindings() 
        {
            Container.Bind<ShopManager>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<CurrencyManager>().AsSingle().NonLazy();
            Container.Bind<ShopItemsConfig>().FromInstance(shopItemsConfig).AsSingle();
        }
    }
}
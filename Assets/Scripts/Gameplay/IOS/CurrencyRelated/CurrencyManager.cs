using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using IInitializable = Zenject.IInitializable;

namespace Gameplay.IOS.CurrencyRelated
{
    public enum CurrencyType
    {
        Gold,
    }
    
    public class CurrencyManager : IInitializable, IDisposable
    {
        private const string SaveKeyPrefix = "Currency_";
        
        private Dictionary<CurrencyType, ReactiveProperty<int>> _balances = new();
        
        public void Initialize()
        {
            foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType)))
            {
                int value = PlayerPrefs.GetInt($"{SaveKeyPrefix}{type}", 0);
                _balances[type] = new(value);
            }
        }
        public void Dispose()
        {
            foreach (var kvp in _balances) PlayerPrefs.SetInt($"{SaveKeyPrefix}{kvp.Key}", kvp.Value.Value);
            PlayerPrefs.Save();
        }

        public ReactiveProperty<int> GetReactive(CurrencyType type) => _balances[type];
        public int Get(CurrencyType type) => _balances[type].Value;
        
        public void Add(CurrencyType type, int amount)
        {
            _balances[type].Value += amount;
        }
        public bool TrySpend(CurrencyType type, int amount)
        {
            if (_balances[type].Value < amount) return false;

            _balances[type].Value -= amount;
            return true;
        }
    }
}
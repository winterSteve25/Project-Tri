using System;
using System.Threading.Tasks;
using SaveLoad.Tasks;
using UnityEngine;

namespace Liquid
{
    public class Tank
    {
        private LiquidStack _liquidStack;
        private float _capacity;

        public event Action OnChanged;
        public LiquidStack Liquid => _liquidStack;
        public float Capacity => _capacity;
        
        public Tank(float capacity = 1000)
        {
            _capacity = capacity;
        }
        
        public bool Add(LiquidStack liquidStack)
        {
            if (!CanAdd(liquidStack))
            {
                Debug.LogWarning("Can not add a liquidstack with different liquid into a tank with liquids already");
                return false;
            }

            if (_liquidStack.IsEmpty)
            {
                _liquidStack = liquidStack;
            }
            else
            {
                _liquidStack += liquidStack;
            }
            
            OnChanged?.Invoke();
            return true;
        }

        public void Remove(float volume)
        {
            if (volume == 0)
            {
                return;
            }
            
            if (_liquidStack.volume - volume < 0)
            {
                _liquidStack.volume = 0;
                return;
            }

            _liquidStack.volume -= volume;
            OnChanged?.Invoke();
        }

        public bool CanAdd(LiquidStack liquidStack, bool allowOverflow = false)
        {
            if (!_liquidStack.IsEmpty && liquidStack.liquid != _liquidStack.liquid) return false;
            return !(liquidStack.volume + _liquidStack.volume > _capacity) || allowOverflow;
        }

        public async Task Serialize(SaveTask task)
        {
            await task.Serialize(_liquidStack);
        }

        public async Task Deserialize(LoadTask task)
        {
            _liquidStack = await task.Deserialize<LiquidStack>();
        }
    }
}
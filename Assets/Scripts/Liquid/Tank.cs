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
        
        public bool Add(LiquidStack liquidStack, out float unAddedAmount)
        {
            if (liquidStack.liquid != _liquidStack.liquid)
            {
                Debug.LogWarning("Can not add a liquidstack with different liquid into a tank with liquids already");
                unAddedAmount = 0f;
                return false;
            }

            OnChanged?.Invoke();
            
            if (_liquidStack.volume + liquidStack.volume > _capacity)
            {
                unAddedAmount = liquidStack.volume - (_capacity - liquidStack.volume);
                _liquidStack.volume = _capacity;
                return true;
            }
            
            _liquidStack += liquidStack;
            unAddedAmount = 0f;
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
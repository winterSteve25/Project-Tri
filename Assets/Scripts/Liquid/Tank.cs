using System.Threading.Tasks;
using SaveLoad.Tasks;
using UnityEngine;

namespace Liquid
{
    public class Tank
    {
        private LiquidStack _liquidStack;
        private float _capacity;
        
        public bool Add(LiquidStack liquidStack, out float unAddedAmount)
        {
            if (liquidStack.liquid != _liquidStack.liquid)
            {
                Debug.LogWarning("Can not add a liquidstack with different liquid into a tank with liquids already");
                unAddedAmount = 0f;
                return false;
            }

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
            if (_liquidStack.volume - volume < 0)
            {
                _liquidStack.volume = 0;
                return;
            }

            _liquidStack.volume -= volume;
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
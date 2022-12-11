using Items;
using UnityEngine;

namespace Player.Interaction
{
    public interface IUpdateBehaviourItem
    {
        void UpdateBehaviour(ref ItemStack itemStack, Vector3 playerPosition);
    }
}
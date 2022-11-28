using UnityEngine;

namespace Items.ItemTypes
{
    public interface IUpdateBehaviourItem
    {
        void UpdateBehaviour(Vector3 playerPosition);
    }
}
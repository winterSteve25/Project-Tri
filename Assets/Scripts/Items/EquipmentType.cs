using System.Collections.Generic;
using System.Linq;

namespace Items
{
    public enum EquipmentType
    {
        Outer = 0,
        Middle = 1,
        Inner = 2
    }

    public static class EquipmentTypeExtension
    {
        public static bool CanPlaceIn(this IEnumerable<EquipmentType> equipmentType, int index)
        {
            return equipmentType.Select(e => (int) e).Contains(index);
        }
    }
}
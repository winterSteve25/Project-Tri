using UnityEngine;
using KevinCastejon.MoreAttributes;

public class TestReadOnlyAttribute: MonoBehaviour
{
    [ReadOnly]
    [SerializeField] private int _healthPoints;
    [ReadOnly]
    [SerializeField] private int _damages;
}

using UnityEngine;
using KevinCastejon.MoreAttributes;

public class TestReadOnlyOnPlayAttribute : MonoBehaviour
{
    [ReadOnlyOnPlay]
    [SerializeField] private int _healthPoints;

    [ReadOnlyOnPlay(true)]
    [SerializeField] private int _damages;
}

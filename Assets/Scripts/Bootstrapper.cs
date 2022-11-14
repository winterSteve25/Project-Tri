using UnityEngine;
using UnityEngine.AddressableAssets;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod]
    public static void Run()
    {
        Addressables.InstantiateAsync("Prefabs/----SCENE MANAGER----");
    }
}
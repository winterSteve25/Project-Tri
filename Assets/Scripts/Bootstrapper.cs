using UnityEngine;
using UnityEngine.AddressableAssets;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod]
    public static void Run()
    {
        Addressables.InstantiateAsync("Prefabs/----SCENE MANAGER----");
        Addressables.InstantiateAsync("Prefabs/----AUDIO PLAYER----");
        Addressables.LoadAssetAsync<Texture2D>("UI/Mouse Cursor").Completed += handle =>
        {
            Cursor.SetCursor(handle.Result, Vector2.zero, CursorMode.Auto);
        };
    }
}
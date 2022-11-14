using System.Diagnostics;
using CommandTerminal;
using Items;
using Player;
using UnityEngine;
using Utils.Data;
using World;
using Debug = UnityEngine.Debug;

public class Commands
{
    /// <summary>
    /// Debug command used to generate a chunk of the world
    /// First parameter used to determine the xOffset of the chunk. Where in the xPosition will it start generating
    /// Second parameter used to determine the yOffset of the chunk. Where in the yPosition will it start generating
    /// Third parameter used to determine the width of the chunk
    /// Fourth parameter used to determine the height of the chunk
    /// </summary>
    [RegisterCommand(command_name: "generate", Name = "generate", Help = "Generates cave by calling \"Generate\" in the CaveGenerator present in the current level", MinArgCount = 4, MaxArgCount = 4)]
    private static void GenerateCave(CommandArg[] args)
    {
        var caveGenerator = Object.FindObjectOfType<WorldGenerator>();
        if (caveGenerator == null)
        {
            Debug.LogError("Tried to generate world but no valid CaveGenerator is present");
            return;
        }

        var xOffset = args[0].Int;
        var yOffset = args[1].Int;
        var width = args[2].Int;
        var height = args[3].Int;

        if (Terminal.IssuedError)
        {
            return;
        }

        var stopwatch = Stopwatch.StartNew();
        caveGenerator.GenerateArea(width, height, xOffset, yOffset);
        Debug.Log($"{width}x{height} was generated with offset ({xOffset}, {yOffset}). Took {stopwatch.ElapsedMilliseconds}ms");
    }

    /// <summary>
    /// Debug command used to give an item to the player inventory
    /// First parameter is the name of the item
    /// [Optional - defaults to 1 if no argument is provided] Second parameter is the amount of item
    /// </summary>
    [RegisterCommand(command_name: "give", Name = "give", Help = "Gives item to player", MinArgCount = 1, MaxArgCount = 2)]
    private static void GiveItem(CommandArg[] args)
    {
        var playerInv = Object.FindObjectOfType<PlayerInventory>();
        if (playerInv == null)
        {
            Debug.LogError("Tried to give item to player but no valid player inventory is present");
            return;
        }

        var name = args[0].String.ToLower().Replace("_", " ");
        var item = ItemsRegistry.Instance.Items.Find(i => i.name.ToLower() == name);

        if (item == null || Terminal.IssuedError)
        {
            Debug.LogError($"Can not find item with name {name}");
        }

        var itemStack = new ItemStack(item, args.Length == 1 ? 1 :args[1].Int);
        if (playerInv.TryAddItem(playerInv.transform.position, itemStack))
        {
            Debug.Log($"Added {itemStack} to player inventory");
        }
    }

    /// <summary>
    /// Debug command used to list all the items available
    /// </summary>
    [RegisterCommand(command_name: "items", Name = "items", Help = "Prints a list of all items loaded", MinArgCount = 0, MaxArgCount = 0)]
    private static void ListItems(CommandArg[] args)
    {
        foreach (var i in ItemsRegistry.Instance.Items)
        {
            Debug.Log(i.name.Replace(" ", "_"));
        }
    }

    [RegisterCommand(command_name: "seed", Name = "seed", Help = "Prints the seed of the current world", MinArgCount = 0, MaxArgCount = 0)]
    private static void Seed(CommandArg[] args)
    {
        var worldGenerator = Object.FindObjectOfType<WorldGenerator>();
        if (Terminal.IssuedError) return;
        Debug.Log("The world seed is: " + worldGenerator.WorldSeed);
    }
}
using System.Linq;
using Items;
using Registries;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Systems.Craft;
using Tiles;
using UnityEditor;
using UnityEngine;

namespace Editor.Registries
{
    public sealed class GameRegistriesEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("Project Tri/Registries")]
        private static void ShowWindow()
        {
            var window = GetWindow<GameRegistriesEditorWindow>();
            window.titleContent = new GUIContent("Project Tri Registries");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            var duplicate = new OdinMenuTree(true);
            
            TilesRegistry.Instance.UpdateEntries();
            ItemsRegistry.Instance.UpdateEntries();
            RecipesRegistry.Instance.UpdateEntries();
            
            SetupTree(tree);
            SetupTree(duplicate);

            duplicate.EnumerateTree(menuItem => menuItem.ChildMenuItems.Any() && menuItem.GetFullPath().Contains("/"), false)
                .ForEach(menuItem =>
                {
                    var path = menuItem.GetFullPath();
                    if (path.StartsWith("Tiles"))
                    {
                        tree.Add(path, new BaseTable<TileTableEntry, TriTile>(Resources.LoadAll<TriTile>(path), x => new TileTableEntry(x)));
                    }
                    else if (path.StartsWith("Items"))
                    {
                        tree.Add(path, new BaseTable<ItemTableEntry, TriItem>(Resources.LoadAll<TriItem>(path), x => new ItemTableEntry(x)));
                    }
                });
            
            return tree;
        }
        
        private void AddDragAndDeleteHandlers(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
            menuItem.OnRightClick += item =>
            {
                if (item.ChildMenuItems.Any()) return;
                var genericMenu = new GenericMenu();
                genericMenu.AddItem(new GUIContent("Delete"), false, () =>
                {
                    AssetDatabase.DeleteAsset("Assets/Resources/" + item.GetFullPath() + ".asset");
                });
                genericMenu.ShowAsContext();
            };
        }
        
        protected override void OnBeginDrawEditors()
        {
            var selected = MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = MenuTree.Config.SearchToolbarHeight;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Tile")))
                {
                    ScriptableObjectCreator.ShowDialog<TriTile>("Assets/Resources/Tiles/", TrySelectMenuItemWithObject);
                }
                
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Item")))
                {
                    ScriptableObjectCreator.ShowDialog<TriItem>("Assets/Resources/Items/", TrySelectMenuItemWithObject);
                }
                
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Recipe")))
                {
                    ScriptableObjectCreator.ShowDialog<CraftingRecipe>("Assets/Resources/Recipes/", TrySelectMenuItemWithObject);
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        private void SetupTree(OdinMenuTree tree)
        {
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;

            tree.Add("Tiles", new BaseTable<TileTableEntry, TriTile>(TilesRegistry.Instance.Entries.Keys.ToList(), x => new TileTableEntry(x)));
            tree.Add("Items", new BaseTable<ItemTableEntry, TriItem>(ItemsRegistry.Instance.Entries.Keys.ToList(), x => new ItemTableEntry(x)));
            tree.Add("Recipes", new BaseTable<RecipeTableEntry, CraftingRecipe>(RecipesRegistry.Instance.Entries.Keys.ToList(), x => new RecipeTableEntry(x)));
            
            tree.AddAllAssetsAtPath("Tiles", "Assets/Resources/Tiles/", typeof(TriTile), true)
                .ForEach(AddDragAndDeleteHandlers)
                .AddIcons<TriTile>(x => x.Sprite);

            tree.AddAllAssetsAtPath("Items", "Assets/Resources/Items/", typeof(TriItem), true)
                .ForEach(AddDragAndDeleteHandlers)
                .AddIcons<TriItem>(x => x.sprite);
            
            tree.AddAllAssetsAtPath("Recipes", "Assets/Resources/Recipes/", typeof(CraftingRecipe), true)
                .ForEach(AddDragAndDeleteHandlers)
                .AddIcons<CraftingRecipe>(x => x.result.IsEmpty ? null : x.result.item.sprite);
        }
    }
}
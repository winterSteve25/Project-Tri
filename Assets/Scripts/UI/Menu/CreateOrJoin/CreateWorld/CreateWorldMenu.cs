using System.Linq;
using TMPro;
using UI.Utilities;
using UnityEngine;
using Utils.Data;
using World;
using Random = UnityEngine.Random;

namespace UI.Menu.CreateOrJoin.CreateWorld
{
    public class CreateWorldMenu : Menu
    {
        [SerializeField] private TMP_InputField worldName;
        [SerializeField] private ParsedInput seed;
        [SerializeField] private ParsedInput worldWidth;
        [SerializeField] private ParsedInput worldHeight;

        private void Start()
        {
            NewRandomSeed();
        }

        public void Create()
        {
            var isInt = seed.TryInt(out var result);
            var s = isInt ? result : seed.Text.Aggregate(0, (current, c) => current + c);
            GlobalData.Set(GlobalDataKeys.GameState, GameState.GeneratingWorld);
            GlobalData.Set(GlobalDataKeys.CurrentWorldSettings, new WorldSettings(worldName.text, s, worldWidth.Int, worldHeight.Int));
            StartCoroutine(NextScene());
        }

        public void NewRandomSeed()
        {
            var inputField = seed.InputField == null ? seed.GetComponent<TMP_InputField>() : seed.InputField;
            inputField.text = Random.Range(-10000, 10000).ToString();
        }
    }
}
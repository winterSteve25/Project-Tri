using System;
using System.Linq;
using KevinCastejon.MoreAttributes;
using UI.Utilities;
using UnityEngine;
using Utils;
using Utils.Data;
using World;
using Random = UnityEngine.Random;

namespace UI.Menu
{
    public class CreateWorldMenu : MonoBehaviour
    {
        [SerializeField] private ParsedInput seed;
        [SerializeField] private ParsedInput worldWidth;
        [SerializeField] private ParsedInput worldHeight;
        [Scene, SerializeField] private string loadingScene;

        private void Start()
        {
            NewRandomSeed();
        }

        public void Create()
        {
            var isInt =  seed.TryInt(out var result);
            var s = isInt ? result : seed.Text.Aggregate(0, (current, c) => current + c);
            GlobalData.Set(GlobalDataKeys.WorldSettings, new WorldSettings(s, worldWidth.Int, worldHeight.Int));
            SceneManager.Instance.LoadScene(loadingScene);
        }

        public void NewRandomSeed()
        {
            seed.InputField.text = Random.Range(-10000, 10000).ToString();
        }
    }
}
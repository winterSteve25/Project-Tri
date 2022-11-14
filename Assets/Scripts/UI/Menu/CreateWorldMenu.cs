using KevinCastejon.MoreAttributes;
using UnityEngine;
using Utils;
using Utils.Data;
using World;

namespace UI.Menu
{
    public class CreateWorldMenu : MonoBehaviour
    {
        [SerializeField] private ParsedInput seed;
        [SerializeField] private ParsedInput worldWidth;
        [SerializeField] private ParsedInput worldHeight;
        [Scene, SerializeField] private string loadingScene;
        
        public void Create()
        {
            var s = Random.Range(-10000, 10000);
            
            if (seed.Text != string.Empty)
            {
                s = seed.Int;
            }
            
            GlobalData.Set(GlobalDataKeys.WorldSettings, new WorldSettings(s, worldWidth.Int, worldHeight.Int));
            SceneManager.Instance.LoadScene(loadingScene);
        }
    }
}
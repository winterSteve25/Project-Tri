using System.Collections;
using System.Linq;
using SaveLoad.Interfaces;
using UI.TextContents;
using UnityEngine;

namespace SaveLoad
{
    public class AutoSave : MonoBehaviour
    {
        [SerializeField] private GameObject savingNotification;
        private TextContent _notification;
        
        private void Start()
        {
            // auto save every 10 minutes
            InvokeRepeating(nameof(Save), 120, 600);
            
            _notification = TextContent.Empty();
            _notification.AddLayer(savingNotification);
        }

        private void Save()
        {
            StartCoroutine(SaveCoroutine());
        }

        private IEnumerator SaveCoroutine()
        {
            NotificationManager.CreateNotification(_notification, NotificationManager.NotificationPosition.TopLeft, 200);

            yield return new WaitForSeconds(1f);
            
            yield return FindObjectsOfType<MonoBehaviour>()
                .OfType<ICustomWorldData>()
                .ToList()
                .Save();
        }
    }
}
using System.Collections.Generic;
using Audio;
using UnityEngine;

namespace UI.Menu.Title
{
    public class TitleScreen : MonoBehaviour
    {
        private Transform _camera;
        private Stack<MenuTransition> _lastTransition;
        
        [SerializeField] private MenuTransition toPlay;
        [SerializeField] private MenuTransition toSettings;
        [SerializeField] private MenuTransition toCreate;
        [SerializeField] private MenuTransition toJoin;
        [SerializeField] private PlayableAudio menuMusic;

        private void Start()
        {
            _camera = Camera.main.transform;
            _lastTransition = new Stack<MenuTransition>();
            menuMusic.Play();
        }

        public void Play()
        {
            TransitionTo(toPlay);
        }

        public void Settings()
        {
            TransitionTo(toSettings);
        }

        public void Back()
        {
            _lastTransition.Pop()?.TransitionBack(_camera);
        }

        public void CreateWorld()
        {
            TransitionTo(toCreate);
        }

        public void JoinWorld()
        {
            TransitionTo(toJoin);
        }

        public void Exit()
        {
            Application.Quit();
        }

        public void TransitionTo(MenuTransition menuTransition)
        {
            menuTransition.Transition(_camera);
            _lastTransition.Push(menuTransition);
        }
    }
}

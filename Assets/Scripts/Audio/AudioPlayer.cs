﻿using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Audio
{
    public class AudioPlayer : SerializedSingleton<AudioPlayer>
    {
        [OdinSerialize]
        private Dictionary<AudioType, AudioSource> audioSources;

        [TabGroup("UI")]
        [AssetList(Path = "/Resources/Audio/UI", AutoPopulate = true)]
        public List<SoundClip> uiAudio;

        [TabGroup("Music")]
        [AssetList(Path = "/Resources/Audio/Music", AutoPopulate = true)]
        public List<SoundClip> musicAudio;

        public static void PlayAudio(PlayableAudio playableAudio, bool waitToFinish = true, float fadeDuration = 1f, AudioSource audioSource = null)
        {
            if (audioSource == null)
                audioSource = Instance.audioSources[playableAudio.audioType];

            if (audioSource == null)
            {
                Debug.LogError("No default audio source found!");
                return;
            }

            if (audioSource.isPlaying && waitToFinish) return;

            var sound = playableAudio.soundToPlay;
            audioSource.clip = sound.clip;
            audioSource.loop = sound.loop;
            if (fadeDuration > 0)
            {
                audioSource.volume = 0;
                audioSource.DOFade(sound.volume + Random.Range(-sound.volumeVariation, sound.volumeVariation), fadeDuration);
            }
            else
            {
                audioSource.volume = sound.volume + Random.Range(-sound.volumeVariation, sound.volumeVariation);
            }
            audioSource.pitch = sound.pitch + Random.Range(-sound.pitchVariation, sound.pitchVariation);
            audioSource.Play();
        }
        
        [ShowIf("@audioSources.Count <= 0")]
        [Button]
        private void AddAudioSourceForAudioTypes()
        {
            audioSources = new Dictionary<AudioType, AudioSource>();
            audioSources.Clear();
            foreach (AudioType type in Enum.GetValues(typeof(AudioType)))
            {
                audioSources.Add(type, gameObject.AddComponent<AudioSource>());
            }
        }
    }
}






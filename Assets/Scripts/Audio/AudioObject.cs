using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Audio
{
    [System.Serializable]
    public class AudioObject
    {
        [LabelText("SFX Type")]
        [LabelWidth(100)]
        [OnValueChanged(nameof(SfxChange))]
        [InlineButton(nameof(Play))]
        public AudioType audioType = AudioType.UI;

        [LabelText("$_soundLabel")]
        [LabelWidth(100)]
        [OnValueChanged(nameof(SfxChange))]
        public SoundClip soundToPlay;
        private string _soundLabel = "SFX";

        [SerializeField]
        private bool showSettings = false;

        [ShowIf("showSettings")]
        [SerializeField]
        private bool editSettings = false;

        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        [ShowIf("showSettings")]
        [EnableIf("editSettings")]
        [SerializeField]
        private SoundClip sfxBase;

        [Title("Audio Source")]
        [ShowIf("showSettings")]
        [EnableIf("editSettings")]
        [SerializeField]
        private bool waitToPlay = true;

        [ShowIf("showSettings")]
        [EnableIf("editSettings")]
        [SerializeField]
        private bool useDefault = true;

        [DisableIf("useDefault")]
        [ShowIf("showSettings")]
        [EnableIf("editSettings")]
        [SerializeField]
        private AudioSource audiosource;

        private void SfxChange()
        {
            // keep the label up to date
            _soundLabel = audioType + " SFX";

            // keep the displayed "SFX clip" up to date
            sfxBase = soundToPlay;
        }

        private List<SoundClip> SfxType()
        {
            return audioType switch
            {
                AudioType.UI => AudioPlayer.Instance.uiAudio,
                AudioType.Music => AudioPlayer.Instance.musicAudio,
                _ => AudioPlayer.Instance.uiAudio
            };
        }

        public void Play()
        {
            if (useDefault || audiosource == null)
                AudioPlayer.PlayAudio(this, waitToPlay);
            else
                AudioPlayer.PlayAudio(this, waitToPlay, audioSource: audiosource);
        }
    }
}

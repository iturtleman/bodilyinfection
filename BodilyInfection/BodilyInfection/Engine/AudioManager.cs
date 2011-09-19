using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BodilyInfection
{
    class AudioManager
    {
        public Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();
        public Dictionary<string, Song> backgroundMusic = new Dictionary<string, Song>();

        public void AddBackgroundMusic(string name)
        {
            backgroundMusic[name] = This.Game.Content.Load<Song>("Audio/" + name);
        }

        public void PlayBackgroundMusic(string name)
        {
            if (backgroundMusic.ContainsKey(name))
            {
                try
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(backgroundMusic[name]);
                }
                catch (InvalidOperationException)
                {
                    MediaPlayer.Stop();
                }
            }
        }

        public void AddSoundEffect(string name)
        {
            soundEffects[name] = This.Game.Content.Load<SoundEffect>("Audio/" + name);
        }

        public void PlaySoundEffect(string name)
        {
            if (soundEffects.ContainsKey(name))
            {
                soundEffects[name].Play();
            }
        }

        public void PlaySoundEffect(string name, float volume)
        {
            if (soundEffects.ContainsKey(name))
            {
                var sound = soundEffects[name].CreateInstance();
                sound.Volume = volume;
                sound.Play();
            }
        }

        public void Pause()
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Pause();
            }
            else if (MediaPlayer.State == MediaState.Paused)
            {
                MediaPlayer.Resume();
            }
        }

        internal void Stop()
        {
            MediaPlayer.Stop();
        }
    }
}

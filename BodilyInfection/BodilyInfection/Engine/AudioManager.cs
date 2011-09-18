﻿using System;
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
        public Dictionary<string, Song> backgroundMusic = new Dictionary<string,Song>();

        public void AddBackgroundMusic(string name)
        {
            if ( !backgroundMusic.ContainsKey(name))    // load fix
                backgroundMusic.Add(name, This.Game.Content.Load<Song>("Audio/" + name));
        }

        public void PlayBackgroundMusic(string name)
        {
            if (backgroundMusic.ContainsKey(name))
            {
                MediaPlayer.Play(backgroundMusic[name]);
            }
        }

        public void PlaySoundEffect(string name)
        {
            if (soundEffects.ContainsKey(name))
            {
                soundEffects[name].Play();
            }
        }

        public void Pause()
        {
            if(MediaPlayer.State == MediaState.Playing){
                MediaPlayer.Pause();
            }
            else if(MediaPlayer.State == MediaState.Paused){
                MediaPlayer.Resume();
            }
        }

        internal void Stop()
        {
            MediaPlayer.Stop();
        }
    }
}
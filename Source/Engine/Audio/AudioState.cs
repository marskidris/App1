using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace App1.Source.Engine.Audio;

public class AudioState
{
    private Song Jammin;
    private SoundEffect moneySoundEffect;
    private SoundEffectInstance moneySoundInstance;
    private List<SoundEffectInstance> activeSoundInstances = new List<SoundEffectInstance>();
    private float originalMusicVolume = 1.0f;
    private bool isMoneySoundPlaying = false;
    
    private static AudioState instance;

    public static AudioState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AudioState();
            }
            return instance;
        }
    }

    public void LoadContent(ContentManager content)
    {
        // Load the Toejam Jammin song
        Jammin = content.Load<Song>("Audio/01 - Toejam Jammin");
        
        // Load the Money sound effect
        moneySoundEffect = content.Load<SoundEffect>("Audio/Money!");
    }

    public void PlayBackgroundMusic()
    {
        if (Jammin != null)
        {
            // Set the song to repeat
            MediaPlayer.IsRepeating = true;
            // Set volume (0.0f to 1.0f)
            MediaPlayer.Volume = 1.0f;
            // Play the song
            MediaPlayer.Play(Jammin);
        }
    }

    public void StopMusic()
    {
        MediaPlayer.Stop();
    }

    public void PauseMusic()
    {
        MediaPlayer.Pause();
    }

    public void ResumeMusic()
    {
        MediaPlayer.Resume();
    }

    public void SetVolume(float volume)
    {
        // Clamp volume between 0.0 and 1.0
        MediaPlayer.Volume = MathHelper.Clamp(volume, 0.0f, 1.0f);
    }
    
    public void PlayMoneySoundWithVolumeAdjustment()
    {
        if (moneySoundEffect == null)
        {
            return;
        }
        
        // Store original volume and lower the music to 0.8
        originalMusicVolume = MediaPlayer.Volume;
        MediaPlayer.Volume = 0.8f;
        isMoneySoundPlaying = true;
        
        foreach (var soundInstance in activeSoundInstances)
        {
            if (soundInstance.State == SoundState.Playing)
            {
                soundInstance.Volume = 0.8f;
            }
        }
        
        // Play the money sound at full volume
        moneySoundInstance = moneySoundEffect.CreateInstance();
        moneySoundInstance.Volume = 1.0f;
        moneySoundInstance.Play();
        
        activeSoundInstances.Add(moneySoundInstance);
    }
    
    public void Update()
    {
        // Clean up stopped sound instances
        activeSoundInstances.RemoveAll(sound => sound.State == SoundState.Stopped);
        
        // Restore music volume when money sound finishes
        if (isMoneySoundPlaying && moneySoundInstance != null && moneySoundInstance.State == SoundState.Stopped)
        {
            MediaPlayer.Volume = originalMusicVolume;
            isMoneySoundPlaying = false;
            moneySoundInstance = null;
        }
    }
}
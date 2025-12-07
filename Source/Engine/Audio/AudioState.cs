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
    private SoundEffect boogeymanSoundEffect;
    private SoundEffectInstance boogeymanSoundInstance;
    private List<SoundEffectInstance> activeSoundInstances = new List<SoundEffectInstance>();
    private float originalMusicVolume = 1.0f;
    private bool isMoneySoundPlaying = false;
    private bool isBoogeymanSoundPlaying = false;
    
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
        Jammin = content.Load<Song>("Audio/01 - Toejam Jammin");
        moneySoundEffect = content.Load<SoundEffect>("Audio/Money!");
        boogeymanSoundEffect = content.Load<SoundEffect>("Audio/Boogeyman");
    }

    public void PlayBackgroundMusic()
    {
        if (Jammin != null)
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 1.0f;
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
        MediaPlayer.Volume = MathHelper.Clamp(volume, 0.0f, 1.0f);
    }
    
    public void PlayMoneySoundWithVolumeAdjustment()
    {
        if (moneySoundEffect == null)
        {
            return;
        }
        
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
        
        moneySoundInstance = moneySoundEffect.CreateInstance();
        moneySoundInstance.Volume = 1.0f;
        moneySoundInstance.Play();
        
        activeSoundInstances.Add(moneySoundInstance);
    }
    
    public void PlayBoogeymanSound()
    {
        if (boogeymanSoundEffect == null)
        {
            return;
        }
        
        originalMusicVolume = MediaPlayer.Volume;
        MediaPlayer.Volume = 0.8f;
        isBoogeymanSoundPlaying = true;
        
        foreach (var soundInstance in activeSoundInstances)
        {
            if (soundInstance.State == SoundState.Playing)
            {
                soundInstance.Volume = 0.8f;
            }
        }
        
        boogeymanSoundInstance = boogeymanSoundEffect.CreateInstance();
        boogeymanSoundInstance.Volume = 1.0f;
        boogeymanSoundInstance.Play();
        
        activeSoundInstances.Add(boogeymanSoundInstance);
    }
    
    public void Update()
    {
        activeSoundInstances.RemoveAll(sound => sound.State == SoundState.Stopped);
        
        if (isMoneySoundPlaying && moneySoundInstance != null && moneySoundInstance.State == SoundState.Stopped)
        {
            MediaPlayer.Volume = originalMusicVolume;
            isMoneySoundPlaying = false;
            moneySoundInstance = null;
        }
        
        if (isBoogeymanSoundPlaying && boogeymanSoundInstance != null && boogeymanSoundInstance.State == SoundState.Stopped)
        {
            MediaPlayer.Volume = originalMusicVolume;
            isBoogeymanSoundPlaying = false;
            boogeymanSoundInstance = null;
        }
    }
}
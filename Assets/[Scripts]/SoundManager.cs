//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 24, 2021
//      File            : SoundManager.cs
//      Description     : Singleton containing all needed audio clips that can be accessed and played on attached audio sources.
//      History         :   v0.5 - Added References to all Audio Clips and created corresponding functions to access and play the sounds.
//                          v0.7 - Created "Manager" like functions to dynamically instantiate the needed number of audio Sources for enemy units and towers.
// 
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum PlayerSoundStates
{
    RUN,
    JUMP,
    LIGHTATTACK,
    HEAVYATTACK,
    LANDING,
    HURT,
    DIE
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    public AudioSource menuMusicSource;
    public AudioSource bgMusicSource;
    public AudioSource uiSoundSource;
    public List<AudioSource> playerAudioSourceList;
    public List<AudioSource> monsterAudioSourceList;
    public List<AudioSource> effectAudioSourceList;

    //Music Tracks
    public AudioClip menuMusic;
    public AudioClip gameplayTrack1;
    public AudioClip gameplayTrack2;
    public AudioClip endMusic;

    //Player Sounds:  0 = walk/run, 1 = jump, 2 = attack, 3 = heavyattack, 4 = hurt, 5 = die
    public AudioClip[] playerSounds;

    //UI sounds
    public AudioClip[] clickForward;
    public AudioClip[] clickBack;

    //Enemy Sounds: 0 = walk/run, 1 = attack, 2 = hurt, 3 = die
    public AudioClip[] goblinSounds;
    public AudioClip[] eyeSounds;
    public AudioClip[] skeletonSounds;

    //Mixer Group
    public AudioMixerGroup[] mixergroup;

    private void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// Switches between music tracks in gameplay scene.
    /// </summary>
    private void Update()
    {
        if (!bgMusicSource.isPlaying)
        {
            if (bgMusicSource.clip == gameplayTrack1)
            {
                bgMusicSource.clip = gameplayTrack2;
                bgMusicSource.Play();
            }
            else if(bgMusicSource.clip == gameplayTrack2)
            {
                bgMusicSource.clip = gameplayTrack1;
                bgMusicSource.Play();
            }
        }
    }
    public AudioSource GetEnemyAudioSource()
    {
        foreach (AudioSource source in monsterAudioSourceList)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        AudioSource temp = transform.GetChild(0).gameObject.AddComponent<AudioSource>();
        temp.outputAudioMixerGroup = mixergroup[4];
        temp.playOnAwake = false;
        temp.volume = 0.1f;
        monsterAudioSourceList.Add(temp);
        return temp;
    }
    /// <summary>
    /// Dynamically creating and getting audio sources for tower sounds.
    /// </summary>
    /// <returns></returns>
    public AudioSource GetPlayerAudioSource()
    {
        foreach (AudioSource source in playerAudioSourceList)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        AudioSource temp = transform.GetChild(1).gameObject.AddComponent<AudioSource>();
        temp.outputAudioMixerGroup = mixergroup[3];
        temp.playOnAwake = false;
        temp.volume = 0.1f;
        playerAudioSourceList.Add(temp);
        return temp;
    }
    public void PlayPlayerSound(PlayerSoundStates sound)
    {
        AudioSource temp = GetPlayerAudioSource();
        temp.clip = playerSounds[(int)sound];
        temp.Play();
    }
    public void PlayGoblinSound(int sound)
    {
        AudioSource temp = GetEnemyAudioSource();
        temp.clip = goblinSounds[sound];
        temp.Play();
    }
    public void PlayEyeSound(int sound)
    {
        AudioSource temp = GetEnemyAudioSource();
        temp.clip = eyeSounds[sound];
        temp.Play();
    }
    public void PlaySkeletonSound(int sound)
    {
        AudioSource temp = GetEnemyAudioSource();
        temp.clip = skeletonSounds[sound];
        temp.Play();
    }
    /// <summary>
    /// Plays Main Menu/End Menu music
    /// </summary>
    public void PlayMenuMusic()
    {
        menuMusicSource.Stop();
        bgMusicSource.Stop();
        bgMusicSource.clip = null;
        menuMusicSource.clip = menuMusic;
        menuMusicSource.Play();
    }
    /// <summary>
    /// Plays initial Gameplay music track
    /// </summary>
    public void PlayGameplayMusic()
    {
        menuMusicSource.Stop();
        bgMusicSource.Stop();
        bgMusicSource.clip = gameplayTrack1;
        bgMusicSource.Play();
    }

    public void PlayEndSceneMusic()
    {
        menuMusicSource.Stop();
        bgMusicSource.Stop();
        bgMusicSource.clip = null;
        menuMusicSource.clip = endMusic;
        menuMusicSource.Play();
    }

    /// <summary>
    /// Plays a random click sound for moving forward in menus.
    /// </summary>
    public void PlayRandomClickForward()
    {
        uiSoundSource.clip = clickForward[Random.Range(0, (clickForward.Length))];
        uiSoundSource.Play();
    }
    /// <summary>
    /// Plays a random click sound for moving backward in menus.
    /// </summary>
    public void PlayRandomClickBackward()
    {
        uiSoundSource.clip = clickBack[Random.Range(0, (clickBack.Length))];
        uiSoundSource.Play();
    }

}

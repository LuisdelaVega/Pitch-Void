using UnityEngine;
using UnityEngine.Audio;
using System;
using Random = UnityEngine.Random; //Tells Random to use the Unity Engine random number generator.

public class AudioManager : MonoBehaviour
{
  public static AudioManager instance = null;
  public Sound[] sounds;

  void Awake()
  {
    if (instance == null)
      instance = this;
    else if (instance != this)
    {
      Destroy(gameObject);
      return;
    }

    DontDestroyOnLoad(gameObject);

    foreach (Sound sound in sounds)
    {
      sound.source = gameObject.AddComponent<AudioSource>();
      sound.source.clip = sound.clip;

      sound.source.volume = sound.volume;
      sound.source.pitch = sound.pitch;
      sound.source.loop = sound.loop;
    }
  }

  private void Start() => Play("Theme");

  private Sound FindSound(string name) => Array.Find(sounds, sound => sound.name == name);

  public void Play(string name)
  {
    Sound sound = FindSound(name);
    Play(sound, sound.volume, sound.pitch);
  }
  public void Play(string name, float volume)
  {
    Sound sound = FindSound(name);
    Play(sound, volume, sound.pitch);
  }
  public void Play(String name, float volume, float pitch)
  {
    Sound sound = FindSound(name);
    Play(sound, volume, pitch);
  }
  private void Play(Sound sound, float volume, float pitch)
  {
    if (sound == null)
    {
      Debug.LogWarning("Sound: " + name + " not found!");
      return;
    }

    sound.source.volume = volume;
    sound.source.pitch = pitch;
    sound.source.Play();
  }

  public void PlayWithRandomPitch(string name, float minPitch, float maxPitch) => PlayWithRandomPitch(name, -1, minPitch, maxPitch);
  public void PlayWithRandomPitch(string name, float volume, float minPitch, float maxPitch)
  {
    Sound sound = FindSound(name);
    Play(sound, volume < 0 ? sound.volume : volume, Random.Range(minPitch, maxPitch));
  }
}

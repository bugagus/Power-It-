using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    instance = new GameObject("Spawned AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    private AudioSource musicSource;
    private AudioSource musicSource2;
    private AudioSource sfxSource;

    private bool firstMusicSourceIsPlaying;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        //Crear los AudioSources y guardarlos como referencias
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource2 = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        //Loopear la música
        musicSource.loop = true;
        musicSource2.loop = true;
    }

    public void PlayMusic(AudioClip musicClip)
    {
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicSource : musicSource2;

        activeSource.clip = musicClip;
        activeSource.volume = 1;
        activeSource.Play();
    }

    public void PlayMusicWithFade(AudioClip newClip, float transitionTime = 1.0f)
    {
        //Determinar el source que está sonando
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicSource : musicSource2;

        StartCoroutine(UpdateMusicWithFade(activeSource, newClip, transitionTime));
    }

    public void PlayMusicWithCrossFade(AudioClip musicClip, float transitionTime = 1.0f)
    {
        //Determina que source está activo
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicSource : musicSource2;
        AudioSource newSource = (firstMusicSourceIsPlaying) ? musicSource2 : musicSource;

        //Cambia el source
        firstMusicSourceIsPlaying = !firstMusicSourceIsPlaying;

        //Setea los campos del audioSource y empieza la coroutine hacia el crossfade
        newSource.clip = musicClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, transitionTime));
    }

    private IEnumerator UpdateMusicWithCrossFade(AudioSource original, AudioSource newSource, float transitionTime)
    {
        float t = 0.0f;

        for (t = 0.0f; t <= transitionTime; t+= Time.deltaTime)
        {
            original.volume = (1 - (t / transitionTime));
            newSource.volume = (t / transitionTime);
            yield return null;
        }
        original.Stop();
    }

    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip newClip, float transitionTime)
    {
        //Comprobar que el source esta activo y sonando
        if (!activeSource.isPlaying)
            activeSource.Play();

        float t = 0.0f;

        //Fade out
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = (t / transitionTime);
            yield return null;
        }

        activeSource.Stop();
        activeSource.clip = newClip;
        activeSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void PlaySfx(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        musicSource2.volume = volume;
    }
    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}


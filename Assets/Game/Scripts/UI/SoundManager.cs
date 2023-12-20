using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void PlaySoundOneShot(AudioClip clip)
    {
        musicSource.PlayOneShot(clip);
    }

    public void PlayMusicLoop(AudioClip clip, float volume = 1f)
    {
        StartCoroutine(LoopAudio(clip, volume));
    }

    IEnumerator LoopAudio(AudioClip clip, float volume)
    {
        while (true)
        {
            musicSource.clip = clip;
            musicSource.volume = volume; // Set the volume
            musicSource.Play();

            yield return new WaitForSeconds(musicSource.clip.length);
        }
    }
    
    public void StopMusic()
    {
        musicSource.Stop();
    }
}
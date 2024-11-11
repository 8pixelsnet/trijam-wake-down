using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;   // Source for background music
    [SerializeField] private AudioSource sfxSource;     // Source for one-shot sound effects

    private void Awake()
    {
        // Ensure only one instance of MusicManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    /// <summary>
    /// Plays a one-shot sound effect.
    /// </summary>
    /// <param name="clip">The AudioClip to play.</param>
    public void PlayOneShot(AudioClip clip, float pitch = 1)
    {
        if (clip != null)
        {
            sfxSource.pitch = pitch;
            sfxSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Plays background music with looping enabled.
    /// </summary>
    /// <param name="clip">The AudioClip to play as background music.</param>
    public void PlayBackgroundMusic(AudioClip clip)
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop(); // Stop any currently playing background music
        }

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}

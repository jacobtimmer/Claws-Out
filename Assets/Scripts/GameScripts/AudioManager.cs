using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] backgroundSongs;
    [SerializeField] private float musicVolume = 0.5f;
    [SerializeField] private bool shuffleSongs = false;

    private AudioSource musicSource;
    private int currentSongIndex = 0;

    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.loop = false;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;
        musicSource.spatialBlend = 0f;
    }

    private void Start()
    {
        PlayCurrentSong();
    }

    private void Update()
    {
        if (backgroundSongs.Length == 0)
        {
            return;
        }

        if (!musicSource.isPlaying)
        {
            PlayNextSong();
        }
    }

    private void PlayCurrentSong()
    {
        if (backgroundSongs.Length == 0)
        {
            return;
        }

        musicSource.clip = backgroundSongs[currentSongIndex];
        musicSource.Play();
    }

    private void PlayNextSong()
    {
        if (shuffleSongs)
        {
            currentSongIndex = Random.Range(0, backgroundSongs.Length);
        }
        else
        {
            currentSongIndex++;

            if (currentSongIndex >= backgroundSongs.Length)
            {
                currentSongIndex = 0;
            }
        }

        PlayCurrentSong();
    }
}
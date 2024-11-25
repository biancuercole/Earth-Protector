using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource soundSource;
    [SerializeField] public SliderVolume sliderVolume;

    private List<AudioSource> audioSourcePool = new List<AudioSource>();
    [SerializeField] private int poolSize = 10;

    [Header("Music")]
    public AudioClip pacifistZoneMusic;
    public AudioClip gameMusic;
    public AudioClip bossMusic;
    public AudioClip ambientSound;
    public AudioClip menuMusic;

    [Header("Dinamic Music")]
    public AudioClip battleMusic;
    public AudioClip quietMusic;

    [Header("Player SX")]
    public AudioClip fireShot;
    public AudioClip waterShot;
    public AudioClip airShot;
    public AudioClip earthShot;
    public AudioClip powerOfGod;
    public AudioClip dash;
    public AudioClip steps;
    public AudioClip damage;
    public AudioClip death;

    [Header("Enemy : FUEGO")]
    public AudioClip flameDeath;
    public AudioClip flameShot;

    [Header("Enemy : HUMO")]
    public AudioClip smokeDeath;
    public AudioClip smokeShot; 

    [Header("Enemy : TRONCO")]
    public AudioClip logDeath;
    public AudioClip logShot;

    [Header("Enemy : JEFE")]
    public AudioClip bossDeath;
    public AudioClip bossShot;
    public AudioClip bossAwaken;

    [Header("Enemy : MAQUINAS")]
    public AudioClip machineDeath;
    public AudioClip machineSound;

    [Header("Other")]
    public AudioClip portalSound;
    public AudioClip collectSound;
    public AudioClip healSound;
    public AudioClip shrineDestroy;
    private void Start()
    {
        SetBackgroundMusic();
    }

    void Awake()
    {
        // Crear un pool de AudioSources para los sonidos de disparo
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.playOnAwake = false;
            audioSourcePool.Add(newSource);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        // Buscar un AudioSource libre en el pool
        AudioSource source = audioSourcePool.Find(s => !s.isPlaying);
        if (source != null)
        {
            source.clip = clip;
            source.Play();
        }
    }
    private void SetBackgroundMusic()
    {
       string sceneName = SceneManager.GetActiveScene().name;
       
        switch (sceneName)
        {
            case "Menu":
                ChangeBackgroundMusic(menuMusic);
                break;
           case "GameScene":
                ChangeBackgroundMusic(gameMusic);
                break;
            case "PacificZone":
                ChangeBackgroundMusic(pacifistZoneMusic);
                break;
            case "Cinematics":
                ChangeBackgroundMusic(pacifistZoneMusic);
                break;
            case "EnemyLevel":
                ChangeBackgroundMusic(bossMusic);
                break;
            case "Credits":
                ChangeBackgroundMusic(menuMusic);
                break;
            case "GameOver":
                ChangeBackgroundMusic(pacifistZoneMusic);
                break;
        }
        soundSource.Play();
        musicSource.Play();
        SetMusicVolume(sliderVolume.musicVolume);
        SetSoundVolume(sliderVolume.soundVolume);
    }

    public void playSound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }

    public void ChangeBackgroundMusic(AudioClip newMusic)
    {
        SetMusicVolume(sliderVolume.musicVolume);
        StartCoroutine(FadeOutAndChangeMusic(newMusic, 2f));
    }

    public void FadeOutMusic(float duration)
    {
        StartCoroutine(FadeOut(musicSource, duration));
    }

   
    public void FadeInMusic(float duration)
    {
        StartCoroutine(FadeIn(musicSource, duration));
    }


    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSoundVolume(float volume)
    {
        soundSource.volume = volume;    
    }

    private IEnumerator FadeOutAndChangeMusic(AudioClip newMusic, float duration)
    {
        // Hacer fade out
        yield return StartCoroutine(FadeOut(musicSource, duration));

        // Cambiar la música
        musicSource.clip = newMusic;
        musicSource.Play();

        // Hacer fade in
        yield return StartCoroutine(FadeIn(musicSource, duration));
    }

   
    public IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();
    }

  
    private IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        audioSource.volume = 0;
        audioSource.Play();

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, 0.3f, t / duration);
            yield return null;
        }

        SetMusicVolume(sliderVolume.musicVolume);  // Asegurarse de que el volumen sea 1 al final
    }
}



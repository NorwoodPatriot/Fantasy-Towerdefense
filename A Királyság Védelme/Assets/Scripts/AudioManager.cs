using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton, hogy mindenhonnan elérjük
    public static AudioManager instance;

    [Header("Hanglejátszó (Forrás)")]
    public AudioSource sfxSource;   // Ez játssza le az effekteket
    public AudioSource musicSource; // Ez játssza a zenét

    [Header("Hangfájlok (Clips)")]
    public AudioClip lovesHang;
    public AudioClip robbanasHang;
    public AudioClip talalatHang;
    public AudioClip epitesHang;
    public AudioClip gameOverHang;

    void Awake()
    {
        instance = this;
    }

    // Ezt hívjuk meg bárhonnan: AudioManager.instance.PlaySFX(...)
    public void PlaySFX(AudioClip clip)
    {
        // A PlayOneShot engedi, hogy egyszerre több hang is szóljon egymásra keverve
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
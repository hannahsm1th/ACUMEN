using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    // This class controls the sounds of the game


    //<------- Variables ------->
    public AudioSource efxSource;                                   //The sound effects for the game
    public AudioSource levelMusic;                                 //The background music
    public static SoundManager instance = null;                     //The instance for the sound manager

    //These floats modulate the music so it doesn't sound too repetitive
    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;

    //<------- Methods ------->

    void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingle(AudioClip clip)
    {
        //This plays single instances of music (sound effects)

        efxSource.clip = clip;
        efxSource.Play();
    }

    public void RandomiseSfx(params AudioClip[] clips)
    {
        //This plays a random choice from the music selection for each sound effect

        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.PlayOneShot(efxSource.clip);
    }
}

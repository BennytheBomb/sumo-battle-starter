using UnityEngine;
using SpeechIO;
public class PlayerSoundEffect : MonoBehaviour
{
    public AudioClip dropInClip;
    public AudioClip gameOverClip;
    public AudioClip collisionClip;
    public float maxPitch = 1.2f;
    public float minPitch = 0.8f;
    private GameObject previousEnemy;
    private AudioSource audioSource;
    public SpeechOut speechOut = new SpeechOut();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public float PlayerFellDown()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(gameOverClip);
        return gameOverClip.length;
    }
    public void PlayHit()
    {
        PlayClipPitched(collisionClip, minPitch, maxPitch);
    }
    public void PlayDropIn()
    {
        audioSource.PlayOneShot(dropInClip);
    }
    public void PlayEnemyHitClip(AudioClip clip, GameObject go = null)
    {
        // TODO: add logic to make sure you only play the clip when colliding with a new enemy

        // TODO: play the sound clip that is passed to the function

        // TODO: reset the source clip file to override previous clip

        // TODO: generate the speech using TTS

        // TODO: add method to call asynchronously 
    }

    //TODO: implement async method sayName

    public void StopPlayback()
    {
        audioSource.Stop();
    }

    public void PlayClipPitched(AudioClip clip, float minPitch, float maxPitch)
    {
        // little trick to make clip sound less redundant
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        // plays same clip only once, this way no overlapping
        audioSource.PlayOneShot(clip);
        audioSource.pitch = 1f;
    }

}
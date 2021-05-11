/// Hint: Commenting or uncommenting in VS
/// On Mac: CMD + SHIFT + 7
/// On Windows: CTRL + K and then CTRL + C

using UnityEngine;
using SpeechIO;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 10f;
    public GameObject focalPoint;
    public bool hasPowerup;
    private float powerupStrength = 15f;
    private int powerupAmmo = 0;
    public int powerupTime = 7;
    private bool playerFellDown;
    public GameObject powerupIndicator;
    public float explosionRadius = 10f;
    public float explosionPower = 2000f;
    public float explosionUpwardForce = 5f;
    public LayerMask explosionAffected;
    private PlayerSoundEffect soundEffects;
    // TODO: add SpeechIn component
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        soundEffects = GetComponent<PlayerSoundEffect>();

        // TODO: start listening to speech recognizer and link a function onRecognized to the speechIn object
    }
    // TODO: implement function onRecognized with debug output

    // TODO: extend the onRecognized function with general purpose commands like repeat and quit

    // TODO: implement function onApplicationQuit to kill the speech In component

    // TODO: implement function PowerUpListener that listens to the word boom to trigger a powerup

    // TODO: extend the PowerUpListener to reply when the user is out of ammo and recursively call the function
    void Update()
    {
        powerupIndicator.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);
        if (transform.position.y < -10f && !playerFellDown)
        {
            playerFellDown = true;
            float clipTime = soundEffects.PlayerFellDown();
            Destroy(gameObject, clipTime);
        }

        // TODO: before destroying the game object, kill the speech in component
        if (Input.GetButtonDown("Jump"))
        {
            ExplosionPowerup();
        }
    }

    void FixedUpdate()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);
    }
 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            CancelInvoke("PowerupCountdown"); // if we previously picked up an powerup
            Invoke("PowerupCountdown", powerupTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Enemy"))
        {
            soundEffects.PlayHit();
            soundEffects.PlayEnemyHitClip(other);
        }

        // TODO: add spoken sound clip with enemy name

        if (other.CompareTag("Enemy") && hasPowerup) //blow the enemy away!
        {
            Rigidbody enemyRigidbody = other.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.transform.position - transform.position;
            enemyRigidbody.AddForce(awayFromPlayer.normalized * powerupStrength, ForceMode.Impulse);
        }
    }
    public void ExplosionPowerup()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius, explosionAffected);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, explosionUpwardForce);
        }
    }
    void PowerupCountdown()
    {
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }
}

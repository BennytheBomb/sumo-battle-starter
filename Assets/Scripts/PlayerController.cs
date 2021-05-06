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
<<<<<<< Updated upstream
=======
    private int powerupAmmo = 2;
>>>>>>> Stashed changes
    public int powerupTime = 7;
    private bool playerFellDown;
    public GameObject powerupIndicator;
    public bool playerFellDown;
    private PlayerSoundEffect soundEffects;
    public float explosionRadius = 10f;
    public float explosionPower = 2000f;
    public float explosionUpwardForce = 5f;
    public LayerMask explosionAffected;
<<<<<<< Updated upstream
    private SpeechIn speechIn;
=======
    private PlayerSoundEffect soundEffects;
    SpeechIn speechIn;
>>>>>>> Stashed changes

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        soundEffects = GetComponent<PlayerSoundEffect>();
        speechIn = new SpeechIn(onRecognized);
<<<<<<< Updated upstream
        PowerUpListener();
    }
=======
        speechIn.StartListening();
        PowerUpListener();
    }
    async void onRecognized(string message)
    {
        // handle defined meta-commands
        switch (message)
        {
            case "repeat":
                await soundEffects.speechOut.Repeat();
                break;
            case "quit":
                await soundEffects.speechOut.Speak("Thanks for using our application. Closing down now...");
                OnApplicationQuit();
                Application.Quit();
                break;
            case "options":
                string commandlist = "";
                foreach (string command in speechIn.GetActiveCommands())
                {
                    commandlist += command + ", ";
                }
                await soundEffects.speechOut.Speak("currently available commands: " + commandlist);
                break;
        }
    }

    public void OnApplicationQuit()
    {
        speechIn.StopListening();
    }



    // TODO: implement function onApplicationQuit to kill the speech In component
>>>>>>> Stashed changes

    void Update()
    {
        powerupIndicator.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
        if (transform.position.y < -10f && !playerFellDown)
        {
            playerFellDown = true;
            float clipTime = soundEffects.PlayerFellDown();
            speechIn.StopListening();
            Destroy(gameObject, clipTime);
        }
        if (Input.GetButtonDown("Jump"))
        {
            ExplosionPowerup();
        }
        if (Input.GetButtonDown("Cancel"))
            speechIn.StopListening();
    }
    async void PowerUpListener()
    {
        string powerup = await speechIn.Listen(new string[] { "boom", "jump", "hide" });
        switch (powerup)
        {
            case "boom":
<<<<<<< Updated upstream
=======
                if (powerupAmmo <= 0)
                {
                    await soundEffects.speechOut.Speak("no ammo for:" + powerup);
                    return;
                }
>>>>>>> Stashed changes
                ExplosionPowerup();
                break;
            case "jump":
                //JumpPowerup();
                break;
            case "hide":
                //HidePowerup();
                break;
        }
        PowerUpListener();
    }
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes

    void onRecognized(string message)
    {
        Debug.Log("[" + this.GetType() + "]: " + message);
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
            Enemy enemy = other.GetComponent<Enemy>();
            soundEffects.PlayHit();
<<<<<<< Updated upstream
            soundEffects.PlayEnemyHitClip(enemy.nameClip, other);
        }
            
=======
            soundEffects.PlayEnemyHitClip(other);
        }

        // TODO: add spoken sound clip with enemy name
>>>>>>> Stashed changes

        /// challenge: when collision has tag "Enemy" and we have a powerup
        /// get the enemyRigidbody and push the enemy away from the player
        if (other.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = other.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.transform.position - transform.position;
            enemyRigidbody.AddForce(awayFromPlayer.normalized * powerupStrength, ForceMode.Impulse);
        }
    }
    public void ExplosionPowerup()
    {
        powerupAmmo--;
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
    public void OnApplicationQuit()
    {
        speechIn.StopListening();
    }
}

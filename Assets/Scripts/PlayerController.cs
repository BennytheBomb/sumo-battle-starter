/// Hint: Commenting or uncommenting in VS
/// On Mac: CMD + SHIFT + 7
/// On Windows: CTRL + K and then CTRL + C

using UnityEngine;
using DualPantoFramework;
using SpeechIO;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 10f;
    public GameObject focalPoint;
    public bool hasPowerup;
    private float powerupStrength = 15f;
    public int powerupTime = 7;
    public GameObject powerupIndicator;
    private SpeechIn speech;
    private bool movementFrozen;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        //ActivatePlayer();
        speech = new SpeechIn(onSpeechRecognized);
        speech.StartListening(new string[]{"help", "resume"});
    }

    async void onSpeechRecognized(string command) {
        if (command == "resume" && movementFrozen) {
            ResumeAfterPause();
        } else if (command == "help" && !movementFrozen) {
            ToggleMovementFrozen();
            var powerups = GameObject.FindGameObjectsWithTag("Powerup");
            if (powerups.Length > 0) {
                await GameObject.Find("Panto").GetComponent<LowerHandle>().SwitchTo(powerups[0]);
            }
        }
    }

    void ToggleMovementFrozen() {
        playerRb.constraints = movementFrozen ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Rigidbody>().constraints = movementFrozen
                                           ? RigidbodyConstraints.None
                                           : RigidbodyConstraints.FreezeAll;
        }
        movementFrozen = !movementFrozen;
    }

    async void ResumeAfterPause() {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null)
        {
            await GameObject.Find("Panto").GetComponent<LowerHandle>().SwitchTo(enemy);
        }
        ToggleMovementFrozen();
    }

    public async void ActivatePlayer()
    {
        UpperHandle upperHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
        await upperHandle.SwitchTo(gameObject, 0.2f);
        upperHandle.FreeRotation(); 
    }

    void Update()
    {
        if(!GameObject.FindObjectOfType<SpawnManager>().gameStarted) return;
        powerupIndicator.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);
    }

    void FixedUpdate()
    {
        if(!GameObject.FindObjectOfType<SpawnManager>().gameStarted) return;
        //float forwardInput = Input.GetAxis("Vertical");
        //playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);
        PantoMovement();
    }

    void PantoMovement()
    {
        float rotation = GameObject
                            .Find("Panto")
                            .GetComponent<UpperHandle>()
                            .GetRotation();
        Vector3 direction = Quaternion.Euler(0, rotation, 0) * focalPoint.transform.forward; //Vector3.forward;
        playerRb.AddForce(speed * direction);
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
        /// challenge: when collision has tag "Enemy" and we have a powerup
        /// get the enemyRigidbody and push the enemy away from the player
        if (other.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = other.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.transform.position - transform.position;
            enemyRigidbody.AddForce(awayFromPlayer.normalized * powerupStrength, ForceMode.Impulse);
        }
    }

    void PowerupCountdown()
    {
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }
}

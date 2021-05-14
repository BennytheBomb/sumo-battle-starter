using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 8f;
    private Rigidbody enemyRb;
    private GameObject player;
    public string displayName;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        /// challenge: set lookDirection to "enemy to player" vector
        if (player != null)
        {
            Vector3 lookDirection = player.transform.position - transform.position;
            enemyRb.AddForce(lookDirection.normalized * speed);
        }
    }
}

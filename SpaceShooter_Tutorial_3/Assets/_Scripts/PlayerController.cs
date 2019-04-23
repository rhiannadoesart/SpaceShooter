using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float tilt;
    public Boundary boundary;
    public Text LivesText;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    private AudioSource audioSource;

    private float nextFire;
    private Rigidbody rb;
    private int lives;
    private GameController gameController;

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        lives = 3;
        SetLivesText();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            audioSource.Play();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerUpFire"))
        {
            other.gameObject.SetActive(false);
            fireRate = 0.1f;
        }
        if (other.gameObject.CompareTag("PowerUpLife"))
        {
            other.gameObject.SetActive(false);
            lives = lives + 1;
            SetLivesText();
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            lives = lives - 1;
            SetLivesText();
        }
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * speed;

        rb.position = new Vector3
        (
             Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
             0.0f,
             Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }

    public void SetLivesText()
    {
        LivesText.text = "Lives: " + lives.ToString();
        if (lives < 1)
        {
            gameController.GameOver();
        }
    }   
}
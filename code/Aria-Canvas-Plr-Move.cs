using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Josh_Player_Move : MonoBehaviour
{

    #region Serialized Private Variables
    [SerializeField] private SoundSaveHandler ssH;
    public PlayerInfo info = new PlayerInfo();
    public float xSpeed; // X Axis Speed (Controls how fast the player can move)
    [SerializeField] private float sprintMultiplier = 1.3f; // Multiplier for sprinting speed
    [SerializeField] private float jumpPower; // Y Axis Speed (Controls how high player can jump)
    [SerializeField] private Animator playerAnim;
    [SerializeField] private List<AudioClip> playerSounds;
    [SerializeField] private AudioSource playerAudio;
    [SerializeField] private CameraMove cam;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private PlrSaveSystem PlayersaveSystem;
    #endregion

    #region Private Variables
    private PlrSaveSystem saveSystem;
    private float x;  // X Axis speed in use
    private float y; // Y Axis speed in use
    private Rigidbody2D playerMovement; // Rigidbody
    private bool shouldJump = false;  // Will be set to true when jump button is pressed
    private bool isSprinting = false;  // Will define if the player is sprinting
    private PlrSaveSystem.PlrData savSystem;
    #endregion

    #region Hidden Public Variables
    [HideInInspector] public bool isGrounded; // Will define if the player is on the ground
    [HideInInspector] public bool isFrozen;
    #endregion


    void OnEnable()
    {
        float vol = PlayerPrefs.GetFloat("SFXVolume");
        playerAudio.volume = vol;
    }

    private void Awake()
    {
        if(PlayersaveSystem != null)
        {
            savSystem = PlayersaveSystem.LoadData();
        }
        if (savSystem != null)
        {
            info.playerXP = int.Parse(savSystem.plrXP);
            info.playerHealth = int.Parse(savSystem.plrHealth);
        }

        float.TryParse(ssH.loadedPlayer.SFXSound, out float parsedSFXSound);
        playerAudio.volume = parsedSFXSound;
        transform.position = spawnPoint.position;
        Cursor.lockState = CursorLockMode.Locked;
        playerMovement = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isFrozen)
        {
            playerAnim.speed = 1;
            playerAnim.SetBool("isGrounded", isGrounded);
            if (isGrounded)
            {
                playerAnim.SetBool("canJump", true);
            }
            if (playerMovement) { playerAnim.SetFloat("xVelocity", playerMovement.velocity.x); }
            // Keep input checking in Update to ensure no input is missed
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isSprinting = !isSprinting;
            }

            if (Input.GetButtonDown("Jump"))
            {
                // Maybe set a flag here since we're moving the actual jump logic to FixedUpdate
                shouldJump = true;
            }
        }
        else
        {
            playerAnim.speed = 0;
        }
    }

    private void FixedUpdate()
    {
        if (!isFrozen)
        {
            if (playerMovement)
            {
                if (playerMovement.velocity.x < 0)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    playerAnim.SetBool("isLeft", true);
                }
                else if (playerMovement.velocity.x > 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    playerAnim.SetBool("isLeft", false);
                }

            }

            x = (isSprinting ? xSpeed * sprintMultiplier : xSpeed) * 1000;
            y = jumpPower * 100;

            float horizontalInput = Input.GetAxis("Horizontal");
            if (playerMovement)
            {
                playerMovement.velocity = new Vector2(horizontalInput * x * Time.fixedDeltaTime, playerMovement.velocity.y);
            }

            // Apply gravity
            playerMovement.AddForce(Vector2.down * Mathf.Abs(Physics2D.gravity.y) * 250 * Time.fixedDeltaTime);


            if (shouldJump && isGrounded)
            {
                playerMovement.AddForce(Vector2.up * y);
                PlaySFX(playerSounds[1]);
                shouldJump = false;  // Reset the flag
            }
        }
    }

    private void ChangeJump()
    {
        playerAnim.SetBool("canJump", false);
    }

    private void FlipUpsideDown()
    {
        transform.eulerAngles = new Vector3(0, 0, 180);
    }

    private void PlaySFX(AudioClip sound)
    {
        playerAudio.Stop();
        playerAudio.PlayOneShot(sound, 1);
    }

    private void StopSFX()
    {
        playerAudio.Stop();
    }

    public void Die()
    {
        PlayerPrefs.DeleteAll();
        playerAnim.SetBool("canDuck", false);
        playerAnim.SetBool("isGrounded", false);
        shouldJump = false;
        isGrounded = false;
        playerAnim.SetBool("isDead", true);
        cam.Smoothing = 0;
        Destroy(playerMovement);
        PlaySFX(playerSounds[0]);
        Invoke("Respawn", 0.5f);
    }

    public void Respawn()
    {
        SceneManager.LoadScene(0);
    }
}

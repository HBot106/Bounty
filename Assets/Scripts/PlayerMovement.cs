using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public float playerSpeed = 3f, crouchSpd = 1.5f;

    private CharacterController controller;
    private float curSpeed = 0f, speedSmoothVel = 0f, speedSmoothTime = 0.01f, rotationSpd = 0.15f;
    private Transform cameraTrans;
    private Rigidbody rgdbdy;
    public float guardHitForce;

    [HideInInspector]
    public bool isCrouching = false;
    public bool isJumping = false;
    public float jumpPower = 1f;

    public bool isStabbing = false;
    private float stabSpeed = .5f;
    public int stabTimer;
    public GameObject knife;
    public GameObject knifeHitBox;

    //health
    public int health = 3;
    private HeartsHealthVisual healthUI;
    public GameObject gameOverScreen;

    // Projectiles
    public GameObject player;
    public GameObject projectile_knife;
    public GameObject projectile_rock;
    public GameObject projectile_fireball;
    private Vector3 knife_and_rock_start_pos;

    public static int number_of_knives = 2;
    public static int number_of_rocks = 3;

    //Sound for player 
    public AudioSource Player_AS;
    public AudioClip Whoosh;
    public AudioClip Fireball;

    // Animation
    private Animator animator;

    // Bounty
    public BountyTargetScript bountyScript;


    private GameManager gm;
    private GameManager GM
    {
        get
        {
            if (gm == null)
            {
                gm = (GameManager)FindObjectOfType(typeof(GameManager));
                //^ this is the important line.
            }
            return gm;
        }
    }



    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTrans = Camera.main.transform;
        rgdbdy = GetComponent<Rigidbody>();
        stabTimer = 0;
        knifeHitBox.SetActive(false);
        healthUI = GameObject.Find("HeartHealthVisual").GetComponent<HeartsHealthVisual>();

        animator = GetComponentInChildren<Animator>();

        bountyScript = GameObject.Find( "BountyTarget" ).GetComponent<BountyTargetScript>();
    }

    private void Update()
    {
        if ( health <= 0 )
        {
            return;
        }

        isCrouching = Input.GetButton("Crouch");
        if (isCrouching)
            animator.SetBool("isCrouching", true);
        else
            animator.SetBool("isCrouching", false);

        if (!isStabbing && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Player_AS.clip = Whoosh;
            Player_AS.time = 0.9f;
            Player_AS.Play();
            Player_AS.SetScheduledEndTime(AudioSettings.dspTime + (1.4f - 0.9f));

            isStabbing = true;
            knifeHitBox.SetActive(true);
            animator.SetBool("isStabbing", true);
        }
        if (!isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            rgdbdy.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            isJumping = true;
        }

        knifeOrRockThrow();
        FireBallSpell();
    }

    void FixedUpdate()
    {
        if ( health <= 0 )
        {
            return;
        }

        movePlayer();

        if (isStabbing)
        {
            moveSword();
        }
    }

    void FireBallSpell()
    {
        knife_and_rock_start_pos = transform.position + Vector3.up * 2 + transform.forward * 1;
        if (Input.GetKeyDown(KeyCode.C))
        {
            Player_AS.clip = Fireball;
            Player_AS.time = 5.0f;
            Player_AS.Play();
            Player_AS.SetScheduledEndTime(AudioSettings.dspTime + (6.5f - 5.0f));

            GameObject fireball = Instantiate(projectile_fireball, knife_and_rock_start_pos, Quaternion.identity) as GameObject;
            fireball.transform.rotation = Quaternion.LookRotation(-transform.up);
            fireball.GetComponent<Rigidbody>().AddForce(transform.forward * 2000);
        }
    } 


    void knifeOrRockThrow()
    {

        knife_and_rock_start_pos = transform.position + Vector3.up * 2 + transform.forward * 1;
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (number_of_knives > 0)
            {
                GameObject knife = Instantiate(projectile_knife, knife_and_rock_start_pos, Quaternion.identity) as GameObject;
                knife.transform.rotation = Quaternion.LookRotation(transform.right);
                knife.GetComponent<Rigidbody>().AddForce(transform.forward * 3100);
                number_of_knives = number_of_knives - 1;
                GM.UpdateDaggerCountDecrease();

                Player_AS.clip = Whoosh;
                Player_AS.time = 12.0f;
                Player_AS.Play();
                Player_AS.SetScheduledEndTime(AudioSettings.dspTime + (12.5f - 12.0f));
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (number_of_rocks > 0)
            {
                GameObject rock = Instantiate(projectile_rock, knife_and_rock_start_pos, Quaternion.identity) as GameObject;
                rock.transform.rotation = Quaternion.LookRotation(transform.right);
                rock.GetComponent<Rigidbody>().AddForce(transform.forward * 2100);
                number_of_rocks = number_of_rocks - 1;
                GM.UpdateRockCountDecrease();

                Player_AS.clip = Whoosh;
                Player_AS.time = 12.0f;
                Player_AS.Play();
                Player_AS.SetScheduledEndTime(AudioSettings.dspTime + (12.5f - 12.0f));
            }
        }
    }


    void movePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        

        int vertSign = vertical >= 0 ? 1 : -1;

        Vector3 forward = cameraTrans.forward.normalized; //Forward based on camera's forward
        Vector3 right = cameraTrans.right.normalized;

        Vector3 moveDir = (forward * vertical + right * horizontal).normalized;
        Vector3 camDir = (forward * vertical * vertSign + right * horizontal * vertSign).normalized;
        if (moveDir != Vector3.zero)
        {
            moveDir.y = 0;
            Vector3 lookDir = new Vector3(forward.x, 0, forward.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), rotationSpd);
            Debug.Log(lookDir);
        }

        Vector2 inputMvnt = new Vector2(horizontal, vertical);
        float speed = isCrouching ? crouchSpd : playerSpeed;
        float targetSpd = speed * Mathf.Clamp(inputMvnt.magnitude, -0.1f, 1f);
        curSpeed = Mathf.SmoothDamp(curSpeed, targetSpd, ref speedSmoothVel, speedSmoothTime);
        transform.position += moveDir * Time.deltaTime * curSpeed;

        animator.SetFloat("velocity", curSpeed);
        animator.SetFloat("vertical", vertical);
        animator.SetFloat("horizontal", horizontal);
        Debug.Log(horizontal);
    }

    private void moveSword()
    {
        if(stabTimer < 5)
        {
            // knife.transform.Translate(Vector3.forward * stabSpeed);
            stabTimer++;
        }
        else if(stabTimer < 10)
        {
            // knife.transform.Translate(Vector3.forward * -stabSpeed);
            stabTimer++;
        }
        else
        {
            stabTimer = 0;
            isStabbing = false;
            knifeHitBox.SetActive(false);
            animator.SetBool("isStabbing", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("GuardSword"))
        {
            Debug.Log("Player hit!");
            Vector3 hitDirection = (transform.position - other.transform.root.transform.position).normalized + Vector3.up;

            rgdbdy.AddForce(hitDirection * guardHitForce, ForceMode.Impulse);
            health--;
            healthUI.Damage(4);
            if(health <= 0)
            {
                Debug.Log("Game Over!");
                gameOverScreen.SetActive( true );
                //Destroy(gameObject);
            }
        }
        else if ( other.gameObject.CompareTag( "Ladder" ) && bountyScript.target_killed )
        {
            gameObject.GetComponent<LevelComplete>().level_is_complete = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    public float getCurSpeed()
    {
        return curSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 3f, crouchSpd = 1.5f;

    private CharacterController controller;
    private float curSpeed = 0f, speedSmoothVel = 0f, speedSmoothTime = 0.1f, rotationSpd = 0.05f;
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

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTrans = Camera.main.transform;
        rgdbdy = GetComponent<Rigidbody>();
        stabTimer = 0;
        knifeHitBox.SetActive(false);
    }

    private void Update()
    {
        isCrouching = Input.GetButton("Crouch");

        if (!isStabbing && Input.GetKeyDown(KeyCode.Q))
        {
            isStabbing = true;
            knifeHitBox.SetActive(true);
        }
        if (!isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            rgdbdy.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            isJumping = true;
        }
    }

    void FixedUpdate()
    {
        movePlayer();

        if (isStabbing)
        {
            moveSword();
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
            Vector3 lookDir = new Vector3(camDir.x, 0, camDir.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), rotationSpd);
        }

        Vector2 inputMvnt = new Vector2(horizontal, vertical);
        float speed = isCrouching ? crouchSpd : playerSpeed;
        float targetSpd = speed * inputMvnt.magnitude;
        curSpeed = Mathf.SmoothDamp(curSpeed, targetSpd, ref speedSmoothVel, speedSmoothTime);
        transform.position += moveDir * Time.deltaTime * curSpeed;
    }

    private void moveSword()
    {
        if(stabTimer < 5)
        {
            knife.transform.Translate(Vector3.forward * stabSpeed);
            stabTimer++;
        }
        else if(stabTimer < 10)
        {
            knife.transform.Translate(Vector3.forward * -stabSpeed);
            stabTimer++;
        }
        else
        {
            stabTimer = 0;
            isStabbing = false;
            knifeHitBox.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("GuardSword"))
        {
            Debug.Log("Player hit!");
            Vector3 hitDirection = (transform.position - other.transform.root.transform.position).normalized + Vector3.up;

            rgdbdy.AddForce(hitDirection * guardHitForce, ForceMode.Impulse);
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

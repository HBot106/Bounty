using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 3f, crouchSpd = 1.5f;

    private CharacterController controller;
    private float curSpeed = 0f, speedSmoothVel = 0f, speedSmoothTime = 0.1f, rotationSpd = 0.1f;
    private Transform cameraTrans;
    private bool isCrouching = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTrans = Camera.main.transform;
    }

    private void Update()
    {
        isCrouching = Input.GetButton("Crouch");
    }

    void FixedUpdate()
    {
        movePlayer();
    }

    void movePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraTrans.forward.normalized; //Forward based on camera's forward
        Vector3 right = cameraTrans.right.normalized;

        Vector3 moveDir = (forward * vertical + right * horizontal).normalized;
        
        if (moveDir != Vector3.zero)
        {
            moveDir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), rotationSpd);
        }

        Vector2 inputMvnt = new Vector2(horizontal, vertical);
        float speed = isCrouching ? crouchSpd : playerSpeed;
        float targetSpd = speed * inputMvnt.magnitude;
        curSpeed = Mathf.SmoothDamp(curSpeed, targetSpd, ref speedSmoothVel, speedSmoothTime);
        transform.position += moveDir * Time.deltaTime * curSpeed;
    }
}

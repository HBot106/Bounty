using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float rotateSpd, smoothFactor = 0.5f;

    private PlayerMovement pm;
    private Vector3 curOffset;
    private bool crouch = false;

    // Start is called before the first frame update
    void Start()
    {
        pm = target.GetComponent<PlayerMovement>();
        curOffset = offset;
    }

    private void Update()
    {
        // Zooms in when crouching

        if (!crouch && pm.isCrouching)
        {
            curOffset += (transform.position - target.position).normalized * -3f ;
            crouch = true;
        }
        else if (crouch && !pm.isCrouching)
        {
            curOffset += (transform.position - target.position).normalized * 3f;
            crouch = false;
        }
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Quaternion camTurnAngleX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotateSpd * 0.5f, Vector3.up);
            Quaternion camTurnAngleY = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * rotateSpd, Vector3.left);
            curOffset = camTurnAngleX * camTurnAngleY * curOffset;
            curOffset.y = Mathf.Clamp(curOffset.y, 0, float.MaxValue); // So you can't look under the ground
        }

        Vector3 newPos = target.position + curOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
        transform.LookAt(target);
    }
}

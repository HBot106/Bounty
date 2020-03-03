using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    protected Transform _XForm_Camera;
    protected Transform _XForm_Parent;
    public Transform target;

    protected Vector3 _LocalRotation;
    protected float _CameraDistance = 5f;

    public float MouseSensitivity = 4f;
    public float ScrollSensitvity = 2f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;

    public bool CameraDisabled = false;

    private PlayerMovement pm;
    private Vector3 curOffset;
    private bool crouch = false;


    // Use this for initialization
    void Start()
    {
        this._XForm_Camera = this.transform;
        this._XForm_Parent = this.transform.parent;
        pm = target.GetComponent<PlayerMovement>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void adjustMouseSensitivity( float sensitivity )
    {
        MouseSensitivity = sensitivity;
    }

    void LateUpdate()
    {
        _XForm_Parent.position = target.position;

        //if (Input.GetMouseButton(0))
        //{
            //Rotation of the Camera based on Mouse Coordinates
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                _LocalRotation.y += Input.GetAxis("Mouse Y") * MouseSensitivity * -0.4f;

                //Clamp the y Rotation to horizon and not flipping over at the top
                if (_LocalRotation.y < -18f)
                    _LocalRotation.y = -18f;
                else if (_LocalRotation.y > 90f)
                    _LocalRotation.y = 90f;
            }
        //}

        // Zooms in when crouching
        if (!crouch && pm.isCrouching)
        {
            curOffset += (transform.position - target.position).normalized * -3f;
            crouch = true;
            this._CameraDistance = 6f;
        }
        else if (crouch && !pm.isCrouching)
        {
            curOffset += (transform.position - target.position).normalized * 3f;
            crouch = false;
            this._CameraDistance = 10f;
        }


        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

        if (this._XForm_Camera.localPosition.z != this._CameraDistance * -1f)
        {
            this._XForm_Camera.localPosition = new Vector3(_XForm_Camera.localPosition.x, 
                _XForm_Camera.localPosition.y, 
                Mathf.Lerp(this._XForm_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ScrollDampening));
        }
    }
}
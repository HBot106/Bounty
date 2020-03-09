using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePickups : MonoBehaviour
{
    public float rotation_speed = 10.0f;
    private Vector3 rotationVals = new Vector3( 0, 360, 0 );

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate( 20 * Time.deltaTime, 60 * Time.deltaTime, 20 * Time.deltaTime );
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardHeadFollowBody : MonoBehaviour
{
    public GameObject guardToFollow;
    public Vector3 eyeOffset = new Vector3(0f,3f,0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = guardToFollow.transform.position + eyeOffset;
        transform.rotation = guardToFollow.transform.rotation;
    }
}

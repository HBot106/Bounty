﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milestone2KeyDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Key got!");
        Destroy(gameObject);
    }
}

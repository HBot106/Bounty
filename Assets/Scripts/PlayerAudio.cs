using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private PlayerMovement pm;
    private Rigidbody rgbd_player;
    private AudioSource audioSource;

    private AudioClip walking, crouching;

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponentInParent<PlayerMovement>();
        rgbd_player = GetComponentInParent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        walking = Resources.Load("Audio/Walking") as AudioClip;
        crouching = Resources.Load("Audio/Crouch") as AudioClip;
    }

    void Update()
    {
        playWalkingSounds();
    }

    private void playWalkingSounds()
    {
        //If player is moving
        if (rgbd_player.velocity.magnitude > 0 + Mathf.Epsilon)
        {
            // If player is crouching
            if (audioSource.clip == walking && pm.isCrouching)
            {
                audioSource.clip = crouching;
                audioSource.Play();
            }
            // If player is walking
            else if (audioSource.clip == crouching && !pm.isCrouching || !audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.clip = walking;
                audioSource.Play();
            }
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private Rigidbody rgbd;

    // Start is called before the first frame update
    void Start()
    {
        rgbd = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.Stop();
        audioSource.volume = rgbd.velocity.magnitude;
        audioSource.Play();

        float radius = rgbd.velocity.magnitude * 5f;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in hitColliders) {
            if (collider.gameObject.CompareTag("Guard"))
            {
                collider.gameObject.GetComponentInParent<GuardBehavior>().setGuardActive();
                Debug.Log("Gaurd");
            }
        }
    }
}

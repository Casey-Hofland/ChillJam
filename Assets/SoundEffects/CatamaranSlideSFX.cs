using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatamaranSlideSFX : MonoBehaviour
{
    public AnimationCurve animationCurve;
    public float maxCatamaranVelocity;
    public Rigidbody rigidBody;
    public AudioSource audioSource;
    public AudioClip catamaranStopSFX;
    public AudioClip catamaranSlideSFX;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource.clip = catamaranSlideSFX;
    }

    void Start()
    {
        audioSource.Play();
        audioSource.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidBody)
        {
            audioSource.volume = Mathf.Clamp(rigidBody.velocity.magnitude / maxCatamaranVelocity, 0, 1);
        }
    }
}

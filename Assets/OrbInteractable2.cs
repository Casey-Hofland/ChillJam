using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.VFX;

public class OrbInteractable2 : MonoBehaviour
{
    public GameObject orb;

    private PlayableDirector director;
    private VisualEffect effect;
    private AudioSource audio;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
        effect = GetComponent<VisualEffect>();
        audio = GetComponent<AudioSource>();

        effect.Stop();

        director.stopped += (dir) => SetPlayerInput(true);
    }

    private void SetPlayerInput(bool enabled)
    {
        var player = GameObject.Find("PlayerArmature");

        player.GetComponent<ThirdPersonController>().enabled = enabled;
        player.GetComponentInChildren<Interact>().enabled = enabled;

        if (enabled)
        {
            player.GetComponent<PlayerInput>().ActivateInput();
        }
        else
        {
            player.GetComponent<PlayerInput>().DeactivateInput();
        }
    }

    public void Interact()
    {
        director.Play();

        SetPlayerInput(false);
        Destroy(GetComponent<CapsuleCollider>());

        audio.Play();
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    public float distance = 1f;
    public LayerMask layerMask;

    private void Start()
    {
        TogglePlayerAndBoat(true);
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance, layerMask, QueryTriggerInteraction.Collide))
            {
                if (hit.collider.CompareTag("BoatInteraction"))
                {
                    TogglePlayerAndBoat(false);
                }
            }
        }
    }

    public static void TogglePlayerAndBoat(bool enablePlayer)
    {
        var allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        Array.Find(allGameObjects, gameObject => gameObject.name == "PlayerFollowCamera").SetActive(enablePlayer);

        var player = Array.Find(allGameObjects, gameObject => gameObject.name == "PlayerArmature");
        player.SetActive(enablePlayer);
        if (enablePlayer)
        {
            player.GetComponent<PlayerInput>().ActivateInput();
            player.GetComponent<PlayerInput>().SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
        }
        else
        {
            player.GetComponent<PlayerInput>().DeactivateInput();
        }

        Array.Find(allGameObjects, gameObject => gameObject.name == "CatamaranFollowCamera").SetActive(!enablePlayer);

        var catamaran = GameObject.Find("Catamaran");

        if (!enablePlayer)
        {
            catamaran.GetComponent<PlayerInput>().ActivateInput();
            catamaran.GetComponent<PlayerInput>().SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
        }
        else
        {
            catamaran.GetComponent<PlayerInput>().DeactivateInput();
        }

        catamaran.GetComponent<BoatController>().enabled = !enablePlayer;
        catamaran.GetComponent<ThirdPersonCinemachineController>().enabled = !enablePlayer;

        // Cinemachine brain
        var brain = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
        if (enablePlayer)
        {
            brain.m_UpdateMethod = Cinemachine.CinemachineBrain.UpdateMethod.LateUpdate;
        }
        else
        {
            brain.m_UpdateMethod = Cinemachine.CinemachineBrain.UpdateMethod.FixedUpdate;
        }
    }
}

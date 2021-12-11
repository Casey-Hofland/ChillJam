using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    public float distance = 1f;
    public LayerMask layerMask;

    [SerializeField]
    [HideInInspector]
    public List<GameObject> orbs = new List<GameObject>();

    private int skyboxIndex = 0;

    public List<Material> skyBoxes;

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
                if (hit.collider.TryGetComponent(out OrbInteractable orbInteractable))
                {
                    var orb = orbInteractable.orb;
                    Destroy(orbInteractable.gameObject);

                    orbs.Add(orb);
                    orb.SetActive(false);

                    AdvanceSkybox();
                }
                else if (orbs.Count > 0 && hit.collider.CompareTag("OrbPlacer"))
                {
                    var placeTransform = hit.transform;
                    orbs[0].SetActive(true);
                    orbs[0].transform.SetPositionAndRotation(placeTransform.position, placeTransform.rotation);
                    orbs.RemoveAt(0);

                    Destroy(hit.collider);
                }
                else if (hit.collider.CompareTag("BoatInteraction"))
                {
                    TogglePlayerAndBoat(false);
                }
            }
        }
    }

    private void AdvanceSkybox()
    {
        RenderSettings.skybox = skyBoxes[skyboxIndex];
        skyboxIndex++;
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

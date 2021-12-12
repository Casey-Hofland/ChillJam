using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.VFX;

public class Interact : MonoBehaviour
{
    public float distance = 1f;
    public LayerMask layerMask;

    [SerializeField]
    [HideInInspector]
    public List<GameObject> orbs = new List<GameObject>();

    private int skyboxIndex = 0;

    public List<Material> skyBoxes;

    private int orbsPlaced = 0;

    private void Awake()
    {
        TogglePlayerAndBoat(true);
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (Physics.Raycast(transform.position, transform.parent.forward, out RaycastHit hit, distance, layerMask, QueryTriggerInteraction.Collide))
            {
                if (hit.collider.TryGetComponent(out OrbInteractable2 orbInteractable))
                {
                    orbInteractable.Interact();
                    orbs.Add(orbInteractable.orb);

                    AdvanceSkybox();
                }
                else if (orbs.Count > 0 && hit.collider.CompareTag("OrbPlacer"))
                {
                    var placeTransform = hit.transform;
                    orbs[0].SetActive(true);
                    orbs[0].transform.SetPositionAndRotation(placeTransform.position, placeTransform.rotation);

                    foreach (var effect in orbs[0].GetComponentsInChildren<VisualEffect>())
                    {
                        effect.Play();
                    }

                    orbs.RemoveAt(0);

                    Destroy(hit.collider);

                    orbsPlaced++;
                    if (orbsPlaced == 3)
                    {
                        // Ending
                        GameObject.Find("Ending Sequence").GetComponent<PlayableDirector>().Play();
                    }
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

        // Ainaosn
        Array.Find(allGameObjects, gameObject => gameObject.name == "PlayerArmature_Boat").SetActive(!enablePlayer);

    }
}

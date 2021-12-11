using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapOpener : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.mKey.wasReleasedThisFrame)
        {
            var allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            var map = Array.Find(allGameObjects, gameObject => gameObject.name == "Map");
            map.SetActive(!map.activeSelf);
        }
    }
}

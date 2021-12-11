using UnityEngine;

public class FogFader : MonoBehaviour
{
    public Transform mainCamera;
    public Material material;
    public Material start;
    public Material end;

    [Header("Settings")]
    public float minDistance;
    public float maxDistance;

    private void Update()
    {
        var distance = (mainCamera.transform.position - transform.position).magnitude;

        var materialInterpolant = Mathf.InverseLerp(minDistance, maxDistance, distance);

        material.Lerp(start, end, materialInterpolant);
    }
}

using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class SnowStagon : MonoBehaviour
{
    private Renderer _renderer;
    public Renderer renderer => _renderer ? _renderer : (_renderer = GetComponent<Renderer>());

    public Camera snowStagonCamera;
    public Material materialTemplate;
    public RenderTexture renderTextureTemplate;

    private static readonly int _TrailTexture = Shader.PropertyToID(nameof(_TrailTexture));

    private Material _trailMaterial;
    private RenderTexture _trailTexture;

    private void OnEnable()
    {
        _trailMaterial = new Material(materialTemplate);
        _trailTexture = new RenderTexture(renderTextureTemplate);

        _trailMaterial.SetTexture(_TrailTexture, _trailTexture);
        renderer.sharedMaterial = _trailMaterial;

        snowStagonCamera.enabled = false;
        snowStagonCamera.targetTexture = _trailTexture;
    }

    private void OnDisable()
    {
        Destroy(_trailMaterial);
        Destroy(_trailTexture);
    }

    private void OnBecameVisible()
    {
        snowStagonCamera.enabled = true;
    }

    private void OnBecameInvisible()
    {
        snowStagonCamera.enabled = false;
    }
}

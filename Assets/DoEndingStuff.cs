using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DoEndingStuff : MonoBehaviour
{
    public VisualEffect blizzardEffect;
    public FogFader fogFader;

    private Material fogMaterial;
    private Material start;
    public Material end;

    private float fogStartDensity;
    public float fogEndDensity;

    private void Start()
    {
        blizzardEffect.Stop();

        fogMaterial = fogFader.material;
        start = fogFader.start;

        fogStartDensity = RenderSettings.fogDensity;

        Destroy(fogFader);

        StartCoroutine(Coroutine());
    }

    private IEnumerator Coroutine()
    {
        float duration = 25f;
        float time = duration;

        while (time > 0f)
        {
            time -= Time.deltaTime;
            var t = Mathf.Clamp01(time / duration);

            fogMaterial.Lerp(end, start, t);
            RenderSettings.fogDensity = Mathf.Lerp(fogEndDensity, fogStartDensity, t / 3f);

            yield return null;
        }
    }
}

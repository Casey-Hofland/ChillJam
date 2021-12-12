using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.VFX;

public class OrbInteractable : MonoBehaviour
{
    public GameObject orb;
    public PlayableDirector director;
    public VisualEffect effect;

    private void Start()
    {
        effect.Stop();
    }
}

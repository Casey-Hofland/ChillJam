using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Video;

public class Opening : MonoBehaviour
{
    public Canvas uiCanvas;
    public AudioSource blizzardSFX;
    public Volume globalVolume;

    private VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        globalVolume.enabled = false;
        uiCanvas.enabled = false;
        blizzardSFX.volume = 0f;

        OrbInteractable2.SetPlayerInput(false);

        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        Destroy(videoPlayer);

        uiCanvas.enabled = true;
        globalVolume.enabled = true;
        blizzardSFX.volume = 0f;
        blizzardSFX.Play();

        OrbInteractable2.SetPlayerInput(true);

        var blackScreen = uiCanvas.transform.GetChild(uiCanvas.transform.childCount - 1).GetComponent<Image>();
        var color = blackScreen.color;
        color.a = 1f;
        blackScreen.color = color;

        StartCoroutine(Coroutine());

        IEnumerator Coroutine()
        {
            float duration = 4f;
            float time = duration;

            while (time > 0f)
            {
                time -= Time.deltaTime;
                var t = Mathf.Lerp(1f, 0f, time / duration);

                var color = blackScreen.color;
                color.a = 1f - t;
                blackScreen.color = color;

                blizzardSFX.volume = t;

                yield return null;
            }
        }
    }
}

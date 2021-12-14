using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Video;

public class Opening : MonoBehaviour
{
    public Canvas uiCanvas;
    public Volume globalVolume;

    private VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        globalVolume.enabled = false;
        uiCanvas.enabled = true;

        var blackScreen = uiCanvas.transform.GetChild(uiCanvas.transform.childCount - 1).GetComponent<Image>();
        var color = blackScreen.color;
        color.a = 1f;
        blackScreen.color = color;

        OrbInteractable2.SetPlayerInput(false);

        videoPlayer.started += VideoPlayer_started;
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }

    private void VideoPlayer_started(VideoPlayer source)
    {
        uiCanvas.enabled = false;
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        Destroy(videoPlayer);

        uiCanvas.enabled = true;
        globalVolume.enabled = true;

        OrbInteractable2.SetPlayerInput(true);

        var blackScreen = uiCanvas.transform.GetChild(uiCanvas.transform.childCount - 1).GetComponent<Image>();

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

                yield return null;
            }
        }
    }
}

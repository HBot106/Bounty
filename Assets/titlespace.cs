using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Video;


public class titlespace : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    public CanvasGroup the_big_thing;

    // Start is called before the first frame update
    void Start()
    {
        the_big_thing.alpha = 0;
        StartCoroutine(PlayVideo());
        StartCoroutine(Title(the_big_thing));
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }
        rawImage.texture = videoPlayer.texture;
        audioSource.Play();
        videoPlayer.time = 8.0f;
        videoPlayer.Play();
        
    }
    IEnumerator Title(CanvasGroup Here)
    {

        yield return new WaitForSeconds(4);

        for (float t = 0.01f; t < 3.0f; t += Time.deltaTime)
        {
            t = Mathf.Min(t, 3.0f);
            Here.alpha = Mathf.Lerp(0, 1, Mathf.Min(1, t / 3.0f));
            yield return null;
        }

    }
}

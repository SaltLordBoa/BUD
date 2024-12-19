using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneControl : MonoBehaviour
{
    public VideoPlayer cutscene;
    public GameObject player;
    public GameObject portal;

    private void Awake()
    {
        cutscene = GetComponent<VideoPlayer>();
        cutscene.Play();
        cutscene.loopPointReached += CheckOver;
        player.SetActive(false);
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        player.SetActive(true);
        portal.SetActive(true);
    }
}

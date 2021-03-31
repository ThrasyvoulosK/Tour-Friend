﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VideoScript : MonoBehaviour
{
    UnityEngine.Video.VideoPlayer videoPlayer;
    GameMaster gameMaster;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer= gameObject.transform.Find("RawImage").transform.Find("Video Player").GetComponent<UnityEngine.Video.VideoPlayer>();
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();

    }

    // Update is called once per frame
    void Update()
    {
        if (gameMaster.player[gameMaster.current_screen - 1] == "Tourist")
        {
            gameObject.transform.Find("ButtonRepeat").GetComponent<Image>().sprite = gameMaster.imagehandler["ButtonRepeatTourist"];
            gameObject.transform.Find("Button").GetComponent<Image>().sprite = gameMaster.imagehandler["ButtonTourist"];
        }
        else
        {
            gameObject.transform.Find("ButtonRepeat").GetComponent<Image>().sprite = gameMaster.imagehandler["ButtonRepeatTourfriend"];
            gameObject.transform.Find("Button").GetComponent<Image>().sprite = gameMaster.imagehandler["ButtonTourfriend"];
        }

        if (videoPlayer.isPlaying == false)
        {
            if(gameMaster.player[gameMaster.current_screen-1]=="Tourist")
                gameObject.transform.Find("ButtonPlayPause").GetComponent<Image>().sprite = gameMaster.imagehandler["ButtonPlayTourist"];
            else
                gameObject.transform.Find("ButtonPlayPause").GetComponent<Image>().sprite = gameMaster.imagehandler["ButtonPlayTourfriend"];
        }
        else
        {
            if (gameMaster.player[gameMaster.current_screen-1] == "Tourist")
                gameObject.transform.Find("ButtonPlayPause").GetComponent<Image>().sprite = gameMaster.imagehandler["ButtonPauseTourist"];
            else
                gameObject.transform.Find("ButtonPlayPause").GetComponent<Image>().sprite = gameMaster.imagehandler["ButtonPauseTourfriend"];
        }

    }

    public void PlayOrPause()
    {
        if(videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            //gameObject.transform.Find("ButtonPlayPause").GetComponent<Image>().sprite = gameMaster.imagehandler["ButtonPlay"];
        }
        else
        {
            videoPlayer.Play();
            //gameObject.transform.Find("ButtonPlayPause").GetComponent<Image>().sprite = gameMaster.imagehandler["ButtonPause"];
        }
        
    }
}

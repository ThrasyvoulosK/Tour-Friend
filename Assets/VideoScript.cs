using System.Collections;
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
        if (videoPlayer.isPlaying==false)
            gameObject.transform.Find("ButtonPlayPause").GetComponent<Image>().sprite = gameMaster.imagehandler["ButtonPlay"];
        else
            gameObject.transform.Find("ButtonPlayPause").GetComponent<Image>().sprite = gameMaster.imagehandler["ButtonPause"];

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

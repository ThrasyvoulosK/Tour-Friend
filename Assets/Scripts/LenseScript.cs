using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class LenseScript : MonoBehaviour
{
    GameObject lense;
    bool ison = false;
    bool changed = false;
    public Vector3 orig_pos;

    GameMaster theGameMaster;
    string backGroundName;
    // Start is called before the first frame update
    void Start()
    {
        theGameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();

        backGroundName = "VideoBackground"+theGameMaster.current_location+"TwoVideos";

        //Debug.Log("lense start");
        lense = gameObject.transform.Find("LenseButton").gameObject;

        gameObject.GetComponent<Canvas>().overrideSorting=true;
        orig_pos = gameObject.GetComponent<RectTransform>().position;

        if (gameObject.name.StartsWith("C VideoChoice"))
            magnify();
        else if (gameObject.name.StartsWith("Canvas SelectPhrases"))
            magnifyMap();
        else
            magnify();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (changed&&ison)
        {
            Debug.Log("lense changed");
            gameObject.transform.Find("LenseButton").GetComponentInChildren<Button>().onClick.RemoveAllListeners();

            if (gameObject.name.StartsWith("Canvas SelectPhrases"))
                shrinkMap();
            else
                shrink();

            changed = false;
            
        }
        else if(changed&&(ison==false)) //if(ison)
        {
            Debug.Log("not changed-ison");
            gameObject.transform.Find("LenseButton").GetComponentInChildren<Button>().onClick.RemoveAllListeners();

            if (gameObject.name.StartsWith("C VideoChoice"))
                magnify();
            else if (gameObject.name.StartsWith("Canvas SelectPhrases"))
                magnifyMap();
            else
                magnify();

            changed = false;
            
        }

    }

    public void magnify()
    {
        //Debug.Log("Magnify Code");
        gameObject.transform.Find("LenseButton").GetComponentInChildren<Button>().onClick.AddListener(delegate
        {
            gameObject.transform.Find("BackgroundImage").GetComponent<Image>().sprite = theGameMaster.imagehandler[backGroundName + "Zoom"];

            //gameObject.GetComponent<RectTransform>().localScale = new Vector3(3f, 3f, 0);
            gameObject.GetComponent<RectTransform>().localScale = new Vector3(3.5f, 3.5f, 0);

            //resize objects

            gameObject.transform.Find("RawImage").localScale = new Vector3(0.7f, 0.7f);
            gameObject.transform.Find("RawImage").localPosition += new Vector3(0, -10f);

            //gameObject.transform.Find("DescriptionText").localScale = new Vector3(0.7f, 0.7f);
            gameObject.transform.Find("DescriptionText").localScale = new Vector3(0.6f, 0.6f);
            //gameObject.transform.Find("DescriptionText").localPosition += new Vector3(0, 10f);
            gameObject.transform.Find("DescriptionText").localPosition += new Vector3(-1f, 8f);

            //gameObject.transform.Find("NumberImage").localScale = new Vector3(0.6f, 0.6f);
            gameObject.transform.Find("NumberImage").localScale = new Vector3(0.4f,0.4f);
            //gameObject.transform.Find("NumberImage").localPosition += new Vector3(0, 18f);
            gameObject.transform.Find("NumberImage").localPosition += new Vector3(20, 18f);
            //gameObject.transform.Find("Button").localScale = new Vector3(0.6f, 0.6f);
            gameObject.transform.Find("Button").localScale = new Vector3(0.4f, 0.4f);
            gameObject.transform.Find("Button").localPosition += new Vector3(0, 18f);
            //gameObject.transform.Find("ButtonPlayPause").localScale = new Vector3(0.6f, 0.6f);
            gameObject.transform.Find("ButtonPlayPause").localScale = new Vector3(0.4f, 0.4f);
            //gameObject.transform.Find("ButtonPlayPause").localPosition += new Vector3(0, 18f);
            gameObject.transform.Find("ButtonPlayPause").localPosition += new Vector3(-12.5f, 18f);
            //gameObject.transform.Find("ButtonRepeat").localScale = new Vector3(0.6f, 0.6f);
            gameObject.transform.Find("ButtonRepeat").localScale = new Vector3(0.4f, 0.4f);
            //gameObject.transform.Find("ButtonRepeat").localPosition += new Vector3(0, 18f);
            gameObject.transform.Find("ButtonRepeat").localPosition += new Vector3(12.5f, 18f);
            //gameObject.transform.Find("LenseButton").localScale = new Vector3(0.6f, 0.6f);
            gameObject.transform.Find("LenseButton").localScale = new Vector3(0.4f, 0.4f);
            //gameObject.transform.Find("LenseButton").localPosition += new Vector3(0, 18f);
            gameObject.transform.Find("LenseButton").localPosition += new Vector3(-20, 18f);

            //change sprite
            gameObject.transform.Find("LenseButton").GetComponent<Image>().sprite = theGameMaster.imagehandler["LenseMinus"];


            gameObject.GetComponent<RectTransform>().position = gameObject.GetComponent<RectTransform>().parent.position;
            gameObject.GetComponent<Canvas>().sortingOrder = 1;
            gameObject.transform.Find("RawImage").transform.Find("Video Player").GetComponent<UnityEngine.Video.VideoPlayer>().Play();
            ison = true; changed = true;
        });
    }

    public void shrink()
    {
        Debug.Log("Shrink Code");
        gameObject.transform.Find("LenseButton").GetComponentInChildren<Button>().onClick.AddListener(delegate
        {
            gameObject.transform.Find("BackgroundImage").GetComponent<Image>().sprite = theGameMaster.imagehandler[backGroundName];

            gameObject.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 0);

            //resize objects
            gameObject.transform.Find("RawImage").localScale = new Vector3(1f, 1f);
            gameObject.transform.Find("RawImage").localPosition += new Vector3(0, 10f);

            gameObject.transform.Find("DescriptionText").localScale = new Vector3(1f, 1f);
            //gameObject.transform.Find("DescriptionText").localPosition += new Vector3(0, -10f);
            gameObject.transform.Find("DescriptionText").localPosition += new Vector3(1, -8f);

            gameObject.transform.Find("NumberImage").localScale = new Vector3(1f, 1f);
            //gameObject.transform.Find("NumberImage").localPosition += new Vector3(0, -18f);
            gameObject.transform.Find("NumberImage").localPosition += new Vector3(-20, -18f);
            gameObject.transform.Find("Button").localScale = new Vector3(1f, 1f);
            gameObject.transform.Find("Button").localPosition += new Vector3(0, -18f);
            gameObject.transform.Find("ButtonPlayPause").localScale = new Vector3(1f, 1f);
            //gameObject.transform.Find("ButtonPlayPause").localPosition += new Vector3(0, -18f);
            gameObject.transform.Find("ButtonPlayPause").localPosition += new Vector3(12.5f, -18f);
            gameObject.transform.Find("ButtonRepeat").localScale = new Vector3(1f, 1f);
            //gameObject.transform.Find("ButtonRepeat").localPosition += new Vector3(0, -18f);
            gameObject.transform.Find("ButtonRepeat").localPosition += new Vector3(-12.5f, -18f);
            gameObject.transform.Find("LenseButton").localScale = new Vector3(1f, 1f);
            //gameObject.transform.Find("LenseButton").localPosition += new Vector3(0, -18f);
            gameObject.transform.Find("LenseButton").localPosition += new Vector3(20, -18f);

            //change sprite
            gameObject.transform.Find("LenseButton").GetComponent<Image>().sprite = theGameMaster.imagehandler["LensePlus"];

            gameObject.GetComponent<RectTransform>().position = orig_pos;
            gameObject.GetComponent<Canvas>().sortingOrder = 0;
            gameObject.transform.Find("RawImage").transform.Find("Video Player").GetComponent<UnityEngine.Video.VideoPlayer>().Stop();
            ison = false; changed = true;
        });

    }

    public void magnifyMap()
    {
        Debug.Log("MagnifyMap Code");
        gameObject.transform.Find("LenseButton").GetComponentInChildren<Button>().onClick.AddListener(delegate
        {
            gameObject.transform.Find("C Map").localScale = new Vector3(1.3f, 1.3f);
            gameObject.transform.Find("C Map").localPosition = new Vector3(0, 0);

            //resize objects
            gameObject.transform.Find("LenseButton").localPosition = new Vector3(150f, -100f);

            //change sprite
            gameObject.transform.Find("LenseButton").GetComponent<Image>().sprite = theGameMaster.imagehandler["LenseMinus"];

            ison = true; 
            changed = true;
        });
    }

    public void shrinkMap()
    {
        Debug.Log("ShrinkMap Code");
        gameObject.transform.Find("LenseButton").GetComponentInChildren<Button>().onClick.AddListener(delegate
        {
            gameObject.transform.Find("C Map").localScale = new Vector3(0.33f, 0.33f);
            gameObject.transform.Find("C Map").localPosition = new Vector3(-200, 45);

            //resize objects
            gameObject.transform.Find("LenseButton").localPosition = new Vector3(-200, 45);

            //change sprite
            gameObject.transform.Find("LenseButton").GetComponent<Image>().sprite = theGameMaster.imagehandler["ButtonLensePlus"];

            ison = false; 
            changed = true;
        });
    }
}

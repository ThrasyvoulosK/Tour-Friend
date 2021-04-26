using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

/*GameMaster script controls the main gameplay*/
public class GameMaster : MonoBehaviour
{
    //lists of separate gameobjects are used to collectively create a screen

    //a list of words and descriptions used in the game. Originally in English
    [TextArea]
    public List<string> words_en = new List<string>();
    [TextArea]
    public List<string> words_gr = new List<string>();

    //handle translations of text between languages in this dictionary
    public Dictionary<string, string> texthandler = new Dictionary<string, string>();

    //list of images used in game
    public List<Sprite> images = new List<Sprite>();
    //list of words accompanying images
    public List<string> images_name = new List<string>();

    public Dictionary<string, Sprite> imagehandler = new Dictionary<string, Sprite>();

    //list of videos
    public List<UnityEngine.Video.VideoClip> videos_en = new List<UnityEngine.Video.VideoClip>();
    public List<UnityEngine.Video.VideoClip> videos_gr = new List<UnityEngine.Video.VideoClip>();
    //list of corresponding words to videos
    public List<string> videos_en_names = new List<string>();

    public Dictionary<string, UnityEngine.Video.VideoClip> videohandler = new Dictionary<string, UnityEngine.Video.VideoClip>();
    public Dictionary<string, UnityEngine.Video.VideoClip> videohandler_gr = new Dictionary<string, UnityEngine.Video.VideoClip>();

    //Dictionary Creation

    //list 
    //dictionary of the two above

    public string language_current = "English";
    public string language_currentSign = "English";

    //keep track of score
    public int total_points = 0;
    //this value keeps the amount of points to be either added or subtracted to the score
    int point_base = 3;

    //list of sequential screens
    public List<GameObject> sgo = new List<GameObject>();
    //keep account of the screen number we are currently in
    public int current_screen = 0;

    //llist of prefabs
    public List<GameObject> screenprefabs = new List<GameObject>();


    public GameObject firstscreen;// = new GameObject();

    ScreenObjectScript screenObject=new ScreenObjectScript();//

    //keep each screen's data here
    public List<Screen_SO> screen_SOs = new List<Screen_SO>();
    //testing route select with this list
    public List<Screen_SO> screen_SOsAlt = new List<Screen_SO>();
    public Screen_SO[] reserveScreenSOs;// = new Screen_SO[];

    //whos playing on each screen
    public string[] player;

    public bool correctchoice=false;

    //this is the place select screen position, where the game returns to after a succesful round, or a failed answer
    int locationscreenposition;
    //similarly, the phrase select screen is here
    int phrasescreenposition;

    //keep our locations in place with this list
    public List<string> placelist = new List<string>();
    //list of visited places
    public List<string> placelistvisited = new List<string>();
    //our current location
    public string current_location;

    public GameObject map;//= new GameObject();
    public bool mapinit = false;

    public bool checkdelegate=false;

    public string phrasevideo = null;

    public GameObject previousScreen = null;
    public GameObject backB;

    //keep here the last chosen item from custom questions (ie non correct/false ones)
    [TextArea]
    public string lastChoice;


    // Start is called before the first frame update
    void Start()
    {
        //language assignment test
        //language_current = "Greek";
        language_currentSign = "Greek";

        //initialise dictionaries
        LanguageDictionaryInitialise(language_current);
        ImageDictionaryInitialise();
        VideoDictionaryInitialise();

        backB = GameObject.Find("Canvas");
        backB.SetActive(false);
        //initialise first object
        if (Application.isEditor == true)
        {
            //debug versions

            /*current_screen = 10;
            createPhraseSelectscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Button1text);*/

            current_screen = 0;
            //createTwoImagesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Imagename2, screen_SOs[current_screen].Button1text);
            //createOneImagescreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename,  screen_SOs[current_screen].Button1text);
            GameObject.Find("Canvas Menu").transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(delegate 
            {
                backB.SetActive(true);
                //createTwoImagesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Imagename2, screen_SOs[current_screen].Button1text);
                createOneDescriptionScreen(screenprefabs[13], words_en[40], "no");
                Destroy(GameObject.Find("Canvas Menu")); 
            });
        }
        else
        {
            //original
            current_screen = 0;
            //createTwoImagesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Imagename2, screen_SOs[current_screen].Button1text);
            GameObject.Find("Canvas Menu").transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(delegate 
            {
                backB.SetActive(true);
                //createTwoImagesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Imagename2, screen_SOs[current_screen].Button1text);
                createOneDescriptionScreen(screenprefabs[13], words_en[40], "no");
                Destroy(GameObject.Find("Canvas Menu")); });
        }
        checkdelegate = true;

        locationscreenposition = 5;
        phrasescreenposition = 10;

        //map = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(checkdelegate==false)
        {
            GameObject.Find("Canvas Menu(Clone)").transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(delegate {
                //createTwoImagesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Imagename2, screen_SOs[current_screen].Button1text);
                backB.SetActive(true);
                createOneDescriptionScreen(screenprefabs[13], words_en[40], "no");
                Destroy(GameObject.Find("Canvas Menu(Clone)")); });

            checkdelegate = true;
        }

        if (GameObject.Find("Canvas SelectPhrases(Clone)") == true)
            phrasevideo = GameObject.Find("Canvas SelectPhrases(Clone)").GetComponent<SelectPhrasesScript>().chosenphrase;
        else if(GameObject.Find("Canvas SelectPlace(Clone)") == true)
        {
            current_screen--;
            GameObject.Find("Canvas SelectPlace(Clone)").transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
            ConstructorDecider(GameObject.Find("Canvas SelectPlace(Clone)").transform.Find("Button").GetComponent<Button>());
        }


    }

    void ImageDictionaryInitialise()
    {
        for (int i = 0; i < images.Count; i++)
            imagehandler.Add(images_name[i], images[i]);
    }
    void VideoDictionaryInitialise()
    {
        List<UnityEngine.Video.VideoClip> videolist = null;
        switch(language_currentSign)
        {
            case "Greek":
                videolist = videos_gr;
                //Debug.Log("Greek SignLanguage");
                break;
            default:
                videolist = videos_en;
                //Debug.Log("English SignLanguage");
                break;
        }
        for (int i = 0; i < videos_gr.Count; i++) 
        {
            //Debug.Log($"Assigning video {videos_en_names[i]}");
            videohandler.Add(videos_en_names[i], videolist[i]);
        }
            
    }

    void LanguageDictionaryInitialise(string current_language)
    {
        if (current_language == "English")
            for (int i = 0; i < words_en.Count; i++)
                texthandler.Add(words_en[i], words_en[i]);
        else if (current_language == "Greek")
            for (int i = 0; i < words_en.Count; i++)
            {
                texthandler.Add(words_en[i], words_gr[i]);
            }
    }

    //assign a string based on the initial one
    string AssignString(string initialstring)
    {
        if (language_current == "English")
            return initialstring;
        else if (language_current == "Greek")//test
            return texthandler[initialstring];
            //return words_gr[1];

        Debug.Log("there is no assignment for the word/phrase: " + initialstring +" for "+language_current+", yet!");
        return null;
    }

    //initialise video choice prefab
    GameObject InitialiseVideoChoice(GameObject videochoiceprefab, string vid, string desc, string card,string button)
    {
        GameObject vidchoice = videochoiceprefab;

        /*UnityEngine.Video.VideoClip vidclip;
        vidclip= vidchoice.transform.Find("RawImage").transform.Find("Video Player").GetComponent<UnityEngine.Video.VideoClip>();
         //vidchoice.transform.Find("RawImage").transform.Find("Video Player").GetComponent<UnityEngine.Video.VideoClip>() = videohandler[vid];
        //Debug.Log($"Assigning {vidclip.name} with {vid}");
        Debug.Log($"Assigning vidclip with {vid}");
        vidclip = videohandler[vid];*/

        UnityEngine.Video.VideoPlayer videoPlayer = videochoiceprefab.transform.Find("RawImage").transform.Find("Video Player").GetComponent<UnityEngine.Video.VideoPlayer>();
        Debug.Log($"Video Choice Asked For: {vid}");
        videoPlayer.clip = videohandler[vid];

        vidchoice.transform.Find("DescriptionText").GetComponentInChildren<TextMeshProUGUI>().text = desc;
        vidchoice.transform.Find("Button").GetComponentInChildren<TextMeshProUGUI>().text = button;
        Debug.Log("video choice text: " + desc);

        if(card.Length>1)
        {
            vidchoice.transform.Find("CardImage").GetComponent<Image>().sprite = imagehandler[card];
            vidchoice.transform.Find("CardImage").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        /*if (vidchoice.name.Contains("Magnified") == false)
            vidchoice.transform.Find("LenseButton").GetComponentInChildren<Button>().onClick.AddListener(delegate { GameObject lense = Instantiate(screenprefabs[9]); InitialiseLense(lense, vid, desc, card, button); });
            //vidchoice.transform.Find("LenseButton").GetComponentInChildren<Button>().onClick.AddListener(delegate { InitialiseLense(lense, vid, desc, card, button); });
        else
        {
            Debug.Log("this constructor is called from a magnifying lense");
            //vidchoice.transform.Find("LenseButton").GetComponentInChildren<Button>().onClick.AddListener(delegate { Destroy(vidchoice); });
        }*/
        /*Vector3 orig_pos = vidchoice.GetComponent<RectTransform>().position;
        vidchoice.transform.Find("LenseButton").GetComponentInChildren<Button>().onClick.AddListener(delegate { vidchoice.GetComponent<RectTransform>().localScale=new Vector3(2.25f,2.25f,0); vidchoice.GetComponent<RectTransform>().position =vidchoice.GetComponent<RectTransform>().parent.position; });// new Vector3(0, 0, 0); */

        return vidchoice;
        //return null;
    }


    /*Screen Object Constructors*/
    //Just a description and a button
    public void createOneDescriptionScreen(GameObject prefab_go, string desc, string button)
    {
        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        //description
        newgameobject.transform.Find("Description").GetComponentInChildren<TextMeshProUGUI>().text = desc;

        newgameobject.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate {
            createTwoImagesscreen(screen_SOs[current_screen].prefab, screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Imagename2, screen_SOs[current_screen].Button1text);
            backB.SetActive(true);
            Destroy(newgameobject);
        });
    }
    //One image and one button screen. 
    //It's the most common of the app and serves as the basis of the others
    public void createOneImagescreen(GameObject prefab_go, string desc, string img, string button)
    {
        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        //find the given values
        //Image image = newgameobject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        Image image = newgameobject.transform.Find("Image").GetComponent<Image>();

        //add the indicated values

        //description
        //newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;
        newgameobject.transform.Find("Description").GetComponentInChildren<TextMeshProUGUI>().text = desc;

        /*Debug.Log("description text is: " + newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
        Debug.Log("image to load" + img);
        Debug.Log(words_en[0]);*/

        if(img.Length>=1)
            image.sprite = imagehandler[img];

        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //find next in session to construct a new screen
        //Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        Button button1 = newgameobject.transform.Find("Button").GetComponent<Button>();

        button1.GetComponentInChildren<TextMeshProUGUI>().text = button;

        BackButton();

        Debug.Log(current_screen);
        ConstructorDecider(button1);
    }

    //two images and one button
    public void createTwoImagesscreen(GameObject prefab_go, string desc, string img1,string img2, string button)
    {
        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        //find the given values
        //Image image1 = newgameobject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        Image image1 = newgameobject.transform.Find("Image1").GetComponent<Image>();
        //Image image2 = newgameobject.transform.GetChild(0).transform.GetChild(3).GetComponent<Image>();
        Image image2 = newgameobject.transform.Find("Image2").GetComponent<Image>();

        //add the indicated values

        //newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;
        //newgameobject.transform.Find("Description").GetComponentInChildren<Text>().text = desc;
        TextMeshProUGUI descript = newgameobject.transform.Find("Description").GetComponentInChildren<TextMeshProUGUI>();
        //TextMeshPro descript = newgameobject.transform.Find("Description").GetComponentInChildren<TMPro.TMP_Text>();

        /*descript.text = desc;*/
        descript.text = AssignString(desc);

        //descript.SetText(desc);
        /*Debug.Log("description text is: " + newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
        Debug.Log("image to load" + img);
        Debug.Log(words_en[0]);*/

        if ((img1.Length >= 1)&& (img2.Length >= 1))
        {
            //Debug.Log("Given Images: " + img1 + " + " + img2);
            image1.sprite = imagehandler[img1];
            image2.sprite = imagehandler[img2];
        }

        //if descriptions exist, add them
        if(prefab_go.name.Contains("Desc"))
        {
            newgameobject.transform.Find("Image1").GetComponentInChildren<TextMeshProUGUI>().text = screen_SOs[current_screen].description2;
            newgameobject.transform.Find("Image2").GetComponentInChildren<TextMeshProUGUI>().text = screen_SOs[current_screen].description3;
        }

            

        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;
        //

        //find next in session to construct a new screen
        //Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        Button button1 = newgameobject.transform.Find("Button").GetComponent<Button>();
        button1.GetComponentInChildren<TextMeshProUGUI>().text = AssignString(button);

        BackButton();

        //Debug.Log(current_screen);
        ConstructorDecider(button1);
    }
    //
    public void createMapscreen(GameObject prefab_go, string desc, string img, string button)
    {
        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        //find the given values
        //Image image = newgameobject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        //Image image = newgameobject.transform.Find("Image").GetComponent<Image>();

        //add the indicated values

        //description
        //newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;
        newgameobject.transform.Find("Description").GetComponentInChildren<TextMeshProUGUI>().text = desc;

        /*Debug.Log("description text is: " + newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
        Debug.Log("image to load" + img);
        Debug.Log(words_en[0]);*/

        //if (img.Length >= 1)
            //image.sprite = imagehandler[img];

        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //find next in session to construct a new screen
        //Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        Button button1 = newgameobject.transform.Find("Button").GetComponent<Button>();

        button1.GetComponentInChildren<TextMeshProUGUI>().text = button;

        BackButton();

        Debug.Log(current_screen);
        ConstructorDecider(button1);

    }
    public void createPlaceSelectscreen(GameObject prefab_go, string desc, string img, string button)
    {
        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        //find the given values
        Image image = newgameobject.transform.Find("ImageAirport").GetComponent<Image>();

        //add the indicated values

        newgameobject.transform.Find("Description").GetComponentInChildren<TextMeshProUGUI>().text = desc;
        //newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;

        //find images and change them to their proper ones, depending on whether they've been used properly

        /*image.sprite = imagehandler[img];*/

        //assign points here
        newgameobject.transform.Find("PointsText").GetComponent<TextMeshProUGUI>().text = "POINTS: <color=#ff0000ff>" + total_points.ToString() + "</color>";

        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //find next in session to construct a new screen
        //Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();

        Button button1 = newgameobject.transform.Find("Button").GetComponent<Button>();
        button1.GetComponentInChildren<TextMeshProUGUI>().text = button;

        BackButton();

        Debug.Log(current_screen);
        ConstructorDecider(button1);

    }
    //one video and one button
    public void createOneVideoscreen(GameObject prefab_go, string desc,string desc2, string vid, string button)
    {
        /*Debug.Log("Called OneVideo constructor at "+current_screen);
        Debug.Log("Called OneVideo constructor with prefab"+prefab_go.name);
        Debug.Log("Called OneVideo constructor with desc"+desc);*/

        //change video on prefab
        //UnityEngine.Video.VideoClip videoClip;// = prefab_go.transform.Find("RawImage").transform.Find("Video Player").GetComponent<UnityEngine.Video.VideoClip>();
        UnityEngine.Video.VideoPlayer videoPlayer = prefab_go.transform.Find("RawImage").transform.Find("Video Player").GetComponent<UnityEngine.Video.VideoPlayer>();
        /*videoClip = videoPlayer.clip;
        videoClip = videohandler[vid];*/
        Debug.Log("OneVideo Clip to be used: " + vid+" , on screen "+current_screen);
        if (current_screen != 18 && current_screen != 27)
        {
            videoPlayer.clip = videohandler[vid];
            Debug.Log($"Current screen is {current_screen} and is different than 18 or 27");
        }
        else
        {
            Debug.Log(current_screen + "onevideo");
            foreach (Screen_SO sso in reserveScreenSOs)
            {
                //Debug.Log(gameObject.name);
                if (lastChoice == sso.description2)
                {
                    videoPlayer.clip = videohandler[sso.Video1];
                }
                else if (lastChoice == sso.description3)
                {
                    videoPlayer.clip = videohandler[sso.Video2];
                }
                Debug.Log(lastChoice + videoPlayer.clip);
            }
        }
        Debug.Log("video chosen: " + videoPlayer.clip.name);

        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        //find the given values

        //change background
        newgameobject.transform.Find("VideoBackground").GetComponent<Image>().sprite = imagehandler["VideoBackground"+current_location+"OneVideo"];

        //add the indicated values

        newgameobject.transform.Find("Description").GetComponent<TextMeshProUGUI>().text=desc;

        if (current_screen != 18&&current_screen!=27)
            newgameobject.transform.Find("VideoDescription").GetComponent<TextMeshProUGUI>().text = desc2;
        else
        {
            newgameobject.transform.Find("VideoDescription").GetComponent<TextMeshProUGUI>().text = lastChoice;
        }

        if(current_screen==31||current_screen==22)
            lastChoice = newgameobject.transform.Find("VideoDescription").GetComponent<TextMeshProUGUI>().text;//

        Debug.Log("video text2: " + newgameobject.transform.Find("VideoDescription").GetComponent<TextMeshProUGUI>().text);

        //find images and change them to their proper ones, depending on whether they've been used properly

        /*image.sprite = imagehandler[img];*/
        //assign location icon on top-right of the screen
        newgameobject.transform.Find("LocationIcon").GetComponent<Image>().sprite = imagehandler[current_location];

        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //find video
        //videoClip = videos_en[0];
        /*videoClip = videohandler[vid];
        Debug.Log("video chosen: " + videoClip.name);*/

        //find next in session to construct a new screen
        //Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        Button button1 = newgameobject.transform.Find("Button").GetComponent<Button>();
        button1.GetComponentInChildren<TextMeshProUGUI>().text = button;

        BackButton();

        //Debug.Log(current_screen);
        ConstructorDecider(button1);
    }
    //two videos, with their own buttons
    public void createTwoVideosscreen(GameObject prefab_go, string desc1,string desc2,string desc3, string img1,string img2,string video1,string video2, string button)
    {
        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        //find the given values
        //Image image = newgameobject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();

        //add the indicated values

        //newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;
        newgameobject.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = desc1;

        //find images and change them to their proper ones, depending on whether they've been used properly

        //assign location icon on top-right of the screen
        newgameobject.transform.Find("LocationIcon").GetComponent<Image>().sprite = imagehandler[current_location];

        //change backgrounds
        newgameobject.transform.Find("VideoChoice1").transform.Find("BackgroundImage").GetComponent<Image>().sprite = imagehandler["VideoBackground" + current_location + "TwoVideos"];
        newgameobject.transform.Find("VideoChoice2").transform.Find("BackgroundImage").GetComponent<Image>().sprite = imagehandler["VideoBackground" + current_location + "TwoVideos"];

        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //find next in session to construct a new screen
        //Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();

        Debug.Log(current_screen);

        BackButton();
      
        GameObject[] randomVideos = new GameObject[2];
        //randomVideos[0] = vidch1;
        //randomVideos[1] = vidch2;
        int number1 = Random.Range(0, 2);
        int number2;
        if (number1 == 0)
            number2 = 1;
        else
        {
            number2 = 0;
            number1 = 1;
        }
        Debug.Log($"Choosing video numbers {number1} and {number2}");

        randomVideos[number1] = newgameobject.transform.Find("VideoChoice"+(number1+1).ToString()).gameObject;
        randomVideos[number2] = newgameobject.transform.Find("VideoChoice"+(number2 + 1).ToString()).gameObject;

        //choose a random video to assign on the non-correct choice
        if(current_screen==locationscreenposition+3)
        {
            desc3 = "i want to go" + current_location;
            if (current_location == "Airport")
            {
                desc3 = screen_SOs[locationscreenposition + 1].description2;
                video2 = screen_SOs[locationscreenposition + 1].Video1;
            }
            else
            {
                foreach (Screen_SO sso in reserveScreenSOs)
                {
                    if (sso.name.Contains(screen_SOs[locationscreenposition + 1].name) && sso.name.Contains(current_location))
                    {
                        desc3 = sso.description2;
                        video2 = sso.Video1;
                    }
                }
            }
            //desc2= "i want to go" + current_location;
            do
            {
                desc2 = reserveScreenSOs[Random.Range(0, 8)].description2;
                video1 = reserveScreenSOs[Random.Range(0, 8)].Video1;
            }
            while (desc2 == desc3);
            //Debug.Log($"Two Videos (${desc1} & ${desc2}) Assigned Randomly");
            Debug.Log($"Description1 (${desc3}), DIFFERENT THAN Description2: ${desc2}");
        }
        else if(current_screen==17||current_screen==20)//'select the help you need
        {
            Debug.Log("select the help you need screen");
            if (current_location == "Airport")
            {
                desc2 = screen_SOs[17].description2;
                video1 = screen_SOs[17].Video1;
                desc3 = screen_SOs[17].description3;
                video2 = screen_SOs[17].Video2;
            }
            else
            {
                foreach (Screen_SO sso in reserveScreenSOs)
                {
                    if (sso.name.Contains(screen_SOs[17].name) && sso.name.Contains(current_location))
                    {
                        desc2 = sso.description2;
                        video1 = sso.Video1;
                        desc3 = sso.description3;
                        video2 = sso.Video2;
                    }
                }
            }
        }
        else if (current_screen == 24)//'what did the tourfriend say to you'
        {
            Debug.Log("select the help you need screen");
            if (current_location == "Airport")
            {
                desc2 = screen_SOs[22].description2;
                video1 = screen_SOs[22].Video1;
                desc3 = screen_SOs[22].description3;
                video2 = screen_SOs[22].Video2;
            }
            else
            {
                foreach (Screen_SO sso in reserveScreenSOs)
                {
                    if (sso.name.Contains(screen_SOs[22].name) && sso.name.Contains(current_location))
                    {
                        desc2 = sso.description2;
                        video1 = sso.Video1;
                        desc3 = sso.description3;
                        video2 = sso.Video2;
                    }
                }
            }
        }
        else if (current_screen==26)
        {
            Debug.Log("Two Videos 26");
            int index = 0;
            //airport choice 1
            if (lastChoice == screen_SOs[22].description2)
            {
                desc2 = screen_SOs[26].description2;
                video1 = screen_SOs[26].Video1;
                desc3 = screen_SOs[26].description3;
                video2 = screen_SOs[26].Video2;
            }
            //airport choice 2
            else if (lastChoice == screen_SOs[22].description3)
            {
                index = System.Array.IndexOf(reserveScreenSOs, screen_SOs[26]);
                desc2 = reserveScreenSOs[index + 1].description2;
                video1 = reserveScreenSOs[index + 1].Video1;
                desc3 = reserveScreenSOs[index + 1].description3;
                video2 = reserveScreenSOs[index + 1].Video2;
            }
            else
            {
                Debug.Log("Not Airport");
                foreach (Screen_SO sso in reserveScreenSOs)
                {
                    index = System.Array.IndexOf(reserveScreenSOs, sso);
                    if (lastChoice == sso.description2&&lastChoice.Length==sso.description2.Length)//load so26 +name
                    {
                        Debug.Log("lastChoice == sso.description2");
                        foreach (Screen_SO sso2 in reserveScreenSOs)
                        {
                            //Debug.Log(sso2.name.StartsWith(screen_SOs[26].name) + " " + sso2.name.Contains(current_location));
                            if (sso2.name.StartsWith(screen_SOs[26].name) && sso2.name.Contains(current_location)&&sso2.name.EndsWith("2")==false)
                            {
                                Debug.Log($"choosing sso.description2: {sso.description2} AND 3: {sso.description3}");
                                desc2 = sso2.description2;
                                video1 = sso2.Video1;
                                desc3 = sso2.description3;
                                video2 = sso2.Video2;
                                //break;
                            }
                        }
                    }
                    else if (lastChoice == sso.description3&& lastChoice.Length == sso.description3.Length)//load so26+name+1
                    {
                        Debug.Log("lastChoice == sso.description3");
                        foreach (Screen_SO sso2 in reserveScreenSOs)
                        {
                            Debug.Log(sso2.name.StartsWith(screen_SOs[26].name) + " " + sso2.name.Contains(current_location) + " " + sso2.name.Contains("2"));
                            if (sso2.name.StartsWith(screen_SOs[26].name) && sso2.name.Contains(current_location) && sso2.name.EndsWith("2"))
                            {
                                Debug.Log("choosing sso.description3");
                                Debug.Log($"choosing sso.description2: {sso.description2} AND 3: {sso.description3}");
                                desc2 = sso2.description2;
                                video1 = sso2.Video1;
                                desc3 = sso2.description3;
                                video2 = sso2.Video2;
                                //break;
                            }
                        }
                    }
                }
            }
        }
        else if(current_screen==29)
        {
            Debug.Log("Two Videos 29");
            //int index = 0;
            foreach(Screen_SO sso in reserveScreenSOs)
            {
                if((lastChoice==sso.description2&& lastChoice.Length == sso.description2.Length) || (lastChoice == sso.description3&& lastChoice.Length == sso.description3.Length))
                {
                    desc2 = sso.description2;
                    video1 = sso.Video1;
                    desc3 = sso.description3;
                    video2 = sso.Video2;
                }
            }
        }
        else if ( current_screen == 33)
        {
            int index = 0;
            foreach (Screen_SO sso in reserveScreenSOs)
            {
                index = System.Array.IndexOf(reserveScreenSOs, sso);
                if ((lastChoice == sso.description2&& lastChoice.Length == sso.description2.Length) || (lastChoice == sso.description3 && lastChoice.Length == sso.description3.Length))
                {
                    //Debug.Log(index + 16);
                    desc2 = reserveScreenSOs[index].description2;
                    video1 = reserveScreenSOs[index].Video1;
                    img1 = reserveScreenSOs[index].Imagename;
                    desc3 = reserveScreenSOs[index].description3;
                    video2 = reserveScreenSOs[index].Video2;
                    img2 = reserveScreenSOs[index].Imagename2;
                }
            }
        }
        else
        {
            Debug.Log("Two Videos Current Screen is: " + current_screen);
        }

        Debug.Log($"DESC2: {desc2}, DESC3: {desc3}");
        Debug.Log($"VIDEO1: {video1}, VIDEO2: {video2}");
        if(video1.Length<2||video2.Length<2)
        {
            video1 = "PlaceholderRed";
            video2 = "Placeholder";
        }

        InitialiseVideoChoice(randomVideos[number1], video1, desc2, img1, "SELECT");//"False"
        InitialiseVideoChoice(randomVideos[number2], video2, desc3, img2, "SELECT");//"Correct"

        Button button1 = randomVideos[number1].transform.Find("Button").gameObject.GetComponent<Button>();
        Button button2 = randomVideos[number2].transform.Find("Button").gameObject.GetComponent<Button>();
        
        button1.onClick.AddListener(delegate { lastChoice = desc2; });
        button2.onClick.AddListener(delegate { lastChoice = desc3; });

        Debug.Log(lastChoice+current_screen+desc2+current_screen+desc3+current_screen);
        //handle 'choice' questions here, ie non-correct/false ones
        /*if(current_screen!=20|| current_screen != 24 || current_screen != 29 || current_screen != 33 )
        {
            Debug.Log("Buttons assigned normally");
            ConstructorDecider(button2);//adds 1 to current screen
            ButtonDecider(button1);//doesn't add
        }
        else*/ if (lastChoice == desc2)
        {
            Debug.Log($"Special Choice 1: last choice is {lastChoice}, and desc2 is {desc2}");
            ConstructorDecider(button1);//adds 1 to current screen
            ButtonDecider(button2);//doesn't add
        }
        else if(lastChoice==desc3)
        {
            Debug.Log($"Special Choice 2: last choice is {lastChoice}, and desc2 is {desc3}");
            ConstructorDecider(button2);//adds 1 to current screen
            ButtonDecider(button1);//doesn't add
        }
        else
        {
            Debug.Log("Current screen not 20 and not a choice question etc");
            Debug.Log("Buttons assigned normally");
            Debug.Log(lastChoice + desc2 + desc3);
            ConstructorDecider(button2);//adds 1 to current screen
            //button1.onClick.AddListener(delegate { lastChoice = desc2; });
            ButtonDecider(button1);//doesn't add
        }

        /*ConstructorDecider(button2);//adds 1 to current screen
        ButtonDecider(button1);//doesn't add*/

    }
    void ButtonDecider(Button button1)
    {
        if (screen_SOs[current_screen].prefab.name.EndsWith("Points"))
        {
            //Debug.Log("points screen follows this two-video constructor");
            //button1.onClick.AddListener(delegate { correctchoice = false; });
            //button2.onClick.AddListener(delegate { correctchoice = true; });
            button1.onClick.AddListener(delegate 
            { 
                createPointsscreen(screen_SOs[current_screen].prefab, screen_SOs[current_screen].description2, "Points_Ribbon_Lose", "CONTINUE");
                lastChoice = null;
                //current_screen = locationscreenposition;
            });
        }
        else if (screen_SOs[current_screen].prefab.name.EndsWith("Canvas OneIm TwoOptions"))
        {
            //Debug.Log("this two video constructor isn't followed by a points screen");
            button1.onClick.AddListener(delegate { createOneImageTwoChoicesscreen(screen_SOs[current_screen].prefab, screen_SOs[current_screen].description, images_name[0], screen_SOs[current_screen].Button1text, screen_SOs[current_screen].Button2text); });
        }
        else if (screen_SOs[current_screen].prefab.name.EndsWith("Canvas OneVideo"))
        {
            Debug.Log($"after calling two-video constructor, ON SCREEN {current_screen}, we need a OneVideo constructor");
            //17 or 26
            //lastChoice=GameObject.Find("Canvas TwoVideos(Clone)").transform.Find("")
            //button1.onClick.AddListener(delegate { 
            //createOneVideoscreen(screen_SOs[current_screen].prefab, screen_SOs[current_screen].description, screen_SOs[current_screen].description2, videos_en_names[0], "Continue");
            /*foreach(Screen_SO sso in reserveScreenSOs)
            {
                Debug.Log(gameObject.name);
                if (lastChoice == sso.description2)
                {
                    Debug.Log(lastChoice);
                    button1.onClick.AddListener(delegate
                    {
                        createOneVideoscreen(screen_SOs[current_screen].prefab, screen_SOs[current_screen].description, sso.description2, sso.Video1, "Continue");
                    });
                    break;
                }
                    else if (lastChoice == sso.description3)
                    {
                        Debug.Log(lastChoice);
                    button1.onClick.AddListener(delegate
                    {
                        createOneVideoscreen(screen_SOs[current_screen].prefab, screen_SOs[current_screen].description, sso.description3, sso.Video2, "Continue");
                    });
                        break;
                    }
                    else
                        Debug.Log(lastChoice + "NOT IN SSO");
                    Debug.Log(lastChoice + sso.description2 + sso.description3);
                }
            */
            button1.onClick.AddListener(delegate
            {
                createOneVideoscreen(screen_SOs[current_screen].prefab, screen_SOs[current_screen].description, lastChoice, videos_en_names[0], "Continue");
            });
                //lastChoice = screen_SOs[current_screen].description3;
            //});
        }
        else
            Debug.Log("after calling two-video constructor we need a different constructor");
    }

    public void createPointsscreen(GameObject prefab_go, string desc, string img, string button)
    {
        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        //find the given values
        //Image image = newgameobject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        Image image = newgameobject.transform.Find("Image").GetComponent<Image>();

        //add the indicated values

        //newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;
        newgameobject.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = desc;
        //Debug.Log("printing points on screen: " + total_points);
        newgameobject.transform.Find("PointsText").GetComponent<TextMeshProUGUI>().text = "POINTS: <color=#ff0000ff>"+total_points.ToString()+"</color>";

        /*Debug.Log("description text is: " + newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
        Debug.Log("image to load" + img);
        Debug.Log(words_en[0]);*/

        image.sprite = imagehandler[img];
        //assign location icon on top-right of the screen
        newgameobject.transform.Find("LocationIcon").GetComponent<Image>().sprite = imagehandler[current_location];

        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        BackButton();

        //find next in session to construct a new screen
        //Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        Button button1 = newgameobject.transform.Find("Button").GetComponent<Button>();

        button1.GetComponentInChildren<TextMeshProUGUI>().text = button;

        //Debug.Log(current_screen);

        //decide on next actions, if won or lost
        if(img=="Points_Ribbon_Win")
            ConstructorDecider(button1);
        else
        {
            current_screen = locationscreenposition-1;
            current_location = null;
            
            /* //remove last visited screen, so that we can revisit it again
            placelistvisited.Remove(placelistvisited[placelistvisited.Count - 1]);*/

            ConstructorDecider(button1);
        }
    }

    //
    public void createPhraseSelectscreen(GameObject prefab_go, string desc, string img, string button)
    {
        //Debug.Log("Create PhraseSelect Constructor");

        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        //find the given values
        /*Image image = newgameobject.transform.Find("Image").GetComponent<Image>();*/

        //add the indicated values

        //description
        newgameobject.transform.Find("Description").GetComponentInChildren<TextMeshProUGUI>().text = desc;

        /*if (img.Length >= 1)
            image.sprite = imagehandler[img];*/

        BackButton();

        //find next in session to construct a new screen
        Button button1 = newgameobject.transform.Find("Button").GetComponent<Button>();

        button1.GetComponentInChildren<TextMeshProUGUI>().text = button;

        Debug.Log(current_screen);
        ConstructorDecider(button1);
    }

    //
    public void createOneVideoTwoChoicesscreen(GameObject prefab_go, string desc,string desc2, string vid, string button,string button2)
    {
        Debug.Log("covtcs called on " + current_screen);
        //start by creating a one-video screen
        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        //change background
        newgameobject.transform.Find("VideoBackground").GetComponent<Image>().sprite = imagehandler["VideoBackground" + current_location + "OneVideoTwoOptions"];

        //find the given values
        UnityEngine.Video.VideoClip videoClip = newgameobject.transform.Find("RawImage").transform.Find("Video Player").GetComponent<UnityEngine.Video.VideoClip>();

        //add the indicated values
        newgameobject.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = desc;
        newgameobject.transform.Find("VideoDescription").GetComponent<TextMeshProUGUI>().text = desc2;

        //find video
        videoClip = videos_en[0];
        //Debug.Log("video chosen: " + videoClip.name);

        //
        correctchoice = true;//testing! 

        //assign location icon on top-right of the screen
        newgameobject.transform.Find("LocationIcon").GetComponent<Image>().sprite = imagehandler[current_location];

        BackButton();

        //find next in session to construct a new screen
        //(continue button should send us one screen back)
        Button button1 = newgameobject.transform.Find("Button").GetComponent<Button>();
        button1.GetComponentInChildren<TextMeshProUGUI>().text = button;
        //button1.onClick.AddListener(delegate { set_current_screen(current_screen-1); });
        button1.onClick.AddListener(delegate { OneScreenBack(); });
        button1.onClick.AddListener(delegate { createPhraseSelectscreen(screen_SOs[current_screen].prefab, screen_SOs[current_screen].description, images_name[0], screen_SOs[current_screen].Button1text); });

        //Debug.Log(current_screen);
        /*ConstructorDecider(button1);*/
        //add the second button
        //(finish button should take us to the next screen, as usual)
        Button buttontwo = newgameobject.transform.Find("Button2").GetComponent<Button>();
        buttontwo.GetComponentInChildren<TextMeshProUGUI>().text = button2;
        ConstructorDecider(buttontwo);
        //
        //buttontwo.onClick.AddListener(delegate { MinusTwo(current_screen); });
        //button1.onClick.AddListener(delegate { set_current_screen(current_screen+1); });

        /*current_screen--;*/
    }
    //
    public void createOneImageTwoChoicesscreen(GameObject prefab_go, string desc, string img, string button, string button2)
    {
        //Debug.Log("coitcs called on " + current_screen);
        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        //find the given values
        //Image image = newgameobject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        Image image = newgameobject.transform.Find("Image").GetComponent<Image>();

        //add the indicated values

        //description
        //newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;
        newgameobject.transform.Find("Description").GetComponentInChildren<TextMeshProUGUI>().text = desc;

        /*Debug.Log("description text is: " + newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
        Debug.Log("image to load" + img);
        Debug.Log(words_en[0]);*/

        if (img.Length >= 1)
            image.sprite = imagehandler[img];

        BackButton();

        //find next in session to construct a new screen
        Button button1 = newgameobject.transform.Find("Button").GetComponent<Button>();
        button1.GetComponentInChildren<TextMeshProUGUI>().text = button;
        //button1.onClick.AddListener(delegate { set_current_screen(current_screen - 1); });

        //Debug.Log(current_screen);
        ConstructorDecider(button1);
        //add the second button
        //(the second button here takes us two screens back)
        Button buttontwo = newgameobject.transform.Find("Button2").GetComponent<Button>();
        buttontwo.GetComponentInChildren<TextMeshProUGUI>().text = button2;
        //Debug.Log("buttontwo writes: " + button2);
        //this screen has different functioning when it appears right after phrase-select
        if (phrasescreenposition == (current_screen - 3))
        {
            //Debug.Log("two button image after phrase select");
            buttontwo.onClick.AddListener(delegate { TwoScreensBack(); });
            buttontwo.onClick.AddListener(delegate { createPhraseSelectscreen(screen_SOs[current_screen].prefab, screen_SOs[current_screen].description, images_name[0], screen_SOs[current_screen].Button1text); });
        }
        else
        {
            //Debug.Log("two button image not after phrase select");
            //'NO' button leads to false answer
            buttontwo.onClick.AddListener(delegate 
            { 
                createPointsscreen(screen_SOs[current_screen].prefab, screen_SOs[current_screen].description2, "Points_Ribbon_Lose", "CONTINUE");
                lastChoice = null;
                //current_screen = locationscreenposition;
            });
        }
        /*ConstructorDecider(buttontwo);*/
        //
        //buttontwo.onClick.AddListener(delegate { set_current_screen(current_screen+1); });


        /*current_screen--;*/
    }

    /*End of Constructors*/
    //
    /*public void createMagnifiedVideo()
    {

    }*/
    //decide on which constructor to call
    public void ConstructorDecider(Button button)
    {
        int usedScreen = current_screen;

        if (button.name != "BackButton")
        {
            current_screen++;
            usedScreen++;

            // disable progress bar while showing introductory screens
            if (usedScreen < locationscreenposition && current_screen > 0)
            {
                Debug.Log($"disabling progress bar on {usedScreen - 1} : {screen_SOs[usedScreen - 1].prefab.name}");
                //GameObject.Find(screen_SOs[usedScreen - 1].prefab.name + "(Clone)").transform.Find("ProgressBar").gameObject.SetActive(false);
                GameObject.Find(screen_SOs[usedScreen - 1].prefab.name + "(Clone)").transform.Find("ProgressBar").gameObject.SetActive(false);
            }
        }
        else
        {
            //Debug.Log("Constructor Decider For Back Button!");
            //button.onClick.AddListener(delegate { current_screen -= 2; });current_screen = current_screen-2;
            button.onClick.AddListener(delegate { current_screen = current_screen - 2; });
            usedScreen = current_screen - 1;

            GameObject gb = null;
            gb = GameObject.Find(screen_SOs[usedScreen+1].prefab.name + "(Clone)");
            //Debug.Log($"Looking for {screen_SOs[usedScreen+1].prefab.name}");
            //Debug.Log(gb.name + " Will Be Destroyed by BackButton");
            //find current screen object
            /*foreach (GameObject gob in screenprefabs)
            {
                if (GameObject.Find(gob.name + "(Clone)") != null)
                {
                    gb = GameObject.Find(gob.name + "(Clone)");*/
                    GameObject.Find("Canvas").transform.Find("BackButton").GetComponent<Button>().onClick.AddListener(delegate
                    { Destroy(gb); });
                /*}
            }*/
        }


        //TESTING: Code is temporary
        if (current_screen > screen_SOs.Count)
        {
            Debug.Log("Quitting because we don't have a next screen to show, yet!");
            Application.Quit();
        }
        else if (current_screen == screen_SOs.Count)
        {
            Debug.Log("End of round. Go back to place select");
            current_screen = locationscreenposition;
            usedScreen = current_screen;
            //testing route change
            //screen_SOs = screen_SOsAlt;
        }
        

            if (button == null)
            Debug.Log("null button");
        button.onClick.AddListener(DebugName);

        //Debug.Log("calling constructor on " + usedScreen);

        if (screen_SOs[usedScreen].prefab.name.StartsWith("Canvas OneImage"))
        {
            button.onClick.AddListener(delegate
            {
                createOneImagescreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, screen_SOs[usedScreen].Imagename, screen_SOs[usedScreen].Button1text);
                //GameObject.Find("Canvas OneImage(Clone)").transform.Find("Button").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().color = Color.red;// new Color32(157, 28, 32,255);
                //DO NOT TOUCH! GameObject.Find("Canvas OneImage(Clone)").transform.Find("Description").GetComponent<TextMeshProUGUI>().material.color = Color.white;//new Color(255, 255, 255);
                //GameObject.Find("Canvas OneImage(Clone)").transform.Find("Description").GetComponent<TextMeshProUGUI>().color = Color.red;
                //GameObject.Find("Canvas OneImage(Clone)").transform.Find("Description").GetComponent<TextMeshProUGUI>().color = Color.blue;
            });
        }
        else if (screen_SOs[usedScreen].prefab.name.StartsWith("Canvas OneIm"))//TwoOptions
        {
            button.onClick.AddListener(delegate { createOneImageTwoChoicesscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, screen_SOs[usedScreen].Imagename, screen_SOs[usedScreen].Button1text, screen_SOs[usedScreen].Button2text); });
        }
        else if (screen_SOs[usedScreen].prefab.name.StartsWith("Canvas TwoImages"))
        {
            if (usedScreen != 16)
                button.onClick.AddListener(delegate { createTwoImagesscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, screen_SOs[usedScreen].Imagename, screen_SOs[usedScreen].Imagename2, screen_SOs[usedScreen].Button1text); });
            else//Special Case: "You are a tourist in the: <location>"
            {
                Debug.Log("you are a tourist screen");
                button.onClick.RemoveAllListeners();
                if (current_location != "Airport")//Airport is our default selection
                {
                    Screen_SO screen_SO = null;
                    foreach (Screen_SO sso in reserveScreenSOs)
                    {
                        if (sso.name == screen_SOs[usedScreen].name + current_location)
                        {
                            screen_SO = sso;
                            break;
                        }

                    }
                    if (screen_SO != null)
                        button.onClick.AddListener(delegate { createTwoImagesscreen(screen_SOs[usedScreen].prefab, screen_SO.description, screen_SO.Imagename, screen_SO.Imagename2, screen_SO.Button1text); });

                    //Debug.Log($"We are going to {screen_SO.description2}");

                }
                else
                    button.onClick.AddListener(delegate { createTwoImagesscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, screen_SOs[usedScreen].Imagename, screen_SOs[usedScreen].Imagename2, screen_SOs[usedScreen].Button1text); });
            }
        }
        else if (screen_SOs[usedScreen].prefab.name.StartsWith("Canvas Map"))
        {
            button.onClick.AddListener(delegate { createMapscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, screen_SOs[usedScreen].Imagename, screen_SOs[usedScreen].Button1text); });
        }
        else if (screen_SOs[usedScreen].prefab.name.StartsWith("Canvas SelectPlace"))
        {
            button.onClick.AddListener(delegate { createPlaceSelectscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, images_name[0], screen_SOs[usedScreen].Button1text); });
        }
        else if (screen_SOs[usedScreen].prefab.name.StartsWith("Canvas OneVideo"))
        {
            //Debug.Log("CALLING ONE VIDEO");
            //Special Case: The Video after Place-Select
            //load a video that suits our selection
            if (usedScreen != locationscreenposition + 1&&usedScreen!=22 && usedScreen != 31)//&&usedScreen!=27
            {
                Debug.Log($"video screen typical, screen {usedScreen}");
                button.onClick.AddListener(delegate { createOneVideoscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, screen_SOs[usedScreen].description2, videos_en_names[1], screen_SOs[usedScreen].Button1text); });
            } 
            else if(usedScreen==22)
            {
                Debug.Log("video screen after two-video choice");
                button.onClick.RemoveAllListeners();
                Screen_SO screen_SO = null;
                string newDesc = null;
                foreach (Screen_SO sso in reserveScreenSOs)
                {
                    if (lastChoice==sso.description2)
                    {
                        //we should be in SOs 17
                        int loc=System.Array.IndexOf(reserveScreenSOs, sso);
                        button.onClick.AddListener(delegate { createOneVideoscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description,reserveScreenSOs[loc+8].description2, reserveScreenSOs[loc + 8].Video1, screen_SOs[usedScreen].Button1text); });
                        break;
                    }
                    else if(lastChoice == sso.description3)
                    {
                        int loc = System.Array.IndexOf(reserveScreenSOs, sso);
                        button.onClick.AddListener(delegate { createOneVideoscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, reserveScreenSOs[loc + 8].description3, reserveScreenSOs[loc + 8].Video2, screen_SOs[usedScreen].Button1text); });
                        break;
                    }
                }
                //Debug.Log($"We are going to {screen_SO.description2}");
            }
            /*else if(usedScreen==27)
            {
                //button.onClick.AddListener(delegate { createOneVideoscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, lastChoice, videos_en_names[1], screen_SOs[usedScreen].Button1text); });
                foreach (Screen_SO sso in reserveScreenSOs)
                {
                    if (lastChoice == sso.description2)
                    {
                        button.onClick.AddListener(delegate { createOneVideoscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, sso.description2, sso.Video1, screen_SOs[usedScreen].Button1text); });
                        break;
                    }
                    else if (lastChoice == sso.description3)
                    {
                        button.onClick.AddListener(delegate { createOneVideoscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, sso.description3, sso.Video2, screen_SOs[usedScreen].Button1text); });
                        break;
                    }
                }
            }*/
            else if(usedScreen==31)
            {
                foreach(Screen_SO sso in reserveScreenSOs)
                {
                    if (sso.name.StartsWith(screen_SOs[26].name)) 
                    {
                        int loc = System.Array.IndexOf(reserveScreenSOs, sso);
                        if (lastChoice == sso.description2)
                            button.onClick.AddListener(delegate { createOneVideoscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, reserveScreenSOs[loc + 16].description2, reserveScreenSOs[loc + 16].Video1, screen_SOs[usedScreen].Button1text); });
                        else if (lastChoice == sso.description3)
                            button.onClick.AddListener(delegate { createOneVideoscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, reserveScreenSOs[loc + 16].description3, reserveScreenSOs[loc + 16].Video1, screen_SOs[usedScreen].Button1text); });
                    }
                    
                }
            }
            else
            {
                //Debug.Log("Video screen after location select");
                button.onClick.RemoveAllListeners();
                if (current_location != "Airport")//Airport is our default selection
                {
                    Screen_SO screen_SO = null;
                    foreach (Screen_SO sso in reserveScreenSOs)
                    {
                        if (sso.name == screen_SOs[usedScreen].name + current_location)
                        {
                            screen_SO = sso;
                            //screen_SOs[usedScreen] = sso;
                            break;
                        }

                    }
                    if (screen_SO != null)
                        button.onClick.AddListener(delegate { createOneVideoscreen(screen_SOs[usedScreen].prefab, screen_SO.description, screen_SO.description2, screen_SO.Video1, screen_SO.Button1text);
                                                              });

                    //Debug.Log($"We are going to {screen_SO.description2}");

                }
                else
                    button.onClick.AddListener(delegate { createOneVideoscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, screen_SOs[usedScreen].description2, screen_SOs[usedScreen].Video1, screen_SOs[usedScreen].Button1text); });
                //button.onClick.AddListener(delegate { createOneVideoscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, screen_SOs[usedScreen].description2, videos_en_names[1], screen_SOs[usedScreen].Button1text); });
            }
        }
        else if (screen_SOs[usedScreen].prefab.name.StartsWith("Canvas OneVi"))//TwoOptions
        {
            //current_screen--;
            //Debug.Log("onevi");
            if (usedScreen - 1 == phrasescreenposition)
            {
                Debug.Log("video called after phrase-select");
                button.onClick.AddListener(delegate { createOneVideoTwoChoicesscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, phrasevideo, images_name[0], screen_SOs[usedScreen].Button1text, screen_SOs[usedScreen].Button2text); });
            }
            else
            {
                button.onClick.AddListener(delegate { createOneVideoTwoChoicesscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, screen_SOs[usedScreen].description2, images_name[0], screen_SOs[usedScreen].Button1text, screen_SOs[usedScreen].Button2text); });
            }


            //current_screen--;
        }
        else if (screen_SOs[usedScreen].prefab.name.StartsWith("Canvas TwoVideos"))
        {
            button.onClick.AddListener(delegate { createTwoVideosscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, screen_SOs[usedScreen].description2, screen_SOs[usedScreen].description3, screen_SOs[usedScreen].Imagename, screen_SOs[usedScreen].Imagename2, screen_SOs[usedScreen].Video1, screen_SOs[usedScreen].Video2, screen_SOs[usedScreen].Button1text); });
        }
        else if (screen_SOs[usedScreen].prefab.name.StartsWith("Canvas Points"))
        {
            //Debug.Log("Correct answer Creating");
            button.onClick.AddListener(delegate { total_points += point_base; });
            //Debug.Log("Points added: " + total_points);
            button.onClick.AddListener(delegate { createPointsscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, "Points_Ribbon_Win", "CONTINUE"); });
        }
        else if (screen_SOs[usedScreen].prefab.name.StartsWith("Canvas SelectPhrases"))
        {
            Debug.Log("Creating CanvasSelectPhrases");
            button.onClick.AddListener(delegate { createPhraseSelectscreen(screen_SOs[usedScreen].prefab, screen_SOs[usedScreen].description, images_name[0], screen_SOs[usedScreen].Button1text); });
        }
        else
            Debug.Log("we don't have a constructor for " + screen_SOs[usedScreen].prefab.name + " yet!");

        //decide colours
        button.onClick.AddListener(delegate { ColourChanger(); });

        //previousScreen = screenObject.gameObject;
        button.onClick.AddListener(screenObject.DestroyGameObject);

        

    }

    void BackButton()
    {
        //return;
        //code for back button
        //Debug.Log("Back Button Code!");

        GameObject.Find("Canvas").transform.Find("BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Canvas").transform.Find("BackButton").GetComponent<BackButtonScript>().currentscreen = null;

        int[] backAllowedOn ={3,6,11,18,2 };

        //if (current_screen >= 1)
        foreach (int b in backAllowedOn)
        {
            if (current_screen == b)
            {
                //find current screen object

                GameObject gob = screen_SOs[current_screen - 1].prefab;

                ConstructorDecider(GameObject.Find("Canvas").transform.Find("BackButton").GetComponent<Button>());

                GameObject.Find("Canvas").transform.Find("BackButton").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                return;
            }
            else
            {
                GameObject.Find("Canvas").transform.Find("BackButton").GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                //return;
            }
        }
    }

    //change colours of texts and buttons,depending on the player
    public void ColourChanger()
    {
        int usedScreen = current_screen - 1;

        GameObject currentGameObject = GameObject.Find(screen_SOs[usedScreen].prefab.name + "(Clone)");
        /*if(currentGameObject!=null)
        {
            Debug.Log(screen_SOs[usedScreen].prefab.name + "(Clone)");
        }
        else
        {
            Debug.Log(screen_SOs[usedScreen].prefab.name + "(Clone)");
        }*/

        //Define BackGround's image as well
        GameObject Background = null;
        if(usedScreen>locationscreenposition)
        {
            Background = currentGameObject.transform.Find("Background").gameObject;
            Background.GetComponent<Image>().sprite = imagehandler["Background" + current_location];
            if(currentGameObject.name.Contains("TwoVideos"))
                Background.GetComponent<Image>().sprite = imagehandler["Background" + current_location+"Double"];
        }

        GameObject Description = null;
        if (currentGameObject.transform.Find("Description").gameObject!=null)
        {
            Description = currentGameObject.transform.Find("Description").gameObject;
            if (screen_SOs[usedScreen].Player == "Tourist")
                Description.GetComponent<TextMeshProUGUI>().color = new Color32(157, 28, 32, 255);// Color.red;
            else if (screen_SOs[usedScreen].Player == "Tourfriend")
                Description.GetComponent<TextMeshProUGUI>().color = new Color32(14, 80, 101, 255); // Color.blue;
        }
        else
        {
            Debug.Log("No such gameobject");
        }

        Debug.Log(currentGameObject.name);
        GameObject Button = null;//= currentGameObject.transform.Find("Button").gameObject;
        if(currentGameObject.transform.Find("Button")!=null)
        {
            Button =  currentGameObject.transform.Find("Button").gameObject;
            Button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

            if (screen_SOs[usedScreen].Player == "Tourist")
            {
                Button.GetComponent<Image>().sprite = imagehandler["ButtonTourist"];
            }
            else if (screen_SOs[usedScreen].Player == "Tourfriend")
            {
                Button.GetComponent<Image>().sprite = imagehandler["ButtonTourfriend"];
            }
        }
        else
        {
            //Debug.Log("No button named Button");
        }
        

        return;

        //video buttons (handled on VideoScript)

    }

    //
    /*public void BackGroundDecider()
    {
        GameObject currentGameObject = GameObject.Find(screen_SOs[usedScreen].prefab.name + "(Clone)");
    }*/

    //math functions
    int MinusOne(int number)
    {
        return number--; 
    }
    int MinusTwo(int number)
    {
        return MinusOne(MinusOne(number));
    }
    int PlusTwo(int number)
    {
        int result = number + 2;
        Debug.Log("PlusTwo result " + result);
        return result;
    }
    //screen control functions
    void set_current_screen(int number)
    {
        current_screen = number;
    }
    void OneScreenBack()
    {
        current_screen = current_screen - 2;
    }
    void TwoScreensBack()
    {
        current_screen = current_screen - 3;
    }
    //miscellaneous functions
    public void DebugName()
    {
        Debug.Log(gameObject.name);
    }

}

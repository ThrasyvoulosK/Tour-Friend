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

    //keep track of score
    public int total_points = 0;
    //this value keeps the amount of points to be either added or subtracted to the score
    int point_base = 2;

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
    // Start is called before the first frame update
    void Start()
    {
        //language assignment test
        //language_current = "Greek";

        //initialise dictionaries
        LanguageDictionaryInitialise(language_current);
        ImageDictionaryInitialise();
        VideoDictionaryInitialise();

        //initialise first object
        if (Application.isEditor == true)
        {
            //debug versions

            /*current_screen = 10;
            createPhraseSelectscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Button1text);*/

            current_screen = 0;
            //createTwoImagesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Imagename2, screen_SOs[current_screen].Button1text);
            //createOneImagescreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename,  screen_SOs[current_screen].Button1text);
            GameObject.Find("Canvas Menu").transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(delegate { createTwoImagesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Imagename2, screen_SOs[current_screen].Button1text); });

            GameObject.Find("Canvas Menu").transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(delegate { Destroy(GameObject.Find("Canvas Menu")); });
        }
        else
        {
            //original
            current_screen = 0;
            //createTwoImagesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Imagename2, screen_SOs[current_screen].Button1text);
            GameObject.Find("Canvas Menu").transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(delegate { createTwoImagesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Imagename2, screen_SOs[current_screen].Button1text); });

            GameObject.Find("Canvas Menu").transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(delegate { Destroy(GameObject.Find("Canvas Menu")); });
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
            GameObject.Find("Canvas Menu(Clone)").transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(delegate { createTwoImagesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Imagename2, screen_SOs[current_screen].Button1text); });
            GameObject.Find("Canvas Menu(Clone)").transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(delegate { Destroy(GameObject.Find("Canvas Menu(Clone)")); });

            checkdelegate = true;
        }

        if (GameObject.Find("Canvas SelectPhrases(Clone)") == true)
            phrasevideo = GameObject.Find("Canvas SelectPhrases(Clone)").GetComponent<SelectPhrasesScript>().chosenphrase;


    }

    void ImageDictionaryInitialise()
    {
        for (int i = 0; i < images.Count; i++)
            imagehandler.Add(images_name[i], images[i]);
    }
    void VideoDictionaryInitialise()
    {
        for (int i = 0; i < videos_en.Count; i++)
            videohandler.Add(videos_en_names[i], videos_en[i]);
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

        UnityEngine.Video.VideoClip vidclip;
        vidclip= vidchoice.transform.Find("RawImage").transform.Find("Video Player").GetComponent<UnityEngine.Video.VideoClip>();
        //vidclip= vidchoice.transform.GetChild(2).transform.GetChild(0).GetComponent<UnityEngine.Video.VideoClip>(); 
        //vidclip= vidchoice.transform.GetChild(2).transform.GetChild(0).GetComponent<UnityEngine.Video.VideoClip>(); 
        vidclip = videohandler[vid];

        vidchoice.transform.Find("DescriptionText").GetComponentInChildren<TextMeshProUGUI>().text = desc;
        vidchoice.transform.Find("Button").GetComponentInChildren<Text>().text = button;
        Debug.Log("video choice text: " + desc);
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
    //
    /*GameObject InitialiseLense(GameObject lenseprefab, string vid, string desc, string card, string button)
    {
        //Debug.Log("calling lense initialiser");
        //GameObject vidchoice = Instantiate(lenseprefab);

        //call video-choice initialiser, since it has the same functionality more-or-less
        lenseprefab = InitialiseVideoChoice(lenseprefab,  vid,  desc,  card,  button);

        Debug.Log("lense text: " + desc);
        //add functionality to the lense button
        lenseprefab.transform.Find("LenseButton").GetComponentInChildren<Button>().onClick.AddListener(delegate { Destroy(lenseprefab); });
        //vidchoice.transform.Find("LenseButton").GetComponentInChildren<tmp>().onClick.AddListener(delegate { Destroy(gameObject); });
        //GetComponentInChildren<Button>(). = GameObject.Find("vid").transform.GetComponent<Button>();
        Button newbutton = lenseprefab.transform.Find("Button").GetComponent<Button>();
        Debug.Log(newbutton.GetComponentInChildren<Text>().text);
        Button oldbutton = GameObject.Find("Canvas TwoVideos(Clone)").transform.Find("VideoChoice1").transform.Find("Button").GetComponentInChildren<Button>();
        //newbutton = oldbutton;
        newbutton.onClick.AddListener(delegate { Debug.Log("lense green button pressed"); Destroy(lenseprefab);Destroy(GameObject.Find("Canvas TwoVideos(Clone)")); });
        ButtonDecider(newbutton);
        //copy
        Debug.Log(lenseprefab.name);
        //Destroy(gameObject);
        return lenseprefab;
    }*/

    /*Screen Object Constructors*/

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

        button1.GetComponentInChildren<Text>().text = button;

        BackButton();

        Debug.Log(current_screen);
        ConstructorDecider(button1);


        //BackButton();
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
            Debug.Log("Given Images: " + img1 + " + " + img2);
            image1.sprite = imagehandler[img1];
            image2.sprite = imagehandler[img2];
        }
            

        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;
        //

        //find next in session to construct a new screen
        //Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        Button button1 = newgameobject.transform.Find("Button").GetComponent<Button>();

        /*button1.GetComponentInChildren<Text>().text = button;*/
        button1.GetComponentInChildren<Text>().text = AssignString(button);

        BackButton();

        //Debug.Log(current_screen);
        ConstructorDecider(button1);


        //BackButton();
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

        button1.GetComponentInChildren<Text>().text = button;

        BackButton();

        Debug.Log(current_screen);
        ConstructorDecider(button1);

        //BackButton();

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
        newgameobject.transform.Find("PointsText").GetComponent<TextMeshProUGUI>().text = "Points: <color=#ff0000ff>" + total_points.ToString() + "</color>";

        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //find next in session to construct a new screen
        //Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();

        Button button1 = newgameobject.transform.Find("Button").GetComponent<Button>();
        button1.GetComponentInChildren<Text>().text = button;

        BackButton();

        Debug.Log(current_screen);
        ConstructorDecider(button1);

        //BackButton();

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
        videoPlayer.clip = videohandler[vid];
        //Debug.Log("video chosen: " + videoPlayer.clip.name);

        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        //Debug.Log("child of video: " + newgameobject.transform.GetChild(3).name);

        //find the given values
        //Image image = newgameobject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        /*UnityEngine.Video.VideoClip videoClip= newgameobject.transform.Find("RawImage").transform.Find("Video Player").GetComponent<UnityEngine.Video.VideoClip>();*/
        /*videoClip= newgameobject.transform.Find("RawImage").transform.Find("Video Player").GetComponent<UnityEngine.Video.VideoClip>();*/
        //Debug.Log("videoclip object should be " + newgameobject.transform.GetChild(0).transform.GetChild(5).transform.GetChild(0).name);
        /*UnityEngine.Video.VideoPlayer videoPlayer = newgameobject.transform.Find("RawImage").transform.Find("Video Player").GetComponent<UnityEngine.Video.VideoPlayer>();
        videoClip = videoPlayer.clip;
        videoClip = videohandler[vid];*/

        //add the indicated values

        //newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;
        newgameobject.transform.Find("Description").GetComponent<TextMeshProUGUI>().text=desc;
        newgameobject.transform.Find("VideoDescription").GetComponent<TextMeshProUGUI>().text=desc2;

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
        button1.GetComponentInChildren<Text>().text = button;

        BackButton();

        Debug.Log(current_screen);
        ConstructorDecider(button1);

        //BackButton();
    }
    //two videos, with their own buttons
    public void createTwoVideosscreen(GameObject prefab_go, string desc1,string desc2,string desc3, string img, string button)
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

        /*image.sprite = imagehandler[img];*/
        //assign location icon on top-right of the screen
        newgameobject.transform.Find("LocationIcon").GetComponent<Image>().sprite = imagehandler[current_location];

        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //find next in session to construct a new screen
        //Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();

        Debug.Log(current_screen);

        BackButton();

        //TEST: VideoChoice
        GameObject vidch1 = newgameobject.transform.Find("VideoChoice1").gameObject;
        GameObject vidch2 = newgameobject.transform.Find("VideoChoice2").gameObject;

        InitialiseVideoChoice(vidch1, videos_en_names[0], desc2, desc1, "False");//
        InitialiseVideoChoice(vidch2, videos_en_names[1], desc3, desc1, "Correct");//

        Button button1 = vidch1.transform.Find("Button").gameObject.GetComponent<Button>();
        Button button2 = vidch2.transform.Find("Button").gameObject.GetComponent<Button>();

        //button1.onClick.AddListener(delegate { correctchoice = false; });
        //ConstructorDecider(button1);
        ConstructorDecider(button2);

        //current_screen--;//TEST: Placeholder, because creating two buttons advances it more than it should*/
        ButtonDecider(button1);

        //BackButton();
    }
    void ButtonDecider(Button button1)
    {
        if (sgo[current_screen].name.EndsWith("Points"))
        {
            Debug.Log("points screen follows this two-video constructor");
            //button1.onClick.AddListener(delegate { correctchoice = false; });
            //button2.onClick.AddListener(delegate { correctchoice = true; });
            button1.onClick.AddListener(delegate { createPointsscreen(sgo[current_screen], screen_SOs[current_screen].description2, images_name[0], "Continue"); });
        }
        else if (sgo[current_screen].name.EndsWith("Canvas OneIm TwoOptions"))
        {
            Debug.Log("this two video constructor isn't followed by a points screen");
            button1.onClick.AddListener(delegate { createOneImageTwoChoicesscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], screen_SOs[current_screen].Button1text, screen_SOs[current_screen].Button2text); });
        }
        else if (sgo[current_screen].name.EndsWith("Canvas OneVideo"))
        {
            Debug.Log("after calling two-video constructor we need a OneVideo constructor");
            //button2.onClick.AddListener(delegate { createPointsscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], "Continue"); });
            button1.onClick.AddListener(delegate { createOneVideoscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].description2, videos_en_names[0], "Continue"); });
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
        Debug.Log("printing points on screen: " + total_points);
        newgameobject.transform.Find("PointsText").GetComponent<TextMeshProUGUI>().text = "Points: <color=#ff0000ff>"+total_points.ToString()+"</color>";

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

        button1.GetComponentInChildren<Text>().text = button;

        Debug.Log(current_screen);
        ConstructorDecider(button1);

        //BackButton();
    }

    //
    public void createPhraseSelectscreen(GameObject prefab_go, string desc, string img, string button)
    {
        Debug.Log("Create PhraseSelect Constructor");

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

        button1.GetComponentInChildren<Text>().text = button;

        Debug.Log(current_screen);
        ConstructorDecider(button1);

        //BackButton();
    }

    //
    public void createOneVideoTwoChoicesscreen(GameObject prefab_go, string desc,string desc2, string vid, string button,string button2)
    {
        Debug.Log("covtcs called on " + current_screen);
        //start by creating a one-video screen
        //createOneVideoscreen(prefab_go, desc, vid, button);
        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

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
        button1.GetComponentInChildren<Text>().text = button;
        //button1.onClick.AddListener(delegate { set_current_screen(current_screen-1); });
        button1.onClick.AddListener(delegate { OneScreenBack(); });
        button1.onClick.AddListener(delegate { createPhraseSelectscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], screen_SOs[current_screen].Button1text); });

        Debug.Log(current_screen);
        /*ConstructorDecider(button1);*/
        //add the second button
        //(finish button should take us to the next screen, as usual)
        Button buttontwo = newgameobject.transform.Find("Button2").GetComponent<Button>();
        buttontwo.GetComponentInChildren<Text>().text = button2;
        ConstructorDecider(buttontwo);
        //
        //buttontwo.onClick.AddListener(delegate { MinusTwo(current_screen); });
        //button1.onClick.AddListener(delegate { set_current_screen(current_screen+1); });

        /*current_screen--;*/

        //BackButton();
    }
    //
    public void createOneImageTwoChoicesscreen(GameObject prefab_go, string desc, string img, string button, string button2)
    {
        Debug.Log("coitcs called on " + current_screen);
        //start by creating a one-video screen
        //createOneVideoscreen(prefab_go, desc, vid, button);
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
        button1.GetComponentInChildren<Text>().text = button;
        //button1.onClick.AddListener(delegate { set_current_screen(current_screen - 1); });

        Debug.Log(current_screen);
        ConstructorDecider(button1);
        //add the second button
        //(the second button here takes us two screens back)
        Button buttontwo = newgameobject.transform.Find("Button2").GetComponent<Button>();
        buttontwo.GetComponentInChildren<Text>().text = button2;
        Debug.Log("buttontwo writes: " + button2);
        //this screen has different functioning when it appears right after phrase-select
        if (phrasescreenposition == (current_screen - 3))
        {
            Debug.Log("two button image after phrase select");
            buttontwo.onClick.AddListener(delegate { TwoScreensBack(); });
            buttontwo.onClick.AddListener(delegate { createPhraseSelectscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], screen_SOs[current_screen].Button1text); });
        }
        else
        {
            Debug.Log("two button image not after phrase select");
            //'NO' button leads to false answer
            buttontwo.onClick.AddListener(delegate { createPointsscreen(sgo[current_screen], screen_SOs[current_screen].description2, images_name[0], "Continue"); });
        }
        /*ConstructorDecider(buttontwo);*/
        //
        //buttontwo.onClick.AddListener(delegate { set_current_screen(current_screen+1); });


        /*current_screen--;*/
        //BackButton();
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
        }
        else
        {
            Debug.Log("Constructor Decider For Back Button!");
            //button.onClick.AddListener(delegate { current_screen -= 2; });current_screen = current_screen-2;
            button.onClick.AddListener(delegate { current_screen = current_screen - 2; });
            usedScreen = current_screen - 1;

            GameObject gb = null;
            gb = GameObject.Find(sgo[current_screen].name + "(Clone)");
            Debug.Log(gb.name + " Will Be Destroyed by BackButton");
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
        if (current_screen > sgo.Count)
        {
            Debug.Log("Quitting because we don't have a next screen to show, yet!");
            Application.Quit();
        }
        else if (current_screen == sgo.Count)
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

        Debug.Log("calling constructor on " + usedScreen);

        if (sgo[usedScreen].name.StartsWith("Canvas OneImage"))
        {          
            button.onClick.AddListener(delegate { createOneImagescreen(sgo[usedScreen], screen_SOs[usedScreen].description, screen_SOs[usedScreen].Imagename, screen_SOs[usedScreen].Button1text); });
        }
        else if(sgo[usedScreen].name.StartsWith("Canvas OneIm"))//TwoOptions
        {
            //current_screen--;
            button.onClick.AddListener(delegate { createOneImageTwoChoicesscreen(sgo[usedScreen], screen_SOs[usedScreen].description, screen_SOs[usedScreen].Imagename, screen_SOs[usedScreen].Button1text, screen_SOs[usedScreen].Button2text); });

            //current_screen--;//
        }
        else if (sgo[usedScreen].name.StartsWith("Canvas TwoImages"))
        {
            button.onClick.AddListener(delegate { createTwoImagesscreen(sgo[usedScreen], screen_SOs[usedScreen].description, screen_SOs[usedScreen].Imagename, screen_SOs[usedScreen].Imagename2, screen_SOs[usedScreen].Button1text); });
        }
        else if (sgo[usedScreen].name.StartsWith("Canvas Map"))
        {
            button.onClick.AddListener(delegate { createMapscreen(sgo[usedScreen], screen_SOs[usedScreen].description, screen_SOs[usedScreen].Imagename, screen_SOs[usedScreen].Button1text); });
        }
        else if (sgo[usedScreen].name.StartsWith("Canvas SelectPlace"))
        {
            button.onClick.AddListener(delegate { createPlaceSelectscreen(sgo[usedScreen], screen_SOs[usedScreen].description, images_name[0], screen_SOs[usedScreen].Button1text); });
        }
        else if (sgo[usedScreen].name.StartsWith("Canvas OneVideo"))
        {
            

            button.onClick.AddListener(delegate { createOneVideoscreen(sgo[usedScreen], screen_SOs[usedScreen].description,screen_SOs[usedScreen].description2, videos_en_names[1], screen_SOs[usedScreen].Button1text); });            
        }
        else if (sgo[usedScreen].name.StartsWith("Canvas OneVi"))//TwoOptions
        {
            //current_screen--;
            Debug.Log("onevi");
            if (usedScreen - 1 == phrasescreenposition)
            {
                Debug.Log("video called after phrase-select");
                button.onClick.AddListener(delegate { createOneVideoTwoChoicesscreen(sgo[usedScreen], screen_SOs[usedScreen].description, phrasevideo, images_name[0], screen_SOs[usedScreen].Button1text, screen_SOs[usedScreen].Button2text); });
            }
            else
            {
                button.onClick.AddListener(delegate { createOneVideoTwoChoicesscreen(sgo[usedScreen], screen_SOs[usedScreen].description, screen_SOs[usedScreen].description2, images_name[0], screen_SOs[usedScreen].Button1text, screen_SOs[usedScreen].Button2text); });
            }
            

            //current_screen--;
        }
        else if (sgo[usedScreen].name.StartsWith("Canvas TwoVideos"))
        {
            button.onClick.AddListener(delegate { createTwoVideosscreen(sgo[usedScreen], screen_SOs[usedScreen].description, screen_SOs[usedScreen].description2, screen_SOs[usedScreen].description3, images_name[0], screen_SOs[usedScreen].Button1text); });
        }
        else if (sgo[usedScreen].name.StartsWith("Canvas Points"))
        {
            Debug.Log("Correct answer Creating");
            button.onClick.AddListener(delegate { total_points = PlusTwo(total_points); });
            Debug.Log("Points added two: " + total_points);
            button.onClick.AddListener(delegate { createPointsscreen(sgo[usedScreen], screen_SOs[usedScreen].description, images_name[0], "Continue"); });
        }
        else if (sgo[usedScreen].name.StartsWith("Canvas SelectPhrases"))
        {
            Debug.Log("Creating CanvasSelectPhrases");
            button.onClick.AddListener(delegate { createPhraseSelectscreen(sgo[usedScreen], screen_SOs[usedScreen].description, images_name[0], screen_SOs[usedScreen].Button1text); });
        }
        else
            Debug.Log("we don't have a constructor for " + sgo[usedScreen].name + " yet!");

        //previousScreen = screenObject.gameObject;
        button.onClick.AddListener(screenObject.DestroyGameObject);

        

    }

    void BackButton()
    {
        //return;
        //code for back button
        Debug.Log("Back Button Code!");

        GameObject.Find("Canvas").transform.Find("BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Canvas").transform.Find("BackButton").GetComponent<BackButtonScript>().currentscreen = null;

        if (current_screen >= 1)
        {
            //GameObject.Find("Canvas").transform.Find("BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
            /*GameObject.Find("Canvas").transform.Find("BackButton").GetComponent<Button>().onClick.AddListener(delegate
            {
                //current_screen -= 2;
                //OneScreenBack();
                //TwoScreensBack();
            });*/
            //ConstructorDecider(GameObject.Find("Canvas").transform.Find("BackButton").GetComponent<Button>());

            GameObject gb = null;
            //find current screen object
            //foreach (GameObject gob in screenprefabs)
            //{
            GameObject gob = sgo[current_screen - 1];
                //gb
                //if (GameObject.Find(gob.name + "(Clone)") != null)
                //{
                    /*gb = GameObject.Find(gob.name + "(Clone)");
                    GameObject.Find("Canvas").transform.Find("BackButton").GetComponent<BackButtonScript>().currentscreen = gb;

                    //Debug.Log("Will Destroy From Back Button " + gb.name);
                    GameObject.Find("Canvas").transform.Find("BackButton").GetComponent<Button>().onClick.AddListener(delegate
                    {
                        Destroy(gb);

                        //OneScreenBack();
                        //current_screen = current_screen-2;
                        //ConstructorDecider(GameObject.Find("Canvas").transform.Find("BackButton").GetComponent<Button>());

                        //current_screen = 0;
                        //current_screen = 1;
                        //createTwoImagesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Imagename2, screen_SOs[current_screen].Button1text);
                        //createTwoImagesscreen(sgo[0], screen_SOs[0].description, screen_SOs[0].Imagename, screen_SOs[0].Button1text, screen_SOs[0].Button2text);
                        //createTwoImagesscreen(sgo[1], screen_SOs[1].description, screen_SOs[1].Imagename, screen_SOs[1].Button1text, screen_SOs[1].Button2text);
                        //createOneImagescreen(sgo[0], screen_SOs[0].description, screen_SOs[0].Imagename, screen_SOs[0].Button1text);
                        //createOneImagescreen(sgo[1], screen_SOs[1].description, screen_SOs[1].Imagename, screen_SOs[1].Button1text);
                        //createOneImagescreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Button1text);


                        //GameObject newgb=Instantiate(GameObject.Find("Canvas").transform.Find("BackButton").GetComponent<BackButtonScript>().currentscreen);
                        //newgb.name.Remove(newgb.name.Length-7);

                        //destroy old
                        //Destroy(gb);
                    });*/
                    ConstructorDecider(GameObject.Find("Canvas").transform.Find("BackButton").GetComponent<Button>());
                    //break;
                //}
                /*else
                {
                    Debug.Log("There is no " + gob.name + " GameObject on Screen!");
                    //Destroy(GameObject.Find(gob.name + "(Clone)"));
                    Destroy(GameObject.Find("Canvas SelectPlace(Clone)"));
                }*/
            //}

            if (GameObject.Find("Canvas").transform.Find("BackButton").GetComponent<BackButtonScript>().currentscreen == null)
                Debug.Log("Current Screen Could Not Be Saved");
            
        }
        else return;
    }

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

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
    public List<string> words_en = new List<string>();

    //list of images used in game
    public List<Sprite> images = new List<Sprite>();
    //list of words accompanying images
    public List<string> images_name = new List<string>();

    public Dictionary<string, Sprite> imagehandler = new Dictionary<string, Sprite>();

    //list of videos
    public List<UnityEngine.Video.VideoClip> videos_en = new List<UnityEngine.Video.VideoClip>();
    //list of corresponding words to videos
    public List<string> videos_en_names = new List<string>();

    public Dictionary<string, UnityEngine.Video.VideoClip> videohandler = new Dictionary<string, UnityEngine.Video.VideoClip>();

    //Dictionary Creation

    //list 
    //dictionary of the two above

    

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

    public bool correctchoice=false;

    //this is the place select screen position, where the game returns to after a succesful round, or a failed answer
    int locationscreenposition;
    //similarly, the phrase select screen is here
    int phrasescreenposition;

    //keep our locations in place with this list
    public List<string> placelist = new List<string>();
    //list of visited places
    public List<string> placelistvisited = new List<string>();

    public GameObject map;//= new GameObject();
    public bool mapinit = false;


    // Start is called before the first frame update
    void Start()
    {
        //initialise dictionaries
        ImageDictionaryInitialise();
        VideoDictionaryInitialise();

        //initialise first object
        if (Application.isEditor == true)
        {
            //debug versions

            /*current_screen = 10;
            createPhraseSelectscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Button1text);*/

            current_screen = 0;
            createTwoImagesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Imagename2, screen_SOs[current_screen].Button1text);
            //createOneImagescreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename,  screen_SOs[current_screen].Button1text);
        }
        else
        {
            //original
            current_screen = 0;
            createTwoImagesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Imagename2, screen_SOs[current_screen].Button1text);
        }

        locationscreenposition = 5;
        phrasescreenposition = 10;

        //map = null;
    }

    // Update is called once per frame
    void Update()
    {
        
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

        return vidchoice;
        //return null;
    }

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
        descript.text = desc;
        //descript.SetText(desc);
        /*Debug.Log("description text is: " + newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
        Debug.Log("image to load" + img);
        Debug.Log(words_en[0]);*/

        if ((img1.Length >= 1)&& (img2.Length >= 1))
        {
            image1.sprite = imagehandler[img1];
            image2.sprite = imagehandler[img2];
        }
            

        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;
        //

        //find next in session to construct a new screen
        //Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        Button button1 = newgameobject.transform.Find("Button").GetComponent<Button>();

        button1.GetComponentInChildren<Text>().text = button;

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

        button1.GetComponentInChildren<Text>().text = button;

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

        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //find next in session to construct a new screen
        //Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();

        Button button1 = newgameobject.transform.Find("Button").GetComponent<Button>();
        button1.GetComponentInChildren<Text>().text = button;

        Debug.Log(current_screen);
        ConstructorDecider(button1);

    }
    //one video and one button
    public void createOneVideoscreen(GameObject prefab_go, string desc, string vid, string button)
    {
        Debug.Log("Called OneVideo constructor at "+current_screen);
        Debug.Log("Called OneVideo constructor with prefab"+prefab_go.name);
        Debug.Log("Called OneVideo constructor with desc"+desc);
        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        Debug.Log("child of video: " + newgameobject.transform.GetChild(3).name);

        //find the given values
        //Image image = newgameobject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        //UnityEngine.Video.VideoClip videoClip= newgameobject.transform.GetChild(0).transform.GetChild(5).transform.GetChild(0).GetComponent<UnityEngine.Video.VideoClip>();
        UnityEngine.Video.VideoClip videoClip= newgameobject.transform.Find("RawImage").transform.Find("Video Player").GetComponent<UnityEngine.Video.VideoClip>();
        //Debug.Log("videoclip object should be " + newgameobject.transform.GetChild(0).transform.GetChild(5).transform.GetChild(0).name);

        //add the indicated values

        //newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;
        newgameobject.transform.Find("Description").GetComponent<TextMeshProUGUI>().text=desc;

        //find images and change them to their proper ones, depending on whether they've been used properly

        /*image.sprite = imagehandler[img];*/

        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //find video
        videoClip = videos_en[0];
        Debug.Log("video chosen: " + videoClip.name);

        //find next in session to construct a new screen
        //Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        Button button1 = newgameobject.transform.Find("Button").GetComponent<Button>();
        button1.GetComponentInChildren<Text>().text = button;

        Debug.Log(current_screen);
        ConstructorDecider(button1);

    }
    //two videos, with their own buttons
    public void createTwoVideosscreen(GameObject prefab_go, string desc, string img, string button)
    {
        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        //find the given values
        //Image image = newgameobject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();

        //add the indicated values

        //newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;
        newgameobject.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = desc;

        //find images and change them to their proper ones, depending on whether they've been used properly

        /*image.sprite = imagehandler[img];*/

        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //find next in session to construct a new screen
        //Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();

        Debug.Log(current_screen);

        //TEST: VideoChoice
        GameObject vidch1 = newgameobject.transform.Find("VideoChoice1").gameObject;
        GameObject vidch2 = newgameobject.transform.Find("VideoChoice2").gameObject;
        //GameObject vidch1=newgameobject.transform.Find("VideoChoice1").gameObject;
        InitialiseVideoChoice(vidch1, videos_en_names[0], desc, desc, "False");//
        vidch1.GetComponent<PointsScript>().iscorrect = true;
        InitialiseVideoChoice(vidch2, videos_en_names[1], desc, desc, "Correct");//
        vidch2.GetComponent<PointsScript>().iscorrect = false;

        Button button1 = vidch1.transform.Find("Button").gameObject.GetComponent<Button>();
        Button button2 = vidch2.transform.Find("Button").gameObject.GetComponent<Button>();
        ConstructorDecider(button1);
        /*ConstructorDecider(button2);

        current_screen--;//TEST: Placeholder, because creating two buttons advances it more than it should*/
        if(sgo[current_screen].name.EndsWith("Points"))
        {
            Debug.Log("points screen follows this two-video constructor");
            button2.onClick.AddListener(delegate { createPointsscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], "Continue"); });
        }
        else if(sgo[current_screen].name.EndsWith("Canvas OneIm TwoOptions"))
        {
            Debug.Log("this two video constructor isn't followed by a points screen");
            button2.onClick.AddListener(delegate { createOneImageTwoChoicesscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], screen_SOs[current_screen].Button1text, screen_SOs[current_screen].Button2text); });
        }
        else
        {
            Debug.Log("after calling two-video constructor we need a different constructor");
            button2.onClick.AddListener(delegate { createPointsscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], "Continue"); });
        }
        

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

        /*Debug.Log("description text is: " + newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
        Debug.Log("image to load" + img);
        Debug.Log(words_en[0]);*/

        image.sprite = imagehandler[img];

        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //find next in session to construct a new screen
        //Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        Button button1 = newgameobject.transform.Find("Button").GetComponent<Button>();

        button1.GetComponentInChildren<Text>().text = button;

        Debug.Log(current_screen);
        ConstructorDecider(button1);

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

        //find next in session to construct a new screen
        Button button1 = newgameobject.transform.Find("Button").GetComponent<Button>();

        button1.GetComponentInChildren<Text>().text = button;

        Debug.Log(current_screen);
        ConstructorDecider(button1);

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
    }
    //decide on which constructor to call
    public void ConstructorDecider(Button button)
    {
        current_screen++;

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
        }

            if (button == null)
            Debug.Log("null button");
        button.onClick.AddListener(DebugName);

        Debug.Log("calling constructor on " + current_screen );

        if (sgo[current_screen].name.StartsWith("Canvas OneImage"))
        {          
            button.onClick.AddListener(delegate { createOneImagescreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Button1text); });
        }
        else if(sgo[current_screen].name.StartsWith("Canvas OneIm"))//TwoOptions
        {
            //current_screen--;
            button.onClick.AddListener(delegate { createOneImageTwoChoicesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Button1text, screen_SOs[current_screen].Button2text); });

            //current_screen--;//
        }
        else if (sgo[current_screen].name.StartsWith("Canvas TwoImages"))
        {
            button.onClick.AddListener(delegate { createTwoImagesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Imagename2, screen_SOs[current_screen].Button1text); });
        }
        else if (sgo[current_screen].name.StartsWith("Canvas Map"))
        {
            button.onClick.AddListener(delegate { createMapscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Button1text); });
        }
        else if (sgo[current_screen].name.StartsWith("Canvas SelectPlace"))
        {
            button.onClick.AddListener(delegate { createPlaceSelectscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], screen_SOs[current_screen].Button1text); });
        }
        else if (sgo[current_screen].name.StartsWith("Canvas OneVideo"))
        {
            button.onClick.AddListener(delegate { createOneVideoscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], screen_SOs[current_screen].Button1text); });            
        }
        else if (sgo[current_screen].name.StartsWith("Canvas OneVi"))//TwoOptions
        {
            //current_screen--;
            Debug.Log("onevi");
            button.onClick.AddListener(delegate { createOneVideoTwoChoicesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].description2, images_name[0], screen_SOs[current_screen].Button1text, screen_SOs[current_screen].Button2text); });

            //current_screen--;
        }
        else if (sgo[current_screen].name.StartsWith("Canvas TwoVideos"))
        {
            button.onClick.AddListener(delegate { createTwoVideosscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], screen_SOs[current_screen].Button1text); });
        }
        else if (sgo[current_screen].name.StartsWith("Canvas Points"))
        {
            //GameObject vidch = GameObject.Find("NullScreenGameObject TwoVideos(Clone)");
            //GameObject vidch2= vidch.transform.Find("Canvas").transform.Find("VideoChoice1").gameObject;
            //if (sgo[current_screen].GetComponentInChildren<PointsScript>().iscorrect == true)
            //if (vidch2.GetComponent<PointsScript>().iscorrect==true)
            //if(gameObject.transform.GetComponent<PointsScript>().iscorrect)
            if(correctchoice==true)
            {
                Debug.Log("Correct answer");
                //button.onClick.AddListener(delegate { createPointsscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], screen_SOs[current_screen].Button1text); });
                button.onClick.AddListener(delegate { createPointsscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], "Continue"); });

                //current_screen = locationscreenposition;

            }
            else
            {
                Debug.Log("False answer");
                //button.onClick.AddListener(delegate { createPointsscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], screen_SOs[current_screen].Button1text); });
                button.onClick.AddListener(delegate { createPointsscreen(sgo[current_screen], screen_SOs[current_screen].description2, images_name[0], "Continue"); });
            }
        }
        else if (sgo[current_screen].name.StartsWith("Canvas SelectPhrases"))
        {
            Debug.Log("Creating CanvasSelectPhrases");
            button.onClick.AddListener(delegate { createPhraseSelectscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], screen_SOs[current_screen].Button1text); });
        }
        else
            Debug.Log("we don't have a constructor for " + sgo[current_screen].name + " yet!");

        button.onClick.AddListener(screenObject.DestroyGameObject);

    }

    int MinusOne(int number)
    {
        return number--; 
    }
    int MinusTwo(int number)
    {
        return MinusOne(MinusOne(number));
    }

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
    public void DebugName()
    {
        Debug.Log(gameObject.name);
    }

}

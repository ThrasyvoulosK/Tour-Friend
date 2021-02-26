using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

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
    // Start is called before the first frame update
    void Start()
    {
        //initialise dictionaries
        ImageDictionaryInitialise();
        VideoDictionaryInitialise();

        //initialise first object
        createTwoImagesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Imagename2, screen_SOs[current_screen].Button1text);
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
        //vidclip= vidchoice.transform.Find("RawImage").gameObject.GetComponentInChildren<UnityEngine.Video.VideoClip>(); 
        vidclip= vidchoice.transform.GetChild(2).transform.GetChild(0).GetComponent<UnityEngine.Video.VideoClip>(); 
        vidclip= videos_en[0];

        vidchoice.transform.Find("DescriptionText").GetComponentInChildren<Text>().text = desc;
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
        Image image = newgameobject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();

        //add the indicated values

        newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;

        Debug.Log("description text is: " + newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
        Debug.Log("image to load" + img);
        Debug.Log(words_en[0]);

        if(img.Length>=1)
            image.sprite = imagehandler[img];

        newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //find next in session to construct a new screen
        Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();

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
        Image image1 = newgameobject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        Image image2 = newgameobject.transform.GetChild(0).transform.GetChild(3).GetComponent<Image>();

        //add the indicated values

        newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;

        /*Debug.Log("description text is: " + newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
        Debug.Log("image to load" + img);
        Debug.Log(words_en[0]);*/

        if ((img1.Length >= 1)&& (img2.Length >= 1))
        {
            image1.sprite = imagehandler[img1];
            image2.sprite = imagehandler[img2];
        }
            

        newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //find next in session to construct a new screen
        Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();

        Debug.Log(current_screen);
        ConstructorDecider(button1);

    }

    public void createPlaceSelectscreen(GameObject prefab_go, string desc, string img, string button)
    {
        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        //find the given values
        Image image = newgameobject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();

        //add the indicated values

        newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;

        //find images and change them to their proper ones, depending on whether they've been used properly

        /*image.sprite = imagehandler[img];*/

        newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //find next in session to construct a new screen
        Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();

        Debug.Log(current_screen);
        ConstructorDecider(button1);

    }
    //one video and one button
    public void createOneVideoscreen(GameObject prefab_go, string desc, string vid, string button)
    {
        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        //find the given values
        //Image image = newgameobject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        UnityEngine.Video.VideoClip videoClip= newgameobject.transform.GetChild(0).transform.GetChild(5).transform.GetChild(0).GetComponent<UnityEngine.Video.VideoClip>();
        Debug.Log("videoclip object should be " + newgameobject.transform.GetChild(0).transform.GetChild(5).transform.GetChild(0).name);

        //add the indicated values

        newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;

        //find images and change them to their proper ones, depending on whether they've been used properly

        /*image.sprite = imagehandler[img];*/

        newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //find video
        videoClip = videos_en[0];
        Debug.Log("video chosen: " + videoClip.name);

        //find next in session to construct a new screen
        Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();

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
        Image image = newgameobject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();

        //add the indicated values

        newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;

        //find images and change them to their proper ones, depending on whether they've been used properly

        /*image.sprite = imagehandler[img];*/

        newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //find next in session to construct a new screen
        Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();

        Debug.Log(current_screen);

        //TEST: VideoChoice
        GameObject vidch1 = newgameobject.transform.Find("Canvas").transform.Find("VideoChoice1").gameObject;
        GameObject vidch2 = newgameobject.transform.Find("Canvas").transform.Find("VideoChoice2").gameObject;
        //GameObject vidch1=newgameobject.transform.Find("VideoChoice1").gameObject;
        InitialiseVideoChoice(vidch1, desc, desc, desc, desc);
        InitialiseVideoChoice(vidch2, desc, desc, desc, desc);

        ConstructorDecider(button1);

    }

    public void createPointsscreen(GameObject prefab_go, string desc, string img, string button)
    {
        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        //find the given values
        Image image = newgameobject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();

        //add the indicated values

        newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;

        Debug.Log("description text is: " + newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
        Debug.Log("image to load" + img);
        Debug.Log(words_en[0]);

        image.sprite = imagehandler[img];

        newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //find next in session to construct a new screen
        Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();

        Debug.Log(current_screen);
        ConstructorDecider(button1);

    }

    //decide on which constructor to call
    public void ConstructorDecider(Button button)
    {
        current_screen++;

        //
        if (current_screen >= sgo.Count)
        {
            Debug.Log("Quitting because we don't have a next screen to show, yet!");
            Application.Quit();
        }

        if (button == null)
            Debug.Log("null button");
        button.onClick.AddListener(DebugName);

        if (sgo[current_screen].name.StartsWith("NullScreenGameObject OneImage"))
        {          
            button.onClick.AddListener(delegate { createOneImagescreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Button1text); });
        }
        else if (sgo[current_screen].name.StartsWith("NullScreenGameObject TwoImages"))
        {
            button.onClick.AddListener(delegate { createTwoImagesscreen(sgo[current_screen], screen_SOs[current_screen].description, screen_SOs[current_screen].Imagename, screen_SOs[current_screen].Imagename2, screen_SOs[current_screen].Button1text); });
        }
        else if (sgo[current_screen].name.StartsWith("NullScreenGameObject SelectPlace"))
        {
            button.onClick.AddListener(delegate { createPlaceSelectscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], screen_SOs[current_screen].Button1text); });
        }
        else if (sgo[current_screen].name.StartsWith("NullScreenGameObject OneVideo"))
        {
            button.onClick.AddListener(delegate { createOneVideoscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], screen_SOs[current_screen].Button1text); });            
        }
        else if (sgo[current_screen].name.StartsWith("NullScreenGameObject TwoVideos"))
        {
            button.onClick.AddListener(delegate { createTwoVideosscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], screen_SOs[current_screen].Button1text); });
        }
        else if (sgo[current_screen].name.StartsWith("NullScreenGameObject Points"))
        {
            button.onClick.AddListener(delegate { createPointsscreen(sgo[current_screen], screen_SOs[current_screen].description, images_name[0], screen_SOs[current_screen].Button1text); });
        }
        else
            Debug.Log("we don't have a constructor for " + sgo[current_screen].name + " yet!");

        button.onClick.AddListener(screenObject.DestroyGameObject);

    }

    public void DebugName()
    {
        Debug.Log(gameObject.name);
    }

}

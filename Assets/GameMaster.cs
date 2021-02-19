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
    //list of corresponding words to videos

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

    ScreenObjectScript screenObject = new ScreenObjectScript();
    // Start is called before the first frame update
    void Start()
    {
        //initialise dictionaries
        ImageDictionaryInitialise();

        //initialise first object
        //Instantiate(firstscreen);
        //screenObject.createOneImagescreen(screenprefabs[0], words_en[1], images_name[0], words_en[0]);
        //createOneImagescreen(screenprefabs[0], words_en[1], images_name[0], words_en[0]);
        createOneImagescreen(sgo[current_screen], words_en[1], images_name[0], words_en[0]);
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

    public void createOneImagescreen(GameObject prefab_go, string desc, string img, string button)
    {
        //instantiate its prefab version
        GameObject newgameobject;
        newgameobject = Instantiate(prefab_go);

        //find the given values
        Image image = newgameobject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();

        //add the indicated values
        //
        //newgameobject.transform.GetChild(2).GetComponent<Text>().text = desc;
        newgameobject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = desc;
        Debug.Log("image to load" + img);
        Debug.Log(words_en[0]);
        //newgameobject.GetComponentInChildren<Image>().sprite=gameMaster.imagehandler[img];
        image.sprite = imagehandler[img];
        //newgameobject.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = gameMaster.imagehandler[img];
        newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = button;

        //newgameobject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Button>().onClick = Instantiate(newgameobject);

        //find next in session to construct a new screen
        //GetComponentInChildren<Button>().onClick.AddListener();
        Button button1 = newgameobject.transform.GetChild(0).GetChild(2).GetComponent<Button>();

        Debug.Log(current_screen);
        ConstructorDecider(button1);       
        
    }

    //decide on which constructor to call
    public void ConstructorDecider(Button button)
    {
        current_screen++;
        /*Button button = gObject.GetComponentInChildren<Button>();
        button = gObject.GetComponent<Button>();*/
        if (sgo[current_screen].name.StartsWith("NullScreenGameObject OneImage"))
        {
            //gObject.GetComponentInChildren<Button>().onClick.AddListener(delegate { createOneImagescreen(sgo[current_screen], words_en[1], images_name[0], words_en[0]); });
            if (button == null)
                Debug.Log("null button");
            button.onClick.AddListener(DebugName);
            //button.onClick.AddListener(delegate { createOneImagescreen(sgo[current_screen], words_en[1], images_name[0], words_en[0]); });
            button.onClick.AddListener(delegate { createOneImagescreen(sgo[current_screen], words_en[1], images_name[0], current_screen.ToString()); });
            button.onClick.AddListener(screenObject.DestroyGameObject);
        }

        
    }

    public void DebugName()
    {
        Debug.Log(gameObject.name);
    }

}

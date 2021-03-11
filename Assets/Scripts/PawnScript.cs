using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PawnScript : MonoBehaviour
{
    //raycast code from placeselectscript

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    string current_choice;
    GameMaster gameMaster;

    public GameObject button;
    public Button bu;

    public GameObject placeimage;

    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();

        //
        current_choice = null;
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();

        //find the screen's button and disable it 
        /*button = gameObject.transform.Find("Button").gameObject;
        bu = button.GetComponent<Button>();
        bu.interactable = false;*/
        //button.SetActive(false);

        placeimage = null;

        //Test: Work in Progress
        //find the info point and disable it        
        for(int i=1;i<=9;i++)
        {
            if(gameObject.transform.Find("Image"+i).GetComponent<Image>().sprite==gameMaster.imagehandler["InfoPoint"])
            {
                gameObject.transform.Find("HiddenImages").transform.Find("Image" + i + "Pawn").gameObject.SetActive(false);
            }
            for(int j=0;j<gameMaster.placelistvisited.Count;j++)
            {
                /*if (gameMaster.imagehandler.ContainsKey(gameMaster.placelistvisited[j]) == true)
                    gameObject.transform.Find("HiddenImages").transform.Find("Image" + i + "Pawn").gameObject.SetActive(false);*/
                if(gameObject.transform.Find("Image" + i).GetComponent<Image>().sprite==gameMaster.imagehandler[gameMaster.placelistvisited[j]])
                        gameObject.transform.Find("HiddenImages").transform.Find("Image" + i + "Pawn").gameObject.SetActive(false);
            }
            //else if(gameMaster.imagehandler[gameMaster.placelist[i-1]]==gameObject.transform.Find("Image"+i))
            /*else if(gameMaster.imagehandler.ContainsKey(gameMaster.placelistvisited[i - 1])==true)
                gameObject.transform.Find("HiddenImages").transform.Find("Image" + i + "Pawn").gameObject.SetActive(false);*/
        }
        //find all the visited places and disable them
        //
    }
    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }
    */

    // Update is called once per frame
    void Update()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //keep a usable string to handle data
            string placechosen;

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                Debug.Log("Hit " + result.gameObject.name);

                if (result.gameObject.name.StartsWith("Image")&& result.gameObject.name.EndsWith("Pawn"))
                {
                    //remove 'Image' from choice
                    placechosen = result.gameObject.name;//.Substring(5);

                    Debug.Log("placechosen is: " + placechosen);

                    //if we have a previously disabled item, then enable it now
                    if (placeimage != null)
                        placeimage.SetActive(true);
                    //disable the clicked imageObject from showing
                    placeimage = result.gameObject;
                    placeimage.SetActive(false);

                    //check if it is one of the places we're allowed to go
                    /*foreach(string pl in gameMaster.placelist)
                    {
                        
                        if (pl == placetovisit)
                        {
                            Debug.Log(placetovisit + " is in the list");
                            //if (gameMaster.placelistvisited == null)
                            if (gameMaster.placelistvisited.Count==0)
                            {
                                current_choice = placetovisit;
                                Debug.Log("current choice confirmed");
                            }
                            else
                            {
                                foreach (string plv in gameMaster.placelistvisited)
                                {
                                    if (plv == placetovisit)
                                        Debug.Log("place already visited");
                                    else
                                    {
                                        Debug.Log("place not visited");
                                        current_choice = placetovisit;
                                    }
                                }
                            }                            
                        }
                        //else if
                    }*/
                }
            }
            /*if (current_choice != null)
            {
                Debug.Log("Our current choice to go is: " + current_choice);

                //re-enable the button, now that we have made a choice
                //button.SetActive(true);
                bu.interactable = true;
            }*/
        }
    }

    //add our new place to the list of visited ones
    /*public void addplace()
    {
        gameMaster.placelistvisited.Add(current_choice);
    }*/
}

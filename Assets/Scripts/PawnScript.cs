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

    public GameObject[] button;//=new GameObject[2];
    public Button[] bu;//=new Button[2];

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

        //find the screen's buttons and disable them (if more than one exist)
        button=new GameObject[2];
        bu=new Button[2];
        //if (gameObject.transform.Find("Button2") != null)
        if (GameObject.Find("Canvas OneIm Map TwoOptions(Clone)") != null)
        {
            for (int i = 0; i <= 1; i++)
            {
                
                if (i == 1)
                    button[i] = GameObject.Find("Canvas OneIm Map TwoOptions(Clone)").transform.Find("Button2").gameObject;
                else
                    button[i] = GameObject.Find("Canvas OneIm Map TwoOptions(Clone)").transform.Find("ContinueButton").gameObject;

                bu[i] = button[i].GetComponent<Button>();
                bu[i].interactable = false;
                //button.SetActive(false);
            }
        }

        placeimage = null;

        //Test: Work in Progress
        //find the info point and disable it        
        for(int i=1;i<=9;i++)
        {
            //Debug.Log("for loop:" +i);

            Transform gob = gameObject.transform.Find("Image" + i);
            if (gob==null)
                return;

            //Debug.Log("for loop:" + gameObject.transform.Find("Image" + i).name);
            //Debug.Log("for loop:" + gameMaster.imagehandler["InfoPoint"].name);
            if (gameObject.transform.Find("Image" + i).GetComponent<Image>().sprite == gameMaster.imagehandler["InfoPoint"])
            {

                gameObject.transform.Find("HiddenImages").transform.Find("Image" + i + "Pawn").gameObject.SetActive(false);
            }
            //else                Debug.Log("not");

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

        GameObject.Find("Canvas OneIm Map TwoOptions(Clone)");
    }

    // Update is called once per frame
    void Update()
    {
        //in case this isn't an interactive screen, disable interaction
        if (gameObject.name.Contains("Obscure"))
            return;
        if (GameObject.Find("Canvas MapObscure(Clone)") != null)
            return;

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
                //Debug.Log("Hit " + result.gameObject.name);

                if (result.gameObject.name.StartsWith("Image")&& result.gameObject.name.EndsWith("Pawn"))
                {
                    //remove 'Image' from choice
                    placechosen = result.gameObject.name;//.Substring(5);

                    //Debug.Log("placechosen is: " + placechosen);

                    //if we have a previously disabled item, then enable it now
                    if (placeimage != null)
                        //placeimage.SetActive(true);
                        placeimage.GetComponent<Image>().sprite = gameMaster.imagehandler["Places"];

                    //disable the clicked imageObject from showing
                    placeimage = result.gameObject;


                    //placeimage.SetActive(false);
                    placeimage.GetComponent<Image>().sprite = gameMaster.imagehandler["Pawn"];

                    //re-allow buttons
                    if (GameObject.Find("Canvas OneIm Map TwoOptions(Clone)").transform.Find("Button2") != null)
                        foreach (Button b in bu)
                            b.interactable = true;
                }
            }
            
        }
    }

    
}

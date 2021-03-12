﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaceSelectScript : MonoBehaviour
{
    //initial version copied from
    //https://docs.unity3d.com/2018.1/Documentation/ScriptReference/UI.GraphicRaycaster.Raycast.html

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    string current_choice;
    GameMaster gameMaster;

    public GameObject button;
    public Button bu;

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
        button = gameObject.transform.Find("Button").gameObject;
        bu = button.GetComponent<Button>();
        bu.interactable = false;
        //button.SetActive(false);
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
        /*if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit2D.collider != null)
            {
                Debug.Log("raycast hits " + hit2D.collider.name);
            }
        }*/
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
            string placetovisit;

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                Debug.Log("Hit " + result.gameObject.name);

                if (result.gameObject.name.StartsWith("Image"))
                {
                    //remove 'Image' from choice
                    placetovisit = result.gameObject.name.Substring(5);

                    Debug.Log("placetovisit is: " + placetovisit);

                    //check if it is one of the places we're allowed to go
                    foreach(string pl in gameMaster.placelist)
                    {
                        
                        if (pl == placetovisit)
                        {
                            //Debug.Log(placetovisit + " is in the list");
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
                                    { 
                                        Debug.Log("place already visited");
                                        current_choice = null;
                                        break;
                                    }
                                    else
                                    {
                                        Debug.Log("place not visited");
                                        current_choice = placetovisit;
                                        //break;
                                    }
                                }
                            }                            
                        }
                        //else if
                    }
                }
            }
            if (current_choice != null)
            {
                Debug.Log("Our current choice to go is: " + current_choice);

                //set our current location to the one we selected
                gameMaster.current_location = current_choice;

                //re-enable the button, now that we have made a choice                
                bu.interactable = true;
            }
        }
    }

    //add our new place to the list of visited ones
    public void addplace()
    {
        gameMaster.placelistvisited.Add(current_choice);
    }
}

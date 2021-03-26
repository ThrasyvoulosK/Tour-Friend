﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ScreenObjectScript : MonoBehaviour
{
    public GameMaster gameMaster;
    // Start is called before the first frame update
    void Start()
    {
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        //Debug.Log(gameMaster.total_points);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextscreen(GameObject nextGameObject)
    {
        GameObject newgameobject=Instantiate(nextGameObject);

        //assign buttons if needed
        if (newgameobject.name == "Canvas Menu(Clone)")
        {
            Debug.Log("assigning new start menu button");
            newgameobject.transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(delegate 
            {
                gameMaster.backB.SetActive(true);//
                gameMaster.createTwoImagesscreen(gameMaster.sgo[gameMaster.current_screen], gameMaster.screen_SOs[gameMaster.current_screen].description, gameMaster.screen_SOs[gameMaster.current_screen].Imagename, gameMaster.screen_SOs[gameMaster.current_screen].Imagename2, gameMaster.screen_SOs[gameMaster.current_screen].Button1text);
                
            });

            gameMaster.checkdelegate = false;
        }

        Destroy(gameObject);
    }

    //similar to nextscreen, but with parameters
    public void createnextscreen()
    {
        GameObject newgameobject;
        newgameobject=Instantiate(gameObject);

        //assign description
        //newgameobject.child
        //assign content
        //assign button(s)
    }

    //self explanatory, we'll call it a lot
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    //the simple one image (common) screen
    //
    /*public void createOneImagescreen(GameObject prefab_go,string desc,string img,string button)
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
        Debug.Log(gameMaster.words_en[0]);
        //newgameobject.GetComponentInChildren<Image>().sprite=gameMaster.imagehandler[img];
        image.sprite = gameMaster.imagehandler[img];
        //newgameobject.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = gameMaster.imagehandler[img];
        newgameobject.transform.GetChild(5).GetComponent<Text>().text = button;
        //
    }*/

    public int screenadd(int currentscreen)
    {
        return currentscreen++;
    }

    //quit the app with this function
    public void ExitApp()
    {
        Application.Quit();
    }

    //call the first screen from script (useful for buttons)
    public void callfirstscreen()
    {
        Debug.Log("calling first screen");
        gameObject.transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(delegate { gameMaster.createTwoImagesscreen(gameMaster.sgo[gameMaster.current_screen], gameMaster.screen_SOs[gameMaster.current_screen].description, gameMaster.screen_SOs[gameMaster.current_screen].Imagename, gameMaster.screen_SOs[gameMaster.current_screen].Imagename2, gameMaster.screen_SOs[gameMaster.current_screen].Button1text); });

        Destroy(gameObject);
    }
}

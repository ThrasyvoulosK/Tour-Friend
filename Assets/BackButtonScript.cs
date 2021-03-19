using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackButtonScript : MonoBehaviour
{
    GameMaster theGameMaster;
    // Start is called before the first frame update
    void Start()
    {
        theGameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void BackOne()
    {
        Debug.Log("Back Button Clicked!");
        if (theGameMaster.current_screen >= 2)
        {
            theGameMaster.current_screen -= 2;
            gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            theGameMaster.ConstructorDecider(gameObject.GetComponent<Button>());
        }
        //Debug.Log
    }*/
}

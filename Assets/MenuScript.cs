using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    GameMaster gameMaster;
    // Start is called before the first frame update
    void Start()
    {
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();

        //set image
        transform.Find("SettingsLanguageTextButton").GetComponent<Image>().sprite = gameMaster.imagehandler["Menu" + gameMaster.language_current];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

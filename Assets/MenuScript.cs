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
        if(gameObject.name== "Canvas MenuSettings(Clone)")
            transform.Find("SettingsLanguageTextButton").GetComponent<Image>().sprite = gameMaster.imagehandler["Menu" + gameMaster.language_current];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTextEnglish()
    {
        gameMaster.language_current = "English";
        gameMaster.LanguageDictionaryInitialise(gameMaster.language_current);
    }

    public void SetTextGreek()
    {
        gameMaster.language_current = "Greek";
        gameMaster.LanguageDictionaryInitialise(gameMaster.language_current);
    }
}

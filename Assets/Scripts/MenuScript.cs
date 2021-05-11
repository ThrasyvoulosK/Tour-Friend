using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        TranslateEverything();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //(text menu)
    public void SetText(string languageName)
    {
        gameMaster.language_current = languageName;
        gameMaster.LanguageDictionaryInitialise(gameMaster.language_current);
    }

    public void TranslateEverything()
    {
        int noc = gameObject.transform.childCount;
        //foreach(GameObject child in gameObject.transform)
        for(int i=0;i<noc;i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            if (child.GetComponentInChildren<TextMeshProUGUI>()==true&&child.activeInHierarchy)
            {
                //skip word if already translated
                if(gameMaster.texthandler.ContainsKey(child.GetComponentInChildren<TextMeshProUGUI>().text)==false)
                {
                    Debug.Log($"Hopefully, we have already assigned key: {child.GetComponentInChildren<TextMeshProUGUI>().text} elsewhere");
                    continue;
                }

                child.GetComponentInChildren<TextMeshProUGUI>().text = gameMaster.AssignString(child.GetComponentInChildren<TextMeshProUGUI>().text);

                //get child of child too, if it exists (occurs only once, in select phrases)
                int snoc = child.transform.childCount;
                if (snoc>0)
                {
                    for (int j = 0; j < snoc; j++)
                    {
                        GameObject secondChild = child.transform.GetChild(j).gameObject;
                        if (secondChild.GetComponentInChildren<TextMeshProUGUI>() == true && secondChild.activeInHierarchy)
                        {
                            //skip word if already translated
                            if (gameMaster.texthandler.ContainsKey(secondChild.GetComponentInChildren<TextMeshProUGUI>().text) == false)
                            {
                                Debug.Log($"Hopefully, we have already assigned key: {secondChild.GetComponentInChildren<TextMeshProUGUI>().text} elsewhere");
                                continue;
                            }

                            secondChild.GetComponentInChildren<TextMeshProUGUI>().text = gameMaster.AssignString(secondChild.GetComponentInChildren<TextMeshProUGUI>().text);
                        }
                    }
                }
            }
        }
    }
}

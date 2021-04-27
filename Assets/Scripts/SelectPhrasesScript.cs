using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class SelectPhrasesScript : MonoBehaviour
{
    public string chosenphrase;
    public string chosenVideo;
    // Start is called before the first frame update
    void Start()
    {
        return;
        //foreach (GameObject gameO in gameObject.transform)
        GameObject gameO = gameObject.transform.Find("ButtonsRow").gameObject;
        for (int i = 0; i < gameO.transform.childCount; i++)
        {
            Debug.Log(i + gameO.name);
            gameO.transform.GetChild(i).Find("BackgroundImage").GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //draw a green recctangle around one and invisivise all others
    public void GreenOneGreyOthers(GameObject buttonobject)
    {
        return;
        Debug.Log("phrase script gameobject called: " + buttonobject.name);
        buttonobject.transform.Find("BackgroundImage").GetComponent<Image>().color = new Color(0, 255f, 0, 1f);

        /*foreach (GameObject gameO in gameObject.transform)
            if(gameO.name!=gameObject.name)
                gameO.transform.Find("BackgroundImage").GetComponent<Image>().color = new Color(0, 0, 0, 0);*/
        GameObject gameO = gameObject.transform.Find("ButtonsRow").gameObject;
        for (int i = 0; i < gameO.transform.childCount; i++)
        {
            Debug.Log(i + gameO.name);
            if (gameO.transform.GetChild(i).name != buttonobject.name)
                gameO.transform.GetChild(i).Find("BackgroundImage").GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
    }

    public void GetName(GameObject game)
    {
        chosenphrase=game.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
        chosenVideo=game.name;
    }
}

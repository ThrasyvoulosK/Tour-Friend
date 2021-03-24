using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class LenseScript : MonoBehaviour
{
    GameObject lense;
    bool ison = false;
    bool changed = false;
    public Vector3 orig_pos;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("lense start");
        lense = gameObject.transform.Find("LenseButton").gameObject;
        gameObject.GetComponent<Canvas>().overrideSorting=true;
        orig_pos = gameObject.GetComponent<RectTransform>().position;
        gameObject.transform.Find("LenseButton").GetComponentInChildren<Button>().onClick.AddListener(delegate { gameObject.GetComponent<RectTransform>().localScale = new Vector3(2.25f, 2.25f, 0);
            gameObject.GetComponent<RectTransform>().position = gameObject.GetComponent<RectTransform>().parent.position;
            gameObject.GetComponent<Canvas>().sortingOrder = 1;
            gameObject.transform.Find("RawImage").transform.Find("Video Player").GetComponent<UnityEngine.Video.VideoPlayer>().Play();
            ison = true; changed = true; });
    }

    // Update is called once per frame
    void Update()
    {
        if (changed&&ison)
        {
            Debug.Log("lense changed");
            gameObject.transform.Find("LenseButton").GetComponentInChildren<Button>().onClick.RemoveAllListeners();
            gameObject.transform.Find("LenseButton").GetComponentInChildren<Button>().onClick.AddListener(delegate
            {
                gameObject.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 0);
                gameObject.GetComponent<RectTransform>().position = orig_pos;
                gameObject.GetComponent<Canvas>().sortingOrder=0 ;
                gameObject.transform.Find("RawImage").transform.Find("Video Player").GetComponent<UnityEngine.Video.VideoPlayer>().Stop();
                ison = false; changed = true;
            });
            changed = false;
            
        }
        else if(changed&&(ison==false)) //if(ison)
        {
            
                Debug.Log("not changed-ison");
                gameObject.transform.Find("LenseButton").GetComponentInChildren<Button>().onClick.RemoveAllListeners();
                gameObject.transform.Find("LenseButton").GetComponentInChildren<Button>().onClick.AddListener(delegate
                {
                    gameObject.GetComponent<RectTransform>().localScale = new Vector3(2.25f, 2.25f, 0);
                    gameObject.GetComponent<RectTransform>().position = gameObject.GetComponent<RectTransform>().parent.position;
                    gameObject.GetComponent<Canvas>().sortingOrder = 1;
                    gameObject.transform.Find("RawImage").transform.Find("Video Player").GetComponent<UnityEngine.Video.VideoPlayer>().Play();
                    ison = true; changed = true;
                });
            changed = false;

            
        }

    }
}

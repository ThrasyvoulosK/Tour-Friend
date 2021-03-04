using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MapScript : MonoBehaviour
{
    public List<Sprite> locationsprites = new List<Sprite>();
    public Sprite[] newlocationsprites =  new Sprite[9];
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("GameMaster").GetComponent<GameMaster>().mapinit==false)
        {
            assignmaplocations();
            GameObject.Find("GameMaster").GetComponent<GameMaster>().map = gameObject;
            GameObject.Find("GameMaster").GetComponent<GameMaster>().mapinit = true;
        }
        else
            Debug.Log("we already have a map");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void assignmaplocations()
    {
        //find the images
        for(int i=1;i<=9;i++)
        {
            //string istr = i.ToString();
            Sprite mapspr=gameObject.transform.Find("Image" + i.ToString()).GetComponent<Image>().sprite;
            locationsprites.Add(mapspr);
        }
        //allocate them randomly
        int rand;
        for (int i = 0; i <= 8; i++)
        {
            rand = Random.Range(0, 8);
            newlocationsprites[i] = locationsprites[rand];
            Debug.Log("random sprite i is: " + i);

        }
        //finally, reassign them back to the 'images'
        for (int i = 1; i <= 9; i++)
        {
            Image mapi = gameObject.transform.Find("Image" + i.ToString()).GetComponent<Image>();
            //Sprite mapspr = gameObject.transform.Find("Image" + i.ToString()).GetComponent<Image>().sprite;
            mapi.sprite = newlocationsprites[i-1];
            //newlocationsprites.
        }

    }
}

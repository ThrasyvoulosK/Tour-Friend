﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
/*The game's screens, each individually as a scriptable object*/
[CreateAssetMenu]
public class Screen_SO : ScriptableObject
{
    //public Text Description;
    [TextArea]
    public string description;
    [TextArea]
    public string description2;
    [TextArea]
    public string description3;
    //public GameObject Content;
    //public Image Image1;
    //public Sprite Image1sprite;
    public string Imagename;
    public string Imagename2;
    //
    public string Video1;
    public string Video2;
    //public Button Button1;
    public string Button1text;
    //public Button Button2;
    public string Button2text;

    public string points;
    //public Sprite DescriptionImage;
    public string Player;
    public GameObject prefab;

}

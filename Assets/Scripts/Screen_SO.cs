using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
/*The game's screens, each individually as a scriptable object*/
[CreateAssetMenu]
public class Screen_SO : ScriptableObject
{
    public Text Description;
    [TextArea]
    public string description;
    public GameObject Content;
    public Image Image1;
    public Sprite Image1sprite;
    public Button Button1;
    public string Button1text;
    public Button Button2;

    public string points;
    public Sprite DescriptionImage;

}

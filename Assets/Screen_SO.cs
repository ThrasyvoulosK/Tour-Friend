using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
/*The game's screens, each individually as a scriptable object*/
[CreateAssetMenu]
public class Screen_SO : ScriptableObject
{
    public Text Description;
    public GameObject Content;
    public Button Button;

}

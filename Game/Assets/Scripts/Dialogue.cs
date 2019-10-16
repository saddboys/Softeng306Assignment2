using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Game
{
[System.Serializable]
public class Dialogue 
{
    public string name;
    public Sprite mayor;
    [TextArea(3,10)]
    public string[] sentences;
}
}
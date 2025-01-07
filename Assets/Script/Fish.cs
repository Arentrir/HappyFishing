using UnityEngine;

[CreateAssetMenu(fileName = "Fish", menuName = "Scriptable Objects/Fish")]


public class Fish : ScriptableObject
{
    public int ID;
    public string fishName;
    public Sprite image;
    public float weight;
    public float price;
    
}




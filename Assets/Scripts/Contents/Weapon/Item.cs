using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    string _name;
    [SerializeField]
    int _level;
    [SerializeField]
    float _value;
    [SerializeField]
    string _phrase;
    [SerializeField]
    string _title;
    public string Name { get { return _name; } set { _name = value; } }
    public int Level { get { return _level; } set { _level = value; } }
    public float Value { get { return _value; } set { _value = value; } }
    public string Phrase { get { return _phrase; } set { _phrase = value; } }
    public string Title { get { return _title; } set { _title = value; } }
    private void Start()
    {

    }
    public void SetInfo(string name, int level, float value, string phrase, string title)
    {
        Name = name;
        Level = level;
        Value = value;
        Phrase = phrase;
        Title = title;
    }
}

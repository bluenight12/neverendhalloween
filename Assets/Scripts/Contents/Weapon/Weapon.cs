using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    int _damage;
    [SerializeField]
    float _speed;
    [SerializeField]
    float _rate;
    [SerializeField]
    float _range;
    [SerializeField]
    string _name;
    [SerializeField]
    string _wptype;
    public int Damage { get { return _damage; } set { _damage = value; } }
    public float Speed { get { return _speed; } set { _speed = value; } }
    public float Rate { get { return _rate; } set { _rate = value; } }
    public float Range { get { return _range; } set { _range = value; } }
    public string Name { get { return _name; } set { _name = value; } }
    public string WpType { get { return _wptype; } set { _wptype = value; } }
    private void Start()
    {
        _damage = 0;
        _speed = 1;
        _rate = 1;
        _range = 1;
        _name = "Punch";
        _wptype = "melee";
    }
}

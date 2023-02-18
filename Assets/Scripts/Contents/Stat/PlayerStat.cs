using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{

    [SerializeField]
    protected int _id;
    [SerializeField]
    protected int _soul;
    [SerializeField]
    private float _jumpPower;
    [SerializeField]
    private int _jumpCount;


    public int ID { get { return _id; } set { _id = value; } }
    public int Soul { get { return _soul; } set { _soul = value; } }
    public float JumpPower { get { return _jumpPower; } set { _jumpPower = value;} }
    public int JumpCount { get { return _jumpCount; } set { _jumpCount = value; } }

    private void Start()
    {
        _level = 1;
        _hp = 100;
        _maxHp = 100;
        _attack = 10;
        _defense = 0;
        _moveSpeed = 5.0f;
        _id = 1;
        _soul = 0;
        _jumpCount = 2;
        _jumpPower = 13.0f;
    }
}

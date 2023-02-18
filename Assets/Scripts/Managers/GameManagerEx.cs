using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    GameObject _player;
    //Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
    HashSet<GameObject> _monsters = new HashSet<GameObject>();
    HashSet<GameObject> _npcs = new HashSet<GameObject>();
    public int MonCount = 0;
    public Action<string> _monsterDie;

    public void Init()
    {
        _monsterDie += Managers.Quest.OnMonsterDeath;
    }
    public GameObject GetPlayer() { return _player; }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);
        switch (type)
        {
            case Define.WorldObject.Monster:
                _monsters.Add(go);
                MonCount++;
                break;
            case Define.WorldObject.Player:
                _player = go;
                break;
            case Define.WorldObject.NPC:
                _npcs.Add(go);
                break;
        }
        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return Define.WorldObject.Unknown;

        return bc.WorldObjectType;
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go);

        switch (type)
        {
            case Define.WorldObject.Monster:
                {
                    if (_monsters.Contains(go))
                    {
                        _monsterDie.Invoke(go.name);
                        _monsters.Remove(go);
                        _player.GetComponent<PlayerStat>().Soul++;
                        MonCount--;
                    }
                }
                break;
            case Define.WorldObject.Player:
                {
                    if (_player == go)
                        _player = null;
                }
                break;
            case Define.WorldObject.NPC:
                {
                    if (_npcs.Contains(go))
                    {
                        _npcs.Remove(go);
                    }
                }
                break;
        }

        Managers.Resource.Destroy(go);
    }
    public void MonsterClear()
    {
        _monsters.Clear();
        MonCount = 0;
    }
}

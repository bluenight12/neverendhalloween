using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}
public class DataManager
{
    public Data.Save SaveFile { get; private set; } = new Data.Save();
    public Dictionary<string, Data.Weapon> WeaponDict { get; private set; } = new Dictionary<string, Data.Weapon>();
    public Dictionary<string, Data.Monster> MonsterDict { get; private set; } = new Dictionary<string, Data.Monster>();
    public Dictionary<string, Data.Skill> SkillDict { get; private set; } = new Dictionary<string, Data.Skill>();
    public Dictionary<int, Data.Item> ItemDict { get; private set; } = new Dictionary<int, Data.Item>();
    public Dictionary<int, Data.Quest> QuestDict { get; private set; } = new Dictionary<int, Data.Quest>();
    public void Init()
    {
        WeaponDict = LoadJson<Data.WeaponData, string, Data.Weapon>("WeaponData").MakeDict();
        MonsterDict = LoadJson<Data.MonsterData, string, Data.Monster>("MonsterData").MakeDict();
        SkillDict = LoadJson<Data.SkillData, string, Data.Skill>("SkillData").MakeDict();
        ItemDict = LoadJson<Data.ItemData, int, Data.Item>("ItemData").MakeDict();
        QuestDict = LoadJson<Data.QuestData, int, Data.Quest>("QuestData").MakeDict();
    }
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
    public void SaveData()
    {
        string data = JsonUtility.ToJson(SaveFile);
        File.WriteAllText($"Assets/Resources/Data/Save/Savefile{SaveFile.id}.Json", data);
    }   
    public bool LoadData(int id)
    {
        TextAsset data = Managers.Resource.Load<TextAsset>($"Data/Save/Savefile{id}");
        if(data == null)
        {
            return false;
        }
        SaveFile = JsonUtility.FromJson<Data.Save>(data.text);
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemManager
{
    Dictionary<int, Data.Item> itemDict = new Dictionary<int, Data.Item>();
    Dictionary<string, Data.Weapon> weaponDict = new Dictionary<string, Data.Weapon>();
    List<string> weaponNames = new List<string>();
    GameObject[] itemboxs = new GameObject[3];
    GameObject[] items = new GameObject[3];
    public void Init()
    {
        itemDict = Managers.Data.ItemDict;
        weaponDict = Managers.Data.WeaponDict;
        foreach(Data.Weapon weapon in weaponDict.Values)
        {
            if(!weapon.name.Equals("Punch"))
                weaponNames.Add(weapon.name);
        }
    }
    public GameObject MakeItem(Transform parent = null)
    {
        int i = Random.Range(1, itemDict.Count + 1);
        Data.Item _item; 
        itemDict.TryGetValue(i, out _item);
        GameObject go = Managers.Resource.Instantiate($"Item/{_item.name}", parent);
        go.GetOrAddComponent<Item>().SetInfo(_item.name, _item.level, _item.value, _item.phrase, _item.title);
        return go;
    }
    public GameObject MakeWeaponItem(Transform parent = null)
    {
        int i = Random.Range(0, weaponDict.Count - 1);
        GameObject go = Managers.Resource.Instantiate($"WeaponItem/{weaponNames[i]}",parent);
        return go;
    }
    public void SetItem()
    {
        string[] itemPhrase;
        itemPhrase = new string[3];
        GameObject npc = GameObject.FindGameObjectWithTag("NPC");
        npc.GetComponent<Animator>().SetTrigger("Open");
        GameObject wpitemBox = Util.FindChild(npc, $"0", true).gameObject;
        itemboxs[0] = wpitemBox;
        GameObject wpitem = Managers.Item.MakeWeaponItem(wpitemBox.transform);
        items[0] = wpitem;
        wpitem.transform.position = wpitemBox.transform.position;
        for (int i = 1; i <= 2; i++)
        {
            GameObject itemBox = Util.FindChild(npc, $"{i}", true).gameObject;
            itemboxs[i] = itemBox;
            GameObject item = Managers.Item.MakeItem(itemBox.transform);
            items[i] = item;
            for (int j = 1; j < i; j++)
            {
                if (itemPhrase[j].Equals(item.GetComponent<Item>().Phrase))
                {
                    i--;
                    Managers.Resource.Destroy(item);
                    continue;
                }
            }
            itemPhrase[i] = item.GetComponent<Item>().Phrase;
            item.transform.position = itemBox.transform.position;
        }
    }
    public void OpenItem()
    {
        SetItem();
        foreach (GameObject itembox in itemboxs)
        {
            itembox.GetComponent<Animator>().SetTrigger("Open");
        }
    }
    public void CloseItem()
    {
        foreach(GameObject item in items)
        {
            Managers.Resource.Destroy(item);
        }
    }
    public void Clear()
    {

    }
}

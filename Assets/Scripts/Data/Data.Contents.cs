using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Data
{
    #region Item
    [Serializable]
    public class Item
    {
        public int id;
        public string name;
        public int level;
        public float value;
        public string phrase;
        public string title;
    }
    [Serializable]
    public class ItemData : ILoader<int, Item>
    {
        public List<Item> items = new List<Item>();
        public Dictionary<int, Item> MakeDict()
        {
            Dictionary<int, Item> dict = new Dictionary<int, Item>();
            foreach (Item item in items)
                dict.Add(item.id, item);
            return dict;
        }
    }
    #endregion

    #region Skill
    [Serializable]
    public class Skill
    {
        public string name;
        public int damage;
        public int coolTime;
    }
    [Serializable]
    public class SkillData : ILoader<string, Skill>
    {
        public List<Skill> skills = new List<Skill>();
        public Dictionary<string, Skill> MakeDict()
        {
            Dictionary<string, Skill> dict = new Dictionary<string, Skill>();

            foreach (Skill skill in skills)
                dict.Add(skill.name, skill);
            return dict;
        }
    }
    #endregion

    #region Save

    [Serializable]
    public class Save
    {
        public int id;
        public int soul;
        public int hp;
        public int attack;
    }

    #endregion

    #region Weapon

    [Serializable]
    public class Weapon
    {
        public int damage;
        public float speed;
        public float rate;
        public float range;
        public string name;
        public string wptype;
    }
    [Serializable]
    public class WeaponData : ILoader<string, Weapon>
    {
        public List<Weapon> weapons = new List<Weapon>();
        public Dictionary<string, Weapon> MakeDict()
        {
            Dictionary<string, Weapon> dict = new Dictionary<string, Weapon>();

            foreach (Weapon weapon in weapons)
                dict.Add(weapon.name, weapon);
            return dict;
        }
        
    }
    #endregion

    #region Monster

    [Serializable]
    public class Monster
    {
        public int damage;
        public float speed;
        public float rate;
        public float range;
        public string name;
        public string wptype;
        public float scanrange;
        public float attackrange;
        public int maxhp;
    }
    [Serializable]
    public class MonsterData : ILoader<string, Monster>
    {
        public List<Monster> monsters = new List<Monster>();
        public Dictionary<string, Monster> MakeDict()
        {
            Dictionary<string, Monster> dict = new Dictionary<string, Monster>();

            foreach (Monster monster in monsters)
                dict.Add(monster.name, monster);
            return dict;
        }

    }
    #endregion

    #region Quest

    [Serializable]
    public class Quest
    {
        public int id;
        public string phrase;
        public string name;
        public int amount;
        public int goal;
        public int reward;
        public int status;
        public bool isClear;
    }
    [Serializable]
    public class QuestData : ILoader<int, Quest>
    {
        public List<Quest> quests = new List<Quest>();
        public Dictionary<int, Quest> MakeDict()
        {
            Dictionary<int, Quest> dict = new Dictionary<int, Quest>();

            foreach (Quest quest in quests)
                dict.Add(quest.id, quest);
            return dict;
        }

    }
    #endregion
}
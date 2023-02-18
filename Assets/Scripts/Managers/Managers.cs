using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성 보장
    static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고옴
    DataManager _data = new DataManager();
    GameManagerEx _game = new GameManagerEx();
    InputManager _input = new InputManager();
    ItemManager _item = new ItemManager();
    PoolManager _pool = new PoolManager();
    QuestManager _quest = new QuestManager();
    ResourceManager _resource = new ResourceManager();
    TalkManager _talk = new TalkManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SkillManager _skill = new SkillManager();
    SoundManager _sound = new SoundManager();
    UIManager _ui = new UIManager();
    WeaponManager _weapon = new WeaponManager();
    public static DataManager Data { get { return Instance._data; } }
    public static GameManagerEx Game { get { return Instance._game; } }
    public static InputManager Input { get { return Instance._input; } }
    public static ItemManager Item { get { return Instance._item; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static QuestManager Quest { get { return Instance._quest; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static TalkManager Talk { get { return Instance._talk; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SkillManager Skill { get { return Instance._skill; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static WeaponManager Weapon { get { return Instance._weapon; } }
    void Start()
    {
        Init();
    }
    void Update()
    {
        _input.OnUpdate();
    }
    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
            s_instance._data.Init();
            s_instance._pool.Init();
            s_instance._game.Init();
            s_instance._sound.Init();
            s_instance._weapon.Init();
            s_instance._item.Init();
            s_instance._skill.Init();
            s_instance._quest.Init();
            s_instance._item.Init();
            s_instance._talk.Init();
        }

    }
    public static void Clear()
    {
        Input.Clear();
        Skill.Clear();
        Scene.Clear();
        Sound.Clear();
        UI.Clear();
        Pool.Clear();
    }
}

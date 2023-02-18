using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum State
    {
        Die,
        Moving,
        Idle,
        Channeling,
        EventMove,
        Attack,
        Skill,
        Jumping,
        Falling,
        Knockback,
        Climb
    }
    public enum Layer
    {
        Player = 7,
        Monster = 8,
        Ground = 9,
        Block = 10,
        WeaponItem = 11,
        MonsterBlock = 12,
        Bullet = 13,
        NPC = 14,
        Item = 15,
        PlayerBlock = 16,
        Platform = 17,
        Exit = 18,
        Ladder = 19,
        Trap = 20,
        Door = 21
    }
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
        Town,
        Bonus,
        Intro,
        Tutorial
    }
    public enum Sound
    {
        Bgm,
        Effect,
        FootStep,
        MaxCount,
    }
    public enum UIEvent
    {
        Click,
        Drag,
        Enter,
        Exit
    }
    public enum CameraMode{
        Scroll,
        Dialog,
        Intro,
        Town
    }
    public enum MouseEvent{
        Press,
        Click,
    }
    public enum WorldObject
    {
        Player,
        Monster,
        Unknown,
        NPC
    }
}

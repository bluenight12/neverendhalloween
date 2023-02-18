using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponManager
{
    // 무기 종류별로 나누고 배열 만들기
    private Weapon[] _weapon;
    // 데이터에서 받아서 무기 정보 Dictionary에 등록하기
    Dictionary<string, Data.Weapon> _weapondict;
    public void Init()
    {
        _weapondict = Managers.Data.WeaponDict;
    }
    [SerializeField]
    private string weaponType;
    public static string currentWeapon { get; private set; }
    public void ChangeWeapon(GameObject go, Data.Weapon wp)
    {
        Weapon _weapon = go.GetOrAddComponent<Weapon>();
        currentWeapon = _weapon.name;
        _weapon.Damage = wp.damage;
        _weapon.Speed = wp.speed;
        _weapon.Rate = wp.rate;
        _weapon.Range = wp.range;
        _weapon.Name = wp.name;
        _weapon.WpType = wp.wptype;
    }
    // PlayerController에서 무기 정보 받아와서 무기 바꾸기
    // WeaponStat에 값 할당해주기
    public void Clear()
    {

    }
}

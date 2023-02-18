using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponManager
{
    // ���� �������� ������ �迭 �����
    private Weapon[] _weapon;
    // �����Ϳ��� �޾Ƽ� ���� ���� Dictionary�� ����ϱ�
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
    // PlayerController���� ���� ���� �޾ƿͼ� ���� �ٲٱ�
    // WeaponStat�� �� �Ҵ����ֱ�
    public void Clear()
    {

    }
}

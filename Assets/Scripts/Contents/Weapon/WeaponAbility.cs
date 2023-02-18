using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAbility : MonoBehaviour
{
    [SerializeField]
    protected float _skillRatio;
    [SerializeField]
    protected float _skillSpeed;
    [SerializeField]
    private int _critPer;
    [SerializeField]
    protected float _attackRatio;
    [SerializeField]
    protected float _attackSpeed;
    [SerializeField]
    private int _addDamage;
    [SerializeField]
    private int _poisonPer;
    [SerializeField]
    private int _lifeSteal;
    [SerializeField]
    private int _healthRegen;
    [SerializeField]
    private float _criticalDamage;
    public float SkillRatio { get { return _skillRatio; } set { _skillRatio = value; } }
    public float SkillSpeed { get { return _skillSpeed; } set { _skillSpeed = value; } }
    public int CritPer { get { return _critPer; } set { _critPer = value; } }
    public float AttackRatio { get { return _attackRatio; } set { _attackRatio = value; } }
    public float AttackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }
    public int AddDamage { get { return _addDamage; } set { _addDamage = value; } }
    public int PoisonPer { get { return _poisonPer; } set { _poisonPer = value; } }
    public int LifeSteal { get { return _lifeSteal; } set { _lifeSteal = value; } }
    public int HealthRegen { get { return _healthRegen; } set { _healthRegen = value; } }
    public float CriticalDamage { get { return _criticalDamage; } set { _criticalDamage = value; } }
    private void Start()
    {
        ResetValue();
    }
    public void ResetValue()
    {
        _attackRatio = 1.0f;
        _skillRatio = 1.0f;
        _skillSpeed = 1.0f;
        _attackSpeed = 0.0f;
        _critPer = 0;
        _addDamage = 0;
        _poisonPer = 0;
        _lifeSteal = 0;
        _healthRegen = 0;
        _criticalDamage = 1.2f;
    }
}

using UnityEngine;

[System.Serializable]
public class SoldierData : HpData
{
    [SerializeField] public float damage;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float attackSpeed;
    [SerializeField] public float attackCoolDown;
    [SerializeField] public float attackRange;
}
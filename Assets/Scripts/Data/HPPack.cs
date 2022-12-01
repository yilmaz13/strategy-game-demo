using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HPPack", menuName = "ScriptableObjects/HPPack", order = 2)]
public class HPPack : ScriptableObject
{
    public List<HpData> hp;
    public List<SoldierData> SoldierData;
    
}
using Unit;
using UnityEngine;

public class Barrack : MilitaryBuilding
{
    public void Start()
    {
        base.Start();
    }

    [ContextMenu("Soldier Spawn")]
    public void SoldierSpawn()
    {
        var objSoldier = ObjectPooler.Instance.Spawn("Soldier", transform.position).GetComponent<Soldier>();
        //   objSoldier.SetStats(soldierData, Team.Player);
        spawnedSoldiers.Add(objSoldier);
    }
}
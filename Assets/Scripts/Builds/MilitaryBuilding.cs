using System.Collections.Generic;
using Builds;
using HpSystem;
using Unit;
using UnityEngine;

public class MilitaryBuilding : Build
{
    #region ProtectedValue

    [SerializeField] protected string name;
    [SerializeField] protected Transform soldierSpawnMovePoint;
    [SerializeField] protected Transform soldierSpawnPoint;
    [SerializeField] protected List<Soldier> spawnedSoldiers;
    [SerializeField] protected HpData hpData;
    [SerializeField] protected SoldierData soldierData;
    [SerializeField] protected string productionName;
    [SerializeField] protected string productionSpawnName;
    [SerializeField] public Sprite flag;

    #endregion

    #region UnityMethods

    public void Start()
    {
        base.Start();
        hpData = UnitManager.Instance.GetBuildData(name);
        productionName = hpData.productions[0];
        var sData = UnitManager.Instance.GetSoldierData(productionName);
        soldierData = sData;
        ChangeMaxHp(hpData.hp);
    }

    #endregion

    #region PublicMethods

    public override void OpenInformationPopup()
    {
        UIManager.Instance.OpenInformationPopup(hpData, SoldierSpawn, this);
    }

    public void ChangeSpawnPoint(Vector3 spawnPos)
    {
        soldierSpawnMovePoint.transform.gameObject.SetActive(true);
        soldierSpawnMovePoint.position = spawnPos;
    }

    public void RemoveSpawnPoint()
    {
        soldierSpawnMovePoint.transform.gameObject.SetActive(false);
    }

    public void SoldierSpawn(string str)
    {
        var objSoldier = ObjectPooler.Instance.Spawn(str, soldierSpawnPoint.position).GetComponent<Soldier>();
        var position = soldierSpawnMovePoint.position;
        objSoldier.SetStats(soldierData, Team.Player);
        objSoldier.GetComponent<MoveModule>().Move(position);
        spawnedSoldiers.Add(objSoldier);
    }

    #endregion
}
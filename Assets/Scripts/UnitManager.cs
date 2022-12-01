using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Builds;
using HpSystem;
using Unit;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private static UnitManager _instance = null;
    public static UnitManager Instance => _instance;

    public List<HpData> hp;
    public List<SoldierData> SoldierData;
    public List<HpController> HpControllers;
    public List<MilitaryBuilding> MilitaryBuildings;
    public List<Build> Builds;

    public List<Soldier> spawnedSoldiers;

    private void Awake()
    {
        //singleton pattern
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }

        _instance = this;

        var hPack = Resources.Load<HPPack>("ScriptableObjects/HPPack");
        hp = hPack.hp;
        SoldierData = hPack.SoldierData;
    }

    public HpData GetBuildData(string name)
    {
        return hp.FirstOrDefault(hp => hp.name == name);
    }

    public List<HpData> GetAllBuildData()
    {
        return hp;
    }

    public void Spawn(string spawnObj, Vector3 pos)
    {
        StartCoroutine(ISpawn(spawnObj, pos));
    }

    public IEnumerator ISpawn(string spawnObj, Vector3 pos)
    {
        yield return new WaitForSeconds(0.1f);

        HpData hpData = Instance.GetBuildData(spawnObj);
        var build = ObjectPooler.Instance.SpawnBuild(spawnObj, pos);
        build.SetStats(hpData, Team.Player);

        Builds.Add(build);

        yield return new WaitForSeconds(0.1f);
        GridManager.Instance.UpdateGrid(pos, hpData.height, hpData.width);
    }

    public void RegisterSoldier(Soldier soldier)
    {
        spawnedSoldiers.Add(soldier);
        HpControllers.Add(soldier);
    }

    public void UnRegisterSoldier(Soldier soldier)
    {
        spawnedSoldiers.Remove(soldier);
        HpControllers.Remove(soldier);
    }

    public void UnRegisterBuild(Build build)
    {
        Builds.Remove(build);
        HpControllers.Remove(build);
    }

    public void RegisterBuild(Build build)
    {
        Builds.Add(build);
        HpControllers.Add(build);
    }

    public SoldierData GetSoldierData(string name)
    {
        return SoldierData.FirstOrDefault(soldier => soldier.name == name);
    }

    public HpController ClosestEnemyUnit(Vector3 myPosition, Team team, float attackRange)
    {
        float closestDistance = Mathf.Infinity;
        HpController closestUnit = null;
        foreach (HpController teamUnits in HpControllers)
        {
            if (teamUnits.Team == team) continue;

            float distance = Vector3.Distance(myPosition, teamUnits.transform.position);
            if (attackRange < distance) continue;
            if (closestDistance < distance) continue;
            closestDistance = distance;
            closestUnit = teamUnits;
        }

        return closestUnit;
    }
}
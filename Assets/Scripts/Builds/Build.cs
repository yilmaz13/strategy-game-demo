using DG.Tweening;
using HpSystem;
using UnityEngine;

namespace Builds
{
    public class Build : HpController, IPoolable
    {
        #region UnityMethods

        public void Start()
        {
        }

        #endregion

        #region PublicMethods

        public void SetStats(HpData hpData, Team team)
        {
            CurrentHp = hpData.hp;
            Team = team;
            hpBar.ChangeSliderColor(Team);
            UnitManager.Instance.RegisterBuild(this);
        }

        public void OnReturnPool()
        {
            UnitManager.Instance.UnRegisterBuild(this);
        }

        public void OnPoolSpawn()
        {
            ResetHpController();
            SpawnEffect();
        }

        #endregion
    }
}
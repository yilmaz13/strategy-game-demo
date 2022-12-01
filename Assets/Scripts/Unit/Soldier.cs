using HpSystem;
using UnityEngine;

namespace Unit
{
    public class Soldier : HpController, IPoolable
    {
        #region PublicValue

        public HpController Target;
        public Vector3 movePoint;
        private DamageInfo _damageInfo;
        private PoolObject _poolObject;

        public float attackCoolDown;
        public float AttackCoolDown;
        private float attackTimer;

        [SerializeField] private SoldierData soldierData;

        public void SetStats(SoldierData soldierData, Team team)
        {
            this.soldierData = soldierData;
            MoveSpeed = soldierData.moveSpeed;
            AttackSpeed = soldierData.attackSpeed;
            AttackCoolDown = soldierData.attackCoolDown;
            AttackRange = soldierData.attackRange;
            ChangeMaxHp(soldierData.hp);
            Team = team;
            _damageInfo = new DamageInfo(soldierData.damage, team);
            attack = true;
            hpBar.ChangeSliderColor(Team);
            UnitManager.Instance.RegisterSoldier(this);
            SetCoolDown();
            EnterCooldown();
        }

        public void CoolDownStep(float deltaTime)
        {
            if (attackTimer < attackCoolDown)
            {
                attackTimer += deltaTime;
            }
        }

        public bool IsReadyAttack => attackTimer >= attackCoolDown;

        public void SetCoolDown()
        {
            attackCoolDown = AttackCoolDown;
            attackTimer = AttackCoolDown;
        }

        public void EnterCooldown()
        {
            attackTimer = 0;
        }

        public float MoveSpeed { get; set; }
        public float AttackRange { get; set; }
        private bool attack;

        public float AttackSpeed { get; set; }

        #endregion

        #region UnityMethods

        private void Update()
        {
            GetTarget();

            if (IsReadyAttack)
                Attack();
            else
                CoolDownStep(Time.deltaTime);
        }

        #endregion

        #region PublicMethods

        [ContextMenu("Attack")]
        public virtual void Attack()
        {
            if (Target != null)
            {
                EnterCooldown();
                Target.TakeDamage(_damageInfo);
            }
        }

        public virtual void GetTarget()
        {
            Target = UnitManager.Instance.ClosestEnemyUnit(transform.position, Team, AttackRange);
        }

        public void OnReturnPool()
        {
            UnitManager.Instance.UnRegisterSoldier(this);
        }

        public void OnPoolSpawn()
        {
            ResetHpController();
            SpawnEffect();
        }

        public override void OpenInformationPopup()
        {
            UIManager.Instance.OpenInformationPopup(soldierData, this);
        }

        #endregion
    }
}
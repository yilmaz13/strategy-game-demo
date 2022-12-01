using System.Collections;
using DG.Tweening;
using HpSystem;
using UnityEngine;
using UnityEngine.Events;

public abstract class HpController : MonoBehaviour, IDamageable
{
    #region ProtectedValue

    protected float maxHp;

    private bool IsDead
    {
        get => _isDead;
        set => _isDead = value;
    }

    private bool _isDead = false;

    private string particleToSpawn = "dust";

    #endregion

    #region PublicValue

    public HpBar hpBar;
    public Team Team { get; set; }
    public PoolObject _poolObject { get; set; }
    public float CurrentHp { get; protected set; }

    public float MaxHp
    {
        get => maxHp;
        protected set => maxHp = value;
    }

    #endregion

    #region UnityEvents

    [HideInInspector] public UnityEvent OnDeath = new UnityEvent();
    [HideInInspector] public UnityEvent OnTakeDamage = new UnityEvent();

    #endregion

    #region UnityMethods

    protected virtual void Awake()
    {
        ResetHpController();
        _poolObject = GetComponent<PoolObject>();
    }

    #endregion

    #region PrivateMethods

    private bool CheckDeath()
    {
        if (CurrentHp > 0) return false;
        Die();
        return true;
    }

    private void Die()
    {
        if (_isDead) return;
        CurrentHp = 0;
        _isDead = true;
        StartCoroutine(Death());
        OnDeath?.Invoke();
    }

    protected void ChangeMaxHp(float maxHp)
    {
        this.maxHp = maxHp;
        SetCurrentHp();
    }

    private void SetCurrentHp()
    {
        CurrentHp = maxHp;
    }

    private IEnumerator Death()
    {
        float deathAnimTime = 2.0f;
        ObjectPooler.Instance.Spawn(particleToSpawn, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
        gameObject.GetComponent<SpriteRenderer>().DOFade(0f, deathAnimTime);
        yield return new WaitForSeconds(deathAnimTime + 0.5f);
        _poolObject.GoToPool();
    }

    #endregion


    #region PublicMethods

    public void ResetHpController()
    {
        SetCurrentHp();
        _isDead = false;
        if (hpBar) hpBar.UpdateHpBar(1);
    }

    public virtual void OpenInformationPopup()
    {
    }

    public void SpawnEffect()
    {
        gameObject.GetComponent<SpriteRenderer>().DOFade(1f, 0.25f);
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (IsDead) return;

        CurrentHp -= damageInfo.Damage;
        OnTakeDamage?.Invoke();

        gameObject.GetComponent<SpriteRenderer>().DOFade(0f, 0.25f).OnComplete(() =>
        {
            gameObject.GetComponent<SpriteRenderer>().DOFade(1f, 0.25f);
        });

        CheckDeath();
        if (hpBar) hpBar.UpdateHpBar(CurrentHp / maxHp);
    }

    #endregion
}
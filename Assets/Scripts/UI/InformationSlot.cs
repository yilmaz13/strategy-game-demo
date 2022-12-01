using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationSlot : MonoBehaviour, IPoolable
{
    #region PublicValue

    public TextMeshProUGUI name;
    public TextMeshProUGUI curretHp;
    public TextMeshProUGUI hpMax;
    public TextMeshProUGUI damage;
    public GameObject damageTextParent;
    public GameObject hpMaxTextParent;
    public Image image;
    public UnityStringEvent OnSpawn;
    public Button Button;
    public string spawnName;
    public Action<string> soldierSpawnEvent;
    public HpController hpController;

    #endregion

    #region UnityMethods

    private void Start()
    {
        Button = GetComponent<Button>();
    }

    #endregion

    #region PublicMethods

    public void SetSlot(HpData hpData, HpController hpController)
    {
        name.text = hpData.name;
        curretHp.gameObject.SetActive(true);
        curretHp.text = hpController.CurrentHp.ToString() + "/" + hpController.MaxHp.ToString();
        hpMaxTextParent.SetActive(false);
        image.sprite = hpData.icon;
        damageTextParent.SetActive(false);
        hpController.OnTakeDamage.AddListener(UpdateSlot);
        this.hpController = hpController;
    }

    public void UpdateSlot()
    {
        curretHp.text = hpController.CurrentHp.ToString();
        hpMax.text = hpController.MaxHp.ToString();
        curretHp.text = hpController.CurrentHp.ToString() + "/" + hpController.MaxHp.ToString();
    }

    public void SetSlot(HpData hpData, Action<string> soldierSpawnEvent)
    {
        name.text = hpData.name;
        curretHp.text = hpData.hp.ToString(CultureInfo.InvariantCulture);
        spawnName = hpData.spawnName;
        image.sprite = hpData.icon;
        damageTextParent.SetActive(false);
        hpMaxTextParent.SetActive(true);
        this.soldierSpawnEvent = soldierSpawnEvent;
        Invoke(nameof(SpawnSoldier), 0.5f);
    }

    public void SetSlot(SoldierData hpData, HpController hpController)
    {
        name.text = hpData.name;
        hpMaxTextParent.SetActive(false);
        curretHp.gameObject.SetActive(true);
        curretHp.text = hpController.CurrentHp.ToString() + "/" + hpController.MaxHp.ToString();
        hpMax.text = hpController.MaxHp.ToString();
        image.sprite = hpData.icon;
        damage.gameObject.SetActive(true);
        damageTextParent.SetActive(true);
        damage.text = hpData.damage.ToString(CultureInfo.InvariantCulture);
    }

    public void OnReturnPool()
    {
        transform.localScale = Vector3.one;
        Button.onClick.RemoveAllListeners();
        OnSpawn.RemoveAllListeners();
        hpController = null;
    }

    public void OnPoolSpawn()
    {
        transform.localScale = Vector3.one;
    }

    [ContextMenu("SpawnSoldier")]
    public void SpawnSoldier()
    {
        Button.onClick.AddListener(() => soldierSpawnEvent.Invoke(spawnName));
    }

    #endregion
}
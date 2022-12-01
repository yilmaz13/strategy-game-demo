using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionPanel : MonoBehaviour
{
    public ScrollRect ScrollRect;
    public Transform Content;

    public List<HpData> HpDatas;

    public Transform ProductionTransform;

    // Start is called before the first frame update
    void Start()
    {
        HpDatas = UnitManager.Instance.GetAllBuildData();
        SetPanel();
    }

    #region PublicMethods
    
    public void SetPanel(HpData hpData)
    {
        var slot = ObjectPooler.Instance.Spawn("ProductionSlot", Vector3.zero, new Quaternion(), ProductionTransform);
        slot.GetComponent<ProductionSlot>().SetSlot(hpData, GameManager.Instance.SpawnGhost);
    }

    // Update is called once per frame
    [ContextMenu("Set Hp Data")]
    public void SetHpdata()
    {
        HpDatas = UnitManager.Instance.GetAllBuildData();
    }

    [ContextMenu("Set Panel")]
    public void SetPanel()
    {
        foreach (var hpData in HpDatas)
        {
            SetPanel(hpData);
        }
    }
    
    #endregion
}
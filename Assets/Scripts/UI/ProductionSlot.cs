using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProductionSlot : MonoBehaviour, IPoolable, IPointerClickHandler
{
    #region Value
    
    [SerializeField] private HpData hpData;
    [SerializeField] public Image image;
    public Action<string> productionSpawnAction;
    
    #endregion

    #region PublicMethods
    
    public void SetSlot(HpData hpData, Action<string> productionSpawnAction)
    {
        this.hpData = hpData;
        image.sprite = hpData.icon;
        this.productionSpawnAction = productionSpawnAction;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SpawnProcution();
    }

    public void OnReturnPool()
    {
        productionSpawnAction = null;
    }

    public void OnPoolSpawn()
    {
    }

    public void SpawnProcution()
    {
        productionSpawnAction?.Invoke(hpData.spawnName);
    }
    
    #endregion
}
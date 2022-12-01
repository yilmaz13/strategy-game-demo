using System;
using HpSystem;
using TMPro;
using UnityEngine;

public class InformationPanel : MonoBehaviour
{
    #region PublicValue
    
    public TextMeshProUGUI infoBuildText;
    public Transform infoBuildTransform;

    public TextMeshProUGUI infoProductionText;
    public Transform infoProductionTransform;
    
    #endregion
    
    #region PublicMethods
    public void SetPanel(HpData hpData, Action<string> soldierSpawnEvent, HpController hpController)
    {
        gameObject.SetActive(true);
        Cleancontainer(infoBuildTransform);
        Cleancontainer(infoProductionTransform);

        var slot = ObjectPooler.Instance.Spawn("ItemIcon", Vector3.zero, new Quaternion(), infoBuildTransform)
            .GetComponent<InformationSlot>();
        slot.SetSlot(hpData, hpController);

        infoBuildText.text = hpData.name;
        
        infoProductionText.gameObject.SetActive(false);
        
        if (hpController.Team == Team.Player)
        {
            infoProductionText.gameObject.SetActive(true);

            foreach (var t in hpData.productions)
            {
                var slotChild = ObjectPooler.Instance.Spawn("ItemIcon", Vector3.zero, new Quaternion(),
                    infoProductionTransform).GetComponent<InformationSlot>();
                var sData = UnitManager.Instance.GetSoldierData(t);
                slotChild.SetSlot(sData, soldierSpawnEvent);
            }
        }
    }

    public void SetPanel(SoldierData soldierData, HpController hpController)
    {
        gameObject.SetActive(true);
        Cleancontainer(infoBuildTransform);
        Cleancontainer(infoProductionTransform);

        infoBuildText.text = soldierData.name;
        infoProductionText.gameObject.SetActive(false);

        var slot = ObjectPooler.Instance.Spawn("ItemIcon", Vector3.zero, new Quaternion(), infoBuildTransform);
        slot.GetComponent<InformationSlot>().SetSlot(soldierData, hpController);
    }

    public void CloseInformationPopup()
    {
        gameObject.SetActive(false);
    }
    
    #endregion
    
    #region PrivateMethods

    private void Cleancontainer(Transform transform)
    {
        if (transform.childCount > 0)
            for (var i = transform.childCount - 1; i >= 0; i--)
                if (transform.GetChild(i).TryGetComponent<PoolObject>(out PoolObject poolObject))
                    poolObject.GoToPool();
    }
    
    #endregion
}
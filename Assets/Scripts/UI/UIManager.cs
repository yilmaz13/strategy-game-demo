using System;
using System.Collections;
using System.Collections.Generic;
using HpSystem;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance = null;
    public static UIManager Instance => _instance;
    public InformationPanel InfoPanel;

    private void Awake()
    {
        //singleton pattern
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }

        _instance = this;
    }

    public void OpenInformationPopup(SoldierData soldierData, HpController hpController)
    {
        InfoPanel.SetPanel(soldierData, hpController);
    }

    public void CloseInformationPopup()
    {
        InfoPanel.CloseInformationPopup();
    }

    public void OpenInformationPopup(HpData soldierData, Action<string> soldierSpawnEvent,  HpController hpController)
    {
        InfoPanel.SetPanel(soldierData, soldierSpawnEvent, hpController);
    }
}

[System.Serializable]
public class UnityStringEvent : UnityEvent<string>
{
}
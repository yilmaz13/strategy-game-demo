using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "HpData", menuName = "ScriptableObjects/HpData", order = 3)]
[System.Serializable]
public class HpData
{
    [SerializeField] public string name;
    [SerializeField] public string spawnName;
    [SerializeField] public float hp;
    [SerializeField] public Sprite icon;
    [SerializeField] public Sprite sprite;
    [SerializeField] public List<string> productions;
    [SerializeField] public List<Sprite> productionsImage;
    [SerializeField] public float height;
    [SerializeField] public float width;
}    
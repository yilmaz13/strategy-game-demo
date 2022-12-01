using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationUI : ScriptableObject
{
   public TextMeshPro name;
   public TextMeshPro hp;
   public TextMeshPro damage;
   public Image image;
   public List<Image> productionImages;
   public List<string> productionName;
}

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HpSystem;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    #region ProtectedValue

    [SerializeField] protected SpriteRenderer barImage;

    #endregion

    #region PublicMethods

    public void UpdateHpBar(float percentage)
    {
        barImage.gameObject.transform.DOScaleX(percentage, 0.2f);
    }

    public void ChangeSliderColor(Team team)
    {
        if (team == Team.Enemy)
        {
            barImage.color = Color.red;
        }
        else if (team == Team.Player)
        {
            barImage.color = Color.blue;
        }
    }

    #endregion
}
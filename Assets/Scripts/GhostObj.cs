using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostObj : MonoBehaviour
{
    #region PublicValue

    public bool AvailableZone { get; set; }
    public SpriteRenderer Sprite;
    public BoxCollider2D BoxCollider2D;
    [SerializeField] public float height;
    [SerializeField] public float width;

    #endregion

    #region UnityMethods

    private void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
    }

    #endregion

    #region PublicMethods

    public void Set(float height, float width, Sprite sprite)
    {
        gameObject.SetActive(true);
        this.height = height;
        this.width = width;
        Sprite.sprite = sprite;
        BoxCollider2D.size = new Vector2(height / 128, width / 128);
    }

    public void AvailableGrid(bool available)
    {
        AvailableZone = available;
        if (!available)
            Sprite.color = Color.red;
        else
            Sprite.color = Color.white;
    }

    #endregion
}
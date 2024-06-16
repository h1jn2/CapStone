using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemUi : MonoBehaviour
{
    public static ItemUi instance = null;
    public TMP_Text ItemCount;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
}

    public void UpdateCount()
    {
        ItemCount.text = (5-GameManager.instance.ItemCnt).ToString();
    }
}

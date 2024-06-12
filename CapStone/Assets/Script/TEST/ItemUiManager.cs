using UnityEngine;
using TMPro;

public class ItemUiManager : MonoBehaviour
{
    public TMP_Text itemCounterTMP; // TextMeshPro �ؽ�Ʈ�� ����
    private int itemCount = 0;

    void Start()
    {
        UpdateItemCounter();
    }

    public void AddItem()
    {
        itemCount++;
        UpdateItemCounter();
    }

    void UpdateItemCounter()
    {
        if (itemCounterTMP != null)
        {
            itemCounterTMP.text = itemCount.ToString();
        }
        else
        {
            Debug.LogError("Item Counter TMP is not assigned in the Inspector");
        }
    }
}

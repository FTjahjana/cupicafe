using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[System.Serializable]
public class InventoryItem
{
    public string name;
    public Button slotButtonComponent;
    public GameObject attachedGameObject;
}

public class Inventory : MonoBehaviour
{
    public Animator animator;
    public bool inventoryOpen = false;

    public List<InventoryItem> inventoryItems;
    public InventoryItem equippedItem;

    void Start()
    {
        animator = GetComponent<Animator>();

        equippedItem = inventoryItems[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectButton(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectButton(2);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SelectButton(3);
    }

    public void ToggleOpenClose()
    {
        if (inventoryOpen) Close(); else Open();
    }

    void Open()
    {
        animator.SetTrigger("Open");
        inventoryOpen = true;

        foreach (InventoryItem item in inventoryItems)
        {   if (item.slotButtonComponent != null)
            item.slotButtonComponent.interactable = true;
        }
    }
    void Close()
    {
        animator.SetTrigger("Close");
        inventoryOpen = false;

        foreach (InventoryItem item in inventoryItems)
        {   if (item.slotButtonComponent != null)
            item.slotButtonComponent.interactable = false;
        }
    }

    void SelectButton(int index)
    {
        if (index > 0 && index <= inventoryItems.Count)
        {
            InventoryItem item = inventoryItems[index - 1];

            item.slotButtonComponent.Select();
            item.slotButtonComponent.onClick.Invoke();

            equippedItem = item;
        }
    }
    
    public void DebugLog(string log)
    {
        Debug.Log(log);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; private set; }

    private InventorySlot SourceSlot { get; set; }
    [SerializeField] private InventorySlot dragSlot;

    private GraphicRaycaster raycaster;

    [SerializeField] private ItemTableManager itemTableManager;
    [SerializeField] private Inventory gemTab;
    [SerializeField] private Inventory inventoryTab;
    [SerializeField] private Inventory equipmentTab;

    private void Awake()
    {
        Instance = this;
        raycaster = GetComponent<GraphicRaycaster>();
    }

    private void Start()
    {
        // inventoryTab.Initialize(itemTableManager.GetItemTable());
        inventoryTab.Initialize(null);
        equipmentTab.Initialize(null);
        gemTab.Initialize(null);
    }

    public void StartDrag(InventorySlot source)
    {
        // source를 캐싱을 해준다 
        SourceSlot = source;

        var rTransform = dragSlot.gameObject.GetComponent<RectTransform>();
        rTransform.sizeDelta = SourceSlot.Item.DragSize;

        dragSlot.SetSlot(SourceSlot.Item);
    }

    //드래그 중일때 드래그 중인 슬롯의 위치를 갱신하기 위한 함수 
    public void UpdatePosition(Vector2 position)
    {
        dragSlot.transform.position = position;
    }

    //Drag가 끝날때의 이벤트 데이터를 사용해서 어느 인벤토리인지, 어느슬롯인지 판별
    public void EndDrag(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        foreach (var result in results)
        {
            InventorySlot targetSlot = result.gameObject.GetComponent<InventorySlot>();

            if (!IsValidTarget(targetSlot)) continue;

            Inventory from = FindInventory(SourceSlot);
            Inventory to = FindInventory(targetSlot);

            if (from.Equals(to))
            {
                HandleSameInventoryMove(from, targetSlot);
            }
            else
            {
                HandleDifferentInventoryMove(from, to, targetSlot);
            }
        }

        dragSlot.ClearSlot();
    }

    private bool IsValidTarget(InventorySlot targetSlot)
    {
        return targetSlot != null && targetSlot != SourceSlot && targetSlot != dragSlot;
    }

    private void HandleSameInventoryMove(Inventory from, InventorySlot targetSlot)
    {
        if (from.Equals(gemTab))
        {
            MoveGemInSameTab(targetSlot);
        }
        else if (from.Equals(equipmentTab))
        {
            
        }
        else
        {
            SwapItem(SourceSlot, targetSlot);
        }
    }

    private void MoveGemInSameTab(InventorySlot targetSlot)
    {
        if (!SourceSlot.Item.ItemData.ItemType.Equals(targetSlot.gameObject.name))
        {
            Debug.Log("Item Type이 맞지 않습니다!");
            return;
        }

        var myGemSet = SourceSlot.GetComponentInParent<GemSet>();
        var targetGemSet = targetSlot.GetComponentInParent<GemSet>();

        if (myGemSet.MoveGem(dragSlot.Item, targetGemSet))
        {
            SwapItem(SourceSlot, targetSlot);
        }
    }

    private void HandleDifferentInventoryMove(Inventory from, Inventory to, InventorySlot targetSlot)
    {
        if (from.Equals(inventoryTab) && to.Equals(gemTab)) // 젬 장착
        {
            TryEquipGem(targetSlot);
        }
        else if (from.Equals(gemTab) && to.Equals(inventoryTab)) //젬 해제
        {
            TryUnequipGem(targetSlot);
        }
        else if (from.Equals(inventoryTab) && to.Equals(equipmentTab)) // 장비 장착
        {
            TryEquipEquipment(targetSlot);
        }
        else if (from.Equals(equipmentTab) && to.Equals(inventoryTab)) // 장비 해제
        {
            TryUnequipEquipment(targetSlot);
        }
    }

    private void TryEquipGem(InventorySlot targetSlot)
    {
        if (targetSlot.Item != null)
        {
            Debug.Log("이미 해당 위치에 스킬 젬이 존재합니다!");
            return;
        }

        if (!SourceSlot.Item.ItemData.ItemType.Equals(targetSlot.gameObject.name))
        {
            Debug.Log("Item Type이 맞지 않습니다!");
            return;
        }

        var gemSet = targetSlot.GetComponentInParent<GemSet>();

        if (gemSet.AddGem(dragSlot.Item))
        {
            SwapItem(SourceSlot, targetSlot);
        }
    }

    private void TryUnequipGem(InventorySlot targetSlot)
    {
        if (targetSlot.Item != null)
        {
            Debug.Log("이미 해당 위치에 아이템이 존재합니다!");
            return;
        }

        var gemSet = SourceSlot.GetComponentInParent<GemSet>();

        if (gemSet.RemoveGem(dragSlot.Item))
        {
            SwapItem(SourceSlot, targetSlot);
        }
    }

    private void TryEquipEquipment(InventorySlot targetSlot)
    {
        if (targetSlot.Item != null)
        {
            Debug.Log("이미 해당 위치에 장비를 착용중입니다!"); 
            return;
        }

        if (!SourceSlot.Item.ItemData.ItemType.Equals(targetSlot.gameObject.name))
        {
            Debug.Log("Item Type이 맞지 않습니다!");
            return;
        }

        EquipmentManager.Instance.EquipEquipment(SourceSlot.Item);
        
        SwapItem(SourceSlot, targetSlot);
    }

    private void TryUnequipEquipment(InventorySlot targetSlot)
    {
        if (targetSlot.Item != null)
        {
            Debug.Log("이미 해당 위치에 아이템이 존재합니다!");
            return;
        }
        
        Debug.Log("장비를 해제합니다.");
        EquipmentManager.Instance.UnEquipEquipment(SourceSlot.Item);
        SwapItem(SourceSlot, targetSlot);
    }
    
    private void SwapItem(InventorySlot a, InventorySlot b)
    {
        var temp = a.Item;
        a.SetSlot(b.Item);
        b.SetSlot(temp);
    }

    private Inventory FindInventory(InventorySlot slot)
    {
        if (inventoryTab.IsIn(slot))
        {
            return inventoryTab;
        }
        else if (gemTab.IsIn(slot))
        {
            return gemTab;
        }
        else if (equipmentTab.IsIn(slot))
        {
            return equipmentTab;
        }

        return null;
    }
}
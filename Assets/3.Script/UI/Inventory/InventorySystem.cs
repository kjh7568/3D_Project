using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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
    [SerializeField] private EventSystem eventSystem;

    [SerializeField] private ItemTableManager itemTableManager;
    [SerializeField] private Inventory gemTab;
    [SerializeField] private Inventory inventoryTab;
    [SerializeField] private Inventory equipmentTab;
    [SerializeField] private Inventory shopTab;

    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform floatingUIPosition;
    [SerializeField] private FloatingInfomationUI floatingUI;

    [SerializeField] private GameObject dropedItem;
    [SerializeField] private Transform dropedItemParent;
    [SerializeField] private GameObject dropedItemUI;
    [SerializeField] private RectTransform dropedItemUIParent;

    private void Awake()
    {
        Instance = this;

        raycaster = GetComponent<GraphicRaycaster>();
    }

    private void Start()
    {
        inventoryTab.Initialize(itemTableManager.PlayerTable);
        equipmentTab.Initialize(null);
        gemTab.Initialize(null);
        if (shopTab != null) shopTab.Initialize(null);
    }

    private void Update()
    {
        UpdateFloatingInfoUI();
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
            Inventory from;
            Inventory to;

            InventorySlot targetSlot = result.gameObject.GetComponent<InventorySlot>();

            if (!IsValidTarget(targetSlot) && !result.gameObject.name.Equals("CloseImage")) continue;

            if (targetSlot == null && SourceSlot.Item != null)
            {
                DropItem(SourceSlot.Item);
                break;
            }
            else if (SourceSlot.gameObject.name.Equals("DropedItemUI(Clone)"))
            {
                PickUpItem(targetSlot);
                break;
            }

            from = FindInventory(SourceSlot);
            to = FindInventory(targetSlot);

            if (from.Equals(to))
            {
                HandleSameInventoryMove(from, targetSlot);
            }
            else
            {
                HandleDifferentInventoryMove(from, to, targetSlot);
            }

            break;
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
        else if (from.Equals(shopTab) && to.Equals(inventoryTab)) // 아이템 구매
        {
            TrySaleItem(targetSlot);
        }
        else if (from.Equals(inventoryTab) && to.Equals(shopTab)) // 아이템 판매
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

        EquipmentManager.Instance.UnEquipEquipment(SourceSlot.Item);
        SwapItem(SourceSlot, targetSlot);
    }

    private void TrySaleItem(InventorySlot targetSlot)
    {
        int sellPrice = CalculatePrice();

        if (Player.LocalPlayer.gold >= sellPrice)
        {
            Player.LocalPlayer.gold -= sellPrice;
            FindObjectOfType<Shopper>().SetGoldText();
            SwapItem(SourceSlot, targetSlot);
        }
        else
        {
            Debug.Log($"돈이 부족합니다. {sellPrice}골드 필요");
        }
    }

    private void TryBuyItem(InventorySlot targetSlot)
    {
    }

    private int CalculatePrice()
    {
        if (SourceSlot.Item == null) return 0;

        int price = 0;

        if (SourceSlot.Item is IEquipment equipment)
        {
            switch (equipment.Rarity)
            {
                case 2:
                    price += 300;
                    break;
                case 1:
                    price += 200;
                    break;
                case 0:
                    price += 100;
                    break;
            }

            price += equipment.DescriptionDic.Count * 50;

            return price;
        }
        else
        {
            if (SourceSlot.Item.ItemData.ItemType.Equals("MainGem"))
            {
                return 200;
            }
            else if (SourceSlot.Item.ItemData.ItemType.Equals("SupportGem"))
            {
                return 400;
            }
        }

        return 0;
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
        else if (shopTab.IsIn(slot))
        {
            return shopTab;
        }

        Debug.Log("IsIn Error 일지도?");
        return null;
    }

    private void UpdateFloatingInfoUI()
    {
        if (!inventoryTab.gameObject.activeSelf) return;

        UpdateFloatingUIPosition();
        HandleSlotHover();
    }

    private void UpdateFloatingUIPosition()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        Vector2 offset = new Vector2(-470f, 320f);

        if (pointerData.position.x + 1000f < 1920f)
        {
            offset.x = -offset.x;
        }
        else
        {
            offset.x = -470f;
        }

        if (pointerData.position.y + 600f > 1080f)
        {
            offset.y = -offset.y;
        }
        else
        {
            offset.y = 320f;
        }

        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            floatingUIPosition.parent as RectTransform,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out anchoredPos
        );

        floatingUIPosition.anchoredPosition = anchoredPos + offset;
    }

    private void HandleSlotHover()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            GameObject hitUI = result.gameObject;

            if (hitUI.name.Equals("CloseImage"))
            {
                floatingUI.gameObject.SetActive(false);
            }

            if (hitUI.TryGetComponent(out InventorySlot slot))
            {
                UpdateFloatingInfoVisibility(slot);
                break; // 가장 먼저 맞은 슬롯 하나만 처리
            }
        }
    }

    private void UpdateFloatingInfoVisibility(InventorySlot slot)
    {
        if (slot.Item != null)
        {
            if (!floatingUI.gameObject.activeSelf)
                floatingUI.gameObject.SetActive(true);

            floatingUI.SetItemBaseInfo(slot.Item);
        }
        else
        {
            if (floatingUI.gameObject.activeSelf)
                floatingUI.gameObject.SetActive(false);
        }
    }

    private void DropItem(Item item)
    {
        var itemPos = Player.LocalPlayer.transform.position;

        var dropItem = Instantiate(dropedItem, itemPos + new Vector3(0, 0.5f, 0), Quaternion.identity,
            dropedItemParent);
        var dropItemUI = Instantiate(dropedItemUI, dropedItemUIParent);

        var dropItemSlot = dropItemUI.GetComponent<InventorySlot>();
        var dropItemRect = dropItemUI.GetComponent<RectTransform>();
        var dropItemText = dropItemUI.GetComponentInChildren<Text>();

        dropItemSlot.SetSlot(SourceSlot.Item);
        DropItemUI.Instance.SetUIText(dropItemText, item);
        DropItemUI.Instance.RegisterDrop(dropItem, dropItemRect); // ✅ 추가됨

        SourceSlot.SetSlot(null);
    }

    private void PickUpItem(InventorySlot targetSlot)
    {
        targetSlot.SetSlot(SourceSlot.Item);

        var dropItemRect = SourceSlot.GetComponent<RectTransform>(); // ✅ UI 기준으로 찾음
        DropItemUI.Instance.UnregisterDrop(dropItemRect); // ✅ 등록 해제 및 드롭 오브젝트 제거

        Destroy(SourceSlot.gameObject);
    }
}
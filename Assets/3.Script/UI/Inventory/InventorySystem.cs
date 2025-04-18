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

    private void Awake()
    {
        Instance = this;
        raycaster = GetComponent<GraphicRaycaster>();
    }

    private void Start()
    {
        itemTableManager.LoadItemTable();
        inventoryTab.Initialize(itemTableManager.GetItemTable());
        gemTab.Initialize(null);
    }

    public void StartDrag(InventorySlot source)
    {
        // source를 캐싱을 해준다 
        SourceSlot = source;
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

        for (int i = 0; i < results.Count; i++)
        {
            InventorySlot targetSlot = results[i].gameObject.GetComponent<InventorySlot>();
            if (targetSlot != null && targetSlot != SourceSlot && targetSlot != dragSlot)
            {
                Inventory from = FindInventory(SourceSlot);
                Inventory to = FindInventory(targetSlot);


                if (from.Equals(to)) //아이템 위치만 변경
                {
                    if (from.Equals(gemTab)) //젬 이동
                    {
                        //소켓 타입 계산은 여기서
                        if (SourceSlot.Item.ItemData.ItemType.Equals(targetSlot.gameObject.name))
                        {
                            var myGemSet = SourceSlot.gameObject.GetComponentInParent<GemSet>();
                            var targetGemSet = targetSlot.gameObject.GetComponentInParent<GemSet>();

                            if (myGemSet.MoveGem(dragSlot.Item, targetGemSet))
                            {
                                SwapItem(SourceSlot, targetSlot);
                            }
                        }
                        else
                        {
                            Debug.Log("Item Type이 맞지 않습니다!");
                        }
                    }
                    else //인벤 아이템 이동
                    {
                        SwapItem(SourceSlot, targetSlot);
                    }
                }
                else //아이템 장착과 해제
                {
                    if (from.Equals(inventoryTab) && to.Equals(gemTab)) // 인벤 탭에서 젬 탭으로 -> 장착
                    {
                        if (targetSlot.Item == null)
                        {
                            if (SourceSlot.Item.ItemData.ItemType.Equals(targetSlot.gameObject.name))
                            {
                                var gemSet = targetSlot.gameObject.GetComponentInParent<GemSet>();

                                if (gemSet.AddGem(dragSlot.Item))
                                {
                                    SwapItem(SourceSlot, targetSlot);
                                }
                            }
                            else
                            {
                                Debug.Log("Item Type이 맞지 않습니다!");
                            }
                        }
                        else
                        {
                            Debug.Log("이미 해당 위치에 이미 스킬 젬이 존재합니다!");
                        }
                    }
                    else // 젬 탭에서 인벤 탭으로 -> 해제
                    {
                        if (targetSlot.Item == null)
                        {
                            var gemSet = SourceSlot.gameObject.GetComponentInParent<GemSet>();

                            if (gemSet.RemoveGem(dragSlot.Item))
                            {
                                SwapItem(SourceSlot, targetSlot);
                            }
                        }
                        else
                        {
                            Debug.Log("이미 해당 위치에 아이템이 존재합니다!");
                        }
                    }
                }
            }
        }

        dragSlot.ClearSlot();
    }

    private void SwapItem(InventorySlot a, InventorySlot b)
    {
        var temp = a.Item;
        a.SetSlot(b.Item);
        b.SetSlot(temp);
    }

    private Inventory FindInventory(InventorySlot slot)
    {
        return gemTab.IsIn(slot) ? gemTab : inventoryTab;
    }
}
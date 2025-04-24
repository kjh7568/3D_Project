using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItemUI : MonoBehaviour
{
    public static DropItemUI Instance;

    [SerializeField] private Camera mainCam;

    public Dictionary<GameObject, Transform> dropItems = new(); 
    public List<RectTransform> dropItemsNameUIs = new(); 
    public Dictionary<RectTransform, GameObject> uiToDropObjectMap = new();

    private Vector3 UIOffset = new Vector3(0, 2f, 0);

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        int i = 0;
        foreach (var kv in dropItems)
        {
            Vector3 screenPos = mainCam.WorldToScreenPoint(kv.Value.position + UIOffset);
            dropItemsNameUIs[i].position = screenPos;
            i++;
        }
    }

    public void SetUIText(Text uiText, Item item)
    {
        if (item is IEquipment equip)
        {
            if (equip.Rarity == 0) uiText.color = Color.white;
            else if (equip.Rarity == 1) uiText.color = Color.blue;
            else if (equip.Rarity == 2) uiText.color = Color.yellow;

            uiText.text = $"{item.ItemData.Name}";
        }
    }

    public void RegisterDrop(GameObject dropObj, RectTransform dropUI) 
    {
        dropItems.Add(dropObj, dropObj.transform);
        dropItemsNameUIs.Add(dropUI);
        uiToDropObjectMap[dropUI] = dropObj;
    }

    public void UnregisterDrop(RectTransform dropUI) 
    {
        if (uiToDropObjectMap.TryGetValue(dropUI, out var dropObj))
        {
            dropItems.Remove(dropObj);
            dropItemsNameUIs.Remove(dropUI);
            uiToDropObjectMap.Remove(dropUI);

            Destroy(dropObj); 
        }
    }
}
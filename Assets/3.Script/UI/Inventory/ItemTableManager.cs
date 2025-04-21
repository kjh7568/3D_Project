using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using UnityEngine;

public class ItemTableManager : MonoBehaviour
{
    public static ItemTableManager instance;

    [SerializeField] private TableSAO spriteTable;

    private List<Item> itemTable = new List<Item>();

    public void LoadItemTable()
    {
        using (StreamReader sr = new StreamReader(Path.Combine(Application.streamingAssetsPath, "ItemTable.csv")))
        using (CsvReader cr = new CsvReader(sr, CultureInfo.CurrentCulture))
        {
            var itemDatas = cr.GetRecords<ItemData>().ToList();
            itemTable.Clear();

            foreach (var data in itemDatas)
            {
                Item item = new Item
                {
                    ItemData = data,
                    Sprite = spriteTable.GetItemSprite(data.Key).sprite,
                    DragSize = spriteTable.GetItemSprite(data.Key).dragSize
                };

                if (item.ItemData.Key < 33)
                {
                    
                }
                else if(item.ItemData.Key < 66)
                {
                    
                }
                else if (item.ItemData.Key < 100)
                {
                    
                }

                itemTable.Add(item);
            }
        }
    }

    public List<Item> GetItemTable()
    {
        return itemTable;
    }
}
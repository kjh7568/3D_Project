using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using UnityEngine;

public class ItemTableManager : MonoBehaviour
{
    [SerializeField] private TableSAO spriteTable;

    public List<Item> ItemTable { get; private set; } = new List<Item>();

    private void Start()
    {
        LoadItemTable();
    }

    public void LoadItemTable()
    {
        using (StreamReader sr = new StreamReader(Path.Combine(Application.streamingAssetsPath, "ItemTable.csv")))
        using (CsvReader cr = new CsvReader(sr, CultureInfo.CurrentCulture))
        {
            var itemDatas = cr.GetRecords<ItemData>().ToList();
            ItemTable.Clear();

            foreach (var data in itemDatas)
            {
                Item item = new Item
                {
                    ItemData = data,
                    Sprite = spriteTable.GetItemSprite(data.Key).sprite,
                    DragSize = spriteTable.GetItemSprite(data.Key).dragSize
                };

                ItemTable.Add(item);
            }
        }
    }

    public List<Item> GetItemTable()
    {
        return ItemTable;
    }
}
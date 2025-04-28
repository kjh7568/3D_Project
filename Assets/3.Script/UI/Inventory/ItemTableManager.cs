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
    public List<Item> PlayerTable { get; private set; } = new List<Item>();

    private void Awake()
    {
        LoadItemTables();
    }

    private void LoadItemTables()
    {
        PlayerTable.Clear();
        ItemTable.Clear();

        LoadCsvToList("PlayerInventory.csv", PlayerTable);
        LoadCsvToList("ItemTable.csv", ItemTable);
    }

    private void LoadCsvToList(string fileName, List<Item> targetList)
    {
        using (StreamReader sr = new StreamReader(Path.Combine(Application.streamingAssetsPath, fileName)))
        using (CsvReader cr = new CsvReader(sr, CultureInfo.CurrentCulture))
        {
            var itemDatas = cr.GetRecords<ItemData>().ToList();

            foreach (var data in itemDatas)
            {
                Item item = new Item
                {
                    ItemData = data,
                    Sprite = spriteTable.GetItemSprite(data.Key).sprite,
                    DragSize = spriteTable.GetItemSprite(data.Key).dragSize
                };

                targetList.Add(item);
            }
        }
    }
}
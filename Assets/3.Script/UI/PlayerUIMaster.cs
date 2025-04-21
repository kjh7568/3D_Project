using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIMaster : MonoBehaviour
{
    [SerializeField] private GameObject dragSlot;
    [SerializeField] private GameObject inventoryTab;
    [SerializeField] private GameObject gemTab;

    private IEnumerator Start()
    {
        yield return null;
        
        inventoryTab.SetActive(false);
        gemTab.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryTab.activeSelf)
            {
                inventoryTab.SetActive(false);
            }
            else
            {
                inventoryTab.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            if (gemTab.activeSelf)
            {
                dragSlot.SetActive(false);
                inventoryTab.SetActive(false);
                gemTab.SetActive(false);
            }
            else
            {
                dragSlot.SetActive(true);
                inventoryTab.SetActive(true);
                gemTab.SetActive(true);
            }
        }
    }
    
    
}

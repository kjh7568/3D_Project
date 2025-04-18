using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIMaster : MonoBehaviour
{
    [SerializeField] private Image inventoryTab;
    [SerializeField] private Image gemTab;

    private IEnumerator Start()
    {
        yield return null;
        
        inventoryTab.gameObject.SetActive(false);
        gemTab.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryTab.gameObject.activeSelf)
            {
                inventoryTab.gameObject.SetActive(false);
            }
            else
            {
                inventoryTab.gameObject.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            if (gemTab.gameObject.activeSelf)
            {
                inventoryTab.gameObject.SetActive(false);
                gemTab.gameObject.SetActive(false);
            }
            else
            {
                inventoryTab.gameObject.SetActive(true);
                gemTab.gameObject.SetActive(true);
            }
        }
    }
    
    
}

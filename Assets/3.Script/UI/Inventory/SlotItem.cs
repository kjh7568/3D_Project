using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    [SerializeField] private Image myImage;
    [SerializeField] private Image image;

    public void Set(Item item)
    {
        if (item == null)
        {
            SetActive(false);
        }
        else
        {
            SetActive(true);
            image.sprite = item.Sprite;
        }
    }

    public void Clear()
    {
        SetActive(false);
    }

    public void SetActive(bool active)
    {
        myImage.enabled = active;
        image.enabled = active;
    }
}
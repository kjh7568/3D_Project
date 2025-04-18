using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSet : MonoBehaviour
{
    public bool isInMainGem = false;
    public List<bool> isInSupportGem = new List<bool>();

    private bool CheckRequiredAttributes(Item item)
    {
        var currentStat = Player.LocalPlayer.Stat;

        if (currentStat.Strength < item.ItemData.RequiredStrength)
        {
            Debug.Log("Not enough strength");
            return false;
        }
        else if (currentStat.Dexterity < item.ItemData.RequiredDexterity)
        {
            Debug.Log("Not enough dexterity");
            return false;
        }
        else if (currentStat.Intelligence < item.ItemData.RequiredIntelligence)
        {
            Debug.Log("Not enough intelligence");
            return false;
        }
        else if (currentStat.Level < item.ItemData.RequiredLevel)
        {
            Debug.Log("Not enough level");
            return false;
        }

        return true;
    }

    public bool AddGem(Item item)
    {
        //능력치 안되면 컷
        if (!CheckRequiredAttributes(item))
        {
            return false;
        }

        if (item.ItemData.ItemType.Equals("MainGem"))
        {
            isInMainGem = true;
        }
        else if (item.ItemData.ItemType.Equals("SupportGem"))
        {
            if (isInMainGem)
            {
                isInSupportGem.Add(true);
            }
            else
            {
                Debug.Log("MainGem 없이는 SupportGem을 착용할 수 없습니다.");
                return false;
            }
        }

        return true;
    }

    public bool RemoveGem(Item item)
    {
        if (item.ItemData.ItemType.Equals("MainGem"))
        {
            if (isInSupportGem.Count > 0)
            {
                Debug.Log("SupportGem이 착용되어 있다면, MainGem을 해제할 수 없습니다.");
                return false;
            }
            else
            {
                isInMainGem = false;
            }
        }
        else if (item.ItemData.ItemType.Equals("SupportGem"))
        {
            isInSupportGem.RemoveAt(0);
        }

        return true;
    }

    public bool MoveGem(Item item, GemSet targetSet)
    {
        if (item.ItemData.ItemType.Equals("MainGem"))
        {
            if (isInSupportGem.Count > 0)
            {
                Debug.Log("SupportGem이 착용되어 있어, MainGem을 옮길 수 없습니다.");
                return false;
            }
            else
            {
                if (targetSet.isInMainGem)
                {
                    if (targetSet.isInSupportGem.Count > 0)
                    {
                        Debug.Log("대상 MainGem에 SupportGem이 착용되어 있어, MainGem을 해제할 수 없습니다.");
                        return false;
                    }
                }
                else
                {
                    targetSet.isInMainGem = true;
                    isInMainGem = false;
                }
            }
        }
        else if (item.ItemData.ItemType.Equals("SupportGem"))
        {
            Debug.Log("SupportGem은 이동할 수 없습니다.");
            return false;
        }

        return true;
    }
}
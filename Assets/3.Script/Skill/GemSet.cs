using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GemSet : MonoBehaviour
{
    public bool isInMainGem = false;
    public List<bool> isInSupportGem = new List<bool>();

    [SerializeField] private int gemSetIndex;
    [SerializeField] private int mainGemKey;
    private bool CheckRequiredAttributes(Item item)
    {
        var currentStat = Player.LocalPlayer.RealStat;

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
        else if (Player.LocalPlayer.Stat.Level < item.ItemData.RequiredLevel)
        {
            Debug.Log("Not enough level");
            return false;
        }

        return true;
    }

    public bool AddGem(Item item)
    {
        //메인 젬이면 추가하는 Set Index위치에 오브젝트 풀을 만든다. 근데... gem에 대한 정보는 얘한테 없지 않아? -> 매개 변수로 가지고 있음.
        // string[] tokens = item.ItemData.Parameter.Split('_');

        //능력치 안되면 컷
        if (!CheckRequiredAttributes(item))
        {
            return false;
        }

        if (item.ItemData.ItemType.Equals("MainGem"))
        {
            isInMainGem = true;
            mainGemKey = item.ItemData.Key - 200;
            SkillManager.Instance.isInSkill[gemSetIndex] = true;
            
            SkillManager.Instance.MakePool(gemSetIndex, mainGemKey);
        }
        else if (item.ItemData.ItemType.Equals("SupportGem"))
        {
            if (isInMainGem)
            {
                isInSupportGem.Add(true);
                SkillManager.Instance.AddSkillComponent(gemSetIndex,mainGemKey, item.ItemData.Key);
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
                SkillManager.Instance.isInSkill[gemSetIndex] = false;
                
                SkillManager.Instance.RemovePool(gemSetIndex);
            }
        }
        else if (item.ItemData.ItemType.Equals("SupportGem"))
        {
            isInSupportGem.RemoveAt(0);
            SkillManager.Instance.RemoveSkillComponent(gemSetIndex, item.ItemData.Key);
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
                    
                    SkillManager.Instance.isInSkill[gemSetIndex] = false;
                    SkillManager.Instance.isInSkill[targetSet.gemSetIndex] = true;

                    SkillManager.Instance.RemovePool(gemSetIndex);

                    string[] tokens = item.ItemData.Parameter.Split('_');
                    SkillManager.Instance.MakePool(targetSet.gemSetIndex, int.Parse(tokens[1]));
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
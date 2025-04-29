using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : MonoBehaviour
{
    private Camera mainCam;
    private IMonster currentTargetedMonster;

    [SerializeField] private GameObject hpPanel;
    [SerializeField] private Image HpBar;
    [SerializeField] private Text nameText;

    private void Start()
    {
        mainCam = Camera.main;
    }

    
    
    private void Update()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Enemy"))
            {
                OnMouseHoverMonster(hitObject);
            }

            if (currentTargetedMonster != null)
            {
                UpdateHpBar();
                UpdateName();
            }
        }
    }

    private void OnMouseHoverMonster(GameObject hitObject)
    {
        currentTargetedMonster = hitObject.GetComponent<IMonster>();
        hpPanel.SetActive(true);
    }

    private void UpdateHpBar()
    {
        HpBar.fillAmount = currentTargetedMonster.MonsterStat.hp / currentTargetedMonster.MonsterStat.maxHp;

        if (currentTargetedMonster.MonsterStat.hp <= 0)
        {
            hpPanel.SetActive(false);
        }
    }

    private void UpdateName()
    {
        nameText.text = currentTargetedMonster.MonsterStat.name;
    }
}
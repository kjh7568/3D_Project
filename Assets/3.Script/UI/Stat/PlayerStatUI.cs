using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatUI : MonoBehaviour
{
    [SerializeField] private Image HpBar;
    [SerializeField] private TMP_Text HpText;
    [SerializeField] private Image MpBar;
    [SerializeField] private TMP_Text MpText;
    [SerializeField] private Image ExpBar;
    [SerializeField] private TMP_Text ExpText;
    private void Update()
    {
        UpdateBarsUI();
        UpdateTextUI();
    }

    private void UpdateBarsUI()
    {
        PlayerStat temp = Player.LocalPlayer.Stat;
        
        HpBar.fillAmount = temp.Hp / temp.MaxHp;
        MpBar.fillAmount = temp.Mp / temp.MaxMp;
        ExpBar.fillAmount = temp.Exp / temp.MaxExp;
    }

    private void UpdateTextUI()
    {
        PlayerStat temp = Player.LocalPlayer.Stat;
        
        HpText.text = $"{(int)temp.Hp} / {temp.MaxHp}";
        MpText.text = $"{(int)temp.Mp} / {temp.MaxMp}";
        ExpText.text = $"{(int)temp.Exp} / {temp.MaxExp}";
    }
}

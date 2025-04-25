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
        FinalPlayerStats temp = Player.LocalPlayer.RealStat;
        
        HpBar.fillAmount = temp.Hp / temp.MaxHp;
        MpBar.fillAmount = temp.Mp / temp.MaxMp;
        ExpBar.fillAmount = Player.LocalPlayer.Stat.Exp / Player.LocalPlayer.Stat.MaxExp;
    }

    private void UpdateTextUI()
    {
        FinalPlayerStats temp = Player.LocalPlayer.RealStat;
        
        HpText.text = $"{(int)temp.Hp} / {temp.MaxHp}";
        MpText.text = $"{(int)temp.Mp} / {temp.MaxMp}";
        ExpText.text = $"{(int)Player.LocalPlayer.Stat.Exp} / {Player.LocalPlayer.Stat.MaxExp}";
    }
}

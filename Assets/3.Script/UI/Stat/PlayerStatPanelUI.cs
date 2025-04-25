using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerStatPanelUI : MonoBehaviour
{
    [SerializeField] private Text levelText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text mpText;
    [SerializeField] private Text strengthText;
    [SerializeField] private Text dexterityText;
    [SerializeField] private Text intelligenceText;
    [SerializeField] private Text armourText;
    [SerializeField] private Text evasionText;
    [SerializeField] private Text attackDamageText;
    [SerializeField] private Text attackSpeedText;
    [SerializeField] private Text spellDamageText;
    [SerializeField] private Text castSpeedText;
    [SerializeField] private Text criticalRateText;
    [SerializeField] private Text criticalDamageText;
    [SerializeField] private Text movementSpeedText;

    private void LateUpdate()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        FinalPlayerStats playerStats = Player.LocalPlayer.RealStat;
        playerStats.UpdateStat();

        float expPercent = (Player.LocalPlayer.Stat.Exp / Player.LocalPlayer.Stat.MaxExp) * 100f;
        Debug.Log(expPercent);
        
        levelText.text = $"레벨: {Player.LocalPlayer.Stat.Level} [Exp: {Mathf.FloorToInt(Player.LocalPlayer.Stat.Exp)} / {Mathf.FloorToInt(Player.LocalPlayer.Stat.MaxExp)} ({expPercent:F1}%)]";
        
        hpText.text = $"체력: {Mathf.FloorToInt(playerStats.Hp)} / {Mathf.FloorToInt(playerStats.MaxHp)} (+{playerStats.HpRegenRate:F2}/s)";
        
        mpText.text = $"마나: {Mathf.FloorToInt(playerStats.Mp)} / {Mathf.FloorToInt(playerStats.MaxMp)} (+{playerStats.MpRegenRate:F2}/s)";
        
        strengthText.text = $"힘: {playerStats.Strength}";
        dexterityText.text = $"민첩: {playerStats.Dexterity}";
        intelligenceText.text = $"지능: {playerStats.Intelligence}";
        
        armourText.text = $"방어력: {Mathf.FloorToInt(playerStats.Armour)} (방어율: {playerStats.DamageReductionRate:F2}%)";
        evasionText.text = $"회피: {Mathf.FloorToInt(playerStats.Evasion)} (회피율:  {playerStats.EvasionRate:F2}%)";
        
        attackDamageText.text = $"공격력: {playerStats.MinAttackDamage} ~ {playerStats.MaxAttackDamage - 1}";
        attackSpeedText.text = $"공격속도: {playerStats.AttackSpeed:F2}";
        
        spellDamageText.text = $"주문력: {playerStats.MinSpellDamage} ~ {playerStats.MaxSpellDamage - 1}";
        castSpeedText.text = $"시전속도: {playerStats.CastSpeed:F2}";
        
        criticalRateText.text = $"치명타 확률: {playerStats.CriticalChance * 100f:F1}%";
        criticalDamageText.text = $"치명타 피해: {playerStats.CriticalDamage * 100f:F1}%";
        
        movementSpeedText.text = $"이동속도: {playerStats.MovementSpeed:F2}";
    }
    
}

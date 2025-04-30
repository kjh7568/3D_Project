using UnityEngine;

public class FasterCast : SkillComponent
{
    private Skill rootSkill;
    private float increaseValue;

    public override void AddComponent(Skill skill)
    {
        rootSkill = skill;
        EquipmentManager.Instance.EquipmentStat.IncreaseCastSpeed += 0.3f;
        
        rootSkill.data.costMana += 5;
        rootSkill.tags.Add("FasterCast");
    }

    public override void RemoveComponent(Skill skill)
    {
        rootSkill = skill;
        EquipmentManager.Instance.EquipmentStat.IncreaseCastSpeed -= 0.3f;

        rootSkill.data.costMana -= 5;
        rootSkill.tags.Remove("FasterCast");
    }
}
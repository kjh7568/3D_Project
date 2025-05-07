using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasterProjectiles : SkillComponent
{
    public string SkillTag { get; private set; } = "FasterProjectiles";
    
    private float increaseValue;
    private Skill rootSkill;
    public override void AddComponent(Skill skill)
    {
        rootSkill = skill;
        increaseValue = rootSkill.data.moveSpeed * 0.5f;
        rootSkill.data.moveSpeed += increaseValue;
        rootSkill.data.costMana += 3;
        rootSkill.tags.Add("FasterProjectiles");
    }

    public override void RemoveComponent(Skill skill)
    {
        rootSkill = skill;
        rootSkill.data.moveSpeed -= increaseValue;        
        rootSkill.data.costMana -= 3;
        rootSkill.tags.Remove("FasterProjectiles");
    }
}
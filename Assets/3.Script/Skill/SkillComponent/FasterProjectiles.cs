using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasterProjectiles : SkillComponent
{
    private Skill rootSkill;

    private float projectileMoveSpeed;
    private float projectileRotate;

    public string SkillTag { get; private set; } = "FasterProjectiles";
    
    public override void AddComponent(Skill skill)
    {
        rootSkill = skill;
        rootSkill.data.moveSpeed += 10;
        rootSkill.data.costMana += 5;
        rootSkill.tags.Add("FasterProjectiles");
    }

    public override void RemoveComponent(Skill skill)
    {
        rootSkill = skill;
        rootSkill.data.moveSpeed -= 10;        
        rootSkill.data.costMana -= 5;
        rootSkill.tags.Remove("FasterProjectiles");
    }
}
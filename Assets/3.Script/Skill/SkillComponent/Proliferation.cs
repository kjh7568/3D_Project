using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proliferation : SkillComponent
{
    private Skill rootSkill;

    public override void AddComponent(Skill skill)
    {
        rootSkill = skill;
        rootSkill.data.damageRate += 0.1f;
        rootSkill.data.costMana += 2;
        rootSkill.tags.Add("Proliferation");
    }

    public override void RemoveComponent(Skill skill)
    {
        rootSkill = skill;
        rootSkill.data.damageRate -= 0.1f;
        rootSkill.data.costMana -= 2;
        rootSkill.tags.Remove("Proliferation");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleProjectiles : SkillComponent
{
    private Skill rootSkill;

    public override void AddComponent(Skill skill)
    {
        rootSkill = skill;
        rootSkill.tags.Add("MultipleProjectiles");
    }

    public override void RemoveComponent(Skill skill)
    {
        rootSkill = skill;
        rootSkill.tags.Remove("MultipleProjectiles");
    }
}
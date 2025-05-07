public class MultipleProjectiles : SkillComponent
{
    private Skill rootSkill;
    public override void AddComponent(Skill skill)
    {
        rootSkill = skill;
        rootSkill.data.isIncreasedAOE = true;
        rootSkill.data.costMana += 4;
        rootSkill.tags.Add("MultipleProjectiles");
    }

    public override void RemoveComponent(Skill skill)
    {
        rootSkill = skill;
        rootSkill.data.isIncreasedAOE = false;     
        rootSkill.data.costMana -= 4;
        rootSkill.tags.Remove("MultipleProjectiles");
    }
}
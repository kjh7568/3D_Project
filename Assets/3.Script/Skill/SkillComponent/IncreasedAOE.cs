public class IncreasedAOE : SkillComponent
{
    private Skill rootSkill;
    public override void AddComponent(Skill skill)
    {
        rootSkill = skill;
        rootSkill.data.isIncreasedAOE = true;
        rootSkill.data.costMana += 3;
        rootSkill.tags.Add("IncreasedAOE");
    }

    public override void RemoveComponent(Skill skill)
    {
        rootSkill = skill;
        rootSkill.data.isIncreasedAOE = false;     
        rootSkill.data.costMana -= 3;
        rootSkill.tags.Remove("IncreasedAOE");
    }
}
public class FasterCast : SkillComponent
{
    private Skill rootSkill;
    private float increaseValue;

    public override void AddComponent(Skill skill)
    {
        rootSkill = skill;
        increaseValue = rootSkill.data.castSpeed * 0.3f;
        rootSkill.data.castSpeed -= increaseValue;
        rootSkill.data.costMana += 5;
        rootSkill.tags.Add("Proliferation");
    }

    public override void RemoveComponent(Skill skill)
    {
        rootSkill = skill;
        rootSkill.data.castSpeed += increaseValue;
        rootSkill.data.costMana -= 5;
        rootSkill.tags.Remove("Proliferation");
    }
}
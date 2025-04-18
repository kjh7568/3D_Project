using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastLogic : StateMachineBehaviour
{
    [Range(0f, 1f)] public float startNormalizedTime;
    private bool isCast = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isCast = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float time = stateInfo.normalizedTime % 1f;

        // 정확한 타이밍에 시전하도록
        if (!isCast && time >= startNormalizedTime)
        {
            SkillManager.instance.UseSkill(SkillManager.instance.currentCastingSpellIndex);
            isCast = true;
        }
    }
}
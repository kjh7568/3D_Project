using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastLogic : StateMachineBehaviour
{
    [Range(0f, 1f)] public float startNormalizedTime;
    [Range(0f, 1f)] public float endNormalizedTime;

    private bool isCast = false;
    private float originalSpeed;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isCast = false;
        
        originalSpeed = animator.speed;

        animator.speed = Player.LocalPlayer.RealStat.CastSpeed;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float time = stateInfo.normalizedTime % 1f;

        // 정확한 타이밍에 시전하도록
        if (!isCast && time >= startNormalizedTime)
        {
            SkillManager.Instance.UseSkill(SkillManager.Instance.currentCastingSpellIndex);
            isCast = true;
        }
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = originalSpeed;
    }
}
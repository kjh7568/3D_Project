using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackLogic : StateMachineBehaviour
{
    [Range(0f, 1f)] public float startNormalizedTime;
    [Range(0f, 1f)] public float endNormalizedTime;

    private bool isPassStartNormalizedTime;
    private bool isPassEndNormalizedTime;
    private Collider weaponCollider;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isPassStartNormalizedTime = false;
        isPassEndNormalizedTime = false;
        
        weaponCollider = animator.GetComponent<IMonster>().AttackCollider;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float time = stateInfo.normalizedTime % 1f;

        // 콜라이더 켜기 시점
        if (!isPassStartNormalizedTime && time >= startNormalizedTime)
        {
            isPassStartNormalizedTime = true;
            weaponCollider.enabled = true;  
        }

        // 콜라이더 끄기 시점
        if (!isPassEndNormalizedTime && time >= endNormalizedTime)
        {
            isPassEndNormalizedTime = true;
            weaponCollider.enabled = false;
        }
    }
}

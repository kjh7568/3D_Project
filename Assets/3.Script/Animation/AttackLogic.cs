using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AttackLogic : StateMachineBehaviour
{
    [Range(0f, 1f)] public float startNormalizedTime;

    [Range(0f, 1f)] public float endNormalizedTime;
    
    private float originalSpeed;
    private bool isPassStartNormalizedTime;
    private bool isPassEndNormalizedTime;
    private Collider weaponCollider;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        originalSpeed = animator.speed;

        animator.speed = Player.LocalPlayer.RealStat.AttackSpeed; //todo 나중엔 여기에 플레이어 스탯 곱해서 증가해줄 것
        
        isPassStartNormalizedTime = false;
        isPassEndNormalizedTime = false;
        
        weaponCollider = Player.LocalPlayer.currentWeapon.gameObject.GetComponent<Collider>();
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
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = originalSpeed;
    }
}

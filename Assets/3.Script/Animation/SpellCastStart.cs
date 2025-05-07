using UnityEngine;

public class SpellCastStart : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = Player.LocalPlayer.RealStat.CastSpeed;
    }
}
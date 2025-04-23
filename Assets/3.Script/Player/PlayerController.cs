using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int DirX = Animator.StringToHash("DirX");
    private static readonly int DirZ = Animator.StringToHash("DirZ");
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private static readonly int Skill1 = Animator.StringToHash("Skill");

    [Header("moving Settings")] [SerializeField]
    private Transform rotatePlayer;

    public float moveSpeed;

    [Header("Camera Settings")] [SerializeField]
    private Camera mainCam;

    [SerializeField] private Vector3 offSet;

    [Header("Animation Settings")] [SerializeField]
    private Animator animator;

    private AnimatorStateInfo animInfo;

    private void Start()
    {
        if (rotatePlayer == null)
            Debug.LogError("rotatePlayer가 할당되지 않았습니다!");
    }

    void Update()
    {
        animInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (!animInfo.IsName("Attack01") && !animInfo.IsName("Attack01 - Start")
                                         && !animInfo.IsName("Attack01 - Casting") &&
                                         !animInfo.IsName("Attack01 - Release"))
        {
            RotatePlayer();
            MovePlayer();
            UpdateCombatInput();
        }
    }

    private void MovePlayer()
    {
        if (animInfo.IsName("Attack01") || animInfo.IsName("Spell01 - Start") || animInfo.IsName("Spell01 - Casting") ||
            animInfo.IsName("Spell01 - Release"))
        {
            return;
        }

        Vector3 getAxis = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        Vector3 camForward = mainCam.transform.forward;
        Vector3 camRight = mainCam.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 direction = getAxis.z * camForward + getAxis.x * camRight;

        direction.Normalize();
        transform.Translate(direction * (moveSpeed * Time.deltaTime), Space.World);

        mainCam.transform.position = transform.position + offSet;

        Vector3 localDir = transform.InverseTransformDirection(direction.normalized);

        animator.SetBool(Walk, getAxis != Vector3.zero);

        animator.SetFloat(DirX, localDir.x);
        animator.SetFloat(DirZ, localDir.z);
    }

    private void RotatePlayer()
    {
        if (animInfo.IsName("Attack01"))
        {
            return;
        }

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 direction = targetPoint - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = lookRotation;
            }
        }
    }

    private void UpdateCombatInput()
    {
        if (animInfo.IsName("Attack01") || animInfo.IsName("Spell01 - Start") || animInfo.IsName("Spell01 - Casting") ||
            animInfo.IsName("Spell01 - Release"))
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Skill(0);
            SkillManager.instance.currentCastingSpellIndex = 0;
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Skill(1);
            SkillManager.instance.currentCastingSpellIndex = 1;
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Skill(2);
            SkillManager.instance.currentCastingSpellIndex = 2;
            return;
        }
    }

    private void Attack()
    {
        animator.ResetTrigger(Attack1);
        animator.SetTrigger(Attack1);
    }

    private void Skill(int idx)
    {
        if (!SkillManager.instance.isInSkill[idx])
        {
            Debug.Log("슬롯에 스킬이 없습니다!");
        }
        else
        {
            var temp = SkillManager.instance.skillPool[idx].Dequeue();
            if (temp.TryGetComponent(out Skill skill))
            {
                if (Player.LocalPlayer.RealStat.Mp >= skill.data.costMana)
                {
                    Player.LocalPlayer.RealStat.Mp -= skill.data.costMana;
                    
                    SkillManager.instance.skillPool[idx].Enqueue(temp);

                    animator.ResetTrigger(Skill1);
                    animator.SetTrigger(Skill1);
                }
                else
                {
                    SkillManager.instance.skillPool[idx].Enqueue(temp);
                    Debug.Log("마나가 부족합니다.");
                }
            }
        }
    }
}
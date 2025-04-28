using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int DirX = Animator.StringToHash("DirX");
    private static readonly int DirZ = Animator.StringToHash("DirZ");
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private static readonly int Skill1 = Animator.StringToHash("Skill");
    private static readonly int Recall = Animator.StringToHash("Recall");

    [Header("moving Settings")] [SerializeField]
    private Transform rotatePlayer;

    [Header("Camera Settings")] [SerializeField]
    private Camera mainCam;

    [SerializeField] private Vector3 offSet;

    [Header("Animation Settings")] [SerializeField]
    private Animator animator;

    [SerializeField] private GameObject recallPrefab;
    
    private AnimatorStateInfo animInfo;
    private Vector3 lastPosition;

    private void Start()
    {
        if (rotatePlayer == null)
            Debug.LogError("rotatePlayer가 할당되지 않았습니다!");
        lastPosition = transform.position;
    }

    void Update()
    {
        animInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (!animInfo.IsName("Attack01") && !animInfo.IsName("Attack01 - Start")
                                         && !animInfo.IsName("Attack01 - Casting") &&
                                         !animInfo.IsName("Attack01 - Release") &&
                                         !animInfo.IsName("Recall"))
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
        transform.Translate(direction * (Player.LocalPlayer.RealStat.MovementSpeed * Time.deltaTime), Space.World);
    
        mainCam.transform.position = transform.position + offSet;
    
        Vector3 localDir = transform.InverseTransformDirection(direction.normalized);
    
        // (1) 이동량을 통해 실제 움직였는지 판단
        Vector3 displacement = transform.position - lastPosition;
        displacement.y = 0;
    
        bool isMoving = displacement.sqrMagnitude > 0.0001f; // 이동량이 거의 0보다 크면 이동 중으로 본다
    
        animator.SetBool(Walk, isMoving);
    
        animator.SetFloat(DirX, localDir.x);
        animator.SetFloat(DirZ, localDir.z);
    
        // (2) 현재 위치 저장
        lastPosition = transform.position;
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
            SkillManager.Instance.currentCastingSpellIndex = 0;
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Skill(1);
            SkillManager.Instance.currentCastingSpellIndex = 1;
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Skill(2);
            SkillManager.Instance.currentCastingSpellIndex = 2;
            return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            recallPrefab.SetActive(true);
            animator.ResetTrigger(Recall);
            animator.SetTrigger(Recall);
        }
    }

    private void Attack()
    {
        // UI를 클릭했으면 공격 무시
        // if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        // {
        //     Debug.Log("UI클릭!");
        //     return;
        // }

        // NPC 클릭 상태면 공격 무시

        try
        {
            if (FindObjectOfType<NPCEventManager>().IsClickingNpc())
            {
                return;
            }
        }
        catch
        {
            // ignored
        }

        animator.ResetTrigger(Attack1);
        animator.SetTrigger(Attack1);
    }

    private void Skill(int idx)
    {
        if (!SkillManager.Instance.isInSkill[idx])
        {
            Debug.Log("슬롯에 스킬이 없습니다!");
        }
        else
        {
            var temp = SkillManager.Instance.skillPool[idx].Dequeue();
            if (temp.TryGetComponent(out Skill skill))
            {
                if (Player.LocalPlayer.RealStat.Mp >= skill.data.costMana)
                {
                    Player.LocalPlayer.RealStat.Mp -= skill.data.costMana;

                    SkillManager.Instance.skillPool[idx].Enqueue(temp);

                    animator.ResetTrigger(Skill1);
                    animator.SetTrigger(Skill1);
                }
                else
                {
                    SkillManager.Instance.skillPool[idx].Enqueue(temp);
                    Debug.Log("마나가 부족합니다.");
                }
            }
        }
    }
}
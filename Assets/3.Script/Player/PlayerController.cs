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
        if (!animInfo.IsName("Attack01"))
        {
            RotatePlayer();
            MovePlayer();
            UpdateCombatInput();
        }
    }

    private void MovePlayer()
    {
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
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SkillManager.instance.UseSkill(0);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SkillManager.instance.UseSkill(1);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SkillManager.instance.UseSkill(2);
        }
    }

    private void Attack()
    {
        if (!animInfo.IsName("Attack01"))
        {
            animator.ResetTrigger(Attack1);
            animator.SetTrigger(Attack1);
        }
    }
}
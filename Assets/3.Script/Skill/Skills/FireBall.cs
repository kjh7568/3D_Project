using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireBall : Skill
{
    [SerializeField] private GameObject explosion;

    private Camera mainCam;
    private Vector3 moveDirection;
    
    private float radius = 1.5f;
    private LayerMask enemyLayer;
    
    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void OnEnable()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(enter);
            moveDirection = (mouseWorldPosition - transform.position).normalized;
        }
    }

    private void Update()
    {
        transform.Translate(moveDirection * (Time.deltaTime * data.moveSpeed));
    }

    private void OnTriggerEnter(Collider other)
    {
        // 적이랑 충돌했을 때 한 번만 실행
        if (other.CompareTag("Enemy"))
        {
            if (data.isIncreasedAOE)
            {
                radius = 2f;
                explosion.transform.localScale = Vector3.one * 1.2f;
            }
            else
            {
                radius = 1.5f;
                explosion.transform.localScale = Vector3.one * 0.7f;
            }
            
            // 스킬의 중심 위치 기준으로 반경 탐색
            Collider[] hits = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Enemy"));

            foreach (Collider hit in hits)
            {
                if (!hit.CompareTag("Enemy")) continue;

                var monster = CombatSystem.Instance.GetMonsterOrNull(hit);

                if (monster != null)
                {
                    CombatEvent combatEvent = new CombatEvent
                    {
                        Sender = Player.LocalPlayer,
                        Receiver = monster,
                        HitPosition = hit.ClosestPoint(transform.position),
                        Collider = hit,
                        Damage = CalculateDamage()
                    };

                    CombatSystem.Instance.AddInGameEvent(combatEvent);
                }
            }
        }

        // 이펙트 생성 및 반환 처리
        Instantiate(explosion, transform.position, Quaternion.identity);
        SkillManager.Instance.skillPool[0].Enqueue(gameObject);
        gameObject.SetActive(false);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class FireBall : Skill
{
    private Camera mainCam;
    private Vector3 moveDirection;
    
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

    public override void Cast()
    {
        if (tags.Count == 0)
        {
            Debug.Log("No tags found");
            return;
        }
        
        foreach (var tag in tags)
        {
            Debug.Log(tag);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            
        }
        else if(other.CompareTag("Wall"))
        {
            SkillManager.instance.skillPool[0].Enqueue(gameObject);
            gameObject.SetActive(false);
        }
    }
}
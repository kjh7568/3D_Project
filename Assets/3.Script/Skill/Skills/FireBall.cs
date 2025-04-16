using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class FireBall : Skill
{
    private void Start()
    {
        tags.Add("spell");
        tags.Add("projectile");
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * (Time.deltaTime * data.moveSpeed));
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
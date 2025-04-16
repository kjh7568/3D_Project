using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [System.Serializable]
    public class SkillData
    {
        public string skillName;
        public float damageRatio;
        public float moveSpeed;
        public float castSpeed;
    }

    public SkillData data;
    public List<string> tags = new List<string>();
    public abstract void Cast();
}

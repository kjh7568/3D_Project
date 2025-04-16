using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 컴포넌트에 대한 정의
/// </summary>
public abstract class SkillComponent : MonoBehaviour
{
    /// <summary>
    /// 스킬에 스킬컴포넌트가 추가되면 적용해야 할 내용
    /// </summary>
    /// <param name="skill">컴포넌트가 적용될 스킬</param>
    /// <returns></returns>
    public abstract void AddComponent(Skill skill);

    /// <summary>
    /// 스킬에 스킬컴포넌트가 제거되면 적용해야 할 내용
    /// </summary>
    /// <param name="skill">컴포넌트가 제거될 스킬</param>
    /// <returns></returns>
    public abstract void RemoveComponent(Skill skill);
}

public static class SkillAssembler
{
    /// <summary>
    /// 컴포넌트를 추가할 때 호출하는 메서드
    /// </summary>
    /// <param name="skill">추가하는 스크립트에서 this. 으로 호출하면 매개변수는 안넣어도 된다.</param>
    /// <typeparam name="T">추가할 스킬컴포넌트를 입력한다.</typeparam>
    /// <returns></returns>
    public static void Add<T>(this Skill skill) where T : SkillComponent
    {
        var addingComponent = skill.gameObject.AddComponent<T>();
        addingComponent.AddComponent(skill);
    }

    /// <summary>
    /// 컴포넌트를 삭제할 때 호출하는 메서드
    /// </summary>
    /// <param name="skill">삭제하는 스크립트에서 this. 으로 호출하면 매개변수는 안넣어도 된다.</param>
    /// <typeparam name="T">삭제할 스킬컴포넌트를 입력한다.</typeparam>
    public static void Remove<T>(this Skill skill) where T : SkillComponent
    {
        var existingComponent = skill.gameObject.GetComponent<T>();
        existingComponent.RemoveComponent(skill);
        Object.Destroy(existingComponent);
    }
}
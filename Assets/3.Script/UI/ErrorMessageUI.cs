using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessageUI : MonoBehaviour
{
    public static ErrorMessageUI Instance;
    
    [SerializeField]private Text errorText;

    private void Awake()
    {
        Instance = this;
    }

    public void StartFade(int errorCode)
    {
        StopAllCoroutines();

        switch (errorCode)
        {
            case 0:
                SetErrorText("마나가 부족합니다.");
                SetErrorTextColor(Color.blue);
                break;
        }

        StartCoroutine(FadeText_Coroutine());
    }

    private IEnumerator FadeText_Coroutine()
    {
        Color startColor = errorText.color;
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            errorText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        errorText.color = new Color(startColor.r, startColor.g, startColor.b, 0f); // 최종 알파 0
    }
    
    private void SetErrorText(string errorMessage)
    {
        errorText.text = errorMessage;
    }

    private void SetErrorTextColor(Color errorColor)
    {
        errorText.color = errorColor;
    }
}

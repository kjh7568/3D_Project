using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NPCEventManager : MonoBehaviour
{
    private Camera mainCam;

    [SerializeField]private Collider shopNpcCollider;
    
    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectClick();
        }
    }

    private void DetectClick()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            // UI 클릭이면 무시
            Debug.Log("UI 클릭");
            return;
        }

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log($"클릭한 오브젝트: {hit.collider.gameObject.name}");
            // 필요하면 PlayerController나 GameManager에 알림

            if (hit.collider.Equals(shopNpcCollider))
            {
                Debug.Log(Vector3.Distance(hit.collider.gameObject.transform.position, Player.LocalPlayer.gameObject.transform.position));
            }
        }
    }
}

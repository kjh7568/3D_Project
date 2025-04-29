using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NPCEventManager : MonoBehaviour
{
    private Camera mainCam;

    [SerializeField] private Collider shopNpcCollider;
    [SerializeField] private Collider portalCollider;
    
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject shopPanel;

    private bool isClickingNpc = false;

    private void Start()
    {
        mainCam = Camera.main;
        shopPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            DetectClick();
        }
        
        if(Vector3.Distance(shopNpcCollider.gameObject.transform.position, Player.LocalPlayer.transform.position) > 2.0f)
        {
            shopPanel.SetActive(false);
        }
    }

    private void DetectClick()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI 클릭");
            return;
        }

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.Equals(shopNpcCollider))
            {
                isClickingNpc = true;
                StopAllCoroutines();
                StartCoroutine(MoveToShop(hit.collider.transform));
            }
            else if (hit.collider.Equals(portalCollider))
            {
                isClickingNpc = true;
                StopAllCoroutines();
                StartCoroutine(MoveToPortal(hit.collider.transform));
            }
            else
            {
                isClickingNpc = false;
            }
        }
    }

    private IEnumerator MoveToShop(Transform target)
    {
        while (Vector3.Distance(target.position, Player.LocalPlayer.transform.position) > 2.0f)
        {
            Vector3 direction = (target.position - Player.LocalPlayer.transform.position).normalized;
            Player.LocalPlayer.transform.Translate(
                direction * (Player.LocalPlayer.RealStat.MovementSpeed * Time.deltaTime), Space.World);

            yield return null; // 한 프레임 쉬고 다시 반복
        }

        inventoryPanel.SetActive(true);
        shopPanel.SetActive(true);
    }
    
    private IEnumerator MoveToPortal(Transform target)
    {
        while (Vector3.Distance(target.position, Player.LocalPlayer.transform.position) > 4.0f)
        {
            Vector3 direction = target.position - Player.LocalPlayer.transform.position;
            direction.y = 0f; // Y축 무시
            direction = direction.normalized;

            Player.LocalPlayer.transform.Translate(
                direction * (Player.LocalPlayer.RealStat.MovementSpeed * Time.deltaTime), Space.World);


            yield return null; // 한 프레임 쉬고 다시 반복
        }

        // SceneManager.LoadScene("Dungeon 1");        
        SceneManager.LoadScene("DungeonTest");        
    }

    public bool IsClickingNpc()
    {
        return isClickingNpc;
    }
}
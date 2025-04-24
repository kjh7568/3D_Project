using UnityEngine;

public class DropGoldTrigger : MonoBehaviour
{
    private RectTransform dropUI;
    private int goldAmount;

    public void Setup(RectTransform ui, int amount)
    {
        dropUI = ui;
        goldAmount = amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 골드 지급 처리
        Debug.Log($"골드 {goldAmount} 획득!");
        Player.LocalPlayer.gold += goldAmount;
        Debug.Log($"현재 총 골드 {Player.LocalPlayer.gold} 획득!");

        // UI 제거
        DropItemUI.Instance.UnregisterGoldDrop(dropUI);

        // 골드 오브젝트 제거
        Destroy(gameObject);
    }
}
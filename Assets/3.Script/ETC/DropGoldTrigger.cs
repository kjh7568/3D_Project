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
        Player.LocalPlayer.gold += goldAmount;

        // UI 제거
        DropItemUI.Instance.UnregisterGoldDrop(dropUI);

        // 골드 오브젝트 제거
        Destroy(gameObject);
    }
}
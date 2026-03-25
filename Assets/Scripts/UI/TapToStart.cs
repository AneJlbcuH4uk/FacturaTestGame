using UnityEngine;
using UnityEngine.EventSystems;

public class TapToStart : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameManager gameManager;

    public void OnPointerDown(PointerEventData eventData)
    {
        gameManager.StartGame();
        gameObject.SetActive(false);
    }
}
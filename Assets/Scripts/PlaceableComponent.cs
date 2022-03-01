using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceableComponent : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        CardControl droppedCard = eventData.pointerDrag.GetComponent<CardControl>();
        droppedCard.CurrentParent.GetComponent<IPlaceable>()?.RemoveCard(droppedCard);
        GetComponent<IPlaceable>()?.PlaceCard(droppedCard);
    }
}

interface IPlaceable
{
    void PlaceCard(CardControl card);
    void RemoveCard(CardControl card);
}

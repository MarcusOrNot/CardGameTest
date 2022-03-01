using UnityEngine;

public class TableControl : MonoBehaviour, IPlaceable
{
    public void PlaceCard(CardControl card)
    {
        if (card.transform.parent == transform) return;
        card.transform.SetParent(transform);
        card.transform.position = Vector2.zero;
        card.transform.rotation = Quaternion.identity;
    }

    public void RemoveCard(CardControl card)
    {
        
    } 
}

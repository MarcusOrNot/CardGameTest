using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CardsPoolControl : MonoBehaviour, IPlaceable
{
    [SerializeField] private GameObject card_prefub;
    private List<CardControl> cardsPool;
    private HandControl hand;
    private void Awake()
    {
        hand = FindObjectOfType<HandControl>();
        cardsPool = new List<CardControl>();
        for (int i = 0; i < 10; i++) cardsPool.Add(Instantiate(card_prefub, transform).GetComponent<CardControl>());
    }
    // Start is called before the first frame update
    void Start()
    {
        GiveFirstHand();
    }

    public CardControl DealCard()
    {
        if (cardsPool.Count <= 0) return null;
        CardControl dealingCard = cardsPool[0];
        cardsPool.RemoveAt(0);
        return dealingCard;
    }

    private void GiveFirstHand()
    {
        
        int count = Random.Range(4, hand.GetMaxCards()+1);
        for (int i = 0; i < count; i++)
        {
            CardControl dealCard = DealCard();
            StartCoroutine(DownloadImage("https://picsum.photos/seed/" + (Random.Range(0, 10000)).ToString() + "/200/300", dealCard));
        }
    }

    private IEnumerator DownloadImage(string MediaUrl, CardControl forCard)
    {
        Sprite sprite = null;
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.result ==UnityWebRequest.Result.ConnectionError || request.result==UnityWebRequest.Result.ProtocolError)
            Debug.Log(request.error);
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            sprite = Sprite.Create(((DownloadHandlerTexture)request.downloadHandler).texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100);
        }
            forCard.SetData("Card Name", "Card Description", sprite, Random.Range(1,9), Random.Range(1, 9), Random.Range(1, 9));
        hand.PlaceCard(forCard);
    }

    public void PlaceCard(CardControl card)
    {
        cardsPool.Add(card);
        card.transform.SetParent(transform);
        card.GetComponent<CanvasGroup>().blocksRaycasts = false;
        card.AnimToPos(transform.position, transform.rotation, 2);
    }

    public void RemoveCard(CardControl card)
    {
        
    }
}

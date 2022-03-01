using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandControl : MonoBehaviour, IPlaceable
{
    [SerializeField] private Transform[] cardsPlaces;
    private int counterPos = -1;

    public void PlaceCard(CardControl card)
    {
        int freePlace = GetFreePlace();
        if (freePlace == -1) return;
        PlaceAtPos(freePlace, card);
    }

    private void PlaceAtPos(int cardPos, CardControl card)
    {
        card.transform.SetParent(cardsPlaces[cardPos].transform);
        card.AnimToPos(cardsPlaces[cardPos].transform.position, cardsPlaces[cardPos].transform.rotation, 2);
    }

    public void RemoveCard(CardControl card)
    {
        RefreshPositions();
    }

    private int GetFreePlace()  //Получить незанятое место для карты
    {
        int middle = Mathf.CeilToInt((float) cardsPlaces.Length / 2.0f);
        for (int i = 0; i < middle; i++)
        {
            if (middle - i - 1 >= 0) if (!IsChild(middle - i - 1)) return middle - i - 1;
            if (!IsChild(middle+i)) return middle+i;
        }
        return -1;
    } 

    private bool IsChild(int pos)
    {
        return cardsPlaces[pos].childCount > 0;
    }

    private CardControl GetChildAt(int pos)
    {
        if (IsChild(pos)) return cardsPlaces[pos].GetChild(0).GetComponent<CardControl>();
        return null;
    }

    private void RefreshPositions()  //Расположить карты красиво
    {
        int middle = Mathf.CeilToInt((float)cardsPlaces.Length / 2.0f);
        for (int i = middle; i < cardsPlaces.Length; i++)  //Справа к центру
            if (!IsChild(i))
                for (int j = i + 1; j < cardsPlaces.Length; j++)
                    if (IsChild(j))
                    {
                        PlaceAtPos(i, GetChildAt(j));
                        break;
                    }
        for (int i = middle - 1; i >= 0; i--)  //Слева к центру
            if (!IsChild(i))
                for (int j = i - 1; j >= 0; j--)
                    if (IsChild(j))
                    {
                        PlaceAtPos(i, GetChildAt(j));
                        break;
                    }
    }

    public void ChangeRandomVal()
    {
        int counter = 0;
        do {
            counterPos++; if (counterPos >= cardsPlaces.Length) counterPos = 0;
            if (cardsPlaces[counterPos].childCount > 0)
            {
                cardsPlaces[counterPos].GetChild(0).GetComponent<CardControl>().ChangeMana(Random.Range(-2, 9));
                break;
            }
            counter++;
        } while (counter < cardsPlaces.Length);
    }

    public int GetMaxCards() => cardsPlaces.Length;
}

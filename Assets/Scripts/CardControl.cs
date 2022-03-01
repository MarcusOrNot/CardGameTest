using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardControl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image card_image;
    [SerializeField] private Text mana_text;
    [SerializeField] private Text health_text;
    [SerializeField] private Text attack_text;
    [SerializeField] private Text description_text;
    [SerializeField] private Text name_text;
    [SerializeField] private Image highlite_image;
    private int _mana = 0;
    private int _health = 0;
    private int _attack = 0;
    private GameObject BG;
    private Camera mainCamera;
    private Vector2 drag_offset;
    private Transform _currentParent;
    private IEnumerator healthIEnum;
    private IEnumerator manaIEnum;
    private IEnumerator attackIEnum;
    private void Awake()
    {
        BG = GameObject.Find("Background");
        mainCamera = GameObject.FindObjectOfType<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int Mana
    {
        get => _mana;
        set
        {
            _mana = value;
            mana_text.text = _mana.ToString();
            if (_mana == 0) ReturnToPool();
        }
    }

    public void ChangeMana(int toValue)
    {
        if (manaIEnum != null) StopCoroutine(manaIEnum);
        StartCoroutine(manaIEnum = ManaCoroutine(toValue));
    }

    private int Health
    {
        get => _health;
        set
        {
            _health = value;
            health_text.text = _health.ToString();
        }
    }

    public void ChangeHealth(int toValue)
    {
        if (healthIEnum != null) StopCoroutine(healthIEnum);
        StartCoroutine(healthIEnum = HealthCoroutine(toValue));
    }

    private int Attack
    {
        get => _attack;
        set
        {
            _attack = value;
            attack_text.text = _attack.ToString();
        }
    }

    public void ChangeAttack(int toValue)
    {
        if (attackIEnum != null) StopCoroutine(attackIEnum);
        StartCoroutine(attackIEnum = AttackCoroutine(toValue));
    }

    

    public Transform CurrentParent
    {
        get => _currentParent;
    }

    public void SetData(string cardName, string cardDescription, Sprite cardSprite, int mana, int health, int attack)
    {
        name_text.text = cardName;
        description_text.text = cardDescription;
        card_image.sprite = cardSprite;
        Mana = mana;
        Health = health;
        Attack = attack;
    }

    private void SaveAndSetParent()
    {
        _currentParent = transform.GetComponentInParent<PlaceableComponent>().transform;
        transform.SetParent(BG.transform);
    }

    private void ReturnToPool()
    {
        SaveAndSetParent();
        if (CurrentParent != null) CurrentParent.GetComponent<IPlaceable>()?.RemoveCard(this);
        
        GameObject.FindObjectOfType<CardsPoolControl>().PlaceCard(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        drag_offset = transform.position - mainCamera.ScreenToWorldPoint(eventData.position);
        SaveAndSetParent();
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        highlite_image.enabled = true;
        iTween.Stop(this.gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 drag_pos = mainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = drag_pos+drag_offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent.gameObject == BG)
        {
            _currentParent.GetComponent<IPlaceable>()?.PlaceCard(this);
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        //_currentParent = null;
        highlite_image.enabled = false;
    }

    private IEnumerator HealthCoroutine(int toHealth)
    {
        while (toHealth != Health)
        {
            yield return new WaitForSeconds(0.1f);
            Health += (int)Mathf.Sign(toHealth - Health) * 1;
        }
        healthIEnum = null;
    }

    private IEnumerator ManaCoroutine(int toMana)
    {
        while (toMana != Mana)
        {
            yield return new WaitForSeconds(0.1f);
            Mana += (int)Mathf.Sign(toMana - Mana) * 1;
        }
        manaIEnum = null;
    }

    private IEnumerator AttackCoroutine(int toAttack)
    {
        while (toAttack != Attack)
        {
            yield return new WaitForSeconds(0.1f);
            Attack += (int)Mathf.Sign(toAttack - Attack) * 1;
        }
        attackIEnum = null;
    }

    public void AnimToPos(Vector2 toPos, Quaternion toRot, float inTime)
    {
        iTween.MoveTo(this.gameObject, toPos, inTime);
        iTween.RotateTo(this.gameObject, toRot.eulerAngles, inTime);
    }
}

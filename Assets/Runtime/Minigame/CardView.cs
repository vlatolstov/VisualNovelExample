using UnityEngine;
using UnityEngine.UI;
using System;

public class CardView : MonoBehaviour {
    [Header("UI Elements")]
    [SerializeField] private Image _frontImage;
    [SerializeField] private Image _backImage;
    [SerializeField] private Button _cardButton;
    [SerializeField] private Animator _animator;

    [Header("Card Data")]
    public int CardId { get; private set; }
    public bool IsFlipped { get; private set; }
    public bool IsMatched { get; private set; }

    public event Action<CardView> OnCardClicked;

    private void Awake() {
        _cardButton.onClick.AddListener(HandleClick);
    }

    private void HandleClick() {
        if (IsFlipped || IsMatched)
            return;
        OnCardClicked?.Invoke(this);
    }

    public void SetCard(int id, Sprite frontSprite, Sprite backSprite) {
        CardId = id;
        _frontImage.sprite = frontSprite;
        _backImage.sprite = backSprite;
    }

    public void FlipOpen() {
        IsFlipped = true;
        _frontImage.gameObject.SetActive(true);
        _backImage.gameObject.SetActive(false);
    }

    public void FlipClose() {
        IsFlipped = false;
        if (!IsMatched) {
            _frontImage.gameObject.SetActive(false);
            _backImage.gameObject.SetActive(true);
        }
    }

    public void LockMatched() {
        IsMatched = true;
        IsFlipped = true;
        _frontImage.gameObject.SetActive(true);
        _backImage.gameObject.SetActive(false);
        _cardButton.interactable = false;
    }

    public void ResetCard() {
        IsMatched = false;
        IsFlipped = false;
        _frontImage.gameObject.SetActive(false);
        _backImage.gameObject.SetActive(true);
        _cardButton.interactable = true;
    }
    public void PlayAppearAnimation() {
        _animator?.SetTrigger("Appear");
    }
}
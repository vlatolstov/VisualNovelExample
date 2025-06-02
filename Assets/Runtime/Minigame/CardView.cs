using UnityEngine;
using UnityEngine.UI;
using System;

public class CardView : MonoBehaviour {
    [Header("UI Elements")]
    [SerializeField] private Image frontImage;
    [SerializeField] private Image backImage;
    [SerializeField] private Button cardButton;

    [Header("Card Data")]
    public int CardId { get; private set; }

    public bool IsFlipped { get; private set; }
    public bool IsMatched { get; private set; }

    public event Action<CardView> OnCardClicked;

    private void Awake() {
        cardButton.onClick.AddListener(HandleClick);
    }

    private void HandleClick() {
        if (IsFlipped || IsMatched)
            return;
        OnCardClicked?.Invoke(this);
    }

    public void SetCard(int id, Sprite frontSprite, Sprite backSprite) {
        CardId = id;
        frontImage.sprite = frontSprite;
        backImage.sprite = backSprite;
    }

    public void FlipOpen() {
        IsFlipped = true;
        frontImage.gameObject.SetActive(true);
        backImage.gameObject.SetActive(false);
    }

    public void FlipClose() {
        IsFlipped = false;
        if (!IsMatched) {
            frontImage.gameObject.SetActive(false);
            backImage.gameObject.SetActive(true);
        }
    }

    public void LockMatched() {
        IsMatched = true;
        IsFlipped = true;
        frontImage.gameObject.SetActive(true);
        backImage.gameObject.SetActive(false);
        cardButton.interactable = false;
    }

    public void ResetCard() {
        IsMatched = false;
        IsFlipped = false;
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
        cardButton.interactable = true;
    }
}
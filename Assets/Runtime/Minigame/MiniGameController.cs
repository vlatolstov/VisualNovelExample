using System;
using System.Collections.Generic;
using System.Linq;

using Naninovel;

using UnityEngine;
using UnityEngine.UI;

public class MiniGameController : MonoBehaviour {
    [Header("Card Settings")]
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Transform _cardContainer;
    [SerializeField] private Sprite[] _frontSprites;
    [SerializeField] private Sprite _backSprite;
    [SerializeField] private float _flipBackDelay = 1.2f;
    

    [Header("Timer Settings")]
    [SerializeField] private float _timeLimit = 30f;
    [SerializeField] private Slider _timerBar;

    private readonly List<CardView> _allCards = new();
    private CardView _firstSelected;
    private CardView _secondSelected;

    private Action<bool> _onGameComplete;
    private int _totalMatchesNeeded;
    private int _currentMatches;
    private float _timeRemaining;
    private bool _isGameRunning;
    private bool _isComparing;

    public void StartGame(Action<bool> onComplete) {
        _onGameComplete = onComplete;
        ResetGame();
        GenerateCards();
        StartTimer();
    }

    private void ResetGame() {
        foreach (var card in _allCards)
            Destroy(card.gameObject);

        _allCards.Clear();
        _firstSelected = null;
        _secondSelected = null;
        _currentMatches = 0;
        _isGameRunning = true;
        _isComparing = false;
    }

    private void GenerateCards() {
        var pairCount = _frontSprites.Length;
        _totalMatchesNeeded = pairCount;

        List<(int id, Sprite sprite)> cardData = new();
        for (int i = 0; i < pairCount; i++) {
            cardData.Add((i, _frontSprites[i]));
            cardData.Add((i, _frontSprites[i]));
        }

        cardData = cardData
            .OrderBy(x => UnityEngine.Random.value)
            .ToList();

        foreach (var (id, frontSprite) in cardData) {
            var obj = Instantiate(_cardPrefab, _cardContainer);
            var card = obj.GetComponent<CardView>();
            card.SetCard(id, frontSprite, _backSprite);
            card.OnCardClicked += OnCardClicked;
            card.ResetCard();
            card.PlayAppearAnimation();
            _allCards.Add(card);
        }
    }

    private void OnCardClicked(CardView card) {
        if (!_isGameRunning || _isComparing || card.IsFlipped || card.IsMatched)
            return;

        card.FlipOpen();

        if (_firstSelected == null) {
            _firstSelected = card;
            return;
        }

        _secondSelected = card;
        CompareCards().Forget();
    }

    private async UniTaskVoid CompareCards() {
        _isComparing = true;

        await UniTask.Delay(TimeSpan.FromSeconds(_flipBackDelay));

        if (_firstSelected.CardId == _secondSelected.CardId) {
            _firstSelected.LockMatched();
            _secondSelected.LockMatched();
            _currentMatches++;
        }
        else {
            _firstSelected.FlipClose();
            _secondSelected.FlipClose();
        }

        _firstSelected = null;
        _secondSelected = null;
        _isComparing = false;

        if (_currentMatches >= _totalMatchesNeeded)
            EndGame(true);
    }

    private void StartTimer() {
        _timeRemaining = _timeLimit;
        TickTimer().Forget();
    }

    private async UniTaskVoid TickTimer() {
        while (_timeRemaining > 0f && _isGameRunning) {
            UpdateTimerUI();
            await UniTask.Delay(100);
            _timeRemaining -= 0.1f;
        }

        if (_isGameRunning) {
            UpdateTimerUI();
            EndGame(false);
        }
    }

    private void UpdateTimerUI() {
        float progress = Mathf.Clamp01(_timeRemaining / _timeLimit);

        if (_timerBar != null)
            _timerBar.value = progress;
    }

    private void EndGame(bool won) {
        _isGameRunning = false;
        _onGameComplete?.Invoke(won);
    }
}

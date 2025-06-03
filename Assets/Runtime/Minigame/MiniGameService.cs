using Naninovel;
using UnityEngine;
using System.Threading;

[InitializeAtRuntime]
public class MiniGameService : IMiniGameService {
    private MiniGameController _miniGameUI;
    private CanvasFader _fader;
    private UniTaskCompletionSource<bool> _gameCompletionSource;

    public bool WasLastGameSuccessful { get; private set; }

    public UniTask InitializeServiceAsync() {
        var prefab = Resources.Load<GameObject>("Naninovel/MiniGame/MiniGameUI");
        var instance = Object.Instantiate(prefab);
        Object.DontDestroyOnLoad(instance);
        instance.SetActive(false);

        _miniGameUI = instance.GetComponent<MiniGameController>();
        _fader = instance.GetComponent<CanvasFader>();
        return UniTask.CompletedTask;
    }

    public void ResetService() { }

    public void DestroyService() {
        if (_miniGameUI)
            Object.Destroy(_miniGameUI.gameObject);
    }

    public async UniTask PlayAsync(CancellationToken cancellationToken = default) {
        _gameCompletionSource = new UniTaskCompletionSource<bool>();

        await _fader.FadeInAsync();
        _miniGameUI.StartGame(OnGameCompleted);

        using (cancellationToken.Register(() => _gameCompletionSource.TrySetCanceled())) {
            try {
                WasLastGameSuccessful = await _gameCompletionSource.Task;
            }
            finally {
                await UniTask.Delay(500);
                await _fader.FadeOutAsync();
            }
        }

        Engine.GetService<ICustomVariableManager>()
            .SetVariableValue("G_gameWon", WasLastGameSuccessful.ToString());
    }

    private void OnGameCompleted(bool success) {
        _gameCompletionSource?.TrySetResult(success);
    }
}
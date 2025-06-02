using Naninovel;
using UnityEngine;
using System.Threading;

[InitializeAtRuntime]
public class MiniGameService : IMiniGameService {
    private MiniGameController _miniGameUI;
    private UniTaskCompletionSource<bool> _gameCompletionSource;

    public bool WasLastGameSuccessful { get; private set; }

    public UniTask InitializeServiceAsync() {
        var prefab = Resources.Load<GameObject>("Naninovel/MiniGame/MiniGameUI");
        var instance = Object.Instantiate(prefab);
        Object.DontDestroyOnLoad(instance);
        instance.SetActive(false);

        _miniGameUI = instance.GetComponent<MiniGameController>();
        return UniTask.CompletedTask;
    }

    public void ResetService() { }

    public void DestroyService() {
        if (_miniGameUI)
            Object.Destroy(_miniGameUI.gameObject);
    }

    public async UniTask PlayAsync(CancellationToken cancellationToken = default) {
        _gameCompletionSource = new UniTaskCompletionSource<bool>();

        _miniGameUI.gameObject.SetActive(true);
        _miniGameUI.StartGame(OnGameCompleted);

        using (cancellationToken.Register(() => _gameCompletionSource.TrySetCanceled())) {
            try {
                WasLastGameSuccessful = await _gameCompletionSource.Task;
            }
            finally {
                _miniGameUI.gameObject.SetActive(false);
            }
        }

        Engine.GetService<ICustomVariableManager>()
            .SetVariableValue("G_gameWon", WasLastGameSuccessful.ToString());
    }

    private void OnGameCompleted(bool success) {
        _gameCompletionSource?.TrySetResult(success);
    }
}
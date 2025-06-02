//using System.Threading;

//using Naninovel;

//using UnityEngine;

//public class MiniGameService : IMiniGameService {
//    private MiniGameController miniGameUI;
//    private bool isInitialized;
//    private UniTaskCompletionSource<bool> gameCompletionSource;

//    public bool WasLastGameSuccessful { get; private set; }

//    public UniTask InitializeServiceAsync() {
//        // Префаб должен быть подгружен в Resources/Naninovel/UI
//        var prefab = Resources.Load<GameObject>("Naninovel/UI/MiniGameUI");
//        var instance = Object.Instantiate(prefab);
//        miniGameUI = instance.GetComponent<MiniGameController>();
//        Object.DontDestroyOnLoad(instance.gameObject);
//        instance.SetActive(false);

//        isInitialized = true;
//        return UniTask.CompletedTask;
//    }

//    public void ResetService() { }

//    public void DestroyService() {
//        if (miniGameUI)
//            Object.Destroy(miniGameUI.gameObject);
//    }

//    public async UniTask PlayAsync(CancellationToken cancellationToken = default) {
//        if (!isInitialized)
//            throw new System.InvalidOperationException("MiniGameService not initialized.");

//        gameCompletionSource = new UniTaskCompletionSource<bool>();

//        miniGameUI.gameObject.SetActive(true);
//        //miniGameUI.StartGame(OnGameCompleted);

//        using (cancellationToken.Register(() => gameCompletionSource.TrySetCanceled())) {
//            try {
//                WasLastGameSuccessful = await gameCompletionSource.Task;
//            }
//            finally {
//                miniGameUI.gameObject.SetActive(false);
//            }
//        }
//    }

//    private void OnGameCompleted(bool success) {
//        gameCompletionSource?.TrySetResult(success);
//    }
//}

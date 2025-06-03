using UnityEngine;
using Naninovel;

public class CanvasFader : MonoBehaviour {
    [SerializeField] private CanvasGroup group;
    [SerializeField] private float duration = 0.4f;

    private void Awake() {
        if (!group)
            group = GetComponent<CanvasGroup>();
    }

    public async UniTask FadeInAsync() {
        gameObject.SetActive(true);
        group.alpha = 0;
        group.interactable = false;

        float time = 0;
        while (time < duration) {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(0, 1, time / duration);
            await UniTask.Yield();
        }

        group.alpha = 1;
        group.interactable = true;
    }

    public async UniTask FadeOutAsync() {
        group.interactable = false;

        float time = 0;
        while (time < duration) {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(1, 0, time / duration);
            await UniTask.Yield();
        }

        group.alpha = 0;
        gameObject.SetActive(false);
    }
}

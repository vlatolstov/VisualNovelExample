using UnityEngine;

public class MapLocationButton : MonoBehaviour {
    public string Id;

    public void SetEnabled(bool value) {
        gameObject.SetActive(value);
    }
}

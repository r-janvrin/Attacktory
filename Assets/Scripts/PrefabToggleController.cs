using UnityEngine;

public class PrefabToggleController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject prefabToSet;

    public void changePrefab(bool toggleValue)
    {
        if (toggleValue) mouseScript.controller.setPrefab(prefabToSet);
        else mouseScript.controller.setPrefab(null);
    }
}

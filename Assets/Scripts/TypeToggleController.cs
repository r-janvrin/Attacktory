using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleController : MonoBehaviour
{
    [SerializeField] private GameObject prefabPanel;
    [SerializeField] private Toggle toggle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void setVisible(bool toggleValue)
    {
        prefabPanel.SetActive(toggleValue);
    }
}

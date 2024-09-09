using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tmHealth;

    public void OnChangeHealth(int health)
    {
        _tmHealth.text = $"{health}";
    }
}

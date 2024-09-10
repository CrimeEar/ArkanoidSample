using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private GameObject[] _healthIcons;

    public void OnUpdateHealthUI(int health)
    {
        for(int i = 0; i < _healthIcons.Length; i++)
        {
            _healthIcons[i].SetActive(i + 1 <= health);
        }
    }
}

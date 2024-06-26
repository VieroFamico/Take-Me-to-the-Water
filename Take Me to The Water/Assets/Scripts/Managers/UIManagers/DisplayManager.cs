using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI dayText;
    public Transform moneyChangeParent; // Parent object for spawning money change text
    public GameObject moneyChangePrefab; // Prefab for the money change text

    private PlayerInventory playerInventory;
    private DayNightManager dayNightManager;

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        dayNightManager = FindObjectOfType<DayNightManager>();

        // Initial display update
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        moneyText.text = $"${playerInventory.GetMoney()}";
        //dayText.text = $"{dayNightManager.GetCurrentDay()}";
        fuelText.text = $"{(int)playerInventory.GetPlayerLoadout().GetFuelPercentage()}%";
    }

    public void ShowMoneyChange(float amount)
    {
        GameObject moneyChangeObject = Instantiate(moneyChangePrefab, moneyChangeParent);
        TextMeshProUGUI moneyChangeText = moneyChangeObject.GetComponent<TextMeshProUGUI>();

        if (amount < 0)
        {
            moneyChangeText.text = $"-${-amount}";
            moneyChangeText.color = Color.red;
        }
        else
        {
            moneyChangeText.text = $"+${amount}";
            moneyChangeText.color = Color.green;
        }

        StartCoroutine(FadeAndMoveText(moneyChangeText));
    }

    private IEnumerator FadeAndMoveText(TextMeshProUGUI text)
    {
        float duration = 2f;
        float elapsedTime = 0f;
        Vector3 startPosition = text.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0, -50, 0);

        while (elapsedTime < duration)
        {
            text.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(1, 0, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(text.gameObject);
    }
}

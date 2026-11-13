using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderCreator : MonoBehaviour
{
    public List<string> Ingredients = new List<string>(6)
    {
        "Ketchup", "Mustard", "Mayo", "Cheese", "Pickles", "Onions"
    };

    [System.Serializable]
    public struct orderSlot
    {
        public GameObject slotFather;
        public TMP_Text slotText;
    }

    public List<orderSlot> orderList = new List<orderSlot>();

    public class ActiveOrderBinding
    {
        public ClientLogic client;
        public List<string> ingredients;
    }

    private List<ActiveOrderBinding> activeOrders = new List<ActiveOrderBinding>();

    void Start()
    {
        HideAll();
    }

    void Update()
    {
        UpdateOrder();
    }

    public bool CanRegister()
    {
        int maxAllowed = GameManager.GetMaxOrders();
        return activeOrders.Count < maxAllowed;
    }

    public void RegisterClientOrder(ClientLogic client)
    {
        if (!CanRegister()) return;

        ActiveOrderBinding binding = new ActiveOrderBinding
        {
            client = client,
            ingredients = client.clientOrder
        };
        activeOrders.Add(binding);
    }

    public void RemoveClientOrder(ClientLogic client)
    {
        activeOrders.RemoveAll(binding => binding.client == client);
    }

    public void CompleteClientOrder(ClientLogic client)
    {
        activeOrders.RemoveAll(binding => binding.client == client);
    }

    public List<string> CreateOrder()
    {
        List<string> order = new List<string>();
        int hardness = GameManager.currentHardness;
        int ingredientsCount = 1 + hardness;

        for (int i = 0; i < ingredientsCount; i++)
        {
            int randomIndex = Random.Range(0, Ingredients.Count);
            order.Add(Ingredients[randomIndex]);
        }
        return order;
    }

    public void UpdateOrder()
    {
        for (int i = 0; i < orderList.Count; i++)
        {
            if (i < activeOrders.Count)
            {
                orderList[i].slotFather.SetActive(true);
                ActiveOrderBinding current = activeOrders[i];
                string orderText = string.Join(", ", current.ingredients);
                orderList[i].slotText.text = orderText;
            }
            else
            {
                orderList[i].slotFather.SetActive(false);
                orderList[i].slotText.text = "";
            }
        }
    }

    public void HideAll()
    {
        foreach (var slot in orderList)
        {
            slot.slotFather.SetActive(false);
            slot.slotText.text = "";
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class ClientLogic : MonoBehaviour
{
    public enum ClientState { MovingToStand, Ordering, MovingToArea, Food }
    public ClientState currentState = ClientState.MovingToStand;

    public Vector2 StandPosition;
    public float moveSpeed = 3f;
    public float waitTime = 1f;
    public float finalXMin = -2f;
    public float finalXMax = 10f;
    public float finalY = -5f;

    public List<string> clientOrder = new List<string>();
    public float orderTime = 12f;
    private bool orderActive = false;

    private float waitTimer = 0f;
    public OrderCreator OC;

    void Start()
    {
        OC = FindAnyObjectByType<OrderCreator>();
    }

    void Update()
    {
        if (GameManager.isOver) return;

        switch (currentState)
        {
            case ClientState.MovingToStand: 
                transform.position = Vector2.MoveTowards(transform.position, StandPosition, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, StandPosition) < 0.01f)
                {
                    currentState = ClientState.Ordering;
                    waitTimer = waitTime;
                }
                break;
            case ClientState.Ordering:
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0)
                {
                    AskOrder();
                    float randomX = Random.Range(finalXMin, finalXMax);
                    StandPosition = new Vector2(randomX, finalY);
                    currentState = ClientState.MovingToArea;
                }
                break;
            case ClientState.MovingToArea:
                transform.position = Vector2.MoveTowards(transform.position, StandPosition, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, StandPosition) < 0.01f)
                {
                    currentState = ClientState.Food;
                }
                break;
            case ClientState.Food:
                if (orderActive)
                {
                    orderTime -= Time.deltaTime;
                    if (orderTime <= 0)
                    {
                        GameManager.AddStrikes();
                        OC.RemoveClientOrder(this);
                        Destroy(gameObject);
                    }
                }
                break;
        }
    }

    void AskOrder()
    {
        clientOrder = OC.CreateOrder();
        orderActive = true;
        OC.RegisterClientOrder(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState == ClientState.Food && collision.CompareTag("Dog"))
        {
            DogLogic DL = collision.GetComponentInParent<DogLogic>();
            Transform dogFather = DL.transform;
            
            List<string> dogIngredients = GetDogOrder(dogFather);

            int correctCount = CheckOrderMatch(dogIngredients);
            CreateDog CD = FindAnyObjectByType<CreateDog>();

            if (correctCount > 0)
            {
                GameManager.AddPoints(correctCount);
                DL.isMoving = false;
                DL.isSelected = false;
                DL.UpdateVisuals();
                CD.RemoveDogs(dogFather.gameObject);
                dogFather.SetParent(transform);
                dogFather.localPosition = new Vector3(0f, 2.5f, 0f);
                OC.CompleteClientOrder(this);
                Destroy(gameObject, 1f);
            }
            else
            {
                GameManager.AddStrikes();
                CD.RemoveDogs(dogFather.gameObject);
                Destroy(dogFather.gameObject);
            }
        }
    }

    int CheckOrderMatch(List<string> dogIngredients)
    {
        if (clientOrder.Count != dogIngredients.Count) return 0;

        for (int i = 0; i < clientOrder.Count; i++)
        {
            if (clientOrder[i] != dogIngredients[i]) return 0;
        }

        return clientOrder.Count;
    }

    private List<string> GetDogOrder(Transform dogTransform)
    {
        List<string> orderedIngredients = new List<string>();
        Transform currentTransform = dogTransform;

        while (currentTransform.childCount > 0)
        {
            Transform child = currentTransform.GetChild(0);
            string ingredientName = child.name.Replace("(Clone)", "").Trim();
            orderedIngredients.Add(ingredientName);
            currentTransform = child;
        }

        return orderedIngredients;
    }
}

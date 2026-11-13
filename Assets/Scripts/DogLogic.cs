using UnityEngine;

public class DogLogic : MonoBehaviour
{
    public float fallSpeed = 2f;
    public float moveSpeed = 5f;
    public bool isMoving = false;
    public bool isSelected = false;

    public float minX = -4f;
    public float maxX = 10f;

    public SpriteRenderer SR;
    public Color OC;

    void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        OC = SR.color;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            if (isSelected)
            {
                float input = 0f;
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    input = -1f;
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    input = 1f;
                }
                
                float newX = transform.position.x + (input * moveSpeed * Time.deltaTime);
                newX = Mathf.Clamp(newX, minX, maxX);
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            }

            if (transform. position.y < -7f)
            {
                GameManager.AddStrikes();
                CreateDog CD = FindAnyObjectByType<CreateDog>();
                CD.RemoveDogs(gameObject);
                Destroy(gameObject);
            }
        }
    }

    public void UpdateVisuals()
    {
        if (isSelected)
        {
            SR.color = Color.red;
        }
        else
        {
            SR.color = OC;
        }
    }

    //Para ver el recorrido maximo de izquierda a derecha que puede hacer el perrito
    //Solo se ve si hay un perrito en patalla mientras corres el juego y pausas el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 leftLimit = new Vector3(minX,transform.position.y, 0f);
        Vector3 rightLimit = new Vector3(maxX, transform.position.y,0f);
        Gizmos.DrawLine(leftLimit, rightLimit);
    }
}
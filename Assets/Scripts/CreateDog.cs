using UnityEngine;

public class CreateDog : MonoBehaviour
{
    public GameObject mainPrefab;
    public Vector2 spawnPosition = new Vector2(-6,-3);
    private GameObject lastDog;
    public GameObject lastIngredient;

    public GameObject[] listIngredients = new GameObject[6];
    public float spawnOffset = 0.1f;

    //Area en la cual comprueba si ya existe un perrito
    public Vector2 areaCenter = new Vector2(-6,-3);
    public Vector2 areaSize = new Vector2(3,2);

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnDog();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Destroy(lastDog);
        }
        
        if (lastIngredient != null)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SpawnIngredient(0);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                SpawnIngredient(1);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                SpawnIngredient(2);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                SpawnIngredient(3);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                SpawnIngredient(4);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                SpawnIngredient(5);
            }
        }
    }

    void SpawnDog()
    {
        Collider2D dogArea = Physics2D.OverlapBox(areaCenter,areaSize,0f);

        if (dogArea != null &&dogArea.CompareTag("Dog"))
        {
            return;
        }
        else
        {
            lastDog = Instantiate(mainPrefab, spawnPosition, Quaternion.identity);
            lastIngredient = lastDog;
        }
    }

    void SpawnIngredient(int index)
    {
        GameObject selection = listIngredients[index];
        GameObject ingredient = Instantiate(selection);
        ingredient.transform.SetParent(lastIngredient.transform);
        ingredient.transform.localPosition = new Vector3(0,spawnOffset,-0.1f);
        lastIngredient = ingredient;
    }

    //Para ver el area donde se comprueba si existe un perrito en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}
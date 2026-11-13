using System.Collections.Generic;
using UnityEngine;

public class CreateDog : MonoBehaviour
{
    public GameObject mainPrefab;
    public Vector2 spawnPosition = new Vector2(-6,-3);
    public Vector2 launchPosition = new Vector2(3,5);

    private GameObject lastDog;
    public GameObject lastIngredient;

    public GameObject[] listIngredients = new GameObject[6];
    public List<GameObject> launchedDogs = new List<GameObject>();
    public int selectedDogIndex = 0;
    public float spawnOffset = 0.1f;

    //Area en la cual comprueba si ya existe un perrito
    public Vector2 areaCenter = new Vector2(-6,-3);
    public Vector2 areaSize = new Vector2(3,2);

    void Update()
    {
        if (GameManager.isOver) return;

        launchedDogs.RemoveAll(dog => dog == null);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnDog();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Destroy(lastDog);
            lastIngredient = null;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LaunchDog();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DogSelection(-1);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            DogSelection(1);
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

        SelectionUpdate();
    }

    void SpawnDog()
    {
        Collider2D dogArea = Physics2D.OverlapBox(areaCenter,areaSize,0f);

        if (dogArea != null && dogArea.CompareTag("Dog"))
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

    void LaunchDog()
    {
        DogLogic DL = lastDog.GetComponent<DogLogic>();
        
        if (DL.isMoving == true)
        {
            return;
        }
        else
        {
            lastDog.transform.position = launchPosition;
            DL.isMoving = true;
            launchedDogs.Add(lastDog);
            lastDog = null;
            lastIngredient = null;
        }
    }

    void DogSelection(int selection)
    {
        selectedDogIndex += selection;
        if (selectedDogIndex >= launchedDogs.Count)
        {
            selectedDogIndex = 0;
        }
        else if (selectedDogIndex < 0)
        {
            selectedDogIndex = launchedDogs.Count - 1;
        }
    }

    void SelectionUpdate()
    {
        launchedDogs.RemoveAll(dog => dog == null);

        if (selectedDogIndex >= launchedDogs.Count)
        {
            selectedDogIndex = 0;
        }

        for (int i = 0; i < launchedDogs.Count; i++)
        {
            DogLogic dl = launchedDogs[i].GetComponent<DogLogic>();
            bool ShouldSelected = (i == selectedDogIndex);
            dl.isSelected = ShouldSelected;
            dl.UpdateVisuals();
        }
    }

    public void RemoveDogs(GameObject dog)
    {
        if (launchedDogs.Contains(dog))
        {
            launchedDogs.Remove(dog);

            if (selectedDogIndex >= launchedDogs.Count)
            {
                selectedDogIndex = Mathf.Max(0, launchedDogs.Count - 1);
            }
        }
    }

    public void DestroyFirstDog()
    {
        if (launchedDogs.Count > 0)
        {
            GameObject oldestDog = launchedDogs[0];
            Destroy(oldestDog);
            launchedDogs.RemoveAt(0);
            if (selectedDogIndex >= launchedDogs.Count)
            {
                selectedDogIndex = Mathf.Max(0, launchedDogs.Count - 1);
            }
        }
    }

    //Para ver el area donde se comprueba si existe un perrito en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}
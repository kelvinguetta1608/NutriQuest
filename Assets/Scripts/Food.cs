using UnityEngine;

public class Food : MonoBehaviour
{
    private bool isTouched = false;
    private Transform blenderTransform;
    private float destroyY = -5.5f;
    private GameManager gameManager;
    private FoodType foodType;

    void Start()
    {
        gameManager = GameManager.Instance;
        foodType = GetComponent<FoodType>();
        
        if (blenderTransform == null)
            blenderTransform = GameObject.Find("Blender")?.transform;
    }

    void Update()
    {
        if (!isTouched)
        {
            transform.Translate(Vector3.up * gameManager.GetFoodSpeed() * Time.deltaTime);
            if (transform.position.y > 6f) Destroy(gameObject);
        }
        else if (IsCorrectFood())
        {
            MoveToBlender();
        }
        else
        {
            FallDown();
        }
    }

    bool IsCorrectFood()
    {
        return foodType != null && 
               foodType.category.ToString() == gameManager.GetTargetCategory();
    }

    void MoveToBlender()
    {
        if (blenderTransform != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                blenderTransform.position, 
                5f * Time.deltaTime
            );
            
            if (Vector3.Distance(transform.position, blenderTransform.position) < 0.1f)
                Destroy(gameObject);
        }
    }

    void FallDown()
    {
        transform.Translate(Vector3.down * 5f * Time.deltaTime);
        if (transform.position.y < destroyY)
            Destroy(gameObject);
    }

    void OnMouseDown()
    {
        if (!isTouched && gameManager != null)
        {
            isTouched = true;
            
            if (IsCorrectFood())
            {
                gameManager.AddScore(10);
            }
            else
            {
                gameManager.LoseLife();
                Debug.Log($"Alimento incorrecto tocado. Categoría: {foodType.category}");
            }
        }
    }
}
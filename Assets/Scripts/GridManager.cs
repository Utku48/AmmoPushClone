using UnityEngine;

public class GridManager : MonoBehaviour
{

    public int rows = 10;
    public int columns = 10;
    public float cellSize = 1.0f;


    public GameObject gridPrefab;


    [ContextMenu("Generate Grid")]
    public void GenerateGrid()
    {

        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }


        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {

                GameObject instance = Instantiate(gridPrefab, transform);

                instance.transform.position = new Vector3(x * cellSize, 0, y * cellSize);
            }
        }
    }
}

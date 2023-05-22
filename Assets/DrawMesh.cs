using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEditor;
using UnityEngine;

public class DrawMesh : MonoBehaviour
{

    public static DrawMesh Instance { get; private set; }



    [SerializeField] private Material drawMeshMaterial;

    private GameObject lastGameObject;
    private int lastSortingOrder;
    private Mesh mesh;
    private Vector3 lastMouseWorldPosition;
    private float lineThickness = 10f;
    private Color lineColor = Color.blue;
    private List<GameObject> meshObjects = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!GameManager.IsPointerOverUI()&& GameManager.Instance.IsDrawing())
        {
            Vector3 mouseWorldPosition = GameManager.GetMouseWorldPosition();
            if (Input.GetMouseButtonDown(0))
            {
                CreateMeshObject();
                mesh = MeshUtils.CreateMesh(mouseWorldPosition, mouseWorldPosition, mouseWorldPosition, mouseWorldPosition);
                mesh.MarkDynamic();
                meshObjects.Add(lastGameObject);
                lastGameObject.GetComponent<MeshFilter>().mesh = mesh;
                Material material = new Material(drawMeshMaterial);
                material.color = lineColor;
                lastGameObject.GetComponent<MeshRenderer>().material = material;
            }

            if (Input.GetMouseButton(0))
            {
                float minDistance = 10f;
                if (Vector2.Distance(lastMouseWorldPosition, mouseWorldPosition) > minDistance)
                {
                    Vector2 forwardVector = (mouseWorldPosition - lastMouseWorldPosition).normalized;

                    lastMouseWorldPosition = mouseWorldPosition;

                    MeshUtils.AddLinePoint(mesh, mouseWorldPosition, lineThickness);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                MeshUtils.AddLinePoint(mesh, mouseWorldPosition, 0f);
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z))
            {
                UndoLastMeshObject();
            }
        }
    }

    private void CreateMeshObject()
    {
        lastGameObject = new GameObject("DrawMeshSingle", typeof(MeshFilter), typeof(MeshRenderer));
        lastSortingOrder++;
        lastGameObject.GetComponent<MeshRenderer>().sortingOrder = lastSortingOrder;
    }

    public void SetThickness(float lineThickness)
    {
        this.lineThickness = lineThickness;
    }

    public void SetColor(Color lineColor)
    {
        this.lineColor = lineColor;
    }
    public void UndoLastMeshObject()
    {
        if (meshObjects.Count > 0)
        {
            GameObject lastMeshObject = meshObjects[meshObjects.Count - 1];
            meshObjects.RemoveAt(meshObjects.Count - 1);
            Destroy(lastMeshObject);
        }
    }
    public void ClearCanva()
    {
        if (meshObjects.Count > 0)
        {
            foreach (var mesh in meshObjects)
            {
                Destroy(mesh);
            }
            meshObjects.Clear();
        }
    }

}

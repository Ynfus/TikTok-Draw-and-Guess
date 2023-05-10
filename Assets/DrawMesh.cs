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






















    /////////////////////////////////////////////////////////////////////////////////////////////////////
    //[SerializeField] private Transform debugVisual2;
    //[SerializeField] private Transform debugVisual1;
    //private Vector3 lastMousePosition;
    //private Mesh mesh;

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        mesh = new Mesh();
    //        Vector3[] vertices = new Vector3[4];
    //        Vector2[] uv = new Vector2[4];
    //        int[] triangles = new int[6];

    //        vertices[0] = GameManager.GetMouseWorldPosition();
    //        vertices[1] = GameManager.GetMouseWorldPosition();
    //        vertices[2] = GameManager.GetMouseWorldPosition();
    //        vertices[3] = GameManager.GetMouseWorldPosition();

    //        uv[0] = Vector2.zero;
    //        uv[1] = Vector2.zero;
    //        uv[2] = Vector2.zero;
    //        uv[3] = Vector2.zero;

    //        triangles[0] = 0;
    //        triangles[1] = 3;
    //        triangles[2] = 1;

    //        triangles[3] = 1;
    //        triangles[4] = 3;
    //        triangles[5] = 2;


    //        mesh.vertices = vertices;
    //        mesh.uv = uv;
    //        mesh.triangles = triangles;
    //        mesh.MarkDynamic();

    //        GetComponent<MeshFilter>().mesh = mesh;
    //        lastMousePosition = GameManager.GetMouseWorldPosition();
    //    }
    //    if (Input.GetMouseButton(0))
    //    {
    //        float minDis = .1f;
    //        if (Vector3.Distance(GameManager.GetMouseWorldPosition(), lastMousePosition) > minDis)
    //        {
    //            Vector3[] vertices = new Vector3[mesh.vertices.Length + 2];
    //            Vector2[] uv = new Vector2[mesh.uv.Length + 2];
    //            int[] triangles = new int[mesh.triangles.Length + 6];

    //            mesh.vertices.CopyTo(vertices, 0);
    //            mesh.uv.CopyTo(uv, 0);
    //            mesh.triangles.CopyTo(triangles, 0);


    //            int vIndex = vertices.Length - 4;
    //            int vIndex0 = vIndex + 0;
    //            int vIndex1 = vIndex + 1;
    //            int vIndex2 = vIndex + 2;
    //            int vIndex3 = vIndex + 3;
    //            Vector3 mouseForwardVector = (GameManager.GetMouseWorldPosition() - lastMousePosition).normalized;
    //            Vector3 normal2D = new Vector3(0, 0, -1f);
    //            float lineThickness = .1f;
    //            Vector3 newVertexUp = GameManager.GetMouseWorldPosition() + Vector3.Cross(mouseForwardVector, normal2D) * lineThickness;
    //            Vector3 newVertexDown = GameManager.GetMouseWorldPosition() + Vector3.Cross(mouseForwardVector, normal2D * -1f) * lineThickness;


    //            vertices[vIndex2] = newVertexUp;
    //            vertices[vIndex3] = newVertexDown;

    //            uv[vIndex2] = Vector2.zero;
    //            uv[vIndex3] = Vector2.zero;

    //            int tIndex = triangles.Length - 6;

    //            triangles[tIndex + 0] = vIndex0;
    //            triangles[tIndex + 1] = vIndex2;
    //            triangles[tIndex + 2] = vIndex1;

    //            triangles[tIndex + 3] = vIndex1;
    //            triangles[tIndex + 4] = vIndex2;
    //            triangles[tIndex + 5] = vIndex3;

    //            mesh.vertices = vertices;
    //            mesh.uv = uv;
    //            mesh.triangles = triangles;
    //            mesh.MarkDynamic();

    //            lastMousePosition = GameManager.GetMouseWorldPosition();
    //        }
    //    }
    //}
}

using _Utils;
using _Utils.NoisesLib.NoisesStructs;
using ProceduralMeshes;
using ProceduralMeshes.Generators;
using ProceduralMeshes.Streams;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralSurfaceVisualization : MonoBehaviour
{
    private static AdvancedMeshJobScheduleDelegate[] jobs =
    {
        MeshJob<SquareGrid, SingleStream>.ScheduleParallel,
        MeshJob<SharedSquareGrid, SingleStream>.ScheduleParallel,
        MeshJob<SharedTriangleGrid, SingleStream>.ScheduleParallel,
        MeshJob<FlatHexagonGrid, SingleStream>.ScheduleParallel,
        MeshJob<PointyHexagonGrid, SingleStream>.ScheduleParallel,
        MeshJob<CubeSphere, SingleStream>.ScheduleParallel,
        MeshJob<SharedCubeSphere, PositionStream>.ScheduleParallel,
        MeshJob<Icosphere, PositionStream>.ScheduleParallel,
        MeshJob<GeoIcosphere, PositionStream>.ScheduleParallel,
        MeshJob<Octasphere, SingleStream>.ScheduleParallel,
        MeshJob<GeoOctasphere, SingleStream>.ScheduleParallel,
        MeshJob<UVSphere, SingleStream>.ScheduleParallel
    };

    public enum MeshType
    {
        SquareGrid,
        SharedSquareGrid,
        SharedTriangleGrid,
        FlatHexagonGrid,
        PointyHexagonGrid,
        CubeSphere,
        SharedCubeSphere,
        Icosphere,
        GeoIcosphere,
        Octasphere,
        GeoOctasphere,
        UVSphere
    };
    
    [SerializeField]
    bool recalculateNormals, recalculateTangents;

    [SerializeField] private MeshType meshType;

    [System.Flags]
    public enum MeshOptimizationMode
    {
        Nothing = 0,
        ReorderIndices = 1,
        ReorderVertices = 0b10
    }

    [SerializeField] private MeshOptimizationMode meshOptimization;

    [SerializeField, Range(1, 50)] private int resolution = 1;

    [SerializeField, Range(-1f, 1f)] float displacement = 0.5f;

    [SerializeField] NoiseSettings noiseSettings = NoiseSettings.Default;

    [SerializeField] SpaceTRS domain = new SpaceTRS
    {
        scale = 1f
    };

    [System.Flags]
    public enum GizmoMode
    {
        Nothing = 0,
        Vertices = 1,
        Normals = 0b10,
        Tangents = 0b100,
        Triangles = 0b1000
    }

    [SerializeField] private GizmoMode gizmos;

    public enum MaterialMode
    {
        Displacement,
        Flat,
        LatLonMap,
        CubeMap
    }

    [SerializeField] private MaterialMode material;

    [SerializeField] private Material[] materials;

    private Mesh mesh;

    [System.NonSerialized] private Vector3[] vertices, normals;

    [System.NonSerialized] private Vector4[] tangents;

    [System.NonSerialized] private int[] triangles;

    private void Awake()
    {
        mesh = new Mesh
        {
            name = "Procedural Mesh"
        };
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void OnDrawGizmos()
    {
        if (gizmos == GizmoMode.Nothing || mesh == null)
        {
            return;
        }

        bool drawVertices = (gizmos & GizmoMode.Vertices) != 0;
        bool drawNormals = (gizmos & GizmoMode.Normals) != 0;
        bool drawTangents = (gizmos & GizmoMode.Tangents) != 0;
        bool drawTriangles = (gizmos & GizmoMode.Triangles) != 0;

        if (vertices == null)
        {
            vertices = mesh.vertices;
        }

        if (drawNormals && normals == null)
        {
            drawNormals = mesh.HasVertexAttribute(VertexAttribute.Normal);
            if (drawNormals)
            {
                normals = mesh.normals;
            }
        }

        if (drawTangents && tangents == null)
        {
            drawTangents = mesh.HasVertexAttribute(VertexAttribute.Tangent);
            if (drawTangents)
            {
                tangents = mesh.tangents;
            }
        }

        if (drawTriangles && triangles == null)
        {
            triangles = mesh.triangles;
        }

        Transform t = transform;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 position = t.TransformPoint(vertices[i]);
            if (drawVertices)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(position, 0.02f);
            }

            if (drawNormals)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(position, t.TransformDirection(normals[i]) * 0.2f);
            }

            if (drawTangents)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(position, t.TransformDirection(tangents[i]) * 0.2f);
            }
        }

        if (drawTriangles)
        {
            float colorStep = 1f / (triangles.Length - 3);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                float c = i * colorStep;
                Gizmos.color = new Color(c, 0f, c);
                Gizmos.DrawSphere(
                    t.TransformPoint((
                        vertices[triangles[i]] +
                        vertices[triangles[i + 1]] +
                        vertices[triangles[i + 2]]
                    ) * (1f / 3f)),
                    0.02f
                );
            }
        }
    }

    private void OnValidate() => enabled = true;

    private void Update()
    {
        GenerateMesh();
        enabled = false;

        vertices = null;
        normals = null;
        tangents = null;
        triangles = null;

        GetComponent<MeshRenderer>().material = materials[(int)material];
    }

    private void GenerateMesh()
    {
        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData meshData = meshDataArray[0];

        SurfaceJob<Lattice2D<LatticeNormal, NoisePerlin>>.ScheduleParallel(
            meshData,
            resolution,
            noiseSettings,
            domain,
            displacement,
            jobs[(int)meshType](mesh, meshData, resolution, default, new Vector3(0f, Mathf.Abs(displacement)), true)
        ).Complete();

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        
        if (recalculateNormals) {
            mesh.RecalculateNormals();
        }
        if (recalculateTangents) {
            mesh.RecalculateTangents();
        }

        if (meshOptimization == MeshOptimizationMode.ReorderIndices)
        {
            mesh.OptimizeIndexBuffers();
        }
        else if (meshOptimization == MeshOptimizationMode.ReorderVertices)
        {
            mesh.OptimizeReorderVertexBuffer();
        }
        else if (meshOptimization != MeshOptimizationMode.Nothing)
        {
            mesh.Optimize();
        }
    }
}
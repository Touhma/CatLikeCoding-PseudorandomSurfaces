using System.Runtime.InteropServices;
using ProceduralMeshes;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Mathematics;

using static Unity.Mathematics.math;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AdvancedSingleStreamProceduralMesh : MonoBehaviour {
	private void OnEnable () {
		int vertexAttributeCount = 4;
		int vertexCount = 4;
		int triangleIndexCount = 6;

		Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
		Mesh.MeshData meshData = meshDataArray[0];

		NativeArray<VertexAttributeDescriptor> vertexAttributes = new NativeArray<VertexAttributeDescriptor>(
			vertexAttributeCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory
		);
		vertexAttributes[0] = new VertexAttributeDescriptor(dimension: 3);
		vertexAttributes[1] = new VertexAttributeDescriptor(
			VertexAttribute.Normal, dimension: 3
		);
		vertexAttributes[2] = new VertexAttributeDescriptor(
			VertexAttribute.Tangent, VertexAttributeFormat.Float32, 4
		);
		vertexAttributes[3] = new VertexAttributeDescriptor(
			VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2
		);
		meshData.SetVertexBufferParams(vertexCount, vertexAttributes);
		vertexAttributes.Dispose();

		NativeArray<Vertex> vertices = meshData.GetVertexData<Vertex>();

		float h0 = 0f, h1 = 1f;

		Vertex vertex = new Vertex {
			normal = back(),
			tangent = float4(h1, h0, h0, -1f)
		};

		vertex.position = 0f;
		vertex.texCoord0 = h0;
		vertices[0] = vertex;

		vertex.position = right();
		vertex.texCoord0 = float2(h1, h0);
		vertices[1] = vertex;

		vertex.position = up();
		vertex.texCoord0 = float2(h0, h1);
		vertices[2] = vertex;

		vertex.position = float3(1f, 1f, 0f);
		vertex.texCoord0 = h1;
		vertices[3] = vertex;

		meshData.SetIndexBufferParams(triangleIndexCount, IndexFormat.UInt32);
		NativeArray<uint> triangleIndices = meshData.GetIndexData<uint>();
		triangleIndices[0] = 0;
		triangleIndices[1] = 2;
		triangleIndices[2] = 1;
		triangleIndices[3] = 1;
		triangleIndices[4] = 2;
		triangleIndices[5] = 3;

		Bounds bounds = new Bounds(new Vector3(0.5f, 0.5f), new Vector3(1f, 1f));

		meshData.subMeshCount = 1;
		meshData.SetSubMesh(0, new SubMeshDescriptor(0, triangleIndexCount) {
			bounds = bounds,
			vertexCount = vertexCount
		}, MeshUpdateFlags.DontRecalculateBounds);

		Mesh mesh = new Mesh {
			bounds = bounds,
			name = "Procedural Mesh"
		};
		Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
		
		GetComponent<MeshFilter>().mesh = mesh;
	}
}
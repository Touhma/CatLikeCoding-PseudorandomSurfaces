using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Mathematics;

using static Unity.Mathematics.math;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AdvancedMultiStreamProceduralMesh : MonoBehaviour {
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
			VertexAttribute.Normal, dimension: 3, stream: 1
		);
		vertexAttributes[2] = new VertexAttributeDescriptor(
			VertexAttribute.Tangent, VertexAttributeFormat.Float32, 4, 2
		);
		vertexAttributes[3] = new VertexAttributeDescriptor(
			VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2, 3
		);
		meshData.SetVertexBufferParams(vertexCount, vertexAttributes);
		vertexAttributes.Dispose();

		NativeArray<float3> positions = meshData.GetVertexData<float3>();
		positions[0] = 0f;
		positions[1] = right();
		positions[2] = up();
		positions[3] = float3(1f, 1f, 0f);

		NativeArray<float3> normals = meshData.GetVertexData<float3>(1);
		normals[0] = normals[1] = normals[2] = normals[3] = back();

		float h0 = 0f, h1 = 1f;

		NativeArray<float4> tangents = meshData.GetVertexData<float4>(2);
		tangents[0] = tangents[1] = tangents[2] = tangents[3] =
			float4(h1, h0, h0, -1f);
		
		NativeArray<float2> texCoords = meshData.GetVertexData<float2>(3);
		texCoords[0] = h0;
		texCoords[1] = float2(h1, h0);
		texCoords[2] = float2(h0, h1);
		texCoords[3] = h1;

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
using System.Security.Cryptography;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// https://dots-tutorial.moetsi.com/unity-ecs/spawn-and-move-prefabs-in-unity-ecs
/// </summary>
public class Testing : MonoBehaviour
{
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;
    private Entity cubeEntity;
    // Start is called before the first frame update
    void Start()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype archetype = entityManager.CreateArchetype(
                        typeof(Translation),
                        typeof(Rotation),
                        typeof(RenderMesh),
                        typeof(RenderBounds),
                        typeof(LocalToWorld)
            );

        // Create the cube entity
        cubeEntity = entityManager.CreateEntity(archetype);

        entityManager.AddComponentData(cubeEntity, new Translation
        {
            Value = new float3(1.1f, 1.1f, 1.1f)
        });
        //entityManager.AddComponentData(cubeEntity, new RenderBounds
        //{
        //    Value = _mesh.bounds.ToAABB()
        //});
        entityManager.AddComponentData(cubeEntity, new LocalToWorld());
        var desc = new RenderMeshDescription(
             CreateCube(),
            _material,
            shadowCastingMode: ShadowCastingMode.Off,
            receiveShadows: false
            );
        var entity = entityManager.CreateEntity();
        //RenderMeshUtility.AddComponents(
        //    entity,
        //    entityManager,
        //    desc);

        // Set the render mesh for the cube entity
        entityManager.AddSharedComponentData(cubeEntity, new RenderMesh()
        {
            mesh = _mesh,
            material = _material
        });
    }
    private Mesh CreateCube()
    {
        Vector3[] vertices = {
            new Vector3 (0, 0, 0),
            new Vector3 (1, 0, 0),
            new Vector3 (1, 1, 0),
            new Vector3 (0, 1, 0),
            new Vector3 (0, 1, 1),
            new Vector3 (1, 1, 1),
            new Vector3 (1, 0, 1),
            new Vector3 (0, 0, 1),
        };

        int[] triangles = {
            0, 2, 1, //front
            0, 3, 2,
            2, 3, 4, //top
            2, 4, 5,
            1, 2, 5, //right
            1, 5, 6,
            0, 7, 4, //left
            0, 4, 3,
            5, 4, 7, //back
            5, 7, 6,
            0, 6, 7, //bot
            0, 1, 6
        };

        var mesh = new Mesh { vertices = vertices, triangles = triangles };
        mesh.Optimize();
        mesh.RecalculateNormals();
        return mesh;
    }
}

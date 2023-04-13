using Unity.Entities;

[GenerateAuthoringComponent]
public struct RotatingCubeComponent : IComponentData
{
    public Entity Prefab;
}
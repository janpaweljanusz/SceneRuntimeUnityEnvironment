using Unity.Entities;

[GenerateAuthoringComponent]
public struct CubePrefab : IComponentData
{
    public Entity Value;
}

public struct CubeId : IComponentData
{
    public int Value;
}
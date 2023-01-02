using Unity.Entities;
using UnityEngine;

// Authoring MonoBehaviors are regular gameObject components.
class ConfigAuthoring : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject GroundPrefab;
    public UnityEngine.GameObject BallPrefab;
    public UnityEngine.GameObject CannonPrefab;
    public int xScale;
    public int yScale;
    public int setupStage;
    public int maxHeight;

    public int direction;
    public float cannonCooldown;
}

// bakers convert authoring mb's into entities and components
class ConfigBaker : Baker<ConfigAuthoring>
{
    public override void Bake(ConfigAuthoring authoring)
    {
        AddComponent(new Config
        {
            Ground = GetEntity(authoring.GroundPrefab),
            Ball = GetEntity(authoring.BallPrefab),
            Cannon = GetEntity(authoring.CannonPrefab),
            xScale = authoring.xScale,
            yScale = authoring.yScale,
            setupStage = authoring.setupStage,
            maxHeight = authoring.maxHeight,
            direction = authoring.direction,
            cannonCooldown = authoring.cannonCooldown

        }) ;


    }
}

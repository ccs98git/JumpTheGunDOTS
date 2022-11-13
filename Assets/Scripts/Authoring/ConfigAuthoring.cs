using Unity.Entities;

// Authoring MonoBehaviors are regular gameObject components.
class ConfigAuthoring : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject GroundPrefab;
}

// bakers convert authoring mb's into entities and components
class ConfigBaker : Baker<ConfigAuthoring>
{
    public override void Bake(ConfigAuthoring authoring)
    {
        AddComponent(new Config { 
            Ground = GetEntity(authoring.GroundPrefab)
        });


    }
}

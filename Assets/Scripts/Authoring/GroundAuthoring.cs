using Unity.Entities;

// Authoring MonoBehaviors are regular gameObject components.
class GroundAuthoring : UnityEngine.MonoBehaviour
{
}

// bakers convert authoring mb's into entities and components
class GroundBaker : Baker<GroundAuthoring>
{
    public override void Bake(GroundAuthoring authoring)
    {
        AddComponent<Ground>();
    }
}


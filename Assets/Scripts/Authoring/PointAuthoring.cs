using Unity.Entities;
using Unity.Rendering;

// Authoring MonoBehaviors are regular gameObject components.
class PointAuthoring : UnityEngine.MonoBehaviour
{ }

// bakers convert authoring mb's into entities and components
class PointBaker : Baker<PointAuthoring>
{
    public override void Bake(PointAuthoring authoring)
    {
        AddComponent<Point>();
    }
}
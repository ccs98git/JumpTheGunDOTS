using Unity.Entities;

// Authoring MonoBehaviors are regular gameObject components.
class CannonAuthoring : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject CannonBallPrefab;
}

// bakers convert authoring mb's into entities and components
class CannonBaker : Baker<CannonAuthoring>
{
    public override void Bake(CannonAuthoring authoring)
    {
        AddComponent(new Cannon {
            CannonBall = GetEntity(authoring.CannonBallPrefab),
        });
    }
}
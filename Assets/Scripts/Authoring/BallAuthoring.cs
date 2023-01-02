using Unity.Entities;

// Authoring MonoBehaviors are regular gameObject components.
class BallAuthoring : UnityEngine.MonoBehaviour
{
    public bool isTraversing;
}

// bakers convert authoring mb's into entities and components
class BallBaker : Baker<BallAuthoring>
{
    public override void Bake(BallAuthoring authoring)
    {
        AddComponent<Ball>(new Ball
        {
            isTraversing = false
        }) ;

    }
}


using Unity.Entities;
using UnityEngine;

public class DoNotMoveTagAuthoring : MonoBehaviour
{
    
    public class MyBaker : Baker<DoNotMoveTagAuthoring>
    {
        public override void Bake(DoNotMoveTagAuthoring authoring)
        {
            AddComponent(new DoNotMoveTag());
        }
    }
}


public struct DoNotMoveTag : IComponentData
{
    
}
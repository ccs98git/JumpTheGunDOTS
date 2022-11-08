using Unity.Entities;
using UnityEngine;

public class MovementAuthoring : MonoBehaviour
{
    public float moveSpeed; 
    
    public class MyBaker : Baker<MovementAuthoring>
    {
        public override void Bake(MovementAuthoring authoring)
        {
            AddComponent(new moveSpeed
            {
                Value = authoring.moveSpeed
            });
        }
    }
}


public struct moveSpeed : IComponentData
{
    public float Value;
}
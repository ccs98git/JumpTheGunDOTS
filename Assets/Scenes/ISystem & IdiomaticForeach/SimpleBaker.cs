using Unity.Entities;
using UnityEngine;

public class SimpleBaker : MonoBehaviour
{
    public float speed; 
    
    public class MyBaker : Baker<SimpleBaker>
    {
        public override void Bake(SimpleBaker authoring)
        {
            AddComponent(new IFESpeed{Value = authoring.speed});
        }
    }
}

public struct IFESpeed : IComponentData
{
    public float Value; 
}
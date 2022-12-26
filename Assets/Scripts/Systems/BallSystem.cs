using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

// Unmanaged systems can be BurstCompiled, tag needs to be on the struct itself, as well as derived methods of ISystem
[BurstCompile]
partial struct BallSystem : ISystem
{

    // all ISystem derived methods need to be implemented, even if empty.
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    { }
    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
		var config = SystemAPI.GetSingleton<Config>();

        if (config.setupStage >= 3) {
			// Ball movement - only one ball
			foreach (var player in SystemAPI.Query<BallAspect>())
			{
                
             
            }
        }


        /*
         
        // https://www.youtube.com/watch?v=YzezqDqr7RM
                // get mouse position
                float y = (0.2f + config.maxHeight) / 2;
				Vector3 mouseWorldPos = new Vector3(0, y, 0);
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				float t = (y - ray.origin.y) / ray.direction.y;
				mouseWorldPos.x = ray.origin.x + t * ray.direction.x;
				mouseWorldPos.z = ray.origin.z + t * ray.direction.z;
				Vector3 mouseLocalPos = mouseWorldPos;

                
				if (player.transform.parent != null) // I have no idea what this line of code is for.
				{
					mouseLocalPos = transform.parent.InverseTransformPoint(mouseWorldPos);
				}
				

        Vector2Int mouseBoxPos = new Vector2Int(Mathf.RoundToInt(mouseLocalPos.x / 1), Mathf.RoundToInt(mouseLocalPos.y / 1));
        Vector2Int movePos = mouseBoxPos;
        Vector2Int currentPos = new Vector2Int(Mathf.RoundToInt(player.transform.LocalPosition.x / 1), Mathf.RoundToInt(player.transform.LocalPosition.y / 1));

        if (Mathf.Abs(mouseBoxPos.x - currentPos.x) > 1 || Mathf.Abs(mouseBoxPos.y - currentPos.y) > 1)
        {
            // mouse position is too far away.  Find closest position
            movePos = currentPos;
            if (mouseBoxPos.x != currentPos.x)
            {
                movePos.x += mouseBoxPos.x > currentPos.x ? 1 : -1;
            }
            if (mouseBoxPos.y != currentPos.y)
            {
                movePos.y += mouseBoxPos.y > currentPos.y ? 1 : -1;
            }

        }
        //foreach(var groundE in SystemAPI.)
        // don't move if target is occupied
        //if (TerrainArea.instance.OccupiedBox(movePos.x, movePos.y))
        //{
        //    movePos.Set(endBox.col, endBox.row);
        //}

        */



    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Actions
{
	public override void Act(StateController controller)
	{
		Patrol (controller);
	}

	private void Patrol(StateController controller)
	{
		Debug.Log ("Patrol");
		controller.navMeshAgent.destination = controller.wayPointList [controller.nextWayPoint].position;

		if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending) 
		{
			Debug.Log ("controller.nextWayPoint"+controller.nextWayPoint);
			controller.nextWayPoint = (controller.nextWayPoint + 1) % controller.wayPointList.Count;
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Actions
{
	public override void Act (StateController controller)
	{
		Chase (controller); 
	}

	private void Chase(StateController controller)
	{
//		Debug.Log ("Chase");
		controller.navMeshAgent.destination = controller.chaseTarget.position;
		if (!controller.navMeshAgent.isStopped) {
			controller.navMeshAgent.Move (controller.chaseTarget.position);
		}
	}
}
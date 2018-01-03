﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/Scan")]
public class ScanDecision : Decision 
{
	public override bool Decide (StateController controller)
	{
		bool noEnemyInSight = Scan (controller);
		Debug.Log ("noEnemyInSight"+noEnemyInSight);
		return noEnemyInSight;
	}

	private bool Scan(StateController controller)
	{
		Debug.Log ("Scan");
		controller.navMeshAgent.isStopped = true;

		controller.transform.Rotate (0, controller.enemyStats.searchingTurnSpeed * Time.deltaTime, 0);
		return controller.CheckIfCountDownElapsed (controller.enemyStats.searchDuration);
	}
}
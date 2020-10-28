﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Obi
{
	[RequireComponent(typeof(ObiActor))]
	public class ColorRandomizer : MonoBehaviour
	{
		private ObiActor actor;
		public Gradient gradient = new Gradient();

		private void Start()
        {
			actor = GetComponent<ObiActor>();

            for (int i = 0; i < actor.solverIndices.Length; ++i)
            {
                actor.solver.colors[actor.solverIndices[i]] = gradient.Evaluate(UnityEngine.Random.value);
			}
		}
	
	}
}


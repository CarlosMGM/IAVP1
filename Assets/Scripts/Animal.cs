﻿using UnityEngine;

namespace UCM.IAV.Movimiento
{
	[RequireComponent(typeof(Seguir))]
	public class Animal : Agente
	{
		protected JugadorAgente player;
		protected Seguir seguir;

		// Start is called before the first frame update
		public void Start()
		{
			player = GameObject.FindGameObjectWithTag("Player").GetComponent<JugadorAgente>();
			seguir = GetComponent<Seguir>();
            seguir.transformObjetivo = player.transform;
		}

		// Update is called once per frame
		public new void Update()
		{
			base.Update();

			var sound = player.flauta;

			var layerMask = 1 << 8;

			// This would cast rays only against colliders in layer 8.
			// But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
			layerMask = ~layerMask;

			RaycastHit hit;
			// Does the ray intersect any objects excluding the player layer
			if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2,
				layerMask))
				Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance,
					Color.yellow);
			//  print(hit.);
			else
				Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 2, Color.white);
		}
	}
}
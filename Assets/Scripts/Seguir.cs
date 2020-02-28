/*    
   Copyright (C) 2020 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    ///     Clase para modelar el comportamiento de SEGUIR a otro agente
    /// </summary>
    public class Seguir : ComportamientoAgente
	{
        /// <summary>
        ///     Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        private float cooldown = 1;

		private float count = 1;
		private Vector3 dir = Vector3.zero;
        private float deceleracion = 0;
        public float radioObjetivo;
        public float radioFreno;
        bool freno = false;

		public override Direccion GetDireccion()
		{
			var direccion = new Direccion();
			var layerMask = 1 << 8;

			layerMask = ~layerMask;

			RaycastHit hit;

			count += 0.5f * Time.deltaTime;
			//UnityEngine.Physics.Raycast(transform.position, objetivo.transform.position, out hit, 2, layerMask);
			var tf = transform;
			Debug.DrawRay(tf.position, (objetivo.transform.position - tf.position) * 2, Color.red);
			if (true /*count >= cooldown*/)
			{
				count = 0;
				//direccion.lineal = objetivo.transform.position - transform.position;
				if (Physics.Raycast(transform.position, objetivo.transform.position - transform.position, out hit, 2.0f,
					layerMask))
				{
					direccion.lineal = objetivo.transform.position - transform.position;
					var i = Random.Range(-1.0f, 1.0f);
					var newPath = Vector3.zero /*hit.transform.position*/;
					//print(hit.transform.eulerAngles.y);
					var eulerAngles = hit.transform.eulerAngles;
					if (i > 0)
					{
						newPath = new Vector3(Mathf.Cos(eulerAngles.y), 0,
							Mathf.Sin(eulerAngles.y));
						//direccion.lineal = newPath;
					}

					else
					{
						newPath = new Vector3(Mathf.Cos(eulerAngles.y), 0,
							Mathf.Sin(eulerAngles.y));
					}

					//newPath -= new UnityEngine.Vector3((UnityEngine.Mathf.Cos(hit.transform.rotation.y) * (hit.transform.localScale.x)) / 2,
					//                                    0, (UnityEngine.Mathf.Sin(hit.transform.rotation.y) * (hit.transform.localScale.y)) / 2);
					//direccion.lineal = newPath/* - transform.position*/;
					//direccion.lineal.Normalize();
					direccion.lineal = newPath;
				}
				else
				{
					direccion.lineal = objetivo.transform.position - transform.position;
				}

				dir = direccion.lineal;
			}
			else
			{
				direccion.lineal = dir;
			}

            float instanceAcceleration;

            if(direccion.lineal.magnitude > radioFreno)
            {
                freno = false;
                instanceAcceleration = agente.aceleracionMax;
            }
            else
            {
                if(freno == false)
                {
                    deceleracion = -agente.velocidad.sqrMagnitude / (2 * direccion.lineal.magnitude);
                    freno = true;
                }
                instanceAcceleration = deceleracion;
            }


            direccion.lineal.Normalize();
            direccion.lineal *= instanceAcceleration;

            return direccion;
        }

		public virtual void SeguirJugador()
		{
			var util = new Direccion {lineal = objetivo.transform.position - transform.position};
			var xUtil = Mathf.Abs(util.lineal.x);
			var zUtil = Mathf.Abs(util.lineal.z);
			if (xUtil + zUtil > radioObjetivo)
			{
				agente.Run();
				if (agente.mezclarPorPeso)
					agente.SetDireccion(GetDireccion(), peso);
				else if (agente.mezclarPorPrioridad)
					agente.SetDireccion(GetDireccion(), prioridad);
				else
					agente.SetDireccion(GetDireccion());
			}
			else
			{
				//agente.Stop();
			}
		}
	}
}
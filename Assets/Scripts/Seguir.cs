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
    public class Seguir : ComportamientoMovimiento
	{
        /// <summary>
        ///     Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        private float deceleracion = 0;
        public float radioObjetivo;
        public float radioFreno;
        bool freno = false;

		public override Direccion GetDireccion()
		{
            var direccion = base.GetDireccion();

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
			var util = new Direccion {lineal = transformObjetivo.position - transform.position};
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
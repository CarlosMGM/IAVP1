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
    ///     Clara para el comportamiento de agente que consiste en ser el jugador
    /// </summary>
    public class PlayerBehavior : ComportamientoAgente
	{
        /// <summary>
        ///     Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
		{
			var direccion = new Direccion {lineal = {x = Input.GetAxis("Horizontal"), z = Input.GetAxis("Vertical")}};
			direccion.lineal.Normalize(); // Normalizar el vector
			direccion.lineal *= agente.aceleracionMax;
			return direccion;
		}
	}
}
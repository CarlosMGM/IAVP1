﻿/*    
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
    ///     Clase auxiliar para representar la dirección/direccionamiento con el que corregir el movimiento
    /// </summary>
    public class Direccion
	{
		public float angular;
		public Vector3 lineal;

		public Direccion()
		{
			angular = 0.0f;
			lineal = new Vector3();
		}
	}
}
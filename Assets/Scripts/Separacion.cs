using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class Separacion : ComportamientoMovimiento
    {
        
        public Agente[] animales;

        public float coeficienteDecadencia;

        public float umbral;

        public override Direccion GetDireccion()
        {
            var direccionFinal = new Direccion();

            foreach( Agente animal in animales){
                var direccion = new Direccion();
                direccion.lineal = animal.transform.position - transform.position;

                if(direccion.lineal.magnitude < umbral)
                {
                    var fuerza = Mathf.Min(coeficienteDecadencia / (direccion.lineal.sqrMagnitude), agente.aceleracionMax);

                    direccionFinal.lineal += fuerza * direccion.lineal;
                }
            }


            return direccionFinal;

        }

        public void IrConDistancia()
        {
            if (agente.mezclarPorPeso)
                agente.SetDireccion(GetDireccion(), peso);
            else if (agente.mezclarPorPrioridad)
                agente.SetDireccion(GetDireccion(), prioridad);
            else
                agente.SetDireccion(GetDireccion());
        }
    }
}
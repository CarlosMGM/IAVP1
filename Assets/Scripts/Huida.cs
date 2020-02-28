using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class Huida : ComportamientoMovimiento
    {
        public override Direccion GetDireccion()
        {
            var direccion = base.GetDireccion();

            float instanceAcceleration;

            direccion.lineal = -direccion.lineal;




            direccion.lineal.Normalize();
            direccion.lineal *= agente.aceleracionMax;

            return direccion;
        }

        public virtual void HuirDeJugador()
        {
            agente.Run();
            if (agente.mezclarPorPeso)
                agente.SetDireccion(GetDireccion(), peso);
            else if (agente.mezclarPorPrioridad)
                agente.SetDireccion(GetDireccion(), prioridad);
            else
                agente.SetDireccion(GetDireccion());

        }
    }
}
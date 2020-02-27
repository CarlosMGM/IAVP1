using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UCM.IAV.Movimiento
{
    public class Perro : Seguir
    {
        // Update is called once per frame
        public virtual void Update()
        {
            //Separamos flauta de souns puesto que flauta debería identificar si sound es apto para la rata
            flauta = agente.sound;
            if (!flauta)
            {
                int layerMask = 1 << 8;

                // This would cast rays only against colliders in layer 8.
                // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
                layerMask = ~layerMask;

                RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2, layerMask))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                    //  print(hit.);
                }
                else
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 2, Color.white);
                }
                this.SeguirJugador();
            }
            else
            {
                agente.run();
                Direccion dir = new Direccion();
                dir.lineal = new Vector3(-1.0f, 0.0f, -1.0f);
                if (agente.mezclarPorPeso)
                    agente.SetDireccion(dir, peso);
                else if (agente.mezclarPorPrioridad)
                    agente.SetDireccion(dir, prioridad);
                else
                    agente.SetDireccion(dir);
            }
        }
    }
}

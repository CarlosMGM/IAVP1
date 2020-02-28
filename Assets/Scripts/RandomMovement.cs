using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UCM.IAV.Movimiento
{
    public class RandomMovement : ComportamientoMovimiento
    {
        // Start is called before the first frame update

        public float timeInterval;
        public float lastTime;

        private void Update()
        {
            if (Time.time - lastTime > 1.0f)
            {
                transformObjetivo.transform.position = new Vector3(Random.Range(-3.0f, 3.0f), transform.position.y, Random.Range(-3.0f, 3.0f));
                lastTime = Time.time;
            }
        }

        public override Direccion GetDireccion()
        {

            var direccion = base.GetDireccion();

            

            direccion.lineal.Normalize();
            direccion.lineal *= agente.aceleracionMax;

            return direccion;
        }
    }
}
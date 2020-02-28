using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UCM.IAV.Movimiento
{
    public class ComportamientoMovimiento : ComportamientoAgente
    {

        private float count = 1;
        private Vector3 dir = Vector3.zero;

        public override Direccion GetDireccion()
        {
            var direccion = new Direccion();
            var layerMask = 1 << 8;

            layerMask = ~layerMask;

            RaycastHit hit;

            count += 0.5f * Time.deltaTime;
            //UnityEngine.Physics.Raycast(transform.position, transformObjetivo.position, out hit, 2, layerMask);
            var tf = transform;
            Debug.DrawRay(tf.position, (transformObjetivo.position - tf.position) * 2, Color.red);
            if (true /*count >= cooldown*/)
            {
                count = 0;
                //direccion.lineal = transformObjetivo.position - transform.position;
                if (Physics.Raycast(transform.position, transformObjetivo.position - transform.position, out hit, 2.0f,
                    layerMask))
                {
                    direccion.lineal = transformObjetivo.position - transform.position;
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
                    direccion.lineal = transformObjetivo.position - transform.position;
                }

                dir = direccion.lineal;
            }
            else
            {
                direccion.lineal = dir;
            }
            
            return direccion;
        }
    }
}
/*    
   Copyright (C) 2020 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Movimiento
{

    /// <summary>
    /// Clase para modelar el comportamiento de SEGUIR a otro agente
    /// </summary>
    public class Seguir : ComportamientoAgente
    {
        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        float cooldown = 1;
        float count = 1;
        UnityEngine.Vector3 dir = UnityEngine.Vector3.zero;
        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();
            int layerMask = 1 << 8;
            
            layerMask = ~layerMask;

            UnityEngine.RaycastHit hit;
            
            count += 0.5f * UnityEngine.Time.deltaTime;
            //UnityEngine.Physics.Raycast(transform.position, objetivo.transform.position, out hit, 2, layerMask);
            UnityEngine.Debug.DrawRay(transform.position, (objetivo.transform.position - transform.position) * 2, UnityEngine.Color.red);
            if (true /*count >= cooldown*/)
            {
                count = 0;
                //direccion.lineal = objetivo.transform.position - transform.position;
                if (UnityEngine.Physics.Raycast(transform.position, (objetivo.transform.position - transform.position), out hit, 2.0f, layerMask))
                {
                    direccion.lineal = objetivo.transform.position - transform.position;
                    float i = UnityEngine.Random.Range(-1.0f, 1.0f);
                    UnityEngine.Vector3 newPath = UnityEngine.Vector3.zero /*hit.transform.position*/;
                    //print(hit.transform.eulerAngles.y);
                    if (i > 0)
                    {
                        newPath = new UnityEngine.Vector3(UnityEngine.Mathf.Cos(hit.transform.eulerAngles.y) , 0, UnityEngine.Mathf.Sin(hit.transform.eulerAngles.y));

                        //direccion.lineal = newPath;
                    }
                    else
                    {
                        newPath = new UnityEngine.Vector3(UnityEngine.Mathf.Cos(hit.transform.eulerAngles.y), 0, UnityEngine.Mathf.Sin(hit.transform.eulerAngles.y));
                        //newPath -= new UnityEngine.Vector3((UnityEngine.Mathf.Cos(hit.transform.rotation.y) * (hit.transform.localScale.x)) / 2,
                        //                                    0, (UnityEngine.Mathf.Sin(hit.transform.rotation.y) * (hit.transform.localScale.y)) / 2);
                        //direccion.lineal = newPath/* - transform.position*/;
                    }
                    //direccion.lineal.Normalize();
                    print(newPath);
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
            direccion.lineal.Normalize();
            direccion.lineal = direccion.lineal * agente.aceleracionMax;
            return direccion;
        }
    }
}

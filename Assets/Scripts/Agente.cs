/*    
   Copyright (C) 2020 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

namespace UCM.IAV.Movimiento {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine; 

/// <summary>
/// La clase Agente es responsable de modelar los agentes y gestionar todos los comportamientos asociados para combinarlos (si es posible) 
/// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Agente : MonoBehaviour {
        /// <summary>
        /// Mezclar por peso
        /// </summary>
        [Tooltip("Mezclar por peso.")]
        public bool mezclarPorPeso = false;
        /// <summary>
        /// Mezclar por prioridad
        /// </summary>
        [Tooltip("Mezclar por prioridad.")]
        public bool mezclarPorPrioridad = false;
        /// <summary>
        /// Umbral de prioridad para tener el valor en cuenta
        /// </summary>
        [Tooltip("Umbral de prioridad.")]
        public float umbralPrioridad = 0.2f;
        /// <summary>
        /// Velocidad m�xima
        /// </summary>
        [Tooltip("Velocidad m�xima.")]
        public float velocidadMax;
        /// <summary>
        /// Aceleraci�n m�xima
        /// </summary>
        [Tooltip("Aceleraci�n m�xima.")]
        public float aceleracionMax;
        /// <summary>
        /// Rotaci�n m�xima
        /// </summary>
        [Tooltip("Rotaci�n m�xima.")]
        public float rotacionMax;
        /// <summary>
        /// Aceleraci�n angular m�xima
        /// </summary>
        [Tooltip("Aceleraci�n angular m�xima.")]
        public float aceleracionAngularMax;
        /// <summary>
        /// Orientacion (es como la velocidad angular)
        /// </summary>
        [Tooltip("Orientaci�n.")]
        public float orientacion;
        /// <summary>
        /// Rotatci�n (valor que puede variar, como la velocidad, para cambiar la orientaci�n)
        /// </summary>
        [Tooltip("Rotaci�n.")]
        public float rotacion;
        /// <summary>
        /// Velocidad
        /// </summary>
        [Tooltip("Velocidad.")]
        public Vector3 velocidad;
        /// <summary>
        /// Valor de direcci�n / direccionamiento
        /// </summary>
        [Tooltip("Direccion.")]
        protected Direccion direccion;
        /// <summary>
        /// Grupos de direcciones, agrupados por valor de prioridad
        /// </summary>
        [Tooltip("Grupos de direcciones.")]
        private Dictionary<int, List<Direccion>> grupos;
        /// <summary>
        /// Componente de cuerpo r�gido
        /// </summary>
        [Tooltip("Cuerpo r�gido.")]
        private Rigidbody cuerpoRigido;
        /// <summary>
        /// Boleano que controla cuando el agente debe moverse
        /// </summary>
        [Tooltip("�Se debe mover?")]
        private bool movement;
        /// <summary>
        /// Boleano que controla cuando el agente debe moverse
        /// </summary>
        [Tooltip("�Est� escuchando algo?")]
        public bool sound;

        /// <summary>
        /// Al comienzo, se inicialian algunas variables
        /// </summary>
        public void Awake()
        {
            velocidad = Vector3.zero;
            direccion = new Direccion();
            grupos = new Dictionary<int, List<Direccion>>();
            cuerpoRigido = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// En cada tick fijo, si hay cuerpo r�gido, uso el simulador f�sico aplicando fuerzas o no
        /// </summary>
        public virtual void FixedUpdate()
        {
            if (cuerpoRigido == null)
                return;

            Vector3 displacement = velocidad * Time.fixedDeltaTime;
            orientacion += rotacion * Time.fixedDeltaTime;
            // Necesitamos "constre�ir" inteligentemente la orientaci�n al rango (0, 360)
            if (orientacion < 0.0f)
                orientacion += 360.0f;
            else if (orientacion > 360.0f)
                orientacion -= 360.0f;
            // El ForceMode depender� de lo que quieras conseguir
            // Estamos usando VelocityChange s�lo con prop�sitos ilustrativos
            if (movement) cuerpoRigido.AddForce(displacement, ForceMode.VelocityChange);
            else cuerpoRigido.velocity = Vector3.zero; 
            Vector3 orientationVector = OriToVec(orientacion);
            cuerpoRigido.rotation = Quaternion.LookRotation(orientationVector, Vector3.up);
        }

        /// <summary>
        /// En cada tick, hace lo b�sico del movimiento del agente
        /// </summary>
        public virtual void Update()
        {
            if (cuerpoRigido != null)
                return;
            // ... c�digo previo
            Vector3 desplazamiento = velocidad * Time.deltaTime;
            orientacion += rotacion * Time.deltaTime;
            // Necesitamos "constre�ir" inteligentemente la orientaci�n al rango (0, 360)
            if (orientacion < 0.0f)
                orientacion += 360.0f;
            else if (orientacion > 360.0f)
                orientacion -= 360.0f;
            if(movement)transform.Translate(desplazamiento, Space.World);
            // Restaura la rotaci�n al punto inicial antes de rotar el objeto nuestro valor
            transform.rotation = new Quaternion();
            transform.Rotate(Vector3.up, orientacion);
        }

        /// <summary>
        /// En cada parte tard�a del tick, hace tareas de correcci�n num�rica (ajustar a los m�ximos, la combinaci�n etc.
        /// </summary>
        public void LateUpdate()
        {
            if (mezclarPorPrioridad)
            {
                direccion = GetPrioridadDireccion();
                grupos.Clear();
            }
            velocidad += direccion.lineal * Time.deltaTime;
            rotacion += direccion.angular * Time.deltaTime;

            if (velocidad.magnitude > velocidadMax)
            {
                velocidad.Normalize();
                velocidad *= velocidadMax;
            }

            if (rotacion > rotacionMax)
            {
                rotacion = rotacionMax;
            }

            if (Math.Abs(direccion.angular) < 0.1f)
            {
                rotacion = 0.0f;
            }

            // if (Math.Abs(direccion.lineal.sqrMagnitude) < 0.1f)
            // {
            //     velocidad = Vector3.zero;
            // }

            // En realidad si se quiere cambiar la orientaci�n lo suyo es hacerlo con un comportamiento, no as�:
            transform.LookAt(transform.position + velocidad);

            // Se limpia el steering de cara al pr�ximo tick
            // direccion = new Direccion();
        }

        /// <summary>
        /// Establece la direcci�n tal cual
        /// </summary>
        public void SetDireccion(Direccion direccion)
        {
            this.direccion = direccion;
        }

        /// <summary>
        /// Establece la direcci�n por peso
        /// </summary>
        /// <param name="direccion"></param>
        /// <param name="peso"></param>
        public void SetDireccion(Direccion direccion, float peso)
        {
            this.direccion.lineal += peso * direccion.lineal;
            this.direccion.angular += peso * direccion.angular;
        }

        /// <summary>
        /// Establece la direcci�n por prioridad
        /// </summary>
        /// <param name="direccion"></param>
        /// <param name="prioridad"></param>
        public void SetDireccion(Direccion direccion, int prioridad)
        {
            if (!grupos.ContainsKey(prioridad))
            {
                grupos.Add(prioridad, new List<Direccion>());
            }
            grupos[prioridad].Add(direccion);
        }

        /// <summary>
        /// REcibe informaci�n del comportamiento para saber si debe frenar
        /// </summary>
        public void stop()
        {
            //print("illo por que esto no frena");
            //velocidad = Vector3.zero;
            movement = false;
        }

        public void run()
        {
            //print("illo por que esto no frena");
            //velocidad = Vector3.zero;
            movement = true;
        }

        /// <summary>
        /// Devuelve el valor de direcci�n calculado por prioridad
        /// </summary>
        /// <returns></returns>
        private Direccion GetPrioridadDireccion()
        {
            Direccion direccion = new Direccion();
            List<int> gIdList = new List<int>(grupos.Keys);
            gIdList.Sort();
            foreach (int gid in gIdList)
            {
                direccion = new Direccion();
                foreach (Direccion direccionIndividual in grupos[gid])
                {
                    direccion.lineal += direccionIndividual.lineal;
                    direccion.angular += direccionIndividual.angular;
                }
                if (direccion.lineal.magnitude > umbralPrioridad
                     || Mathf.Abs(direccion.angular) > umbralPrioridad)
                {
                    return direccion;
                }
            }
            return direccion;
        }
        /// <summary>
        /// Calculates el Vector3 dado un cierto valor de orientaci�n
        /// </summary>
        /// <param name="orientacion"></param>
        /// <returns></returns>
        public Vector3 OriToVec(float prientacion)
        {
            Vector3 vector = Vector3.zero;
            vector.x = Mathf.Sin(prientacion * Mathf.Deg2Rad) * 1.0f;
            vector.z = Mathf.Cos(prientacion * Mathf.Deg2Rad) * 1.0f;
            return vector.normalized;
        }

        public void soundPlaying() { sound = true; }
        public void soundStop() { sound = false; }
    }
}

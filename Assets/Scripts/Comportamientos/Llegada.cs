/*    
   Copyright (C) 2020-2025 Federico Peinado
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
    /// Clase para modelar el comportamiento de SEGUIR a otro agente
    /// </summary>
    public class Llegada : ComportamientoAgente
    {
        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>


        // El radio para llegar al objetivo
        public float rObjetivo;

        // El radio en el que se empieza a ralentizarse
        public float rRalentizado;

        // El tiempo en el que conseguir la aceleracion objetivo
        float timeToTarget = 0.1f;
        public override ComportamientoDireccion GetComportamientoDireccion()
        {
            ComportamientoDireccion LastDir = new ComportamientoDireccion();

            Vector3 posAgente = agente.transform.position;
            Vector3 posObjetivo = objetivo.transform.position;

            // Distancia entre los dos agentes
            Vector3 direccion = posObjetivo - posAgente;
            direccion.Normalize();
            float distancia = Vector3.Distance(posAgente, posObjetivo);

            float velocidad = 0;

            // 3 posibles casos
            if (distancia < rObjetivo) // El agente ya ha llegado
            {
                // nada
            }
            else if (distancia < rRalentizado && distancia > rObjetivo) // El agente está realentizandose
            {
                velocidad = agente.velocidadMax;
            }
            else // El agente está siguiendo al objetivo
            {
                velocidad = agente.velocidadMax * (distancia / rRalentizado);
            }

            LastDir.lineal = direccion * velocidad;

            return LastDir;
        }
    }
}

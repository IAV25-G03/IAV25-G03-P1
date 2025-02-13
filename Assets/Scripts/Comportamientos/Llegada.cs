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

        public float fRalentizado;

        // El tiempo en el que conseguir la aceleracion objetivo
        float timeToTarget = 0.1f;
        public override ComportamientoDireccion GetComportamientoDireccion()
        {
            ComportamientoDireccion ComporDir = new ComportamientoDireccion();

            Vector3 posAgente = agente.transform.position;
            Vector3 posObjetivo = objetivo.transform.position;

            // Distancia entre los dos agentes
            float distancia = Vector3.Distance(posAgente, posObjetivo);

            // 3 posibles casos
            if(distancia < rObjetivo) // El agente ya ha llegado
            {

            }
            if(distancia < rRalentizado && distancia > rObjetivo) // El agente está realentizandose
            {
                
            }
            else // El agente está siguiendo al objetivo
            {

            }

            return ComporDir;
        }


    }
}

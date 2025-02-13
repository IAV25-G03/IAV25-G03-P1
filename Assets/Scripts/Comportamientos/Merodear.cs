/*    
   Copyright (C) 2020-2025 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de WANDER a otro agente
    /// </summary>
    public class Merodear : Llegada
    {
        [SerializeField]
        float radioWander = 5.0f;

        [SerializeField]
        float tiempoMaximo = 2.0f;

        [SerializeField]
        float tiempoMinimo = 1.0f;

        float timer = 0f;

        ComportamientoDireccion lastDir = new ComportamientoDireccion();

        public virtual ComportamientoDireccion GetComportamientoDireccion(){

            timer -= Time.deltaTime;

            // si acaba el tiempo de merodear volvemos a generar dir
            if (timer <= 0) {
                if (!objetivo)
                    objetivo = new GameObject();

                float newX = transform.position.x + Mathf.Sin(Random.Range(-1,1));
                float newZ = transform.position.z + Mathf.Cos(Random.Range(-1,1));

                objetivo.transform.position = new Vector3(newX, transform.position.y, newZ);

                timer = Random.Range(tiempoMinimo, tiempoMaximo);
            }

            lastDir = base.GetComportamientoDireccion();

            return lastDir;
        }

    }
}

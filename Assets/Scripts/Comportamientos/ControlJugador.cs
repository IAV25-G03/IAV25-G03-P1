/*    
   Copyright (C) 2020-2025 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Movimiento
{
    using UnityEngine;
    /// <summary>
    /// El comportamiento de agente que consiste en ser el jugador
    /// </summary>
    public class ControlJugador: ComportamientoAgente
    {
        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        /// 

        Camera cam;
        public LayerMask mask;

        public void Start()
        {
            cam = Camera.main;
        }

        public override ComportamientoDireccion GetComportamientoDireccion()
        {
            ComportamientoDireccion direccion = new ComportamientoDireccion();
            
            //Direccion actual (se gestiona a traves de la configuracion de Unity) -> Cambiar por Raycast
            direccion.lineal.x = Input.GetAxis("Horizontal");
            direccion.lineal.z = Input.GetAxis("Vertical");
            
            //TO-DO: ARREGLAR MOVIMIENTO
            /*//Draw Ray (comprobacion de que funciona el Raycast)
            Vector3 mousePos = Input.mousePosition;
            mousePos = cam.ScreenToWorldPoint(mousePos);
            Debug.DrawRay(transform.position, mousePos - transform.position, Color.blue);

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) //Colision con el suelo -> Mover
                {
                    Debug.Log("Holaa");
                    Debug.Log(ray.GetPoint(100));
                    direccion.lineal.x = mousePos.x - transform.position.x;
                    direccion.lineal.z = mousePos.y - transform.position.y;
                }
            }*/

            //Resto de cálculo de movimiento
            direccion.lineal.Normalize();
            direccion.lineal *= agente.aceleracionMax;

            // Podríamos meter una rotación automática en la dirección del movimiento, si quisiéramos

            return direccion;
        }
    }
}
# DOCUMENTO DE DISEÑO PRÁCTICA 1
- Miguel Norberto Pérez Ballester https://github.com/miguelNPB
- Álvaro Piña Sánchez-Sierra
- David Palacios Daza

## Punto de partida

Proyecto de videojuego actualizado a Unity 2022.3.57f1 diseñador para servir como punto de partida en algunas prácticas.

Consiste en un entorno virtual 3D que representa el pueblo de Hamelín, un personaje controlable por el jugador que es el flautista de Hamelín, un perro compañero y un montón de ratas preparadas para controlarse con IA.

## Diseño de la solución

Tendremos un RatController, que controlará el movimiento de todas las ratas. El perro, también tendrá su propio DogController que calculará su movimiento y si debe huir por las ratas cercanas.

### FLAUTISTA
#### Comportamiento
Con el click izquierdo el flautista va en dirección a donde se ha clicado, esquivando los obstáculos del terreno. Con el click derecho, el flautista cambia de animación y se pone a tocar la flauta.

- Como programar el movimiento: Usaremos un navmesh y un navmesh agent que será el flautista, al clicar en el mapa, tomaremos ese punto como destino y unity moverá al flautista por el mapa esquivando hasta llegar allí.
- Tocar la flauta: Al tocar la flauta avisará al RatController de que pasen de estado *MERODEAR* a estado *HIPNOTIZADO*.

### PERRO
#### Comportamiento
Debe seguir al flautista prediciendo su movimiento, y en caso de que hayan muchas ratas cerca de él, debera ladrar y huir de ellas por un tiempo.

- Estados:
    - *SEGUIR*: sigue al flautista.
    - *HUIR*: por 1 segundo huye en la dirección donde menos ratas haya, y si tras esos segundos no hay ratas, vuelve al estado *SEGUIR*.
- Seguimiento del flautista: Si la distancia del perro a el flautista es menor que cierto umbral, no se mueve y está junto a él, en cuanto aumenta la distancia, se mueve en dirección al flautista.
- Orientación: Rotará para mirar en la misma dirección que el flautista. Habrá un gameObject invisible a una distancia del flautista y irá posicionandose a esa distancia enfrente de la dirección de la que mira.
El perro se orientará hacia ese gameObject invisible.
- Detección de ratas: Tenemos un collider circular alrededor del perro, este sumará uno al contador de ratas si entra una y restara uno si salen. 
- Cálculo de dirección de huida: Almacenamos los vectores rata-perro para cada rata dentro del círculo de detección, y la dirección que deberá tomar el perro para huir es la suma de todos los vectores normalizado.
- Cambio de estado a huir: En cuanto superen cierto umbral, cambiará al estado *HUIR* durante varios segundos y volverá al estado *SEGUIR* si después de esos segundos hay pocas ratas.

### RATAS
### Comportamiento
Deben estar merodeando, y si el flautista comienza a tocar, las ratas que se encuentren en el radio de efecto del flautista pasarán a seguirle en bandada con movimiento dinámico en formación.  
- Las ratas tendrán las fuerzas que siguen las reglas de flocking:  
    -   Separación: Los NPC deben mantenerse a una distancia mínima entre sí para evitar colisiones.
    -   Alineación: Los NPC deben alinearse en la misma dirección que sus vecinos cercanos.
    -   Cohesión: Los NPC deben moverse hacia el centro de masa del grupo para mantenerse unidos.
    -   Seguimiento: Los NPC deben seguir a un objetivo, en este caso, el flautista.  

Tenemos varias variables que dictan el comportamiento de la bandada.
Todo esto será manejado por el RatManager.  
La *DISTANCIA MÍNIMA* dicta la cercanía que tendrán las ratas al estar en grupo.   
El *RADIO DE ALINEACIÓN* dicta lo sociables que son las ratas para tener misma dirección.  
El *RADIO DE COHESIÓN* es que tan lejos 'mira' el npc para determinar el centro del grupo y acercarse a él.
La *FUERZA DE SEGUIMIENTO* es la fuerza con la que atrae el flautista.

- Estados:
    - *MERODEAR*: eligen una dirección random cada 0.5 segundos y se mueven en esa dirección.
    - *HIPNOTIZADO*: seguirán al flautista en bandada aplicando separación, alineación , cohesión y seguimiento, una vez estén a una distancia de él se pararán y encararán al flautista.
- Cambio de estado:
    Si entran en el radio del flautista cuando está tocando, pasan al estado *HIPNOTIZADO*, hasta que deje de tocar o salga del radio.
- Seguimiento en bandada:
    Sumamos las 4 fuerzas para sacar la dirección en la que la rata debe moverse.
    - Separación: por cada rata, comparamos con el resto de ratas, si la distancia es menor que la *DISTANCIA MÍNIMA* entre ratas, devolvemos una fuerza en dirección (rata.pos - other.pos), la normalizamos y la dividimos por la distancia. Si la distancia es mayor que la mínima, la fuerza es 0;
    - Alineación: por cada rata, comparamos con el resto de ratas, si estan dentro del *RADIO DE ALINEACIÓN*, sumamos todas las direcciones a un vector y luego lo dividimos por el número de ratas que habían en rango para sacar la dirección promedio y normalizarla. De base sería 0 la fuerza.
    - Cohesión: por cada rata, comparamos con el resto de ratas, si están dentro del *RADIO DE COHESIÓN*, sumamos sus posiciones a un vector y luego lo dividimos por el número de ratas que habían en rango para sacar el centro de masa. Luego sacamos la dirección rata-centro de masa y la normalizamos. De base la fuerza es 0.
    - Seguimiento: sacamos la dirección rata-flautista y la normalizamos.

Con estas 4 fuerzas, les aplicamos pesos y las sumamos para dar la dirección y velocidad de la rata. En caso de pasarse de la velocidad máxima pondriamos un limitador.

### Pseudocódigo


### Comportamiento de las ratas (ratManager)
```
void updateRatas(todasRatas, flautista){
    for (rata in todasRatas):
        if (flautista)

}
```

#### Movimiento de las ratas  
```
void movimientoRata(rata, todasRatas, distanciaMinima, radioAlineacion, radioCohesion, fuerzaSeguimiento, flautista){
    Vector3 fuerzaTotal = Vector3(0,0,0)

    if (rata.hipnotizada):
        fuerzaTotal += separacion(rata, todasRatas, distanciaMinima)
        fuerzaTotal += alineacion(rata, todasRatas, radioAlineacion)
        fuerzaTotal += cohesion(rata, todasRatas, radioCohesion)
        fuerzaTotal += seguimiento(rata, flautista, fuerzaSeguimiento)
    else:
        fuerzaTotal = Vector3().randomDirection()

    if (fuerzaTotal > maxVelocidad):
            fuerzaTotal.normalize()
            fuerzaTotal = fuerzaTotal * maxVelocidad

    rata.velocity = fuerzaTotal
}

Vector3 separacion(rata, todasRatas, distanciaMinima){
    fuerza = Vector3(0,0,0)

    for otro in todasRatas:
        distancia = rata.distancia_con(otro.pos)
        if (distancia < distanciaMinima):
            diferencia = rata.pos - otro.pos
            diferencia.normalize()
            fuerza += diferencia / distancia

    return fuerza
}

Vector3 alineacion(rata, todasRatas, radioAlineacion){
    fuerza = Vector3(0,0,0)
    total = 0
    
    for (otro in todasRatas):
        distancia = rata.distancia_con(otro.pos)
        if (distancia < radioAlineacion):
            fuerza += rata.velocity
            total++
    
    if (total > 0):
        fuerza = fuerza / total
        fuerza.normalize()

    return fuerza
}

Vector3 cohesion(rata, todasRatas, radioCohesion){
    fuerza = Vector3(0,0,0)
    centroMasa = Vector3(0,0,0)
    total = 0
    
    for (otro in todasRatas):
        distancia = rata.distancia_con(otro.pos)
        if (distancia < radioCohesion):
            centroMasa += otro.pos
            total++
    
    if (total > 0):
        centroMasa = centroMasa / total
        fuerza = centroMasa - rata.pos
        fuerza.normalize()

    return fuerza
}

Vector3 seguimiento(rata, flautista, fuerzaSeguimiento){
    fuerza = flautista.pos - rata.pos
    fuerza.normalize()

    return fuerza
}
```

#### Movimiento del perro

```

```

## Ampliaciones

## Pruebas y métricas

- Pruebas:
    - Probar a poner 1000 ratas en un ordenador de los laboratorios y conseguir que vaya a más de 60FPS durante una ejecución prolongada.
    - Usar Unity Profiler para poder monitorear procesamiento, consumo de memoria, FPS entre otras cosas.
- Métricas:
    - FPS.
    - Tiempo de actualización de la IA.
    - Uso de CPU y memoria.
    - Latencia de respuesta de las IAs.

## Conclusiones

## Licencia
A, B y C, autores de la documentación, código y recursos de este trabajo, concedemos permiso permanente a los profesores de la Facultad de Informática de la Universidad Complutense de Madrid para utilizar nuestro material, con sus comentarios y evaluaciones, con fines educativos o de investigación; ya sea para obtener datos agregados de forma anónima como para utilizarlo total o parcialmente reconociendo expresamente nuestra autoría.

Una vez superada con éxito la asignatura se prevé publicar todo en abierto (la documentación con licencia Creative Commons Attribution 4.0 International (CC BY 4.0) y el código con licencia GNU Lesser General Public License 3.0).

## Referencias
* *AI for Games*, Ian Millington.
* [AnalogBit: Sliding Block Puzzle Solver](http://analogbit.com/software/puzzletools/)
* [How Sliding Puzzles Work (How Stuff Works)](https://entertainment.howstuffworks.com/puzzles/sliding-puzzles.htm)
* Sliding Puzzle Collection (Random Box Studio, 2020)
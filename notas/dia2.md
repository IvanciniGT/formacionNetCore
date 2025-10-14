# Pruebas de software

## Vocabulario en el mundo del testing

Estoy creando una mesa. Soy carpintero.

- Causa raíz    La que provoca el error en el humano.
                    > CAUSA RAIZ: Me despisté. Otros ejemplos: Falta de conocimiento, cansancio...
- Error         Los humanos cometemos errores (errar es de humanos). 
                    > ERROR: Mala medida
- Defecto       Al cometer un error, yo como humano, puedo introducir un defecto en el sistema.
                    > DEFECTO: Pata más corta
- Fallo         Los Defectos pueden manifestarse al usar el producto.. o no... en forma de Fallos,
                que son desviaciones del comportamiento del producto con respecto a lo que esperamos de él.
                    > FALLO: La mesa no es estable y se han caído todos los platos.

## Para qué sirven las pruebas?

- Para asegurar el cumplimiento de unos requisitos (funcionales o no funcionales).
- Para ayudarnos a identificar la mayor cantidad posible de Defectos antes del paso a producción (entrega) del producto.
        Esos defectos en inglés los llamamos: BUGS
        Y quiero identificarlos para arreglarlos, quiero entregar un producto sin defectos.. o con launa cantidad mínima de ellos.
        De hecho es lo único que puedo resolver/arreglar.
         Lo ÚNICO QUE PUEDO REPARAR son los Defectos que han quedado presentes en el producto como cicatrices!

         Las pruebas me ayudan a ello, identificando los DEFECTOS... Una vez identificado, procedo a su arreglo (DEPURACIÓN o DEBBUGING)

         En este sentido, tengo 2 opciones:
         - OPCIÓN 1... y a la que más acostumbrados estamos: Usar el producto y tratar de provocar fallos en él. 
           Una vez que un fallo se ha producido, tratar de recopilar toda la información posible para ayudarnos a identificar el BUG(DEFECTO) que provocó ese fallo.
                    No puedo arreglar un falló. ya pasó... no puedo cambiar el pasado.
                    Intentaré evitar nuevos fallos en el futuro.

           ^^^                                          PRUEBA ESTATICA
           PRUEBAS DINAMICAS                                vvvvv

         - OPCIÓN 2... simplemente tratar de identificar los DEFECTOS directamente, sin buscar FALLOS, sin USAR el sistema/producto.
           A veces me basta con REVISARLO! Doy un paso atrás.. y miro! Esto es mucho más barato.

- Para ayudarnos a identificar las causas raíces
         Puedo arreglar un error? No puedo.. ya pasó. No puedo cambiar el pasado.
         Intentaré evitar nuevos errores en el futuro.
         Voy a mirar, qué ha acontecido en el proyecto... Dónde hemos tenido muchos defectos? En el módulo de integración con kafka.
         Y Por qué? Por qué los humanos estamos teniendo tantos errores al acometer ese módulo? 
            Falta de conocimiento --> Acción PREVENTIVA: Formación en Kafka!
            Por cansancio ----> Acción PREVENTIVA: 2 días sin escribir código

- Para aprender del sistema y extraer información que pueda aplicar quizás en el futuro a otros proyectos.
- En las metodologías ágiles, para saber qué tal va el proyecto, si vamos a tiempo, si vamos bien, si el producto es de calidad.
  En este sentido las pruebas son un mecanismo de FEEDBACK.
- ...

---

## Tipos de pruebas

Las pruebas hay muchas formas de clasificarlas (taxonomías) paralelas entre si.

## En base al objeto de prueba (a lo que están probando)

- Funcionales    (asociadas a requisitos funcionales)
- No funcionales (asociadas a requisitos no funcionales):
  - Estrés
  - Carga
  - Rendimiento
  - UX
  - Seguridad
  - Alta disponibilidad
  - ...

## En base al conocimiento interno que tengo del objeto de prueba (o que quiero tener)

- Caja blanca - Cuando tengo y uso el conocimiento del comportamiento interno del objeto de prueba
- Caja negra  - Cuando no tengo o no uso el conocimiento del comportamiento interno del objeto de prueba

## En base al procedimiento que uso para la prueba

- Dinámicas  - Las que requieren poner el sistema en funcionamiento (correr el código)
- Estáticas  - Las que NO requieren poner el sistema en funcionamiento (correr el código)

## En base al ambito (scope) / contexto de ejecución de la prueba

- Unitarias                 No tienen que ver ocn probar una pieza o parte del producto.
                            Tiene que ver con el CONTEXTO en el que hago la prueba.
                            Puedo probar una función/método/rutina de mi código de forma unitaria, integrada o de sistema... las 3
                            Y lo que cambia es el contexto.
                            Una prueba que trabaja sobre un componente AISLADO de mi sistema.
- Integración               Una prueba que se centra en la COMUNICACION entre 2 componentes.
- Sistema (end-end-end)     Una prueba que se centra en el COMPORTAMIENTO del sistema en su conjunto.

> Ejemplo: Soy Decatlon. Fabrico bicicletas.

  Pregunta... Voy a fabricar yo el sillín? NO
              Voy a fabricar yo las ruedas? NO
              Voy a fabricar yo el sistema de frenos? NO

              Yo diseño las especificaciones (de todos los componentes para asegurarme que funcionen bien juntos).
              Integrar las piezas.

  Pregunta... Me llega el sistema de frenos de la bici.. se lo he comprado a Shimano. Qué hago?
    
    Lo pruebo. Cómo lo pruebo? No tengo bici.. tengo que inventar algo.
            CONTEXTO:
            Básicamente monto el sistema de frenos en un bastidor (banco de pruebas.. 4 hierros mal soldaos.. que aguantes)
            
            Y pruebo el sistema de frenos > apretarPalanca()        ACCIÓN QUE QUIERO PROBAR
            Qué compruebo?
                1º Que las pinzas de freno se cierran
                2º Que lo hacen con la presión adecuada (Sensor entre las zapatas)

                LO QUE ACABAMOS DE HACER ES UNA PRUEBA UNITARIA / FUNCIONAL. Sobre el sistema de frenos.

  Me llega el sillín. Qué hago?
    Probemos. Montémoslo en un bastidor.
    Pruebas?  Carga: Aguanta a una persona de 130Kgs?
              UX:    Cuando una persona la tengo sentada 2 horas encima, le duele el culo?
              Estrés: Si pongo una persona a sentarse y levantarse 1000 veces al día, se desgasta mucho?

              Eso son pruebas UNITARIAS / NO FUNCIONALES

    Así con todos los componentes...
    Pregunta: Si me han llegado todos los componentes y a todos les he hecho sus pruebas unitarias y me han dado ok,
              eso me garantiza que la bici va a funcionar adecuadamente? NO

              Entonces.. para qué he hecho estas pruebas? Qué he ganado? CONFIANZA +1 
                Vamos bien! 
                Voy dando pasos en firme!
---

Siguiente nivel de pruebas.
Voy a juntar componentes **2 a 2**.. y los voy probando: Pruebas de INTEGRACION:

    Querré juntar (hacer pruebas de integración) entre el sillín y el sistema de frenos? NO... no hay dependencia entre esos componentes.
    Junto el sistema de frenos con las ruedas. Cómo la hago?
        CONTEXTO: 
        Monto el sistema de frenos en el bastidor.. pero en este caso, le pongo en medio de las pinzas de freno la rueda.
        Le pego un viaje a la rueda...
        
        Y pruebo el sistema de frenos > apretarPalanca()        ACCIÓN QUE QUIERO PROBAR

            Qué compruebo?
            1º Que la rueda frena... y mira que no. NO FRENA.
            Resulta que las pinzas cierran .. y cierran con fuerza.. pero no llegan a tocar la llanta.. es muy estrecha!

            La rueda está mal? NO
            El sistema de frenos está mal? NO FUNCIONA? NO.. está bien!

            Dónde está el problema? En la integración entre ambos componentes.
            Ambos. juntos no funcionan bien. El sistema de frenos no es capaz de COMUNICAR la energía de rozamiento a la rueda.

Y así con todas las parejitas de componentes.

    Pregunta: Si he hecho todas estas pruebas a todas las parejitas de componentes y me han dado ok,
              eso me garantiza que la bici va a funcionar adecuadamente? NO

              Entonces.. para qué he hecho estas pruebas? Qué he ganado? CONFIANZA +1 
                Sigo bien! 
                SIGO dando pasos en firme!

---

Monto una bici completa con mis componentes concretos. Y hago una prueba de sistema.
Pongo a un tio encima, mochila en la espada con bocadillo de chorizo y bien de agua.. y alé! pa' cuenca! hazte kms.
Si llega bien, sano, salvo, si que le duela el culo. BICI OK!

    Pregunta: Si he hecho todas estas pruebas a la bici, y me han dado ok,
              eso me garantiza que la bici va a funcionar adecuadamente? SI -> A VENDERLA -> PRODUCTO ACABADO...

              Y entonces si hago las pruebas.. de sistema.. y me dan todas ok... Necesito hacer pruebas unitarias y de integración?
              NO.. ya. tengo bici y está en marcha y funcionando!

              Ahí hay truco (doble):
              - Y si no van bien? Qué está pasando? NPI ... ponte a averiguar!
              - Cuando puedo hacer estas pruebas? Cuando tengo la bici completa... COÑO cuando supuestamente he acabado.. y hasta entonces.. voy a ciegas?

---

Al igual que en el mundo del desarrollo de software seguimos ciertos principios (como SOLID), en el mundo del testing también tenemos principios FIRST:

- F = Fast -> Rápidas
- I = Independent -> Independientes unas de otras
- R = Repeatable -> Repetibles
- S = Self-validating -> Auto-validante (Debe comprobar en automático TODO lo que deba ser comprobado)
- T = Timely -> Oportunas (en el momento adecuado)

---

Probando la función borrarUsuario(id):
    Contexto
        Asegurar (esto implica que si no existe, lo creo) que existe el usuario con ese id 
    Acción
        Llamar a borrarUsuario(id)
    Comprobación
        Comprobar que el usuario ya no existe

---

Al definir las pruebas solemos usar un patrón Given-When-Then: Dado-Cuando-Entonces
Es la misma mierda que arriba he puesto como:
  Contexto
  Acción
  Comprobación

---

Hoy en día, si tengo un código con 2000 líneas... posiblemente en test (pruebas) acabe con 5000-10000 líneas de código.

---

Y ese bastidor? Y el sensor? Eso no va a mi código... pero lo necesito para hacer las pruebas.
- Doble de pruebas (Test Double):
  - Mock
  - Stub
  - Fake
  - Spy
  - Dummy


---

Para qué usa un programa la memoria RAM ?
- Para poner su propio código cuando se está ejecutando
- Para almacenar datos de trabajo
- Para crear caches... que hagan que el programa funcione más rápido
   Una vez que hemos leído un diccionario (Son muchas palabras), me interesaría no tenerlo que leer de nuevo... y prefiero guardarlo en RAM... eso si.. si me quedo sin RAM, lo tiro a la basura...
   y ya volveré a leerlo del disco duro cuando me lo vuelvan a pedir.. que quizás en ese momento tiro a la basura otro diccionario que tenía en RAM y que no me estaban pidiendo.


   En C# tenemos una clase que me permite gestionar este tipo de caches.
   Es una estructura de datos de tipo Clave-Valor.
   A través de la clave, puedo recuperar un valor, siempre que esté en memoria.

    TABLA 
    CLAVE(idioma)      |   VALOR (IDiccionario)
    ES                 |   DiccionarioEspanol
    EN                 |   DiccionarioIngles

    La clase que me ofrece esta estructura de datos en C# es Dictionary<TKey,TValue>
    En nuestro caso TKey es string (el idioma) y TValue es IDiccionario (el diccionario en si)

    Dictionary<string, IDiccionario> _cacheDiccionarios = new Dictionary<string, IDiccionario>();

    Pero esto tendría un problema... si tengo un diccionario en RAM... Ese seguirá allí de por vida.
    Cómo me aseguro que si no hay memoria suficiente, C# tome la decisión de tirar a la basura ese diccionario que tengo en RAM y que no me están pidiendo?

    Y Ahi unos sale una clase llamada WeakReference<T>
    Esa clase me permite crear referencias débiles a un objeto que tengo en RAM.
    Qué es una referencia débil?... es como una variable normal, pero que simi programa necesita memoria, el RECOLECTOR DE BASURA puede tirar a la basura ese objeto al que yo tenía una referencia débil.
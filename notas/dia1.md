# .NET FRAMEWORK

Antiguo framework de desarrollo de software que hacía la gente de Microsoft.
Los programas que generaba con .NET Framework solo podían correr en Windows.
Además era un ecosistema muy cerrado.

# .NET CORE (especialmente usaremos C#)

Migración del antiguo .NET Framework a un nuevo framework de desarrollo de software.

Framework de desarrollo de software Open Source (código abierto).
Los programas que generamos con .NET Core pueden correr en Windows, Linux y MacOS.
Y además ha habido evoluciones muy grandes en los lenguajes que se usan en .NET Core (C#, F# y Visual Basic),
adaptándose a las nuevas prácticas, arquitecturas, principios de diseño y patrones de desarrollo de software.

Un framework no es lo mismo que una librería:
- Los frameworks suelen tener muchas librerías., pero además tienen programas que nos ayudan a compilar, ejecutar, testear, desplegar, etc. Imponen formas de hacer las cosas.
- Las librerías son solo código que podemos usar en nuestros programas.

# Ruptura con las formas de trabajo tradicionales.

Sistemas monolíticos, con bases de datos relacionales, con despliegues en servidores físicos, con metodologías de desarrollo en cascada (waterfall), etc. Ese mundo, salvo en proyectos legados, ha quedado atrás.
De hecho los problemas de hoy en día no son los mismos que los de hace 10 o 20 años... y por eso ¡necesitamos nuevas herramientas, lenguajes, frameworks, arquitecturas, patrones de diseño y principios de desarrollo!

> Ejemplo: Sistema de Animalitos Fermín.

Hace 15/10 años, básicamente se habría tratado de montar un MEGASISTEMA, corriendo sobre SQL Server, con una única base de datos relacional ABERRANTEMENTE GRANDE, con un equipo de desarrollo enorme, con una planificación exhaustiva, con un análisis de requisitos larguísimo (Metodología en cascada), con un despliegue en un servidor físico, etc. Esa aplicación tendrían un frontal WEB que se consumía mediante un navegador instalado en un PC / Escritorio.

> Hoy en día.. cómo ha cambiado la cosa...

1. Ese sistema, años después de haberlo montado, nos hemos dado cuenta que no hay quién lo mantenga. El equipo de desarrollo original se ha ido, y los nuevos desarrolladores no entienden nada del código legado. Cualquier cambio es un dolor de cabeza!

2. Ese sistema, estaba pensado para consumirse mediante un navegador en un PC. Esa es la principal forma de manejar/consumir un sistema informático hoy en día? Hoy en día el principal hardware que usamos para acceder a un sistema: MOVIL... y además, no solemos usar tanto el navegador, sino que usamos APPs nativas.
Y eso sin contar nuevos dispositivos/medios de acceso:
- Smart TV
- Sistemas de voz (Alexa, Google Home, Siri, etc)
- IVR (sistemas de respuesta por voz)

3. Ese sistema se habría desarrollado con una metodología en cascada (waterfall). Que no serviría hoy en día.

    TOMA DE REQUISITOS -> Documento WORD de 300 páginas!
        ↓
    ANÁLISIS DEL PROYECTO
        ↓
    DISEÑO DEL SISTEMA
        ↓
    IMPLEMENTACIÓN / CODIFICACIÓN
        ↓
    PRUEBAS
        ↓
    DESPLIEGUE
        ↓
    MANTENIMIENTO

Esto no sirve por muchos motivos:
1. Nos hemos dado cuenta con el paso de los años que estas formas de trabajo tenían sus problemas:
   - Falta de transmisión de información / cambios. 
   - Falta de adaptabilidad al cambio.
2. Hoy en día un sistema no es una aplicación... son un huevo de aplicaciones, servicios, etc. que interactúan entre sí.

    HACE AÑOS:

            [Navegador] ---> [Sistema Monolítico]           ---> [Base de Datos Relacional]
            Cliente             Servidor de Aplicaciones            Servidor
                                Programas (asp...)
                        <- html -        

    HOY EN DÍA:                        xml
            [APP Móvil Android]   <---json---
            [APP Móvil iOS]                     Programas estúpidos
            [APP Escritorio]                     para cosas muy concretas:
            [Smart TV]                              - Listar los animalitos     ---------> SQLServer
            [Sistemas Voz]                          - Gestión de citas          ---------> MongoDB
                    audio <-  json                  - Compras de comida y collares... ---> MySQL
            [IVR]           
            [Navegador]                                 v1.0.0  Gestion de Animalitos
                    js                                              - Datos de los animalitos: 
                                                                        Nombre, Edad, Foto
                    html <--- json                      v2.0.0  - Tenemos problemas con la actualización
                                                                        de datos: Fecha de nacimiento
                                                                        Muchas fotos.. y además videos.




    Aquí estamos hablando que rompemos con el concepto de arquitectura monolítica, y pasamos a arquitecturas basadas de componentes desacoplados (como la arquitectura de microservicios).
    Esos programas / componentes osn independendientes. Eso implica que:
    - Pueden desplegarse de forma independiente. Cuando subo una nueva versión de un componente, no tengo que tocar el resto.
    - Se pueden escalar de forma independiente. Si un componente tiene mucha carga, puedo escalar solo ese componente.

    En el escenario antiguo... usando metodologías en cascada... si había que cambiar algo, el impacto y los tiempos se disparaban.

    Hoy en día hemos migrado a metodologías ágiles (Scrum, Kanban, Extreme Programming...).

# Metodologías Ágiles

La principal característica de las metodologías ágiles es entregar el producto al cliente de forma INCREMENTAL.

Antiguamente entregaba al cliente el producto final, cuando ya estaba todo terminado.. al año.. 2 años...
Hoy en día, en un proyecto a 18 meses... le entrego en PRODUCCIÓN cada mes.

Esto me ofrece:
- Al cliente: disponer de actualizaciones frecuentes con novedades y avances... muy rápido
- A mí: recibir feedback del cliente muy rápido, y poder adaptar el desarrollo a lo que realmente necesita el cliente.

---

Se definían diagramas de gantt.

    HITO 17: 17-noviembre: *R17, R18, R19*
        Si el 17 de noviembre el R19 no estaba acabado:
        - Alarmas
        - Ostias pa' todos los laos
        - Replanificación del HITO17 -> nueva fecha: 22-noviembre. VAMOS CON RETRASO

    HITO 18: 15-diciembre: R20, R21, R22

Hoy en día trabajamos con el concepto de Iteración (o Sprint en Scrum).

    SPRINT 1: **17-noviembre**: R17, R18, R19
        Si el 17 de noviembre el R19 no está acabado:
        - El R19 no se entrega... pero el resto SI.
        - La fecha NO SE TOCA
        - HAY PASO A PRODUCCIÓN
        - Se replanifica el R19 para el siguiente sprint
        - Siga habiendo ostias pa' todos los laos
        - VAMOS CON RETRASO
     
            5% de la funcionalidad (100% funcional)
                PRUEBAS? 5% de la funcionalidad

    SPRINT 18: 15-diciembre: R20, R21, R22

            +10% de la funcionalidad (100% funcional)
                PRUEBAS? 10% de la funcionalidad nueva + 5% de la funcionalidad anterior

Antiguamente cómo controlaba el grado de avance de un proyecto?
- Preguntar al desarrollador

> El software funcionando es la MEDIDA principal de progreso. <<< INDICADOR PARA UN CUADRO DE MANDO

La MEDIDA principal de progreso es el software funcionando.
------------------------------- --------------------------
  SUJETO                        PREDICADO
 
La forma de medir qué tal va el proyecto es mediante el concepto SOFTWARE FUNCIONANDO.

SOFTWARE FUNCIONANDO? Software que hace lo que tiene que hacer.

Y quién dice eso? 
    - Cliente.  AYUDA CON LOS REQUISITOS.
    - Las Pruebas son lo que determina si el software funciona o no. Si el producto cumple con lo que debe cumplir.. con lo solicitado.

Las metodologías ágiles han venido con sus propios problemas.

Cuántas veces instalaba antiguamente en producción? 1 al acabar el proyecto
Y ahora con estas metodologías (cada mes)

Y pregunta... un paso a producción implica por necesidad: PRUEBAS A NIVEL DE PRODUCCIÓN!
Las pruebas se disparan... me crecen como enanos.

Y de donde sale la pasta ? y el tiempo? Y los recursos? NO LOS HAY!

Solo hay una solución a este entuerto: AUTOMATIZAR TRABAJO: Pruebas + Instalaciones

No hay forma de ir a una metodología ágil sin automatizar trabajos. ES IMPOSIBLE... por el coste que supone.

- Necesitamos nuevas metodologías de desarrollo de software.
- Necesitamos comenzar a automatizar tareas repetitivas. Necesito abrazar una cultura DEVOPS.
- Necesitamos nuevas arquitecturas de software, para adaptarnos a los nuevos problemas (muchos frontales... mejorar el mnto ... etc).
- Necesitamos nuevos lenguajes y frameworks, que nos permitan hacer uso de nuevos patrones de diseño y principios de desarrollo para facilitar el desarrollo y el mantenimiento.

Y todas esas cosas evolucionan en paralelo.
Y si saco una pieza de ahí de forma aislada y la intento aplicar en otro contexto... no va a funcionar.

# DEVOPS

Es una cultura, es una filosofía que hoy en dia estamos adoptando en las empresas: QUEREMOS AUTOMATIZAR TODOS LOS TRABAJOS QUE PODAMOS EN EL CICLO DE VIDA COMPLETO DEL DESARROLLO DE SOFTWARE. DEV---> OPS

- Si no voy a automatizar pruebas
- Si no voy a hacer un montón de instalaciones a mi cliente
- Si no quiero tener mi sistema construido con componentes desacoplados
NO LE VOY A SACAR PARTIDO A .net core.

---

# App para consultar diccionarios.

App -> Idioma + Palabra -> 
    OK -> Listado de definiciones
    KO -> Mensaje de error: No existe la palabra en el diccionario
                            No tengo un diccionario para ese idioma

La primera versión se va a operar desde una terminal de linea de comandos:

c:\> significadode.exe --idioma es --palabra melón
    Definiciones de "melón" en Español:
    1. Fruto comestible de la planta Cucumis melo.
    2. Persona con pocas luces.

c:\> significadode.exe --idioma fr --palabra melon
    No existe en el diccionario para el idioma "fr" la melón.

Para este sistema necesitamos montar (si queremos un buen diseño/arquitectura) al menos 4 repositorios de GIT.
No queremos volver al puñetero monolito. El día que haya un cambio... vamos a flipar!

- 1. La parte del programa que gestiona diccionarios y permite hacer búsquedas en ellos
- 2. La parte del programa que muestra los resultados por pantalla y gestiona la comunicación con el usuario
- 3. API de comunicación entre ambos. <- Estandar acerca de cómo esos 2 deben hablar.
- 4. Los datos. Los diccionarios.


```c# 
// API

namespace Diccionarios.API;

public interface IDiccionario {
    string getIdioma();
    boolean existePalabra(string palabra);
    List<string>? getDefiniciones(string palabra);
}

public interface ISuministradorDeDiccionarios {
    boolean tienesDiccionarioDe(string idioma);
    IDiccionario? getDiccionario(string idioma);
}
```

---

# Programa de consola gráfica (línea de comandos)

```c#

namespace Diccionarios.App;

using Diccionarios.API;      // Aquí estamos importando un API
//using Diccionarios.BBDD;     // Aquí estamos importando una implementación concreta del API
                             // Esta linea que acabamos de meter, es la muerte del proyecto.
                             // Mejor tirarlo a la basura que subir a producción!
                             // Al meter esta linea acabamos de cagarnos en el PRINCIPIO DE INVERSIÓN DE DEPENDENCIAS
public class ProgramaDeConsola {
 
    public void run(string[] args) {
        // Voy a leer los argumentos (idioma y palabra) desde lo que me pasan por la termina
        // Los valido
        // Obtener los significados o no
    }

    private void buscarPalabra(string idioma, string palabra, ISuministradorDeDiccionarios suministrador) { // Patrón inyección de dependencias
        //ISuministradorDeDiccionarios suministrador = new SuministradorDeDiccionariosEnBBDD(cadenaDeConexion, usuario, password);
        if(suministrador.tienesDiccionarioDe(idioma)) {
            IDiccionario diccionario = suministrador.getDiccionario(idioma);
            if(diccionario.existePalabra(palabra)) {
                List<string> definiciones = diccionario.getDefiniciones(palabra);
                // El código que muestra las definiciones
            } else {
                // Mostrar mensaje de error: No existe en el diccionario para el idioma "fr" la melón.
            }
        } else {
            // Mostrar mensaje de error: No tengo un diccionario para ese idioma
        }
    }
}

```

---

# Implementación de la librería que opera con diccionarios leyéndolos de ficheros

```c#

namespace Diccionarios.Ficheros;
using Diccionarios.API;

public class DiccionarioEnFichero : IDiccionario {
    ///... Todo el código guay
}

public class SuministradorDeDiccionariosEnFicheros : ISuministradorDeDiccionarios {

    // Constructor:
    public SuministradorDeDiccionariosEnFicheros(string carpetaDeDiccionarios) {
        // Aquí podría cargar en memoria los diccionarios que tengo en ficheros
        // O podría simplemente leer de disco cada vez que me pidan un diccionario
    }

    ///... Todo el código guay
}

```# Implementación de la librería que opera con diccionarios leyéndolos de bbdd

```c#

namespace Diccionarios.BBDD;
using Diccionarios.API;

public class DiccionarioEnBBDD : IDiccionario {
    ///... Todo el código guay
}

public class SuministradorDeDiccionariosEnBBDD : ISuministradorDeDiccionarios {
    ///... Todo el código guay
    /// Constructor:
    public SuministradorDeDiccionariosEnBBDD(string cadenaDeConexion, string usuario, string password) {
        // Aquí podría conectar a la base de datos
    }
}

```
---



Principios SOLID de desarrollo de software? 
D: Inversión de la dependencia (Dependency Inversion Principle)

Patrón de Inyección de Dependencias (Dependency Injection Pattern)

Controlador de Inversión de Control

---

TDD: Test Driven Development

    Primero definir las pruebas, después escribir el código.
    Siguiente parar cuando las pruebas pasen.
    Refactorizar el código.


Pruebas
    Unitarias               No es probar un trocito de código (una función)
                            Probar un trocito de código o una funcionalidad puede ser:
                                - Unitarias
                                - De Integración
                                - End to End
                            Depende del contexto en el que ejecutemos la prueba. 
    Integración
    Sistema(end to end)


---

NOTA AL MARGEN:

Un producto de software por definición es un producto sujeto a cambios y mantenimientos.

        Escribir código <> Pruebas -> OK -> Refactorización <> PRUEBAS -> OK -> Liberamos el programa (lo entregamos)

        <--- 50% del trabajo -------------> <--- 50% del trabajo ------------->
            20 horas                            20 horas



        Empieza el proyecto       Qué ha pasao???
        --------------------->.........................----------------------->
        15 días - 60% del proyecto                     15 días antes de la entrega
                                                            Acabamos

---

# Paradigmas de programación

No son sino un nombre hortera que los desarrolladores nos hemos inventado para referirnos a las formas de usar un lenguaje de programación.
Pero en los lenguajes naturales (los que usamos los seres humanos, existe el mismo concepto).

- Imperativo                 Dar instrucciones (órdenes) que la máquina debe ejecutar secuencialmente.
                             En ocasiones necesitamos romper esa secuencialidad (condicionales, bucles, etc).  IF, FOR, WHILE, SWITCH...

- Procedural                 Cuando juntamos muchas de esas órdenes en un bloque, y le damos un nombre.
                             Así definimos el concepto de : Procedimiento, método, rutina, función, etc.
                             Posteriormente podemos invocar ese bloque de órdenes por su nombre.
                             Qué me aporta?
                             - Reutilización de código
                             - Mejorar la estructura del código, y por ende, su legibilidad y mantenibilidad.

- Orientado a Objetos        Todo lenguaje me permite trabajar con Datos. Y esos datos son de unos determinados TIPOS.
                             Todo lenguaje ofrece una serie de tipos por defecto: Enteros, Decimales, Textos, Listas, Fechas...
                             Algunos lenguajes me permiten definir mis propios Tipos de datos, con sus características y comportamientos.

                                            Qué caracteríza a un texto?              Qué puedo hacer sobre ese dato?
                                string      La secuencia de caracteres que alberga   Convertir a mayúsculas, minúsculas, subcadena
                                fecha       Día+Mes+Año                              Formatearla DD-MM-YYYY, sumarle días
                                                                                     Preguntar si cae en bisiesto
                                Coche       Marca, Modelo, Color, Matrícula          Arrancar, Parar, Acelerar, Frenar
                            
                             Los lenguajes que me permiten definir mis propios tipos de datos, y trabajar con ellos, son lenguajes que soportan el paradigma de programación orientada a objetos.

- Funcional                  Cuando el lenguaje me permite que una variable apunte a una función y posteriormente ejecutar la función
                             desde la variable decimos que el lenguaje soporta el paradigma funcional.
                             
                             ```c#
                             string texto = "Hola";  Estamos asignando la variable "texto" al dato "hola"
                             //^^^^^^ ^^^^^   ^^^^^^
                             // tipo           Dato / objeto
                             // de la variable
                             ```
    
                             1º "Hola"          Guarda el "Hola" en memoria RAM, que es un dato de tipo string, ya que está entre comillas.
                             2º "string texto"  Prepara una variable con nombre "texto" y que puede apuntar a datos de tipo string.
                                                Si la RAM es como un cuaderno de hojas cuadriculadas, la variable es como un post-it 
                             3º "texto"         Asigno la variable (pego el post-it) al lado del dato "Hola" en la RAM.
                                                La variable apunta al dato "Hola" en la RAM.

                             ```c#
                             string texto = "Hola";
                             texto = "adios"; // (2)
                             ```

                             Que es lo primero que se ejecuta en (2)?
                             1. Se guarda "Adios" en la RAM... dónde? Donde estaba "hola" o en otro sitio?
                                En otro sitio .. Y en este punto, tengo en RAM 2 datos de tipo string: "Hola" y "Adios"
                             2. Reasignar el postsit... Reasignar la variable "texto" que estaba apuntando a "Hola", ahora apunta a "Adios"
                                Por cierto.. el "hola" queda huérfano de variable... y se convierte en un dato IRRECUPERABLE. En C, si podría recuperarlo... si tuviera la dirección de memoria RAM.. pero en C#, que es un lenguaje de más alto nivel, no trabajamos con direcciones de memoria RAM.... y ese dato ya no podría recuperarse.
                                SE CONVIERTE EN BASURA (GARBAGE)
                                Y potencialmente (puede que si... o puede que no) el Garbage Collector se encargará de liberar ese espacio en RAM para que pueda ser reutilizado por otros datos.

                                Tango C#, como Java, Como JS, Como PY tienen un Recolector de Basura (Garbage Collector) que se encarga de liberar espacio en RAM que ya no se usa. Eso hace que el desarrollo en estos lenguajes se más sencillo que por ejemplo el desarrollo en C o C++., ya que no tengo que lidiar con reservar y liberar memoria RAM. Por contra, se hace un uso más ineficiente de la RAM... se va generando basura... con la esperanza de que cuando necesite RAM el Garbage Collector haya hecho su trabajo y haya liberado espacio en RAM.

                             Qué es una variable? Una variable no es una cajita donde pongo una cosa y luego la quito y pongo otra.
                             Al menos no en C#. De hecho el concepto de variable, cambia de lenguaje a lenguaje.

                             Esa sería una definición válida para C++, C, Fortran, Pascal, etc.
                             En Python, JS, Java, C#, ... una variable es otra cosita.
                             En estos lenguajes el concepto de variable tiene más que ver con el concepto de puntero en C.
                             Una variable es una referencia a un dato que tengo en memoria RAM.

                            ```C#
                            // Programación procedural:
                            string generarSaludo(string nombre) {
                                return "Hola " + nombre;
                            }
                            string generarSaludoFormal(string nombre) {
                                return "Buenos días, Sr./Sra. " + nombre;
                            }
                            string saludo = generarSaludo("Felipe");
                            Console.WriteLine(saludo);

                            // Programación funcional.. sin lambda
                            Func<string, string> miFuncion = generarSaludo; // Solo estoy referenciando la función
                            string saludo2 = miFuncion("Felipe");           // Pa' que vale
                            Console.WriteLine(saludo2);
                            ```
                            El punto de la programación funciona no es lo que es... que es sencillo. Es lo que puedo hacer cuando el lenguaje soporta el paradigma funcional.

                            Entonces puedo hacer locuras.. como por ejemplo:
                            - Crear una función que reciba como parámetro otra función.
                            - Crear una función que devuelva otra función.
                           
                            ```c#
                            void imprimirSaludo(Func<string, string> funcionGeneradoraDeSaludos, string nombre) {
                                string saludo = funcionGeneradoraDeSaludos(nombre); 
                                // Esto me permite INYECTAR LOGICA EN TIEMPO DE JECUCION A UNA FUNCION... DESDE FUERA
                                Console.WriteLine(saludo);
                            }

                            imprimirSaludo(generarSaludo, "Felipe");
                            ```

                            Al usr programación funcional, creamos funciones no solo para mejorar la estructura del código, o reutilizar código. A veces creamos funciones SOLO PORUQUE QUEREMOS EJECUTAR UNA FUNCION QUE REQUIERE UNA FUNCION... y no me queda más remedio. Y a veces, no quiero ni reusar esa función, ni el tenerla en otro sitio definida me mejora la legibilidad... En ese escenario, C# (igual que todo el resto de lenguajes modernos) me permite definir esa función de otra forma: EXPRESION LAMBDA

                            ```c#
                            // Programación funcional con lambda
                            imprimirSaludo( (string nombre) => { return "Hola " + nombre; }, "Felipe");
                            ```


- Declarativo                En .netCore (c#) lo usamos mucho. Es lo que llamamos [atributos] o Anotaciones (Annotations en Java).

> Felipe, IF (Si) hay algo que no sea una silla debajo de la ventana, 
    >   quítalo.   IMPERATIVO
> Felipe, IF no ya hay silla debajo de la ventana, 
    > IF NO SILLA (silla == False) GOTO IKEA !
    > Felipe, Pon una silla debajo de la ventana.   IMPERATIVO

  Estamos muy acostumbrados a lenguaje imperativo... pero cada día le odiamos más. 

  Claro.. al usar un paradigma imperativo, perdemos de vista el problema que estamos tratando de resolver. MI OBJETIVO.
  Pasamos a centrarnos en cómo conseguir ese objetivo.

> Felipe, Quiero una silla debajo de la ventana. Es tu responsabilidad conseguirlo.  DECLARATIVO
  
  En el primer escenario, de quién era la responsabilidad de definir los pasos que había que seguir para conseguir el objetivo? MIA!
  En el segundo escenario? Felipe... Le he delegado la responsabilidad a él.
 
---

# Principios de desarrollo de software

Hay muchos. Qué son? Principios que guían mi toma de decisiones a la hora de escribir código.
Igual que como ser humano tengo principios morales... que guían mi toma de decisiones en mi vida diaria.

Cuidado con la palabra principio. 
En ciencias exactas (FISICA, MATEMATICAS...), Principio es sinónimo de Ley.
Pero fuera de las ciencias exactas, un principio no es una ley. Es una guía... que me ayuda a tomar decisiones.
Y nosotros no estamos en una ciencia exacta (Ciencias de la computación). Estamos en Ingeniería de software.
Donde tenemos que resolver un problema concreto con restricciones de recursos (tiempo, dinero...)

Como ser humano, puedo tener un principio que diga "Priorizo mi vida familiar sobre mi vida laboral".

En el mundo del software los principios son guías que me ayudan a tomar decisiones a la hora de escribir código.
Si sigo/respeto esos principios, mi código será mejor (más fácil de mantener, más fácil de entender, más fácil de evolucionar, más fácil de probar, etc).
Esa es la promesa!

De entre todos ellos (YAGNI, SoC, KISS, DRY, etc) los que más gente sigue son los principios SOLID. Recopilados por Robert C. Martin (Uncle Bob) por primera vez en su libro "Clean code":
- S: Single Responsibility Principle (SRP)    Principio de Responsabilidad Única
- O: Open/Closed Principle (OCP)              Principio de Abierto/Cerrado
- L: Liskov Substitution Principle (LSP)      Principio de Sustitución de Liskov
- I: Interface Segregation Principle (ISP)    Principio de Segregación de Interfaces
- D: Dependency Inversion Principle (DIP)     Principio de Inversión de Dependencias
     Un componente (case) de alto nivel de un sistema NUNCA debe de depender de una implementación concreta (clase) de un componente de bajo nivel. En su lugar ambos deben depender de una abstracción (interfaz=API).

l

       Diccionarios.App  -> Diccionarios.API <- Diccionarios.Ficheros
                  |                                      ^
                  +--------------------------------------+  <--- Esta dependencia es la viola el principio DIP

# Cómo salgo de aquí.

Estos problemas por suerte, están muy estudiados. Y las soluciones son conocidas.
Hay varios patrones de desarrollo de software que me permiten salir de este entuerto.

Un patrón es una forma de escribir mi código estandarizada... un principio NO. Un principio es algo que puedo respetar o no.Es un concepto... una idea.

## Patrón de Inyección de Dependencias (Dependency Injection Pattern)

El patrón que usamos hoy en día para salir de este entuerto es el patrón de Inyección de Dependencias (Dependency Injection Pattern).
Qué es esto?

Es un patrón que me dice: Cuando escribas tu código, asegúrate que nunca crees en una clase una instancia de un objeto que necesites.
Me dice... en su lugar, haz que ese objeto te sea suministrado desde fuera.

Aunque... realmente lo único que estamos haciendo es pasarle la pelota a otro!
Al final, alguien habrá que tenga que decir: `new SuministradorDeDiccionariosEnBBDD(cadenaDeConexion, usuario, password)`
Y eso está bien... el problema es dónde decimos eso.

---

# Somos una empresa fabricante de bicicletas.

> BTWIN (Las del decathlon)

Voy a fabricar el modelo de bicicleta "BTT 900"
- Cuando estoy diseñando esa bicicleta, que voy a decir:
   - OPCION A: Esa bicicleta lleva una rueda de marca "Mavic" modelo: "Crossride XP1892746"
   - OPCION B: Esa bicicleta lleva una rueda de 26 pulgadas, de radios de acero, con buje sellado y llanta de aluminio, de grosor 2.1 pulgadas.

    No quiero atarme a un modelo concreto. Me ato a qué? A una ESPECIFICACIÓN de la rueda: API! (Interfaz, contrato)
 
 Así haré con todas los componentes de la bicicleta:
    - Cuadro: Quiero un cuadro de aluminio, talla M, color rojo
    - Frenos: Quiero frenos de disco hidráulicos de 160mm con una presión de frenado de 800N
    - Sillín de cuero... etc
 Diseño el sistema en base a ESPECIFICACIONES (APIs, Interfaces, Contratos) de los componentes que necesito.

 Ahora bien.. al final la bici, la física, la que entrego no es una colección de especificaciones.
 Al final la bici es una colección de componentes concretos:
 - Rueda: Marca "Mavic" modelo: "Crossride XP1892746"
 - Cuadro: Marca "BTWIN" modelo: "Aluminio talla M color rojo
 - Frenos: Marca "Shimano" modelo: "Deore XT hidráulicos de
 - Sillín: Marca "Selle Royal" modelo: "Lookin"

 Y mañana, puedo sacar una segunda edición de la bici, con otros componentes concretos.... siempre y cuando cumplan las mismas especificaciones... sin mucho trabajo.


.netCore nos ayuda con esto. Nos ayuda a tener un sitio final donde decir: `new SuministradorDeDiccionariosEnBBDD(cadenaDeConexion, usuario, password)`. De qué fichero estamos hablando? De un fichero llamado: Program.cs

En ese fichero es donde junto los componentes concretos que necesito para mi aplicación.

Y esto lo puedo hacer gracias a que .net Core incorpora un contenedor de Inversión de Control (IoC Container).

# Contenedor de inversión de control (IoC Container)

Imaginad lo siguiente. Quiero montar una ETL. (Extract, Transform, Load)
El típico proceso Batch que ejecuto los jueves a las 3 de la mañana, que extrae datos de un sistema origen, los transforma y los carga en un sistema destino.

# Paradigma declarativo.
- Ah.. y que cuando acabe, me mande un email informando.
- Ah! Que de las personas lea: Nombre, Apellidos, DNI, Fecha de Nacimiento, Email.
- Ah.. y que valide el dni de la persona.. si está mal, que no lleve a la persona al destino.
- Por cierto.. el destino es una bbdd SQL Server.
- Ah.. y que cuando arranque también me mande un email informando.
- Ah.. y que el email de las personas lo valide con una expresión regular.
- Ah.. y los datos de las personas que no valide, los guarde en un fichero de texto plano con el error que tienen.
- Quiero un programa que lea datos de personas de un archivo EXCEL

Al escribir lenguaje declarativo, no me preocupo del flujo. 
Un motor (contenedor) de inversión de control se encargará de aportar el flujo a mi programa.... partiendo de las piezas que yo le he dicho que use.... los componentes concretos que he elegido para mi aplicación.
Al usar un contenedor de inversión de control, yo no daré el flujo. El flujo lo dará el contenedor de inversión de control.
Yo lo que tengo es que conocer el flujo que aporta el contenedor de inversión de control.
.netCore va a aportar el flujo a mi programa... no lo voy a escribir yo. 
Si no lo conozco... me volveré loco al intentar entender mi programa.... porque en él no habrá un flujo explicito.

Nuestro programa, básicamente va a tener 1 línea de código: .netCore... ejecuta mi programa.
Y lo único que tendré que hacer es explicarle / detallarle, DECLARARLE a .netCore que componentes concretos quiero usar en mi programa.

Y él se encarga. Esto tiene 2 ventajas gigantes:
1. Me olvido de tener que escribir el flujo de mi programa.
2. Puedo usar un patrón de diseño llamado Inyección de Dependencias (Dependency Injection Pattern) de forma muy sencilla.
   De hecho viene de serie con .netCore.

No es solo cambiar un framework por otro. Es cambiar la forma de trabajar, la forma de diseñar el sistema, la forma de gestionar el proyecto, las herramientas que uso, los principios que sigo, los patrones que uso, etc.

Si no hago nada de eso.. y sigo escribiendo código monolítico, con arquitecturas monolíticas, con metodologías en cascada, donde solo voy a hacer pruebas al final del proyecto 1 sola vez... entonces mejor sigo con el .net Framework. Para qué quiero venir a esta otra fiesta? .net Framework se creo en ese ecosistema/contexto.. Y ahí se movía como pez en el agua.
El problema es que en el contexto actual, .net Framework no sirve... porque no me permite hacer todo eso que necesito hacer hoy en día.


---

# Escribamos el programa en pseudocódigo: IMPERATIVO!

1. Mandar un email informando que arranca el proceso
2. Abrir el fichero EXCEL
3. Por cada fila del fichero EXCEL: FOR
  3.1 Leer los datos de la persona (Nombre, Apellidos, DNI, Fecha de Nacimiento, Email) 
  3.2 Validar el email con una expresión regular
  3.3 Si el email no es válido: IF
    3.3.1: Escribir los datos de la persona en un fichero de texto plano con el error que tiene
  3.4 Validar el dni de la persona
  3.5 Si el dni no es válido: IF
    3.5.1: Escribir los datos de la persona en un fichero de texto plano con el error que tiene
  3.6 Si el email y el dni son válidos: IF
    3.6.1: Insertar la persona en la bbdd
4. Cerrar el fichero EXCEL
5. Mandar un email informando que el proceso ha finalizado

Al escribir código imperativo, es importante establecer el FLUJO de ejecución del programa... de hecho es lo que estamos haciendo.
---

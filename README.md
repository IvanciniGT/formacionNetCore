
# Pasos para crear la app:

1. Crear una solucion con dotnet

2. Crear el proyecto (librería) del API de diccionarios             CLASSLIB (librería)
   Añadir ese proyecto a la solución

3. Crear el proyecto implementación a ficheros del api              CLASSLIB (librería)
   Añadir ese proyecto a la solución

4. Crear un proyecto de pruebas para el proyecto de implementación  XUNIT(librería pero de pruebas)
   Añadir ese proyecto a la solución


Lo siguiente es establecer las DEPENDENCIAS ENTRE PROYECTOS
 (esto lo ejecutamos dentro de la carpeta del proyecto donde queremos meter la dependencia) 
 dotnet add reference <ruta del proyecto dependencia> 
    Ficheros -> API
    Ficheros.Test -> Ficheros, API


Tenemos que montar ahora una app de consola que trabaje con esta librería de diccionarios.

La vamos a montar poco a poco:
1. Código hardocodeado = SENCILLO => Inmantenible a largo plazo
^^^
50 líneas de código

2. Código con inyección de dependencias manual = MAS CURRO HOY Y algo más mantenible a largo plazo
3. Código con inyección de dependencias automática = MAS CURRO HOY Y MÁS MANTENIBLE A LARGO PLAZO
4. Con inversión de control (HOST) y la inyección de dependencias automática = MUCHISISMO MÁS CURRO HOY Y MUCHISIMO MAS MANTENIBLE A LARGO PLAZO
^^^
300 líneas de código

Estamos con una app de consola... y nos podemos permitir el lujo de comenzar con un código hardcodeado y sin usar Inversión de Control (HOST).

Pero.. en una app web no nos lo podemos permitir. Tenemos que comenzar con Inversión de Control (HOST) y la inyección de dependencias automática.
---

Por ahora, el objetivo es montar un proyecto nuevo, DiccionariosAppConsola, que use la librería DiccionariosFicheros y que haga lo siguiente:
- Mostrar un mensaje de bienvenida (Con iconos chulos)
- Recibe como argumentos el idioma y la palabra a buscar
- - Mira a ver si hay un diccionario para ese idioma
- - Si no lo hay, avisa al usuario y termina (Con iconos chulos)
- - Si lo hay, mira a ver si la palabra existe en el diccionario
- - - Si no existe, avisa al usuario y termina (Con iconos chulos)
- - - Si existe, muestra los significados (Con iconos chulos)
- - Termina
- Mostrar un mensaje de despedida y agradecimiento (Con iconos chulos)

Por ahora, nada de nada de inyección de dependencias ni inversión de control.



----

# Segunda variante del proyecto de AppConsola:

En este caso vamos a comenzar con la inyección de dependencias manual. Y no vamos a usar todavía inversión de control (HOST).
Esto ya me revuelve algunos problemas de mnto. 
Voy a seguir yo creando las instancias de las clases, pero solamente tendré una función en todo el proyecto que se encargue de crear las instancias. Si algun día hay que cambiar una implementación por otra, solamente habrá que cambiar esa función.
Pasra esto vamos a usar una clase que nos ofrece .net llamada ServiceCollection:
- Nos creamos un serviceCollection
- Registramos en el serviceCollection las implementaciones de las interfaces que vamos a usar
- Creamos un serviceProvider a partir del serviceCollection
- La misión del ServiceCollection es registrar las implementaciones de las interfaces que le pidamos.
    - A tal Interfaz , usas tal Implementación
- La misión del ServiceProvider es darnos instancias de las clases que le pidamos, y resolver las dependencias que tengan esas clases.
    - Dame una instancia de tal Interfaz, y resuelve las dependencias que tenga esa implementación.

Dejaremos una función aparte en el código (incluso en su propio fichero) que se encargue de la gestión de dependencias, del trabajo con el ServiceCollection y el ServiceProvider.

PERO! OJO! Todo esto lo montaremos en un nuevo proyecto, para no liarla con el proyecto anterior.
El que tenemos ahora es el DiccionariosAppConsola.
El nuevo que vamos a montar es DiccionariosAppConsolaConDI (DI = Dependency Injection = Inyección de Dependencias)


```csharp
   var collection = new ServiceCollection();
   //collection.AddSingleton<IInterfaz, Implementacion>();
   collection.AddTransient<ISuministradorDeDiccionarios, SuministradorDeDiccionariosDesdeFicheros>();


   var provider = collection.BuildServiceProvider();
   var suministrador = provider.GetService<ISuministradorDeDiccionarios>();
   // Podría incluso haber muchas implementaciones de IDiccionario, en ese caso, obtendría todas ellas y elegiría la que me interesara.. o si incluso quiero trabajar con todas ellas.
   var suministradores = provider.GetServices<ISuministradorDeDiccionarios>();
   // Para cada suministrador podríamos preguntar si tiene el diccionario que queremos, y si lo tiene, usarlo.

   foreach (var suministrador in suministradores)
   {
       if (suministrador.TieneDiccionario(idioma))
       {
           var diccionario = suministrador.ObtenerDiccionario(idioma);
           // Usar el diccionario
           break;
       }
   }
```
---

Queremos el siguiente paso ahora... Será un tercer proyecto de app de consola, que usará inyección de dependencias manual (Suministrando nosotros la instancia que debe entregarse al solicitar una interfaz) y además usará inversión de control (HOST) para gestionar el ciclo de vida de la aplicación y la inyección de dependencias automática (El HOST se encargará de crear las instancias y resolver las dependencias).
Aquí trabajamos con otros concepto:
- Host: Es el contenedor que gestiona el ciclo de vida de la aplicación y la inyección de dependencias automática. Es el que ofrece la Inversión de Control.
- Configuración del Host: Es el proceso de configurar el Host:
  - Registro de los servicios y las implementaciones de las interfaces que vamos a usar.
  - Configuración del logging
  - Lectura de parametros de configuración (appsettings.json, variables de entorno, argumentos de línea de comandos, etc.)
  - Lectura de variables de entorno para aplciar configuraciones específicas para cada entorno (desarrollo, producción, etc.)

El host ya lleva su propio flujo.
Lo que hará será:
- Crear el host
- Configurar el host
- Configurar los servicios (Inyección de dependencias)
- Leer los parámetros de configuración
- Leer las variables de entorno
- Construir el host
- Iniciar el host
- Ejecutar la aplicación

Aqui va a haber un cambio grande de enfoque...
La propia aplicacionDeConsola, la vamos a convertir en un servicio más del host.

# Lo que hay:
      MAIN (arranque)
        v
    AppConsola ---> DiccionariosAPI
    
    De donde saca la AppDeConsola la instancia de DiccionariosAPI?
    Del ServicePrivider... La coge ella (esa clase). Esa clase(AppConsola)solicita explicitamente la instancia que necesita al ServiceProvider.

# Lo nuevo

    MAIN > HOST > Servicios: - DiccionariosAPI                < DiccionariosFicheros
             |               - UIAplicacionDeDiccionariosAPI  < UIAplicacionDeDiccionariosConsola
             +-> Coge la AplicacionDeDiccionariosAPI que se ha registrado como servicio



                  y la ejecuta. Nuestro AplicacionDeDiccionariosConsola solicitaré en su constructor
                    la instancia de DiccionariosAPI que necesita, y el HOST se la suministrará

                    La responsabilidad de obtener un suministradorDeDiccionarios ya no la tendrá nuestra app, la tendrá el HOST.

                    Esto me permitirá el día de mañana tener varias implementaciones de la App:
                    - Consola
                    - Escritorio
                    - Web
                 Y el código no cambiará nada. Solamente cambiará el registro del servicio en el HOST.

Esa UIAplicacionDeDiccionariosAPI
tendrá sus nuevos métodos:
- MostrarMensajeBienvenida()
- MostrarMensajeDespedida()
- MostrarSignificadosDePalabra()
- MostrarErrorNoHayDiccionario()
- MostrarErrorNoHayPalabra()

Nuestra implementación actual de DiccionarioFicheros, necesita una ruta de ficheros para funcionar.
Vamos a meter esa ruta en un fichero de configuración appsettings.json
Y vamos a configurar el HOST para que lea ese fichero de configuración se encargue de pasarle esa ruta a la implementación de DiccionarioFicheros. SuministradorDeDiccionariosDesdeFicheros pedirá esa ruta en su constructor, y el HOST se la suministrará.


---

Si somos Decatlon,creando bicletas, lo que harías es trabajar con ESPECIFICACIONES = APIs
 - DiccionariosAPI (Interfaz)
 - UIAplicacionDeDiccionariosAPI (Interfaz)

Ahora, cuando monte una bici, ya no trabajo con especificaciones, trabajo con COMPONENTES concretas.

Nuestra bici es el HOST (la aplicación), que trabajará con componentes concretos:

 - DiccionariosFicheros (Implementación de DiccionariosAPI)
 - UIAplicacionDeDiccionariosConsola (Implementación de UIAplicacionDeDiccionariosAPI)


---

# UML? 

Estándar (ISO) para definir GRAFICOS/DIAGRAMAS
Nos ofrece un lenguaje para crear DIAGRAMAS.

La idea es estandarizar los diagramas para que todo el mundo los entienda.
Esto tiene más años que maricataña... Y CAYO TOTALMENTE EN DESUSO.
Por qué? La idea era buena.. muy buena...
Pero los programas que había para pintar esos diagramas eran un tostón. Te pasabas horas! Eran tipo PowerPoint.. con cajitas.. flechitas..
Y encima hay que mantenerlo!

Hoy en día estamos empezando de nuevo a usarlos muchísimo, porque la idea es buena, y ahora tenemos herramientas que nos permiten generarlos automáticamente.

Lo que tenemos hoy en día son LENGUAJES para escribir diagramas de UML
Ya no los pintamos... los escribimos en un lenguaje de texto, y luego una herramienta los pinta por nosotros:
- PlantUML                      Soporta el estandar completo de UML. Es un lenguaje ASPERO! Y Además los graficos son feos.
- Mermaid (El que usa GitHub)  No soporta el estandar completo de UML. Es un lenguaje MUY SUAVE! Y los graficos son bonitos.

Y la gracia es que con ellos, los diagramas no los pintamos, los escribimos. Y de hecho, ni los escribimos. Hay gente hoy en día que son mucho mejores que nosotros escribiendo textos: Las IAs
Y les vamos a pedir que nos ayuden con los diagramas.


```csharp
public class HolaMundo
{
    public static void Main()
    {
        Console.WriteLine("Hola Mundo");
    }
}
```

Estos programas suelen admitir Mermaid sin problema como código


Yo ahora quiero generar dentro de este archivo (MARKDOWN) un diagrama de componentes que represente la arquitectura de la aplicación que hemos montado. En Markdown podemos usar código
Que básicamente son:

    MAIN > HOST > Servicios: - DiccionariosAPI                < DiccionariosFicheros
             |               - UIAplicacionDeDiccionariosAPI  < UIAplicacionDeDiccionariosConsola
             +-> Coge la AplicacionDeDiccionariosAPI que se ha registrado como servicio
Orientacion girada Derecha-Izquierda
```mermaid

classDiagram
    direction LR
    class Main {
        +Main()
    }
    class Host {
        +Start()
        +ConfigureServices()
        +Build()
        +Run()
    }
    class DiccionariosAPI {
        <<interface>>
        +ObtenerDiccionario(idioma: string): IDiccionario
        +TieneDiccionario(idioma: string): bool
    }
    class DiccionariosFicheros {
        +DiccionariosFicheros(ruta: string)
        +ObtenerDiccionario(idioma: string): IDiccionario
        +TieneDiccionario(idioma: string): bool
    }
    class UIAplicacionDeDiccionariosAPI {
        <<interface>>
        +MostrarMensajeBienvenida()
        +MostrarMensajeDespedida()
        +MostrarSignificadosDePalabra(palabra: string, significados: List<string>)
        +MostrarErrorNoHayDiccionario(idioma: string)
        +MostrarErrorNoHayPalabra(palabra: string)
    }
    class UIAplicacionDeDiccionariosConsola {
        +UIAplicacionDeDiccionariosConsola()
        +MostrarMensajeBienvenida()
        +MostrarMensajeDespedida()
        +MostrarSignificadosDePalabra(palabra: string, significados: List<string>)
        +MostrarErrorNoHayDiccionario(idioma: string)
        +MostrarErrorNoHayPalabra(palabra: string)
    }

    Main --> Host : Inicia
    Host --> UIAplicacionDeDiccionariosAPI : Registra como servicio
    Host --> DiccionariosAPI : Registra como servicio
    DiccionariosAPI <|.. DiccionariosFicheros : Implementa
    UIAplicacionDeDiccionariosAPI <|.. UIAplicacionDeDiccionariosConsola : Implementa
```
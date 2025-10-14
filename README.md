
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
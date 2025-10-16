
App Diccionarios: .net Core
----------------------------------

Realmente lo estamos montando con .NET

Hoy en día ya no se llama .NET Core, sino simplemente .NET

Dentro de eso estamos usando C#.

¿Qué diferencias vemos entre .NET Framework y .NET Core/.NET?
- .NET Framework es Windows only. .NET Core/.NET es cross-platform (Windows, Linux, MacOS)
- Antiguamente (.net Framework) teníamos que instalar el framework en cada máquina donde quisiéramos ejecutar una aplicación .NET. 
- Ahora (.NET Core/.NET) las aplicaciones son auto-contenidas, llevan todo lo necesario para ejecutarse... y no necesitan tener el framework instalado en la máquina. De hecho si una app usa solo 4 librerías del framework, solo esas 4 librerías se incluyen en la app.

- Con el paso de los años los lenguajes también han evolucionado. C# ha cambiado mucho en los últimos años. Ahora tenemos LINQ, async/await, records, pattern matching...

La arquitectura, los planteamientos es lo que ha cambiado. Ahora todo es mucho más modular, mucho más ligero, mucho más portable.


---

# Qué es Linux?

No es un Sistema Operativo. Es un kernel de SO.

Igual que Windows no es un sistema operativo. Windows es una familia de sistemas operativos:
- MSDOS, Windows 95, Windows 98, Windows XP, Windows 7, Windows 10, Windows 11... Server 2003, Server 2008, Server 2012, Server 2016, Server 2019...

Un sistema operativo no es un programa, que instalo en mi máquina. Son cientos o miles de programas, que trabajan juntos.
Y todos sistema operativo tiene partes:
- Cargador de arranque
- Kernel
- Shell: cmd, powershell, sh, bash...
- Herramientas de sistema: Utilidad de discos, explorador de archivos, administrador de tareas...
- Algunos SO traen incluso entorno gráfico: Windows: FluentDesign, MacOS Aqua, GNU/Linux: Gnome, KDE...

Microsoft a lo largo de su historia ha creado y usado 2 kernels de SO:
- El kernel de DOS (Disk Operating System) y se usó para montar muchos sistemas operativos muy diferentes entre si:
  - MS-DOS
  - Windows 3., 95, 98, Millenium
- El kernel NT (New Technology) y se usa para montar muchos sistemas operativos muy diferentes entre si:
  - Windows NT
  - Windows XP, Servers, 7, 10, 11

Linux es un kernel que se usa para montar MUCHOS SO:
- Android
- GNU/LINUX:  GNU 70% /Linux 30% -> Éste es el que se distribuye mediante compilaciones: 
  - Debian -> Ubuntu
  - Redhat Enterprise Linux: -> CentOS, Fedora, Oracle Linux
  - SUSE -> openSUSE

Windows 10, Server... llevan su propio kernel NT... pero, en paralelo con él me permiten ejecutar un kernel Linux, mediante WSL (Windows Subsystem for Linux).

Por qué microsoft ha gastado una pasta en hacer que su SO pueda rodar nativamente un kernel Linux? CONTENEDORES!

Todo, TODO, absolutamente todo el software empresarial hoy en día está disponible para su distribución en contenedores. Y de hecho es la forma favorita y preferida de distribuir software empresarial:
- SQL Server
- Oracle database
- Weblogic
- SonarQube
- Nginx
- Apache
- Redis
- RabbitMQ
- MongoDB

Los contenedores se basan en funcionalidad que ofrece en kernel Linux. Por eso Microsoft decidió implementar WSL en Windows.
La apuesta de microsoft por los contenedores y Linux es ENORME!

---

DiccionariosAPI (interfaces x 2)
DiccionarioFicheros (implementación)

      App(Host) -> UI                   <- UIConsola
                -> API De diccionarios  <- DiccionariosFicheros <- Tests
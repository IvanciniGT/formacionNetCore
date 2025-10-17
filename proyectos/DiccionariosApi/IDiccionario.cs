namespace DiccionariosApi;

public interface IDiccionario
{
    // Estamos definiendo una property de los diccionarios... que puede consultarse
    string Idioma { get; }
    string Codigo {     // Esta implementación está aquí simplemente para asegurarnos que el resto del código sigue compilando
        get => "DIC_" + Idioma;
    }

    // Vamos a crear una función, para ver si un diccionario contiene una palabra 
    bool Existe(string palabra);

    // Vamos a crear una función para obtener los significados de una palabra
    // Estamos usando tipos de datos nullables, que pueden aceptar null
    // Eso es una característica que tenemos en C# 8.0 en adelante
    // Y tiene que estar activada.. aunque se activa por defecto en .NET Core
    // Está en el fichero de configuración del proyecto: DiccionariosApi.csproj
    // Dentro del nodo <PropertyGroup> hay que poner:
    // <Nullable>enable</Nullable>
    IList<string>? GetSignificados(string palabra);
}

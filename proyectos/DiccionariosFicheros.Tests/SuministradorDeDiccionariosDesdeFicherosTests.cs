namespace DiccionariosFicheros.Tests;

using DiccionariosFicheros;

public class UnitTest1
{
    // Ese [Fact] es un atributo que podemos poner antes de una método para indicar que es un test
    // Cuando ejecutemos dotnet test, se ejecutarán todos los métodos que tengan ese atributo
    [Fact]
    public void ComprobarQueMeDevuelveTrueSiLePreguntoPorUnIdiomaDelQueTieneDiccionario()
    {
        // Contexto         Given
        // Dado que tengo un suministrador de diccionarios desde ficheros
        var suministrador = new SuministradorDeDiccionariosDesdeFicheros("./Diccionarios");
        // Y que ese suministrador tiene acceso a un diccionario en español
        // Aunque ponga la carpeta así, esa carpeta no la va a encontrar dentro del dll
        // HAy que pedirlo. Lo tenemos que configurar en el archivo .csproj
        // Acción           When
        // Cuando le pregunto si tiene un diccionario en español
        var respuesta = suministrador.TienesDiccionarioDe("ES");
        // Comprobación     Then
        // Entonces me tiene que devolver true
        Assert.True(respuesta);
        // La libreria XUNIT de .netCore nos ofrece muchos métodos de aserción
        // Están dentro de Assert.
        // Estos métodos son los que usamos SIEMPRE en el bloque de comprobación (THEN)
        // Aseguran que una condición se cumpla. 
        // Si se cumple, dan la prueba por Superada
        // Si no se cumple, dan la prueba por Fallida
    }

    [Fact]
    public void ComprobarQueMeDevuelveFalseSiLePreguntoPorUnIdiomaDelQueNoTieneDiccionario()
    {
        // Contexto         Given
        // Dado que tengo un suministrador de diccionarios desde ficheros
        var suministrador = new SuministradorDeDiccionariosDesdeFicheros("./Diccionarios");
        // Y que ese suministrador no tiene acceso a un diccionario del idioma de los elfos
        // Acción           When
        // Cuando le pregunto si tiene un diccionario del idioma de los elfos
        var respuesta = suministrador.TienesDiccionarioDe("ELF");
        // Comprobación     Then
        // Entonces me tiene que devolver false
        Assert.False(respuesta);
    }

    [Fact]
    public void ComprobarQueMeDevuelveUnDiccionarioSiLePidoUnDiccionarioDeUnIdiomaDelQueTieneDiccionario()
    {
        // Contexto         Given
        // Dado que tengo un suministrador de diccionarios desde ficheros
        var suministrador = new SuministradorDeDiccionariosDesdeFicheros("./Diccionarios");
        // Y que ese suministrador tiene acceso a un diccionario en español
        // Acción           When
        // Cuando le pido el diccionario en español
        var diccionario = suministrador.DameDiccionarioDe("ES");
        // Comprobación     Then
        // Entonces me tiene que devolver un diccionario no nulo
        Assert.NotNull(diccionario);
        // Y ese diccionario tiene que ser del idioma español
        Assert.Equal("ES", diccionario.Idioma);
    }


    [Fact]
    public void ComprobarQueNoMeDevuelveUnDiccionarioSiLePidoUnDiccionarioDeUnIdiomaDelQueNOTieneDiccionario()
    {
        // Contexto         Given
        // Dado que tengo un suministrador de diccionarios desde ficheros
        var suministrador = new SuministradorDeDiccionariosDesdeFicheros("./Diccionarios");
        // Y que ese suministrador tiene acceso a un diccionario en español
        // Acción           When
        // Cuando le pido el diccionario del idioma de los elfos
        var diccionario = suministrador.DameDiccionarioDe("ELF");
        // Comprobación     Then
        // Entonces me tiene que devolver un diccionario no nulo
        Assert.Null(diccionario);
    }
}
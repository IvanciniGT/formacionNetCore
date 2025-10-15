namespace DiccionariosFicheros.Tests;

using DiccionariosFicheros;

public class DiccionarioDesdeFicherosTests
{
    private readonly DiccionarioDesdeFichero diccionarioEspanol;

    public DiccionarioDesdeFicherosTests()
    {
        var palabrasYDefiniciones = new Dictionary<string, IList<string>>
        {
            { "MELÓN", new List<string> { "Fruto de la melonera", "Persona poco inteligente" } },
            { "CASA", new List<string> { "Edificio para habitar", "Familia o linaje" } },
            { "ÁRBOL", new List<string> { "Planta leñosa de gran tamaño" } }
        };

        diccionarioEspanol = new DiccionarioDesdeFichero("Español", palabrasYDefiniciones);
    }
    [Fact]
    public void ComprobarElIdiomaDeUnDiccionario()
    {
        // Dado un diccionario de Idioma Español
        // (ya tenemos diccionarioEspanol)

        // Cuando le pregunto por su idioma
        var idioma = diccionarioEspanol.Idioma;

        // Entonces me tiene que devolver Español
        Assert.Equal("Español", idioma);
    }

    [Fact]
    public void ComprobarSiMeDiceNoExisteUnaPalabraCuandoLePasoComoPalabraNull()
    {
        // Dado un diccionario 
        // (ya tenemos diccionarioEspanol)

        // Cuando le pregunto si existe la palabra null
        var existe = diccionarioEspanol.Existe(null!);

        // Entonces me tiene que devolver false
        Assert.False(existe);
    }

    [Fact]
    public void ComprobarSiMeDiceQueExisteUnaPalabraQueSeQueExiteEnElDiccionario()
    {
        // Dado un diccionario con la palabra Melón
        // (ya tenemos diccionarioEspanol con "melón")

        // Cuando le pregunto si existe la palabra Melón    
        var existe = diccionarioEspanol.Existe("melón");

        // Entonces me tiene que devolver true
        Assert.True(existe);
    }

    [Fact]
    public void ComprobarSiMeDiceQueExisteUnaPalabraQueSeQueExiteEnElDiccionarioEscritaDeFormaDistinta()
    {
        // Dado un diccionario con la palabra "melón"
        // (ya tenemos diccionarioEspanol con "melón")

        // Cuando le pregunto si existe la palabra "Melón" (con mayúscula)    
        var existe = diccionarioEspanol.Existe("Melón");

        // Entonces me tiene que devolver true
        Assert.True(existe);
    }

    [Fact]
    public void ComprobarSiMeDiceQueNoExisteUnaPalabraQueSeQueNoExiteEnElDiccionario()
    {
        // Dado un diccionario sin la palabra Archilococo
        // (nuestro diccionario no tiene esta palabra)

        // Cuando le pregunto si existe la palabra Archilococo    
        var existe = diccionarioEspanol.Existe("Archilococo");

        // Entonces me tiene que devolver false
        Assert.False(existe);
    }

    [Fact]
    public void ComprobarSiMeDevuelveNullAlPedirLosSignificadosDeUnaPalabraQueNoExisteEnElDiccionario()
    {
        // Dado un diccionario sin la palabra Archilococo
        // (nuestro diccionario no tiene esta palabra)

        // Cuando le pido los significados de la palabra Archilococo
        var significados = diccionarioEspanol.GetSignificados("Archilococo");

        // Entonces me tiene que devolver null
        Assert.Null(significados);
    }

    [Fact]
    public void ComprobarSiMeDevuelveLosSignificadosAlPedirLosSignificadosDeUnaPalabraQueSiExisteEnElDiccionario()
    {
        // Dado un diccionario con la palabra melón y sus significados
        // (ya tenemos diccionarioEspanol con "melón")

        // Cuando le pido los significados de la palabra melón
        var significados = diccionarioEspanol.GetSignificados("melón");

        // Entonces me tiene que devolver los significados de la palabra melón
        Assert.NotNull(significados);
        Assert.Equal(2, significados.Count);
        Assert.Contains("Fruto de la melonera", significados);
        Assert.Contains("Persona poco inteligente", significados);
    }

    [Fact]
    public void ComprobarSiMeDevuelveNullAlPedirLosSignificadosDeUnaPalabraNull()
    {
        // Dado un diccionario
        // (ya tenemos diccionarioEspanol)

        // Cuando le pido los significados de la palabra null
        var significados = diccionarioEspanol.GetSignificados(null!);

        // Entonces me tiene que devolver null
        Assert.Null(significados);
    }

    [Fact]
    public void ComprobarSiMeDevuelveLosSignificadosDeUnaPalabraEscritaDeFormaDistinta()
    {
        // Dado un diccionario con la palabra "melón"
        // (ya tenemos diccionarioEspanol con "melón")

        // Cuando le pido los significados de la palabra "Melón" (con mayúscula)
        var significados = diccionarioEspanol.GetSignificados("Melón");

        // Entonces me tiene que devolver los significados de la palabra melón
        Assert.NotNull(significados);
        Assert.Equal(2, significados.Count);
        Assert.Contains("Fruto de la melonera", significados);
        Assert.Contains("Persona poco inteligente", significados);
    }
}

// Todo modelo de IA se alimenta de 2 cosas:
// 1.- Prompts
// 2.- Contexto: Ficheros con el código de vuestro proyecto
//               El historial completo de la conversacion que haya mantenido con él
// Consejos:
// No hay prisa. No intenteis que con un prompt la herramienta consiga hacer lo que quereis. IMPOSIBLE.. FALLARA!
// Hablar con ella, explicarle, FRENARLA
// Estos modelos son una herramienta. NO SON EL TIPO LISTO
// El tipo listo SOY YO. Yo soy quien va a los mandos de la herramienta
// No puedo puedo permitir que la herramienta me domine a mi y vaya marcando el ritmo = RUINA ENORME!
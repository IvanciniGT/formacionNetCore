namespace DiccionariosFicheros;

public static class Utilidades
{
    public static bool TengoUnArchivoParaElIdioma(string idioma, string rutaCarpetaDiccionarios)
    {
        var rutaFicheroDiccionario = Path.Combine(rutaCarpetaDiccionarios, $"{idioma}.txt");
        return File.Exists(rutaFicheroDiccionario);
    }
}

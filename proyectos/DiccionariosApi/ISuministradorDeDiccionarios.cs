namespace DiccionariosApi;

public interface ISuministradorDeDiccionarios
{

    // Una función que nos confirme si tenemos disponible un diccionario para un idioma
    bool TienesDiccionarioDe(string idioma);

    // Otra función para obtener el diccionario de un idioma
    IDiccionario? DameDiccionarioDe(string idioma);
}
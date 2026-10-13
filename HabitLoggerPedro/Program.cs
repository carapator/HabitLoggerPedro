using HabitLoggerPedro;

LogUsuario.Registrar("Aplicación iniciada");

try
{
    if (BaseDatos.Inicializar())
    {
        Menu.Ejecutar();
        Console.WriteLine("¡Hasta pronto!");
    }
    else
    {
        Consola.MostrarError("No se pudo iniciar la base de datos. La aplicación se cerrará.");
        Consola.Pausa();
    }
}
catch (Exception ex)
{
    RegistroErrores.Registrar("Program", ex);
    Consola.MostrarError("Error inesperado. La aplicación se cerrará de forma controlada.");
    Consola.Pausa();
}

LogUsuario.Registrar("Aplicación cerrada");

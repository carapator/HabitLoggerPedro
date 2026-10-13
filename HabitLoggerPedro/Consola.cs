using System.Globalization;

namespace HabitLoggerPedro;

public static class Consola
{
    public static void MostrarError(string mensaje) => MostrarColor(mensaje, ConsoleColor.Red);
    public static void MostrarExito(string mensaje) => MostrarColor(mensaje, ConsoleColor.Green);
    public static void MostrarAviso(string mensaje) => MostrarColor(mensaje, ConsoleColor.Yellow);

    private static void MostrarColor(string mensaje, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(mensaje);
        Console.ResetColor();
    }

    public static void Pausa()
    {
        Console.Write("\nPulsa Enter para continuar...");
        Console.ReadLine();
    }

    public static string LeerTexto(string mensaje)
    {
        Console.Write(mensaje);
        string? linea = Console.ReadLine();

        // ReadLine devuelve null si se cierra la entrada (Ctrl+Z / Ctrl+D)
        if (linea == null)
        {
            Console.WriteLine("\nEntrada cerrada. Saliendo...");
            Environment.Exit(0);
        }

        return linea.Trim();
    }

    public static int LeerEntero(string mensaje, int minimo, int maximo)
    {
        while (true)
        {
            string texto = LeerTexto(mensaje);

            if (!int.TryParse(texto, out int numero))
                MostrarError("Debes introducir un número entero.");
            else if (numero < minimo || numero > maximo)
                MostrarError($"El número debe estar entre {minimo} y {maximo}.");
            else
                return numero;
        }
    }

    public static DateTime LeerFecha(string mensaje)
    {
        while (true)
        {
            string texto = LeerTexto($"{mensaje} (dd/mm/aaaa, o 'h' para hoy): ").ToLower();

            if (texto == "h" || texto == "hoy")
                return DateTime.Today;

            if (!DateTime.TryParseExact(texto, "d/M/yyyy", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out DateTime fecha))
                MostrarError("Fecha no válida. Usa el formato dd/mm/aaaa (ej. 25/12/2026).");
            else if (fecha > DateTime.Today)
                MostrarError("La fecha no puede ser futura.");
            else
                return fecha;
        }
    }

    public static bool Confirmar(string mensaje)
    {
        while (true)
        {
            string respuesta = LeerTexto($"{mensaje} (s/n): ").ToLower();

            if (respuesta == "s") return true;
            if (respuesta == "n") return false;

            MostrarError("Responde 's' para sí o 'n' para no.");
        }
    }
}

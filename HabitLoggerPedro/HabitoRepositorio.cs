using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitLoggerPedro;

public static class HabitoRepositorio
{
    public static bool Insertar(DateTime fecha, int cantidad)
    {
        int filas = BaseDatos.EjecutarComando(
            "INSERT INTO habitos (Fecha, Cantidad) VALUES (@fecha, @cantidad)",
            ("@fecha", FechaATexto(fecha)),
            ("@cantidad", cantidad));

        return filas > 0;
    }

    public static List<Habito> ObtenerTodos()
    {
        return BaseDatos.EjecutarConsulta(
            "SELECT Id, Fecha, Cantidad FROM habitos ORDER BY Fecha, Id",
            ConvertirAHabito);
    }

    public static Habito? ObtenerPorId(int id)
    {
        var resultado = BaseDatos.EjecutarConsulta(
            "SELECT Id, Fecha, Cantidad FROM habitos WHERE Id = @id",
            ConvertirAHabito,
            ("@id", id));

        return resultado.FirstOrDefault();
    }

    public static bool Actualizar(int id, DateTime fecha, int cantidad)
    {
        int filas = BaseDatos.EjecutarComando(
            "UPDATE habitos SET Fecha = @fecha, Cantidad = @cantidad WHERE Id = @id",
            ("@fecha", FechaATexto(fecha)),
            ("@cantidad", cantidad),
            ("@id", id));

        return filas > 0;
    }

    public static bool Eliminar(int id)
    {
        int filas = BaseDatos.EjecutarComando(
            "DELETE FROM habitos WHERE Id = @id",
            ("@id", id));

        return filas > 0;
    }

    private static string FechaATexto(DateTime fecha)
    {
        return fecha.ToString(Configuracion.FormatoFechaBD, CultureInfo.InvariantCulture);
    }

    private static Habito ConvertirAHabito(SqliteDataReader lector)
    {
        int id = lector.GetInt32(0);
        DateTime fecha = DateTime.ParseExact(lector.GetString(1), Configuracion.FormatoFechaBD, CultureInfo.InvariantCulture);
        int cantidad = lector.GetInt32(2);

        return new Habito(id, fecha, cantidad);
    }
}

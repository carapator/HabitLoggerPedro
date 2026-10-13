namespace HabitLoggerPedro;

public class Habito
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public int Cantidad { get; set; }

    public Habito(int id, DateTime fecha, int cantidad)
    {
        Id = id;
        Fecha = fecha;
        Cantidad = cantidad;
    }

    public override string ToString()
    {
        return Id.ToString().PadRight(5)
             + Fecha.ToString(Configuracion.FormatoFechaPantalla).PadRight(14)
             + Cantidad;
    }
}

namespace HabitLoggerPedro;

public static class Menu
{
    public static void Ejecutar()
    {
        bool salir = false;

        while (!salir)
        {
            // El try/catch va dentro del bucle para volver al menú tras un error
            try
            {
                MostrarOpciones();
                string opcion = Consola.LeerTexto("Elige una opción: ");

                switch (opcion)
                {
                    case "1": VerRegistros(); break;
                    case "2": InsertarRegistro(); break;
                    case "3": ActualizarRegistro(); break;
                    case "4": EliminarRegistro(); break;
                    case "0": salir = true; break;
                    default:
                        Consola.MostrarError("Opción no válida. Elige un número del 0 al 4.");
                        break;
                }
            }
            catch (Exception ex)
            {
                RegistroErrores.Registrar("Menu.Ejecutar", ex);
                Consola.MostrarError("Ha ocurrido un error inesperado. Vuelves al menú.");
            }
        }
    }

    private static void MostrarOpciones()
    {
        Console.WriteLine();
        Console.WriteLine("==============================");
        Console.WriteLine($"  HABIT LOGGER - {Configuracion.NombreHabito}");
        Console.WriteLine("==============================");
        Console.WriteLine("1. Ver registros");
        Console.WriteLine("2. Insertar registro");
        Console.WriteLine("3. Actualizar registro");
        Console.WriteLine("4. Eliminar registro");
        Console.WriteLine("0. Salir");
        Console.WriteLine("------------------------------");
    }

    private static void VerRegistros()
    {
        var registros = HabitoRepositorio.ObtenerTodos();
        MostrarTabla(registros);
        LogUsuario.Registrar("Consulta de registros");
    }

    private static void InsertarRegistro()
    {
        DateTime fecha = Consola.LeerFecha("Fecha del registro");
        int cantidad = PedirCantidad();

        bool correcto = HabitoRepositorio.Insertar(fecha, cantidad);

        MostrarResultado(correcto,
            $"Registro insertado: {FormatearFecha(fecha)} - {cantidad} {Configuracion.Unidad}",
            "No se pudo insertar el registro.");
    }

    private static void ActualizarRegistro()
    {
        Habito? registro = PedirRegistroExistente();
        if (registro == null) return;

        Console.WriteLine($"\nVas a modificar el registro {registro.Id} ({FormatearFecha(registro.Fecha)} - {registro.Cantidad} {Configuracion.Unidad})");
        DateTime nuevaFecha = Consola.LeerFecha("Nueva fecha");
        int nuevaCantidad = PedirCantidad();

        bool correcto = HabitoRepositorio.Actualizar(registro.Id, nuevaFecha, nuevaCantidad);

        MostrarResultado(correcto,
            $"Registro {registro.Id} actualizado: {FormatearFecha(nuevaFecha)} - {nuevaCantidad} {Configuracion.Unidad}",
            "No se pudo actualizar el registro.");
    }

    private static void EliminarRegistro()
    {
        Habito? registro = PedirRegistroExistente();
        if (registro == null) return;

        if (!Consola.Confirmar($"¿Seguro que quieres eliminar el registro {registro.Id}?"))
        {
            Consola.MostrarAviso("Operación cancelada.");
            return;
        }

        bool correcto = HabitoRepositorio.Eliminar(registro.Id);

        MostrarResultado(correcto,
            $"Registro {registro.Id} eliminado ({FormatearFecha(registro.Fecha)} - {registro.Cantidad} {Configuracion.Unidad})",
            "No se pudo eliminar el registro.");
    }

    private static int PedirCantidad()
    {
        return Consola.LeerEntero(
            $"Cantidad de {Configuracion.Unidad} ({Configuracion.CantidadMinima}-{Configuracion.CantidadMaxima}): ",
            Configuracion.CantidadMinima,
            Configuracion.CantidadMaxima);
    }

    private static Habito? PedirRegistroExistente()
    {
        var registros = HabitoRepositorio.ObtenerTodos();

        if (registros.Count == 0)
        {
            Consola.MostrarAviso("No hay registros todavía.");
            return null;
        }

        MostrarTabla(registros);
        int id = Consola.LeerEntero("Id del registro: ", 1, int.MaxValue);

        Habito? registro = HabitoRepositorio.ObtenerPorId(id);
        if (registro == null)
            Consola.MostrarError($"No existe ningún registro con el Id {id}.");

        return registro;
    }

    private static void MostrarTabla(List<Habito> registros)
    {
        if (registros.Count == 0)
        {
            Consola.MostrarAviso("No hay registros todavía.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("ID".PadRight(5) + "FECHA".PadRight(14) + Configuracion.Unidad.ToUpper());
        Console.WriteLine(new string('-', 30));

        foreach (Habito h in registros)
        {
            Console.WriteLine(h);
        }

        Console.WriteLine(new string('-', 30));
        Console.WriteLine($"Total: {registros.Sum(h => h.Cantidad)} {Configuracion.Unidad} en {registros.Count} registros");
    }

    private static void MostrarResultado(bool correcto, string mensajeOk, string mensajeError)
    {
        if (correcto)
        {
            Consola.MostrarExito(mensajeOk);
            LogUsuario.Registrar(mensajeOk);
        }
        else
        {
            Consola.MostrarError(mensajeError);
        }
    }

    private static string FormatearFecha(DateTime fecha)
    {
        return fecha.ToString(Configuracion.FormatoFechaPantalla);
    }
}

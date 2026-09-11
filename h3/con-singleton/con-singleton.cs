public class Cliente
{
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = "";
    public string Documento { get; set; } = "";
    public string Plan { get; set; } = "";
}

public class Cuota
{
    public int IdCuota { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public decimal Monto { get; set; }
    public string Estado { get; set; } = "vigente";

    public void CambiarEstado(string nuevoEstado)
    {
        Estado = nuevoEstado;
    }
}


// ============================================================
// INICIO DEL PATRÓN SINGLETON
// ============================================================

/*
Singleton para la configuración de mora.

El sistema necesita una única configuración
compartida de tasa de mora y días de gracia.
 */
public sealed class ConfiguracionDeMora
{
    private static readonly ConfiguracionDeMora
        _instancia =
        new ConfiguracionDeMora();

    public static ConfiguracionDeMora Instancia =>
        _instancia;

    public decimal TasaDeInteresMoratorio
    {
        get;
        private set;
    } = 0.03m;

    public int DiasDeGracia
    {
        get;
        private set;
    } = 5;

    private ConfiguracionDeMora()
    {
    }

    public void ActualizarReglas(
        decimal nuevaTasa,
        int nuevosDiasDeGracia)
    {
        TasaDeInteresMoratorio =
            nuevaTasa;

        DiasDeGracia =
            nuevosDiasDeGracia;
    }
}

// FIN DEL PATRÓN SINGLETON


public class CalculoDeMora
{
    public int IdCalculo { get; set; }
    public int DiasAtraso { get; set; }

    public decimal CalcularMora(
        decimal montoBase,
        int diasAtraso)
    {
        var configuracion = ConfiguracionDeMora.Instancia;

        int diasCobrables =Math.Max(0,diasAtraso - configuracion.DiasDeGracia);
        return montoBase * configuracion.TasaDeInteresMoratorio * diasCobrables;
    }
}

public class EnlaceDePago
{
    public int IdEnlace { get; set; }
    public string Url { get; set; } = "";
    public decimal MontoTotal { get; set; }
    public DateTime FechaGeneracion { get; set; }

    public string GenerarEnlace(
        string metodoPago)
    {
        IdEnlace =
            new Random().Next(1000, 9999);

        FechaGeneracion =
            DateTime.Now;

        Url = metodoPago switch
        {
            "tarjeta" =>
                $"https://pagos.miempresa.bo/tarjeta/{IdEnlace}",

            "transferencia" =>
                $"https://pagos.miempresa.bo/transferencia/{IdEnlace}",

            "qr" =>
                $"https://pagos.miempresa.bo/qr/{IdEnlace}",

            _ => throw new ArgumentException(
                $"Metodo de pago desconocido: {metodoPago}")
        };

        return Url;
    }
}

public class Pago
{
    public int IdPago { get; set; }
    public DateTime FechaPago { get; set; }
    public decimal MontoPagado { get; set; }
    public string MetodoPago { get; set; } = "";
}

public static class DemoSingleton
{
    public static void Correr()
    {
        var cliente = new Cliente
        {
            IdCliente = 1,
            Nombre = "Noelia Paz",
            Documento = "9871234",
            Plan = "Plan Salud"
        };
        var cuota = new Cuota
        {
            IdCuota = 10,
            FechaVencimiento =
                DateTime.Now.AddDays(-12),
            Monto = 350.00m,
            Estado = "vencida"
        };

        var calculo = new CalculoDeMora
        {
            IdCalculo = 1,
            DiasAtraso = 12
        };

        decimal mora =
            calculo.CalcularMora(
                cuota.Monto,
                calculo.DiasAtraso);

        Console.WriteLine(
            $"[MORA] Cliente {cliente.Nombre} — " +
            $"mora calculada: {mora:0.00} Bs");

        // USO DEL SINGLETON
        var configuracion1 =
            ConfiguracionDeMora.Instancia;
        var configuracion2 =
            ConfiguracionDeMora.Instancia;
        Console.WriteLine(
            $"[CONFIG] Tasa actual: " +
            $"{configuracion1.TasaDeInteresMoratorio:P0}");
        Console.WriteLine(
            $"[CONFIG] Dias de gracia: " +
            $"{configuracion1.DiasDeGracia}");
        Console.WriteLine(
            $"¿Son la MISMA instancia? " +
            $"{ReferenceEquals(
                configuracion1,
                configuracion2)}");
        Console.WriteLine("---");
        // Se modifica la configuración.
        configuracion2.ActualizarReglas(
            0.05m,
            3);
        Console.WriteLine(
            $"[CONFIG] Nueva tasa: " +
            $"{configuracion1.TasaDeInteresMoratorio:P0}");
        Console.WriteLine(
            $"[CONFIG] Nuevos dias de gracia: " +
            $"{configuracion1.DiasDeGracia}");
        // FIN DEL USO DEL SINGLETON
        
        decimal nuevaMora =
            calculo.CalcularMora(
                cuota.Monto,
                calculo.DiasAtraso);

        Console.WriteLine(
            $"[MORA] Nueva mora calculada: " +
            $"{nuevaMora:0.00} Bs");

        var enlace = new EnlaceDePago
        {
            MontoTotal =
                cuota.Monto + nuevaMora
        };

        string url = enlace.GenerarEnlace("tarjeta");

        Console.WriteLine(
            $"[ENLACE] {url} — " +
            $"por {enlace.MontoTotal:0.00} Bs");

        var pago = new Pago
        {
            IdPago = 1,
            FechaPago = DateTime.Now,
            MontoPagado =
                enlace.MontoTotal,
            MetodoPago = "tarjeta"
        };

        cuota.CambiarEstado("pagada");

        Console.WriteLine(
            $"[PAGO] {pago.IdPago} registrado — " +
            $"cuota {cuota.IdCuota} ahora esta " +
            $"{cuota.Estado}");
    }
}
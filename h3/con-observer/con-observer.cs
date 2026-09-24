using System;
using System.Collections.Generic;

public class Cliente
{
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = "";
    public string Documento { get; set; } = "";
    public string Plan { get; set; } = "";
}

// INICIO PATRÓN OBSERVER
public interface IObservadorCuota
{
    void AlCambiarEstado(Cuota cuota, string estadoAnterior, string estadoNuevo);
}
public class Cuota
{
    public int IdCuota { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public decimal Monto { get; set; }
    public string Estado { get; private set; } = "vigente";
    private readonly List<IObservadorCuota> _observadores = new List<IObservadorCuota>();
    public void Suscribir(IObservadorCuota obs)
    {
        _observadores.Add(obs);
    }
    public void CambiarEstado(string nuevoEstado)
    {
        if (nuevoEstado == Estado) return;
        string anterior = Estado;
        Estado = nuevoEstado;
        var copia = new List<IObservadorCuota>(_observadores);
        foreach (var obs in copia)
        {
            try
            {
                obs.AlCambiarEstado(this, anterior, nuevoEstado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR OBSERVER] {ex.Message}");
            }
        }
    }
}
public class NotificadorCliente : IObservadorCuota
{
    public void AlCambiarEstado(Cuota cuota, string estadoAnterior, string estadoNuevo)
    {
        if (estadoNuevo == "en mora")
        {
            Console.WriteLine($"[AVISO] Estimado cliente, su cuota {cuota.IdCuota} entró en mora. " + $"Monto: {cuota.Monto:0.00} Bs");
        }
    }
}
public class AuditoriaCuota : IObservadorCuota
{
    public void AlCambiarEstado(Cuota cuota, string estadoAnterior, string estadoNuevo)
    {
        Console.WriteLine($"[AUDITORIA] Cuota {cuota.IdCuota}: {estadoAnterior} -> {estadoNuevo}");
    }
}
// FIN PATRÓN OBSERVER

public class CalculoDeMora
{
    private const decimal TasaDeInteresMoratorio = 0.03m;
    private const int DiasDeGracia = 5;
    public int IdCalculo { get; set; }
    public int DiasAtraso { get; set; }

    public decimal CalcularMora(decimal montoBase, int diasAtraso)
    {
        int diasCobrables = Math.Max(0, diasAtraso - DiasDeGracia);
        return montoBase * TasaDeInteresMoratorio * diasCobrables;
    }
}
public class EnlaceDePago
{
    public int IdEnlace { get; set; }
    public string Url { get; set; } = "";
    public decimal MontoTotal { get; set; }
    public DateTime FechaGeneracion { get; set; }

    public string GenerarEnlace(string metodoPago)
    {
        IdEnlace = new Random().Next(1000, 9999);
        FechaGeneracion = DateTime.Now;
        if (metodoPago == "tarjeta")
            Url = $"https://pagos.miempresa.bo/tarjeta/{IdEnlace}";
        else if (metodoPago == "transferencia")
            Url = $"https://pagos.miempresa.bo/transferencia/{IdEnlace}";
        else if (metodoPago == "qr")
            Url = $"https://pagos.miempresa.bo/qr/{IdEnlace}";
        else
            throw new ArgumentException($"Metodo de pago desconocido: {metodoPago}");

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

public static class DemoBase
{
    public static void Correr()
    {
        Console.WriteLine("=======Patron OBSERVER==========");
        var cliente = new Cliente { IdCliente = 1, Nombre = "Noelia Paz", Documento = "9871234", Plan = "Plan Salud" };
        var cuota = new Cuota { IdCuota = 10, FechaVencimiento = DateTime.Now.AddDays(-12), Monto = 350.00m };
        cuota.Suscribir(new NotificadorCliente());
        cuota.Suscribir(new AuditoriaCuota());

        var calculo = new CalculoDeMora { IdCalculo = 1, DiasAtraso = 12 };
        decimal mora = calculo.CalcularMora(cuota.Monto, calculo.DiasAtraso);
        Console.WriteLine($"[MORA] Cliente {cliente.Nombre} — cuota {cuota.IdCuota} — mora: {mora:0.00} Bs");
        cuota.CambiarEstado("en mora");

        var enlace = new EnlaceDePago { MontoTotal = cuota.Monto + mora };
        string url = enlace.GenerarEnlace("tarjeta");
        Console.WriteLine($"[ENLACE] {url} — por {enlace.MontoTotal:0.00} Bs");

        var pago = new Pago { IdPago = 1, FechaPago = DateTime.Now, MontoPagado = enlace.MontoTotal, MetodoPago = "tarjeta" };
        cuota.CambiarEstado("pagada");
        Console.WriteLine($"[PAGO] {pago.IdPago} registrado — cuota {cuota.IdCuota} ahora esta {cuota.Estado}");
    }
}
public class Program
{
    public static void Main(string[] args)
    {
        DemoBase.Correr();
        Console.ReadKey();
    }
}
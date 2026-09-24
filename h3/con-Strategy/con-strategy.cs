using System;
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

// INICIO PATRÓN STRATEGY
public interface IReglaMora
{
    decimal Calcular(decimal montoBase, int diasAtraso);
}

public class ReglaMoraNormal : IReglaMora
{
    private const decimal TasaDeInteresMoratorio = 0.03m;
    private const int DiasDeGracia = 5;

    public decimal Calcular(decimal montoBase, int diasAtraso)
    {
        int diasCobrables = Math.Max(0, diasAtraso - DiasDeGracia);
        return montoBase * TasaDeInteresMoratorio * diasCobrables;
    }
}

public class ReglaMoraPreferencial : IReglaMora
{
    private const decimal TasaDeInteresMoratorio = 0.015m;
    private const int DiasDeGracia = 10;

    public decimal Calcular(decimal montoBase, int diasAtraso)
    {
        int diasCobrables = Math.Max(0, diasAtraso - DiasDeGracia);
        return montoBase * TasaDeInteresMoratorio * diasCobrables;
    }
}

public class ReglaMoraCorporativa : IReglaMora
{
    private const decimal TasaDeInteresMoratorio = 0.02m;
    private const int DiasDeGracia = 3;

    public decimal Calcular(decimal montoBase, int diasAtraso)
    {
        int diasCobrables = Math.Max(0, diasAtraso - DiasDeGracia);
        return montoBase * TasaDeInteresMoratorio * diasCobrables;
    }
}

public class CalculoDeMora
{
    public int IdCalculo { get; set; }
    public int DiasAtraso { get; set; }

    private readonly IReglaMora _regla;

    public CalculoDeMora(IReglaMora regla)
    {
        _regla = regla ?? throw new ArgumentNullException(nameof(regla));
    }

    public decimal CalcularMora(decimal montoBase, int diasAtraso)
    {
        return _regla.Calcular(montoBase, diasAtraso);
    }
}
// FIN PATRÓN STRATEGY
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
        Console.WriteLine("==========STRATEGY==========");
        var cliente = new Cliente { IdCliente = 1, Nombre = "Noelia Paz", Documento = "9871234", Plan = "Plan Salud" };
        var cuota = new Cuota { IdCuota = 10, FechaVencimiento = DateTime.Now.AddDays(-12), Monto = 350.00m, Estado = "vencida" };

        IReglaMora regla;
        if (cliente.Plan == "Plan Salud")
            regla = new ReglaMoraPreferencial();
        else if (cliente.Plan == "Plan Corporativo")
            regla = new ReglaMoraCorporativa();
        else
            regla = new ReglaMoraNormal();

        var calculo = new CalculoDeMora(regla) { IdCalculo = 1, DiasAtraso = 12 };
        decimal mora = calculo.CalcularMora(cuota.Monto, calculo.DiasAtraso);
        Console.WriteLine($"[MORA] Cliente {cliente.Nombre} — plan {cliente.Plan} — mora: {mora:0.00} Bs");

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
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
        Url = metodoPago switch
        {
            "tarjeta" => $"https://pagos.miempresa.bo/tarjeta/{IdEnlace}",
            "transferencia" => $"https://pagos.miempresa.bo/transferencia/{IdEnlace}",
            "qr" => $"https://pagos.miempresa.bo/qr/{IdEnlace}",
            _ => throw new ArgumentException($"Metodo de pago desconocido: {metodoPago}")
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

public static class DemoBase
{
    public static void Correr()
    {
        var cliente = new Cliente { IdCliente = 1, Nombre = "Noelia Paz", Documento = "9871234", Plan = "Plan Salud" };
        var cuota = new Cuota { IdCuota = 10, FechaVencimiento = DateTime.Now.AddDays(-12), Monto = 350.00m, Estado = "vencida" };

        var calculo = new CalculoDeMora { IdCalculo = 1, DiasAtraso = 12 };
        decimal mora = calculo.CalcularMora(cuota.Monto, calculo.DiasAtraso);
        Console.WriteLine($"[MORA] Cliente {cliente.Nombre} — cuota {cuota.IdCuota} — mora calculada: {mora:0.00} Bs");

        var enlace = new EnlaceDePago { MontoTotal = cuota.Monto + mora };
        string url = enlace.GenerarEnlace("tarjeta");
        Console.WriteLine($"[ENLACE] {url} — por {enlace.MontoTotal:0.00} Bs");

        var pago = new Pago { IdPago = 1, FechaPago = DateTime.Now, MontoPagado = enlace.MontoTotal, MetodoPago = "tarjeta" };
        cuota.CambiarEstado("pagada");
        Console.WriteLine($"[PAGO] {pago.IdPago} registrado — cuota {cuota.IdCuota} ahora esta {cuota.Estado}");
    }
}
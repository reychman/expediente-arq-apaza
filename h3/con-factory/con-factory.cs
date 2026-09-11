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
// aplicando factory
public abstract class EnlaceDePago
{
    public int IdEnlace { get; protected set; }
    public string Url { get; protected set; } = "";
    public decimal MontoTotal { get; protected set; }
    public DateTime FechaGeneracion { get; protected set; }

    protected EnlaceDePago(decimal montoTotal)
    {
        IdEnlace = Random.Shared.Next(1000, 9999);
        MontoTotal = montoTotal;
        FechaGeneracion = DateTime.Now;
    }

    public abstract string GenerarEnlace();

    public virtual bool ValidarEnlace()
    {
        return !string.IsNullOrWhiteSpace(Url);
    }
}

public class EnlaceTarjeta : EnlaceDePago
{
    public EnlaceTarjeta(decimal montoTotal)
        : base(montoTotal)
    {
    }

    public override string GenerarEnlace()
    {
        Url =
            $"https://pagos.miempresa.bo/tarjeta/{IdEnlace}";

        return Url;
    }
}

public class EnlaceTransferencia : EnlaceDePago
{
    public EnlaceTransferencia(decimal montoTotal)
        : base(montoTotal)
    {
    }

    public override string GenerarEnlace()
    {
        Url =
            $"https://pagos.miempresa.bo/transferencia/{IdEnlace}";

        return Url;
    }
}

public class EnlaceQR : EnlaceDePago
{
    public EnlaceQR(decimal montoTotal)
        : base(montoTotal)
    {
    }

    public override string GenerarEnlace()
    {
        Url =
            $"https://pagos.miempresa.bo/qr/{IdEnlace}";

        return Url;
    }
}
//fin
public class Pago
{
    public int IdPago { get; set; }
    public DateTime FechaPago { get; set; }
    public decimal MontoPagado { get; set; }
    public string MetodoPago { get; set; } = "";
}

public abstract class FabricaEnlaceDePago
{
    public abstract EnlaceDePago CrearEnlace(decimal monto);
}

public class FabricaTarjeta : FabricaEnlaceDePago
{
    public override EnlaceDePago CrearEnlace(decimal monto)
    {
        return new EnlaceTarjeta(monto);
    }
}

public class FabricaTransferencia : FabricaEnlaceDePago
{
    public override EnlaceDePago CrearEnlace(decimal monto)
    {
        return new EnlaceTransferencia(monto);
    }
}

public class FabricaQR : FabricaEnlaceDePago
{
    public override EnlaceDePago CrearEnlace(decimal monto)
    {
        return new EnlaceQR(monto);
    }
}

public static class DemoFactory
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
            FechaVencimiento = DateTime.Now.AddDays(-12),
            Monto = 350.00m,
            Estado = "vencida"
        };

        var calculo = new CalculoDeMora
        {
            IdCalculo = 1,
            DiasAtraso = 12
        };

        decimal mora = calculo.CalcularMora(
            cuota.Monto,
            calculo.DiasAtraso
        );

        Console.WriteLine(
            $"[MORA] Cliente {cliente.Nombre} — " +
            $"cuota {cuota.IdCuota} — " +
            $"mora calculada: {mora:0.00} Bs"
        );

        decimal montoTotal = cuota.Monto + mora;

        FabricaEnlaceDePago fabrica =
            new FabricaTarjeta();

        EnlaceDePago enlace =
            fabrica.CrearEnlace(montoTotal);

        string url = enlace.GenerarEnlace();

        Console.WriteLine(
            $"[ENLACE] {url} — " +
            $"por {enlace.MontoTotal:0.00} Bs"
        );

        var pago = new Pago
        {
            IdPago = 1,
            FechaPago = DateTime.Now,
            MontoPagado = enlace.MontoTotal,
            MetodoPago = "tarjeta"
        };

        cuota.CambiarEstado("pagada");

        Console.WriteLine(
            $"[PAGO] {pago.IdPago} registrado — " +
            $"cuota {cuota.IdCuota} ahora esta {cuota.Estado}"
        );
    }
}
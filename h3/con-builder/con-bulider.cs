public class Cliente
{
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = "";
    public string Documento { get; set; } = "";
    public string Plan { get; set; } = "";

    public int CantidadDeCuotas { get; set; }
    public decimal MontoPorCuota { get; set; }
    public int DiaDePago { get; set; }

    public bool TieneDescuento { get; set; }
    public decimal PorcentajeDescuento { get; set; }
}

public class ClienteBuilder
{
    private readonly Cliente _cliente = new();

    public ClienteBuilder ConId(int idCliente)
    {
        _cliente.IdCliente = idCliente;
        return this;
    }

    public ClienteBuilder ConNombre(string nombre)
    {
        _cliente.Nombre = nombre;
        return this;
    }

    public ClienteBuilder ConDocumento(string documento)
    {
        _cliente.Documento = documento;
        return this;
    }

    public ClienteBuilder ConPlan(string plan)
    {
        _cliente.Plan = plan;
        return this;
    }

    public ClienteBuilder ConCantidadDeCuotas(
        int cantidadDeCuotas)
    {
        _cliente.CantidadDeCuotas = cantidadDeCuotas;
        return this;
    }

    public ClienteBuilder ConMontoPorCuota(
        decimal montoPorCuota)
    {
        _cliente.MontoPorCuota = montoPorCuota;
        return this;
    }

    public ClienteBuilder ConDiaDePago(int diaDePago)
    {
        _cliente.DiaDePago = diaDePago;
        return this;
    }

    public ClienteBuilder ConDescuento(
        decimal porcentajeDescuento)
    {
        _cliente.TieneDescuento = true;
        _cliente.PorcentajeDescuento =
            porcentajeDescuento;

        return this;
    }

    public Cliente Construir()
    {
        if (string.IsNullOrWhiteSpace(_cliente.Nombre))
        {
            throw new InvalidOperationException(
                "El cliente debe tener nombre."
            );
        }

        if (string.IsNullOrWhiteSpace(_cliente.Documento))
        {
            throw new InvalidOperationException(
                "El cliente debe tener documento."
            );
        }

        if (string.IsNullOrWhiteSpace(_cliente.Plan))
        {
            throw new InvalidOperationException(
                "El cliente debe tener un plan."
            );
        }

        if (_cliente.CantidadDeCuotas <= 0)
        {
            throw new InvalidOperationException(
                "La cantidad de cuotas debe ser mayor a cero."
            );
        }

        return _cliente;
    }
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

    public decimal CalcularMora(
        decimal montoBase,
        int diasAtraso)
    {
        int diasCobrables =
            Math.Max(0, diasAtraso - DiasDeGracia);

        return montoBase *
               TasaDeInteresMoratorio *
               diasCobrables;
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
        IdEnlace = Random.Shared.Next(1000, 9999);
        FechaGeneracion = DateTime.Now;

        Url = metodoPago switch
        {
            "tarjeta" =>
                $"https://pagos.miempresa.bo/tarjeta/{IdEnlace}",

            "transferencia" =>
                $"https://pagos.miempresa.bo/transferencia/{IdEnlace}",

            "qr" =>
                $"https://pagos.miempresa.bo/qr/{IdEnlace}",

            _ => throw new ArgumentException(
                $"Metodo de pago desconocido: {metodoPago}"
            )
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

public static class DemoBuilder
{
    public static void Correr()
    {
        var cliente = new ClienteBuilder()
            .ConId(1)
            .ConNombre("Noelia Paz")
            .ConDocumento("9871234")
            .ConPlan("Plan Salud")
            .ConCantidadDeCuotas(12)
            .ConMontoPorCuota(350.00m)
            .ConDiaDePago(5)
            .ConDescuento(10)
            .Construir();

        var cuota = new Cuota
        {
            IdCuota = 10,
            FechaVencimiento =
                DateTime.Now.AddDays(-12),
            Monto = cliente.MontoPorCuota,
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
            $"[CLIENTE] {cliente.Nombre} — " +
            $"documento {cliente.Documento}"
        );

        Console.WriteLine(
            $"[PLAN] {cliente.Plan} — " +
            $"{cliente.CantidadDeCuotas} cuotas de " +
            $"{cliente.MontoPorCuota:0.00} Bs"
        );

        Console.WriteLine(
            $"[MORA] Mora calculada: " +
            $"{mora:0.00} Bs"
        );

        var enlace = new EnlaceDePago
        {
            MontoTotal = cuota.Monto + mora
        };

        string url =
            enlace.GenerarEnlace("tarjeta");

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
            $"cuota {cuota.IdCuota} ahora esta " +
            $"{cuota.Estado}"
        );
    }
}
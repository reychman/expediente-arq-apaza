public interface IAnulable
{
    bool Anular();
}

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
    public virtual bool ValidarEnlace() => !string.IsNullOrWhiteSpace(Url);
}

public class EnlaceTarjeta : EnlaceDePago, IAnulable
{
    public EnlaceTarjeta(decimal montoTotal) : base(montoTotal) { }

    public override string GenerarEnlace()
    {
        Url = $"https://pagos.miempresa.bo/tarjeta/{IdEnlace}";
        return Url;
    }

    public bool Anular()
    {
        Console.WriteLine($"[TARJETA] Enlace {IdEnlace} anulado.");
        return true;
    }
}

public class EnlaceTransferencia : EnlaceDePago, IAnulable
{
    public EnlaceTransferencia(decimal montoTotal) : base(montoTotal) { }

    public override string GenerarEnlace()
    {
        Url = $"https://pagos.miempresa.bo/transferencia/{IdEnlace}";
        return Url;
    }

    public bool Anular()
    {
        Console.WriteLine($"[TRANSFERENCIA] Enlace {IdEnlace} anulado.");
        return true;
    }
}

public class EnlaceQR : EnlaceDePago
{
    public EnlaceQR(decimal montoTotal) : base(montoTotal) { }

    public override string GenerarEnlace()
    {
        Url = $"https://pagos.miempresa.bo/qr/{IdEnlace}";
        return Url;
    }
}

public static class FabricaDeEnlaces
{
    public static EnlaceDePago Crear(string metodoPago, decimal monto) => metodoPago switch
    {
        "tarjeta"       => new EnlaceTarjeta(monto),
        "transferencia" => new EnlaceTransferencia(monto),
        "qr"            => new EnlaceQR(monto),              
        _ => throw new ArgumentException($"Metodo de pago desconocido: {metodoPago}")
    };
}
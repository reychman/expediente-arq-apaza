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

public static class DemoFabricaDeEnlaces
{
    public static void Correr()
    {
        var enlace1 = FabricaDeEnlaces.Crear("tarjeta", 350.00m);
        Console.WriteLine($"[ENLACE] {enlace1.GetType().Name} generado: {enlace1.GenerarEnlace()} - {enlace1.MontoTotal:0.00} Bs");
        var enlace2 = FabricaDeEnlaces.Crear("qr", 120.00m);
        Console.WriteLine($"[ENLACE] {enlace2.GetType().Name} generado: {enlace2.GenerarEnlace()} - {enlace2.MontoTotal:0.00} Bs");
        var enlace3 = FabricaDeEnlaces.Crear("transferencia", 500.00m);
        Console.WriteLine($"[ENLACE] {enlace3.GetType().Name} generado: {enlace3.GenerarEnlace()} - {enlace3.MontoTotal:0.00} Bs");
        Console.WriteLine("---");
        if (enlace1 is IAnulable anulableTarjeta)
            anulableTarjeta.Anular();
        if (enlace2 is IAnulable anulableQr)
            anulableQr.Anular();
        else
            Console.WriteLine("[QR] Este enlace no se puede anular (no implementa IAnulable).");
        Console.WriteLine("---");
        try
        {
            FabricaDeEnlaces.Crear("bitcoin", 10.00m);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"[ERROR] {ex.Message}");
        }
        Console.WriteLine("El metodo nuevo entro tocando UN solo lugar. El Operador ni se entero.");
    }
}

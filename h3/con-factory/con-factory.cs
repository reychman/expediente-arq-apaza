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
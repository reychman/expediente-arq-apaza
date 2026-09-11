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
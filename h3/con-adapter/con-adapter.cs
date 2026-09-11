public interface IPasarelaDePago
{
    string GenerarLink(decimal monto);
    bool ConfirmarPago(string idEnlace);
}

public class PasarelaBanco
{
    public string CrearCobro(int montoEnCentavos)
        => $"TXN-{montoEnCentavos}-{Guid.NewGuid().ToString()[..6]}";
    public string ConsultarEstado(string codigoDeTransaccion)
    {
        if (codigoDeTransaccion.Contains("999")) return "RECHAZADO";
        if (codigoDeTransaccion.Contains("ERR")) return "EN_REVISION";
        return "APROBADO";
    }
}

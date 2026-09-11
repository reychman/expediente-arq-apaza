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

public class AdaptadorPasarelaBanco : IPasarelaDePago
{
    private readonly PasarelaBanco _banco = new();
    public string GenerarLink(decimal monto)
    {
        int montoEnCentavos = (int)(monto * 100);
        string codigoDeTransaccion = _banco.CrearCobro(montoEnCentavos);
        return codigoDeTransaccion;
    }

    public bool ConfirmarPago(string idEnlace)
    {
        string estado = _banco.ConsultarEstado(idEnlace);
        if (estado != "APROBADO" && estado != "RECHAZADO")
            throw new InvalidOperationException($"El banco devolvio un estado desconocido: {estado}");
        return estado == "APROBADO";
    }
}

public class GeneradorDeEnlaces
{
    private readonly IPasarelaDePago _pasarela;

    public GeneradorDeEnlaces(IPasarelaDePago pasarela)
        => _pasarela = pasarela;

    public void EmitirEnlace(decimal monto)
    {
        string enlace = _pasarela.GenerarLink(monto);
        Console.WriteLine($"[ENLACE] Generado: {enlace} — por {monto:0.00} Bs");
    }

    public void VerificarPago(string idEnlace)
    {
        bool aprobado = _pasarela.ConfirmarPago(idEnlace);
        Console.WriteLine(aprobado
            ? $"[PAGO] {idEnlace} — APROBADO"
            : $"[PAGO] {idEnlace} — RECHAZADO");
    }
}


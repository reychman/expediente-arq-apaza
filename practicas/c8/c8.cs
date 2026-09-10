public class GeneradorDeEnlacesPago
{
    private static GeneradorDeEnlacesPago? _instancia;

    private int _ultimoIdTransaccion = 0;

    public static GeneradorDeEnlacesPago Instancia
    {
        get
        {
            if (_instancia == null) _instancia = new GeneradorDeEnlacesPago();
            return _instancia;                      // el mismo emisor para TODO el sistema
        }
    }

    private GeneradorDeEnlacesPago() { }             // el CANDADO: nadie mas hace new

    public (int idTransaccion, string claveDeSeguridad) EmitirDatosDeEnlace(double montoTotal, int idCliente)
    {
        _ultimoIdTransaccion++;
        string claveDeSeguridad = GenerarClaveDeSeguridad(_ultimoIdTransaccion, montoTotal, idCliente);
        return (_ultimoIdTransaccion, claveDeSeguridad);
    }

    private string GenerarClaveDeSeguridad(int idTransaccion, double montoTotal, int idCliente)
    {
        // combina id + monto + cliente para que la clave quede atada a ESOS datos exactos
        string base_ = $"{idTransaccion}-{montoTotal:0.00}-{idCliente}";
        return Convert.ToHexString(
            System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(base_)));
    }
}

public static class DemoGeneradorDeEnlacesPago
{
    public static void Correr()
    {
        var generadorDesdeOperadorA = GeneradorDeEnlacesPago.Instancia;
        var generadorDesdeOperadorB = GeneradorDeEnlacesPago.Instancia;

        var enlaceA = generadorDesdeOperadorA.EmitirDatosDeEnlace(150.00, idCliente: 12);
        var enlaceB = generadorDesdeOperadorB.EmitirDatosDeEnlace(300.00, idCliente: 45);

        Console.WriteLine($"Enlace A -> id {enlaceA.idTransaccion}, clave {enlaceA.claveDeSeguridad[..12]}...");
        Console.WriteLine($"Enlace B -> id {enlaceB.idTransaccion}, clave {enlaceB.claveDeSeguridad[..12]}...");
        Console.WriteLine($"Mismo emisor: {ReferenceEquals(generadorDesdeOperadorA, generadorDesdeOperadorB)}");
    }
}
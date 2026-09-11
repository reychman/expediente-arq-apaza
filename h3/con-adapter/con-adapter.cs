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

    public decimal CalcularMora(
        decimal montoBase,
        int diasAtraso)
    {
        int diasCobrables =
            Math.Max(
                0,
                diasAtraso - DiasDeGracia);

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

    public string GenerarEnlace(
        string metodoPago)
    {
        IdEnlace =
            new Random().Next(1000, 9999);

        FechaGeneracion =
            DateTime.Now;

        Url = metodoPago switch
        {
            "tarjeta" =>
                $"https://pagos.miempresa.bo/tarjeta/{IdEnlace}",

            "transferencia" =>
                $"https://pagos.miempresa.bo/transferencia/{IdEnlace}",

            "qr" =>
                $"https://pagos.miempresa.bo/qr/{IdEnlace}",

            _ => throw new ArgumentException(
                $"Metodo de pago desconocido: {metodoPago}")
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


// INICIO DEL PATRÓN ADAPTER

//Interfaz que nuestro sistema espera utilizar.

public interface IPasarelaDePago
{
    string GenerarLink(decimal monto);

    bool ConfirmarPago(string idEnlace);
}


/*
Servicio externo.
Esta clase representa una pasarela bancaria
que tiene una interfaz diferente.
 */
public class PasarelaBanco
{
    public string CrearCobro(
        int montoEnCentavos)
    {
        return
            $"TXN-{montoEnCentavos}-" +
            $"{Guid.NewGuid().ToString()[..6]}";
    }

    public string ConsultarEstado(
        string codigoDeTransaccion)
    {
        if (codigoDeTransaccion.Contains("999"))
        {
            return "RECHAZADO";
        }

        if (codigoDeTransaccion.Contains("ERR"))
        {
            return "EN_REVISION";
        }

        return "APROBADO";
    }
}


/*
Adapter.
Convierte la interfaz de PasarelaBanco 
en la interfaz IPasarelaDePago que necesita nuestro sistema.
 */
public class AdaptadorPasarelaBanco :
    IPasarelaDePago
{
    private readonly PasarelaBanco _banco =
        new();

    public string GenerarLink(
        decimal monto)
    {
        int montoEnCentavos =
            (int)(monto * 100);

        string codigoDeTransaccion =
            _banco.CrearCobro(
                montoEnCentavos);

        return codigoDeTransaccion;
    }

    public bool ConfirmarPago(
        string idEnlace)
    {
        string estado =
            _banco.ConsultarEstado(
                idEnlace);

        if (estado != "APROBADO" &&
            estado != "RECHAZADO")
        {
            throw new InvalidOperationException(
                $"El banco devolvio un estado " +
                $"desconocido: {estado}");
        }

        return estado == "APROBADO";
    }
}


/*
Clase que pertenece a nuestro sistema.
Trabaja solamente con IPasarelaDePago.
No necesita conocer cómo funciona el banco.
 */
public class GeneradorDeEnlaces
{
    private readonly IPasarelaDePago _pasarela;

    public GeneradorDeEnlaces(
        IPasarelaDePago pasarela)
    {
        _pasarela = pasarela;
    }

    public void EmitirEnlace(
        decimal monto)
    {
        string enlace =
            _pasarela.GenerarLink(monto);

        Console.WriteLine(
            $"[ENLACE] Generado: {enlace} — " +
            $"por {monto:0.00} Bs");
    }

    public void VerificarPago(
        string idEnlace)
    {
        bool aprobado =
            _pasarela.ConfirmarPago(
                idEnlace);

        Console.WriteLine(
            aprobado
                ? $"[PAGO] {idEnlace} — APROBADO"
                : $"[PAGO] {idEnlace} — RECHAZADO");
    }
}


// FIN DEL PATRÓN ADAPTER


public static class DemoAdapter
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
            FechaVencimiento =
                DateTime.Now.AddDays(-12),
            Monto = 350.00m,
            Estado = "vencida"
        };

        var calculo = new CalculoDeMora
        {
            IdCalculo = 1,
            DiasAtraso = 12
        };
        decimal mora =
            calculo.CalcularMora(
                cuota.Monto,
                calculo.DiasAtraso);
        decimal montoTotal =
            cuota.Monto + mora;
        Console.WriteLine(
            $"[MORA] Cliente {cliente.Nombre} — " +
            $"mora: {mora:0.00} Bs");
        Console.WriteLine(
            $"[CUOTA] Cuota {cuota.IdCuota} — " +
            $"monto total: {montoTotal:0.00} Bs");
        Console.WriteLine("---");

        /*
            Aquí utilizamos el Adapter.
            El sistema trabaja con IPasarelaDePago,
            mientras que el banco utiliza PasarelaBanco.
        */
        IPasarelaDePago pasarela =
            new AdaptadorPasarelaBanco();

        var generador =
            new GeneradorDeEnlaces(pasarela);
        generador.EmitirEnlace(montoTotal);
        Console.WriteLine("---");
        generador.VerificarPago("TXN-35000-a1b2c3");
        Console.WriteLine("---");
        generador.VerificarPago("TXN-99900-999xyz");
        Console.WriteLine("---");
        generador.VerificarPago("TXN-30000-ERRabc");
        cuota.CambiarEstado("pagada");

        var pago = new Pago
        {
            IdPago = 1,
            FechaPago = DateTime.Now,
            MontoPagado = montoTotal,
            MetodoPago = "tarjeta"
        };

        Console.WriteLine(
            $"[PAGO] {pago.IdPago} registrado — " +
            $"cuota {cuota.IdCuota} ahora esta " +
            $"{cuota.Estado}");
    }
}
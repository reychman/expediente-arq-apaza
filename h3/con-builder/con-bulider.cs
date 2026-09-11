public class Cliente
{
    public string Nombre { get; set; } = "";
    public string Documento { get; set; } = "";
    public string Plan { get; set; } = "";
    public int CantidadDeCuotas { get; set; }
    public decimal MontoPorCuota { get; set; }
    public int DiaDePago { get; set; }
    public bool TieneDescuento { get; set; }
    public decimal PorcentajeDescuento { get; set; }
}

public class ArmadorDeCliente
{
    private readonly Cliente _cliente = new();

    public ArmadorDeCliente ConNombre(string nombre)
    {
        _cliente.Nombre = nombre;
        return this;
    }

    public ArmadorDeCliente ConDocumento(string documento)
    {
        _cliente.Documento = documento;
        return this;
    }

    public ArmadorDeCliente ConPlan(string plan)
    {
        _cliente.Plan = plan;
        return this;
    }

    public ArmadorDeCliente ConCantidadDeCuotas(int cantidadDeCuotas)
    {
        _cliente.CantidadDeCuotas = cantidadDeCuotas;
        return this;
    }

    public ArmadorDeCliente ConMontoPorCuota(decimal montoPorCuota)
    {
        _cliente.MontoPorCuota = montoPorCuota;
        return this;
    }

    public ArmadorDeCliente ConDiaDePago(int diaDePago)
    {
        _cliente.DiaDePago = diaDePago;
        return this;
    }

    public ArmadorDeCliente ConDescuento(decimal porcentaje)
    {
        _cliente.TieneDescuento = true;
        _cliente.PorcentajeDescuento = porcentaje;
        return this;
    }

    public Cliente Registrar()
    {
        // GUARDIAN 1: sin nombre o documento no hay cliente identificable
        if (string.IsNullOrWhiteSpace(_cliente.Nombre) || string.IsNullOrWhiteSpace(_cliente.Documento))
            throw new InvalidOperationException("Falta nombre o documento: sin eso no hay cliente.");

        // GUARDIAN 2: un plan sin cuotas no es un plan
        if (_cliente.CantidadDeCuotas <= 0)
            throw new InvalidOperationException("Falta la cantidad de cuotas: sin cuotas no hay plan.");

        return _cliente;
    }
}

public static class DemoArmadorDeCliente
{
    public static void Correr()
    {
        var cliente = new ArmadorDeCliente()
            .ConNombre("Noelia Paz")
            .ConDocumento("9871234")
            .ConPlan("Plan Salud")
            .ConCantidadDeCuotas(12)
            .ConMontoPorCuota(350.00m)
            .ConDiaDePago(5)
            .Registrar();

        Console.WriteLine($"[REGISTRO] {cliente.Nombre} — plan {cliente.Plan}, {cliente.CantidadDeCuotas} cuotas de {cliente.MontoPorCuota:0.00} Bs, paga el dia {cliente.DiaDePago}");
    }
}
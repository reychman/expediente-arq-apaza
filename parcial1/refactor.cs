// refactor Apaza Carballo Reychman Cristopher
namespace Parcial1.Farmacia

public interface IEmpleadoDeFarmacia
{
    void RegistrarPedido(string medicamento, int cantidad);
}

public interface IAutorizadorDeControlados
{
    void AutorizarVentaControlada(String medicamento);
    void VerLibrosDeControlados();
}

public interface IAjustadorDePrecios{
    void AjustarPrecio(string medicamento, decimal nuevoPrecio);
}

public class Farmaceutico : IempleadoDeFarmacia, IAutorizadorDeControlados, IAjustadorDePrecios
{
    public void RegistrarPedido(string medicamento, int cantidad)
    => Console.WriteLine($"[FARM] Pedido: {cantidad} x {medicamento}");
    public void AutorizarVentaControlada(string medicamento)
    => Console.WriteLine($"[FARM] Venta controlada de {medicamento} autorizada");
    public void AjustarPrecio(string medicamento, decimal nuevoPrecio)
    => Console.WriteLine($"[FARM] {medicamento} ahora cuesta {nuevoPrecio:0.00} Bs");
    public void VerLibroDeControlados()
    => Console.WriteLine("[FARM] Libro de medicamentos controlados");
}
public class Cajero : IEmpleadoDeFarmacia
{
    public void RegistrarPedido(string medicamento, int cantidad)
    => Console.WriteLine($"[CAJA] Pedido: {cantidad} x {medicamento}");
}
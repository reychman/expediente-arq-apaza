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

// CURA 2 INVERSION DE DEPENDENCIAS
public interface IRepositorioDePedidos
{
    void GuardarPedido(string Cliente, string medicamento, int cantidad, decimal total);
}
public interface INotificadorDeCliente
{
    void Enviar(string mensaje);
}
public class BaseDeDatosMysql : IRepositorioDePedidos
{
    public void GuardarPedido(string Cliente, string medicamento, int cantidad, decimal total)
    => Console.WriteLine($"[MYSQL] INSERT INTO pedidos VALUES ('{cliente}', '{medicamento}', {cantidad}, {total})");
}
public class CorreoSmtp : INotificadorDeCliente
{
    public void Enviar(string mensaje)
    => Console.WriteLine($"[SMTP] Enviando correo: {mensaje}");
}
public class GestorDePedidos
{
    private readonly IRepositorioDePedidos _repositorio;
    private readonly INotificadorDeCliente _notificador;

    public GestorDePedidos(IRepositorioDePedidos repositorio, INotificadorDeCliente notificador)
    {
        _repositorio = repositorio;
        _notificador = notificador;
    }

    public void ProcesarPedido(string cliente, string tipoCliente, string medicamento, int cantidad, decimal precioUnitario)
    {
        decimal total = cantidad * precioUnitario;

        decimal descuento;
        switch (tipoCliente)
        {
            case "particular":
                descuento = 0;
                break;
            case "asegurado":
                descuento = total * 0.20m;
                break;
            case "convenio":
                descuento = total * 0.10m;
                break;
            default:
                descuento = 0;
                break;
        }
        decimal totalFinal = total - descuento;

        _repositorio.GuardarPedido(cliente, medicamento, cantidad, totalFinal);

        Console.WriteLine("----- COMPROBANTE -----");
        Console.WriteLine($"{cantidad} x {medicamento}");
        Console.WriteLine($"Cliente: {cliente} ({tipoCliente})");
        Console.WriteLine($"TOTAL: {totalFinal:0.00} Bs");

        _notificador.Enviar($"Su pedido de {medicamento} fue registrado, {cliente}");
    }
}

public static class Demo
{
    public static void Correr()
    {
        var gestor = new GestorDePedidos(new BaseDeDatosMySql(), new CorreoSmtp());
        gestor.ProcesarPedido("Noelia", "asegurado", "Paracetamol 500mg", 2, 8.50m);
    }
}
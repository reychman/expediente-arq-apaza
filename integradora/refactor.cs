//Apaza Carballo Reychman Cristopher
//DIP principio de inversion de dependencias

public interface IRepositorioPedidos
{
    void GuardarPedido(string estudiante, string menu, int cantidad, decimal total);
}
public interface INotificadorPedido
{
    void Enviar(string mensaje);
}
public class GestorDePedidos
{
    private readonly IRepositorioPedidos _repositorio;
    private readonly INotificadorPedido _notificador;
    public GestorDePedidos(IRepositorioPedidos repositorio, INotificadorPedido notificador)
    {
        _repositorio = repositorio;
        _notificador = notificador;
    }
    public void ProcesarPedido(string estudiante, string tipoMenu, int cantidad)
    {
        decimal precioBase;
        switch (tipoMenu)
        {
            case "estandar":
                precioBase = 12;
                break;
            case "vegetariano":
                precioBase = 14;
                break;
            case "beca":
                precioBase = 5;
                break;
            default:
                precioBase = 12;
                break;
        }
        decimal total = precioBase * cantidad;
        _repositorio.GuardarPedido(estudiante, tipoMenu, cantidad, total);
        Console.WriteLine("----- VALE DE COMEDOR -----");
        Console.WriteLine($"{estudiante}: {cantidad} x menú {tipoMenu}");
        Console.WriteLine($"TOTAL: {total:0.00} Bs");
        _notificador.Enviar($"Pedido registrado: {cantidad} x {tipoMenu}, {estudiante}");
    }
}
public class BaseDeDatosComedor : IRepositorioPedidos
{
    public void GuardarPedido(string estudiante, string menu, int cantidad, decimal total)
        => Console.WriteLine($"[BD] INSERT INTO pedidos VALUES ('{estudiante}', '{menu}', {cantidad}, {total})");
}
public class CorreoUniversitario : INotificadorPedido
{
    public void Enviar(string mensaje)
        => Console.WriteLine($"[CORREO] {mensaje}");
}
public static class Demo
{
    public static void Correr()
    {
        var gestor = new GestorDePedidos(new BaseDeDatosComedor(), new CorreoUniversitario());
        gestor.ProcesarPedido("Noelia", "vegetariano", 2);
    }
}
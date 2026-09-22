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
    public GestorDePedidos(
        IRepositorioPedidos repositorio,
        INotificadorPedido notificador)
    {
        _repositorio = repositorio;
        _notificador = notificador;
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
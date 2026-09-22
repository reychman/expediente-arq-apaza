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
public class BaseDeDatosComedor : IRepositorioPedidos
{
    public void GuardarPedido(string estudiante, string menu, int cantidad, decimal total)
        => Console.WriteLine($"[BD] INSERT INTO pedidos VALUES ('{estudiante}', '{menu}', {cantidad}, {total})");
}

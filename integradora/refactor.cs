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


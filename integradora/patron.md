cuando un pedido queda preparado, el estudiante debe recibir un aviso
esto es cuando algo cambia, avisale a los interesados sin que el que cambia tenga que saber a quien ni como -> patron Observer

Pedido es el sujeto, porque guarda una lista de observaciones y, cuando CambiarEstado lo pasa a PREPARADO, los recorre y les avisa. los observadores concretos son quienes saben como avisar, ya sea correo notificacion, o lo que se  quiera agregar en un futuro

public interface IObservadorPedido
{
    void Actualizar(Pedido pedido);
}
public class AvisoCorreoEstudiante : IObservadorPedido
{
    public void Actualizar(Pedido pedido)
        => Console.WriteLine($"[CORREO] Tu pedido #{pedido.Id} está listo, {pedido.Estudiante.Nombre}");
}
public class AvisoAppEstudiante : IObservadorPedido
{
    public void Actualizar(Pedido pedido)
        => Console.WriteLine($"[APP] Notificación push: pedido #{pedido.Id} preparado");
}
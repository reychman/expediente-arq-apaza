digrama de clases
```mermaid
classDiagram
    direction LR
    class TipoMenu{
        <<enumeration>>
        ESTANDAR
        VEGETARIANO
        BECA
    }
    class EstadoPedido{
        <<enumeration>>
        SOLICITADO
        PREPARADO
        ENTREGADO
        ANULADO
    }
    class Estudiante{
        -string codigo
        -string nombre
    }
    class Cajero{
        -string nombre
        +registrarPedido(estudiante, tipoMenu, cantidad)
    }
    class Administrador{
        -string nombre
        +ajustarPrecio(tipoMenu, nuevoPrecio)
        +anularPedido(pedido)
    }
    class GestorDePedidos{
        -IRepositorioPedidos repositorio
        -INotificadorPedido notificador
        +procesarPedido(Estudiante, tipoMenu, cantidad) pedido
        +anularPedido(pedido)
        +ajustarPrecio(tipoMenu, nuevoPrecio)
    }


    note "Apaza Carballo Reychman Cristopher"
    Cajero --> GestorDePedidos : usa
    Administrador --> GestorDePedidos : usa
    
```mermaid
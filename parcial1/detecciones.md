las 4 violaciones que alcanzo a reconocer serian las: S, O, I, D

S(Pirncipio de responsabilidad)
    IEmpleadoDeFarmacia y clase Cajero
        La interfaz obliga a todos los empleados a tener AutorizarVentaControlada, AjustarPrecio y VerLibroDeControlados
        pero el Cajero no puede hacer ninguna de las tres y solo lanzaria excepciones  
O(ABIERTO/CERRADO)
    en GestorDePedidos().ProcesarPedido
        Estos metodo calcula el descuento, tambien guarda en la BDD, imprime el comprobante y envia el correo.
        Son 4 Trabajos distintos que cumple una sola funcion.
I(segregacion de intefaces)
D(inversion de dependencias)
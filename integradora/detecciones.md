primera deteccion el principio de responsabilidad unica, presente en el metodo ProcesarPedido
    porque en un solo metodo se calcula el precio, se guarda en la base de datos y tambien se imprime el vaucher o recibo por consola, y luego tambien manda un correo. 
    son 4 razones distintas para que este metodo cambie, porque puede cambiar precios, puede cambiar el modelo del vaucher o recibo y tambien podria cambiar como se avisa. ya que si se quiere cambiar el formato del correo hay que tocar la misma clase en la que se deciden los precios

SEGUNDA DETECCION: open/close, presente en el switch que define precioBase
    porque para agregar un menu nuevo hay que abrir ProcesarPedido y meter un case mas, la clase no estaria cerrada a modificacion, cada tipo de menu nuevo obliga a editar codigo que ya funcionaba y ya estaba probado

TERCERA DETECCION el principio de inversion de dependencias, presente en la linea new BaseDeDatosComedor() y new CorreoUniversitario() dentro de ProcesarPedido
    porque en GestorDePedidos depende directamente de dos clases concretas, y no asi de una abstraccion, no se puede cambiar el motor de la BDD ni el canal de aviso sin modificar GestorDePedidos por dentro
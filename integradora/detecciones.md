primera deteccion el principio de responsabilidad unica, presente en el metodo ProcesarPedido
    porque en un solo metodo se calcula el precio, se guarda en la base de datos y tambien se imprime el vaucher o recibo por consola, y luego tambien manda un correo. 
    son 4 razones distintas para que este metodo cambie, porque puede cambiar precios, puede cambiar el modelo del vaucher o recibo y tambien podria cambiar como se avisa. ya que si se quiere cambiar el formato del correo hay que tocar la misma clase en la que se deciden los precios
    
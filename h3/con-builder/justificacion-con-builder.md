en mi  RF1 de mi variente, (registrar cliente con su plan de cuotas) tiene la forma exacta del problema del sándwich que vimoe en el ejemplo de clases
que serian varios datos seguidos, algunos del mismo tipo, facil de confundir el orden sin que el compilador nos avise

Registrar un cliente con su plan de cuotas junta muchos datos de distinto proposito pero tipos similares 
    dos enteros (cantidadDeCuotas, diaDePago)
    dos decimales (montoPorCuota, posible descuento)
    tambien algunos opcionales...
    Con un constructor tradicional de 7-8 parámetros posicionales, invertir sin querer cantidadDeCuotas y diaDePago compila perfecto y produce un cliente 
    con un plan mal armado — el mismo tipo de error silencioso que el sándwich de Kevin.
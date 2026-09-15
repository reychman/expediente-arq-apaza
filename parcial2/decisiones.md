decisiones...
eleccion y justificacion
Avisos de vencimiento de membresia se tendria que abrir y modificar el modulo de los socios, y eso es exactamente lo que resuelve Observer, el modulo de socios seria el sujto que solo avisa que esto se vencio y cualquier modulo interesado se suscribe sin que el sujeto sepa quien es ni cuantos son.

si no se aplica observer cada nuevo modulo nuevo que se quiere enterar nos obliga a abrir y modificar el modulo de socios que ya no tiene nada que ver con agregar suscripciones
con observer el modulo de socios solo emite (esta membresia vencio), sin saber quien escucha ni quienes son: agregar promociones o lo que venga despues. seria sumar un observador nuevo. sin tocar una sola linea del emisor

Situacion 2 calculo de tarifa de la franja horario Factory 
el calculo de la tarifa en un if/else metido en el modulo de cobros y ese mismo if/else esta copiado en cotizaciones, el dueno cambia las reglas cada temporada entonces cada cambio obliga a tocar 2 lugares distintos y arriega que se olviden actualizar uno de los dos.
con factory armo una fabrica que segun la franja manana noche din de semana entrega el objeto de tarifa correcto y armado. cobros y cotizaciones dejan de tener el if/else  propio, los dos le piden el mismo objeto a la fabrica y usan el mismo metodo para calcular el monto
si no aplico cada cambio de temporada sigue significando editar el if/else en dos modulos con el riesgo real de que queden inconsientes entre si
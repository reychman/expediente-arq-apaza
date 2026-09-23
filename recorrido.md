0:00 – 0:20 · Qué construí y para quién
Mi expediente es el panel de mora y enlaces de pago de una empresa de cobranza: el cliente tiene cuotas mensuales, el sistema calcula la mora, genera el enlace de pago y registra cuando llega el pago. Los atributos que fijé en el H1 fueron seguridad y fiabilidad, porque acá un error es plata mal cobrada

0:20 – 1:00 · El problema crítico que encontré
En mi primer diagrama, EnlaceDePago (clase que genera el link de pago) hacía tres trabajos: calculaba el monto, se comunicaba directo con la pasarela externa (procesador de pagos, servicio de terceros) y validaba la seguridad. Eso rompía SRP (principio de responsabilidad única): un cambio en cómo hablo con el banco me obligaba a tocar la misma clase que calcula montos. Además, quedaba atada a un proveedor específico, que es DIP (principio de inversión de dependencias) roto

1:00 – 1:40 · La decisión y por qué ESA
Saqué la comunicación con el banco a una interfaz, IPasarelaDePago, y apliqué Adapter (patrón adaptador): AdaptadorPasarelaBanco traduce la interfaz propia del banco, PasarelaBanco, a la que mi sistema espera. Elegí Adapter y no modificar EnlaceDePago directamente porque el banco es un proveedor externo — no puedo cambiar su interfaz, y mañana puede cambiar de banco sin tocar mi lógica de negocio

1:40 – 2:00 · Dónde se ve y qué haría distinto
Está en h3/con-adapter: la interfaz IPasarelaDePago y la clase AdaptadorPasarelaBanco. Si tuviera una semana más, resolvería algo que hoy quedó a medias: en mi ADR de H4 documenté un Factory Method para el canal de aviso que en el código nunca llegué a construir.
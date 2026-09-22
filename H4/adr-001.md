TITULO
Uso de Factory Method para generar el canal de aviso cuando una cuota entra en mora (RF5)
CONTEXTO
el sistema debe avisar al cliente cuando su couta entra en mora, y ese aviso puede salir por distintos canales
ya sea SMS correo o por whatsaap, segun la preferencia del cliente
pero antes de tomar esta decision la forma mas directa de resolver esto era con un if/else  dentro mi clase de notificacion
que revisara el canal y tomara la decision de como enviar. 
El riesgo que es esta decision del como avisar el canal quedaria repetida si en el futuro otro modulo del sistema tambien necesita notificar al cliente, por ejemplo un recordatorio antes de vencimiento. cada copia del if/else es un lugar mas donde el canal nuevo se puede olvidar agregar, produciendo un aviso que nunca sale y un cliente que entraria en mora sin saberlo.
DECISION
estamos aplicando el patron factory porque se define un contrato IcanalDeAviso con el metodo Enviar(mensaje,destinario) tres piezas concretas AvisoPorSms AvisoPorCorreo y AvisoPorWhatsApp que lo implementan y una fábrica FabricaDeCanalesDeAviso que centraliza la decision  de que canal instanciasr segun el string recibido. la clase Notificacion deja de preguntar que canal es
y en su lugar le pide el canal correcto a la fabrica usandolo a travez del contrato

CONSECUENCIAS
Positivas: agregar un canal nuevo algun push implica escribir un clase nueva y agreagr una linea en la fabrica 
ningun codigo que ya fincuiona se vuelve a tocar, respentando el principio open/close.
Notificacion queda desacoplada de los canales concretos con el  
Principio de inversión de dependencia, dependiendo solo de ICanalDeAviso
Negativas o compromisos
se agregaron mas clases pequeñas una por canal en vez de un solo metodo con un switch, l oque aumenta la cantidad de archivos del proyecto a cambio de aislar el impacto de cada cambio a futuro. para un  sistema con solo 2 canales fijos que nunca van  a crecer esta complejidad extra no se justificaria
TITULO
Uso de Factory Method para generar el canal de aviso cuando una cuota entra en mora (RF5)
CONTEXTO
el sistema debe avisar al cliente cuando su couta entra en mora, y ese aviso puede salir por distintos canales
ya sea SMS correo o por whatsaap, segun la preferencia del cliente
pero antes de tomar esta decision la forma mas directa de resolver esto era con un if/else  dentro mi clase de notificacion
que revisara el canal y tomara la decision de como enviar. 
El riesgo que es esta decision del como avisar el canal quedaria repetida si en el futuro otro modulo del sistema tambien necesita notificar al cliente, por ejemplo un recordatorio antes de vencimiento. cada copia del if/else es un lugar mas donde el canal nuevo se puede olvidar agregar, produciendo un aviso que nunca sale y un cliente que entraria en mora sin saberlo.

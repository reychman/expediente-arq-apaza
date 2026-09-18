Parte A Nivel 1: Contexto
mis primeros actores son los que defini en m i RF4(Operador, supervisor) mas mi cliente que recibe avisos y paga
y los externos son la pasarela de pagos y mi canal de notificaciones

```mermaid
flowchart TD
    Operador["👤 Operador<br/>(genera enlaces de pago)"]
    Supervisor["👤 Supervisor<br/>(condona mora)"]
    Panel["💳 PANEL DE MORA Y ENLACES DE PAGO<br/><br/>Calcula mora, genera enlaces<br/>y registra los pagos que llegan"]
    Notificaciones["📧 Servicio de notificaciones<br/>(externo — SMS/correo/WhatsApp)"]
```
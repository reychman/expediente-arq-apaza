Parte A Nivel 1: Contexto
mis primeros actores son los que defini en m i RF4(Operador, supervisor) mas mi cliente que recibe avisos y paga
y los externos son la pasarela de pagos y mi canal de notificaciones

```mermaid
flowchart TD
    Operador["👤 Operador<br/>(genera enlaces de pago)"]
    Supervisor["👤 Supervisor<br/>(condona mora)"]
    Panel["💳 PANEL DE MORA Y ENLACES DE PAGO<br/><br/>Calcula mora, genera enlaces<br/>y registra los pagos que llegan"]
    Notificaciones["📧 Servicio de notificaciones<br/>(externo — SMS/correo/WhatsApp)"]
    Cliente["👤 Cliente<br/>(recibe avisos y paga)"]
    Pasarela["💳 Pasarela de pagos<br/>(externa)"]
    Operador -->|genera enlace| Panel
    Supervisor -->|condona mora| Panel
    Panel -->|avisa cuota en mora| Notificaciones
    Notificaciones -->|entrega el aviso| Cliente
    Panel -->|cobra en línea| Pasarela
    Cliente -->|paga| Pasarela
```
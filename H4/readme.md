Parte A Nivel 1: Contexto (el sistema y su mundo)
mis primeros actores son los que defini en mi RF4 (Operador, supervisor) mas mi cliente que recibe avisos y paga
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

Parte A Nivel 2 contenedores (el zoom adentro del sistema)
Aqui muestro el zoom adentro, con los dos patrones que use: Strategy para el calculo de mora y Observer para avisar cuando una cuota cambia de estado.

```mermaid
flowchart TD
    Operador["👤 Operador"]
    Supervisor["👤 Supervisor"]
    subgraph Sistema["💳 PANEL DE MORA Y ENLACES DE PAGO"]
        Web["🌐 Aplicación web<br/>C# / ASP.NET<br/><br/>Pantallas de generación<br/>de enlaces y condonación"]
        Logica["⚙️ Lógica de negocio<br/>C#<br/><br/>Strategy:<br/>IReglaMora (Normal,<br/>Preferencial, Corporativa)<br/><br/>Calcula la mora según<br/>el plan del cliente"]
        BD[("🗄️ Base de datos<br/>SQL<br/><br/>Clientes, cuotas, pagos")]
        Cuota["🔔 Cuota<br/>C#<br/><br/>Observer:<br/>notifica a sus observadores<br/>al cambiar de estado"]
        Notificacion["📣 Observadores de Cuota<br/>C#<br/><br/>NotificadorCliente<br/>AuditoriaCuota"]
    end
    ServicioExterno["📧 Servicio de correo/SMS<br/>(externo)"]
    Operador --> Web
    Supervisor --> Web
    Web --> Logica
    Logica --> BD
    Logica -->|calcula mora con| Cuota
    Cuota -->|cambia de estado y avisa| Notificacion
    Notificacion -->|manda el aviso| ServicioExterno
```
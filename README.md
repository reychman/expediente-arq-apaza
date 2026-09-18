EL CASO DEL PROYECTO — EXPEDIENTE DE
ARQUITECTURA

Arquitectura de Software · UAB · Gestión 2026-2 · Ing. Josue Chura
Se publica: jueves 27-ago-2026 (c3) · Se trabaja TODO el mes · Se defiende: jueves 24-sep (c15)

LA CONSIGNA MADRE (común a todas las variantes)
Vas a construir, clase a clase, el expediente de arquitectura de un sistema. No vas a programar el
sistema completo: vas a diseñarlo, documentarlo y defender tus decisiones, que es exactamente el
trabajo del arquitecto. El expediente vive en un repositorio de GitHub y crece con cada hito:
Hito Qué entrega Vence
H1 Inventario del caso: actores, módulos y primer diagrama de clases dom 30-ago 23:59
H2 Diagrama de clases con SOLID aplicado (refactor antes/después) dom 6-sep 23:59
H3 Al menos 2 patrones de diseño aplicados al caso, con justificación dom 13-sep 23:59
H4 Diagramas C4 niveles 1-2 en Mermaid + 1 ADR dom 20-sep 23:59
Defensa Expediente completo + preguntas técnicas + cambio en vivo jue 24-sep en clase
Reglas del juego:
Cada entrega es un commit en tu repositorio con fecha. El historial de commits es tu evidencia
de autoría (la misma doctrina de siempre: el que construye, commitea).
Tu variante se asigna por tu rubro real (abajo). Si tu trabajo no aparece, elegí la más cercana.
Anti-copia natural: cada variante tiene entidades y reglas distintas. Dos expedientes de la misma
variante tampoco pueden ser iguales: las decisiones (y los ADR que las registran) son personales.
La IA se puede usar como copiloto declarándolo (qué le pediste, qué aceptaste, qué corregiste).
En la defensa respondés por cada decisión como propia: si no la podés explicar, no es tuya.

LOS 6 REQUERIMIENTOS FUNCIONALES (comunes a todas las variantes)
Todo sistema del caso debe cubrir estos 6 requerimientos, aterrizados a su dominio:
1. RF1 Registrar la entidad principal del negocio (con sus datos y validaciones).
2. RF2 Listar y buscar esas entidades con al menos un filtro útil.
3. RF3 Flujo de estados: la entidad principal pasa por estados (ej. pendiente, en proceso,
cerrado) con reglas de transición.
4. RF4 Roles: al menos dos tipos de usuario con permisos distintos (operador y supervisor).
5. RF5 Notificar: un evento del negocio dispara un aviso (correo, mensaje o registro de aviso).
6. RF6 Reportar: un resumen para decisiones (totales, estados, período).
Con esos 6 se puede ejercitar TODO el curso: clases y relaciones (c3), SOLID (c4-c6), patrones
(Factory para crear según tipo, Strategy para reglas por rol, Observer para notificaciones,
Adapter para integrar lo externo, Singleton/Builder donde corresponda), C4 y ADR (c12-c13).

VARIANTE 2 · COBRANZA — "Panel de mora y enlaces de pago" 💳
Rubro de origen: quienes trabajan en cobranza (mora, enlaces de pago a clientes).
Contexto: una empresa de servicios cobra cuotas mensuales. El panel calcula la mora de cada cliente,
genera enlaces de pago y registra los pagos que llegan. Un enlace con monto equivocado es plata mal
cobrada a gente real.
Entidades candidatas: Cliente, Cuota, CálculoDeMora, EnlaceDePago, Pago.
Aterrizaje de los RF: RF1 registrar cliente con su plan de cuotas · RF2 buscar clientes por estado de
deuda · RF3 estados de la cuota: vigente → vencida → en mora → pagada · RF4 operador genera
enlaces,
supervisor condona mora · RF5 aviso al cliente cuando su cuota entra en mora · RF6 reporte de cartera:
total al día, en mora, recuperado.
Atributos críticos sugeridos: idoneidad funcional (el cálculo NO puede estar mal) y seguridad (el
enlace no puede ser manipulable). ¿Dónde queda la usabilidad? Discutilo.
# expediente-arq-apaza
Apaza Carballo Reychman Cristopher
VARIANTE 2: Panel de mora y enlaces de pago

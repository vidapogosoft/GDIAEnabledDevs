# Escenarios de Prueba en Formato Gherkin
Este documento contiene la especificación de escenarios de prueba en formato Gherkin generados a partir de la matriz de pruebas de crédito. Los escenarios están listos para ser guardados como archivos `.feature` e implementados en frameworks BDD como Cucumber, SpecFlow o Behave.

---

## 📂 Categoría: Casos de Borde

```gherkin
# language: es
Funcionalidad: Evaluación de Crédito - Casos de Borde
  Como sistema de evaluación crediticia
  Quiero validar los criterios de casos de borde
  Para garantizar una correcta asignación y seguridad del proceso.

  Escenario: Cliente con edad mínima permitida (ej. 18 años)
    Dado que el cliente tiene una edad de 18 años
    Cuando el sistema procesa la solicitud de evaluación de crédito
    Entonces el resultado debe ser: "Crédito asignado si cumple demás criterios"

  Escenario: Cliente con edad máxima permitida (ej. 70 años)
    Dado que el cliente tiene una edad de 70 años
    Cuando el sistema procesa la solicitud de evaluación de crédito
    Entonces el resultado debe ser: "Crédito asignado si cumple demás criterios"

  Escenario: Ingreso justo en el umbral mínimo requerido
    Dado que el cliente tiene ingresos exactamente en el umbral mínimo
    Cuando el sistema procesa la solicitud de evaluación de crédito
    Entonces el resultado debe ser: "Crédito aprobado si no hay otras restricciones"

  Escenario: Cliente con historial crediticio vacío
    Dado que el cliente no cuenta con registros previos en su historial crediticio
    Cuando el sistema procesa la solicitud de evaluación de crédito
    Entonces el resultado debe ser: "Evaluación especial / crédito limitado"

  Escenario: Cliente con deuda exactamente igual al límite permitido
    Dado que el cliente posee una deuda exactamente igual al límite permitido
    Cuando el sistema procesa la solicitud de evaluación de crédito
    Entonces el resultado debe ser: "Crédito aprobado con condiciones"

```

## 📂 Categoría: Flujos Negativos

```gherkin
# language: es
Funcionalidad: Evaluación de Crédito - Flujos Negativos
  Como sistema de evaluación crediticia
  Quiero validar los criterios de flujos negativos
  Para garantizar una correcta asignación y seguridad del proceso.

  Escenario: Cliente menor de edad
    Dado que el cliente tiene una edad de 18 años
    Cuando el sistema procesa la solicitud de evaluación de crédito
    Entonces el resultado debe ser: "Crédito rechazado"

  Escenario: Cliente con ingresos insuficientes
    Dado que el cliente tiene ingresos inferiores al umbral mínimo
    Cuando el sistema procesa la solicitud de evaluación de crédito
    Entonces el resultado debe ser: "Crédito rechazado"

  Escenario: Cliente con historial crediticio negativo
    Dado que el cliente tiene un reporte financiero con morosidad activa
    Cuando el sistema procesa la solicitud de evaluación de crédito
    Entonces el resultado debe ser: "Crédito rechazado"

  Escenario: Cliente con documentos incompletos
    Dado que el cliente inicia la solicitud con falta de identificación o comprobante
    Cuando el sistema procesa la solicitud de evaluación de crédito
    Entonces el resultado debe ser: "Proceso detenido / error"

  Escenario: Cliente con inconsistencias en datos
    Dado que el nombre del cliente no coincide con los documentos oficiales
    Cuando el sistema procesa la solicitud de evaluación de crédito
    Entonces el resultado debe ser: "Solicitud rechazada"

```

## 📂 Categoría: Pruebas de Seguridad

```gherkin
# language: es
Funcionalidad: Evaluación de Crédito - Pruebas de Seguridad
  Como sistema de evaluación crediticia
  Quiero validar los criterios de pruebas de seguridad
  Para garantizar una correcta asignación y seguridad del proceso.

  Escenario: Intento de inyección SQL en formulario de solicitud
    Dado que un usuario ingresa el valor malicioso "' OR '1'='1" en el formulario de solicitud
    Cuando el sistema procesa la solicitud de evaluación de crédito
    Entonces el resultado debe ser: "Sistema bloquea entrada / error controlado"

  Escenario: Acceso no autorizado a módulo de asignación
    Dado que el usuario intenta acceder al módulo de asignación sin el rol de analista
    Cuando el sistema procesa la solicitud de evaluación de crédito
    Entonces el resultado debe ser: "Acceso denegado"

  Escenario: Manipulación de datos en tránsito
    Dado que se altera el payload de los datos de la solicitud en tránsito
    Cuando el sistema procesa la solicitud de evaluación de crédito
    Entonces el resultado debe ser: "Datos rechazados / alerta"

  Escenario: Cliente intenta modificar su score manualmente
    Dado que un cliente intenta modificar su score mediante herramientas del front-end
    Cuando el sistema procesa la solicitud de evaluación de crédito
    Entonces el resultado debe ser: "Validación en backend evita fraude"

  Escenario: Prueba de fuerza bruta en login del sistema
    Dado que se realizan múltiples intentos fallidos de inicio de sesión en el sistema
    Cuando el sistema procesa la solicitud de evaluación de crédito
    Entonces el resultado debe ser: "Cuenta bloqueada / alerta de seguridad"

```


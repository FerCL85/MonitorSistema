# MonitorSistema

Aplicación WPF desarrollada en C# (.NET 8) que monitoriza información del sistema en tiempo real, utilizando el patrón MVVM.

## 🧩 Funcionalidad

- Muestra una lista ordenada por fecha, de modo descendente, con:
  - Fecha/Hora de la muestra
  - Número de serie de la CPU
  - Número de serie de la placa base
  - Número de serie de la GPU - esta funcionalidad no ha podido ser implementada debido a que el método GetGpuID() devuelve el error InsufficientMemoryException, en su lugar aparece un "-"
  - Porcentaje de uso de CPU
  - Consumo de memoria RAM (en GB)
- Permite:
  - Iniciar y detener la toma de muestras
  - Configurar el intervalo de muestreo (de 500ms a 10s)
  - Conservar los datos entre ejecuciones mediante persistencia local en un fichero json (data.json)

## 💻 Requisitos

- Visual Studio 2022 o superior
- .NET 8 SDK

## 🚀 Ejecución

1. Abre la solución `MonitorSistema.sln` en Visual Studio.
2. Establece el proyecto `MonitorSistema` como proyecto de inicio.
3. Ejecutar.

## ✅ Pruebas unitarias

El proyecto incluye pruebas unitarias (MonitorSistemaTest) que validan:

- El ViewModel se inicia correctamente.
- Se generan muestras mediante la lógica real implementada.

Para ejecutarlas:

```bash
dotnet test

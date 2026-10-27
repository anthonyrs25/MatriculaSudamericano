\# Matrícula Sudamericano (.NET MAUI)



App de formulario de matrícula con login, validaciones y registro en archivo TXT.



\## Credenciales de prueba

\- Usuario: `admin`

\- Contraseña: `Sudamericano2026`



\## Cómo ejecutar

1\. Abrir `MatriculaSudamericano.slnx` en Visual Studio 2026 (.NET 10).

2\. Elegir \*\*Windows Machine\*\* o un emulador Android y presionar ▶.

3\. En Windows, las inscripciones se guardan en `Documentos\\MatriculaSudamericano\\inscripciones.txt`.



\## Arquitectura (MVVM)

\- `Models/`: Inscripcion

\- `Services/`: autenticación (hash PBKDF2 + bloqueo por intentos), validaciones, alertas, navegación y guardado en TXT

\- `ViewModels/`: LoginViewModel, MatriculaViewModel

\- `Views/`: LoginPage, MatriculaPage (diseño adaptable)



\## Informe

\[Ver informe en PDF](docs/capturas_tarea_autonoma_picker.pdf)


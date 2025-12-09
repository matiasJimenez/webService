Scripts de datos de prueba
==========================

Este directorio contiene scripts para poblar la base `Gestiones` con datos mínimos de prueba.

Prerequisitos
- Base de datos creada y accesible en SQL Server.
- Herramienta `sqlcmd` instalada (o usar Azure Data Studio/SSMS para ejecutar el .sql).

Comandos rápidos
- Ejecutar seed (ajusta user/password si es necesario):
  sqlcmd -S localhost,1433 -U sa -P "TU_PASSWORD" -d Gestiones -i scripts/seed-dev-data.sql

Notas
- El script usa IDs autoincrementales; si ya tienes datos, ejecuta con cuidado (puedes limpiar tablas antes si corresponde).
- No hay operaciones de escritura en código de la API más allá de las inserciones aquí.

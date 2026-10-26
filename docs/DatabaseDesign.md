# Начален модел на данните — UrbanSignals

| Entity | Ключови полета | Цел |
|---|---|---|
| `ApplicationUser` | Id, FullName, Email | Потребителска идентичност; ролите се управляват чрез ASP.NET Core Identity |
| `Report` | Id, Title, Description, CategoryId, Status, Latitude, Longitude, CreatedAt | Основна информация за сигнал |
| `ReportCategory` | Id, Name, Description, IsActive | Категоризация на проблемите |
| `ReportImage` | Id, ReportId, ImageUrl, UploadedAt | Метаданни и адрес на снимка |
| `ReportStatusHistory` | Id, ReportId, OldStatus, NewStatus, ChangedAt, Note | Проследима история на статуса |
| `Assignment` | Id, ReportId, AssignedToUserId, AssignedByUserId, DueAt, Status | Назначаване на изпълнител и срок |
| `ReportComment` | Id, ReportId, AuthorId, Body, CreatedAt, IsPublic | Коментари и публични актуализации |
| `Notification` | Id, UserId, Title, Message, CreatedAt, IsRead | Известия към потребители |

## Основни връзки
- Една категория има много сигнали (1:N).
- Един сигнал принадлежи към една категория (N:1).
- Един сигнал може да има много снимки, коментари и записи в историята (1:N).
- Един сигнал може да има повече от едно назначение във времето (1:N).
- Един потребител може да подава много сигнали, да коментира и да получава известия (1:N).

## Важни ограничения
- Заглавието и описанието на сигнала са задължителни и с ограничена дължина.
- Географските координати, ако са зададени, трябва да са в допустимите диапазони.
- Името на категорията е уникално.
- При изтриване на потребител/категория трябва да се избегне неволна загуба на историята; правилата за изтриване се определят изрично чрез EF Core конфигурация.
- Снимките се пазят като файлове/обектно хранилище, а базата съдържа само метаданни и контролирани URL адреси.
- Дублирането не се решава само с уникален индекс: планира се бизнес проверка по близост на координатите, категория и времеви интервал.

## Миграции
EF Core Code First е източникът на схемата. След конфигуриране на локалната SQL Server връзка се генерира миграция:
```bash
dotnet ef migrations add InitialCreate --project backend/UrbanSignals.Infrastructure --startup-project backend/UrbanSignals.Api --output-dir Migrations
dotnet ef database update --project backend/UrbanSignals.Infrastructure --startup-project backend/UrbanSignals.Api
```
Миграционните файлове се commit-ват в Git след преглед. Не се публикуват пароли или реални данни.

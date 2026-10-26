-- Използвайте този скрипт САМО ако базата е създадена без EF Core HasData.
-- При изпълнение на миграцията HasData вече добавя тези категории.
IF NOT EXISTS (SELECT 1 FROM dbo.ReportCategories WHERE Name = N'Пътища и тротоари')
 INSERT INTO dbo.ReportCategories (Id, Name, Description, IsActive) VALUES ('10000000-0000-0000-0000-000000000001', N'Пътища и тротоари', N'Дупки, повредени настилки и тротоари', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.ReportCategories WHERE Name = N'Чистота')
 INSERT INTO dbo.ReportCategories (Id, Name, Description, IsActive) VALUES ('10000000-0000-0000-0000-000000000002', N'Чистота', N'Отпадъци и нерегламентирани сметища', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.ReportCategories WHERE Name = N'Улично осветление')
 INSERT INTO dbo.ReportCategories (Id, Name, Description, IsActive) VALUES ('10000000-0000-0000-0000-000000000003', N'Улично осветление', N'Неработещи или повредени осветителни тела', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.ReportCategories WHERE Name = N'Паркове и зелени площи')
 INSERT INTO dbo.ReportCategories (Id, Name, Description, IsActive) VALUES ('10000000-0000-0000-0000-000000000004', N'Паркове и зелени площи', N'Проблеми в паркове, градини и площадки', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.ReportCategories WHERE Name = N'Други')
 INSERT INTO dbo.ReportCategories (Id, Name, Description, IsActive) VALUES ('10000000-0000-0000-0000-000000000005', N'Други', N'Други проблеми в градската среда', 1);

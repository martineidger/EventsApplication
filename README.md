Инструкции к запуску проекта EventsApplication:
1) Для запуска необходима .NET SDK - проект написан на .net 8 ([скачайте и установите последнюю версию, если ее нет](https://dotnet.microsoft.com/en-us/download))
2) IDE - Рекомендуется использовать Visual Studio, Visual Studio Code или JetBrains Rider. Для Visual Studio выберите рабочую нагрузку "ASP.NET и веб-разработка"
3) SQL Server Express [https://www.microsoft.com/en-us/sql-server/sql-server-downloads]
4) Клонируйте репозиторий: [git clone https://github.com/martineidger/EventsApplication-ASP-.Net.git // cd EventsApplication-ASP-.Net]
5) Откройте проект и восстановите зависимости [bash: dotnet restore]
6) Убедитесь, что файл appsettings.json настроен правильно для вашей среды (например, строки подключения к базе данных и другие параметры конфигурации)
7) Запустите миграции [bash: dotnet ef database update]
8) Запустите проект. Откройте браузер [https://localhost:7230/]
9) Запустите тесты [bash: dotnet test]

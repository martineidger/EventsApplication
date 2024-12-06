# Web-API (back-end) приложение для работы с мероприятиями (C# ASP.Net Core)

## **Инструкции к запуску проекта EventsApplication:**
1. Для запуска необходима .NET SDK - проект написан на .net 8 ([скачайте и установите последнюю версию, если ее нет](https://dotnet.microsoft.com/en-us/download))
2. IDE - Рекомендуется использовать Visual Studio, Visual Studio Code или JetBrains Rider. Для Visual Studio выберите рабочую нагрузку "ASP.NET и веб-разработка"
3. [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
4. Клонируйте репозиторий:
  ```
  git clone https://github.com/martineidger/EventsApplication.git
  cd EventsApplication
  ```
6. Откройте проект и восстановите зависимости `dotnet restore`
7. Убедитесь, что файл appsettings.json настроен правильно для вашей среды (например, строки подключения к базе данных и другие параметры конфигурации)
8. Запустите миграции (консоль диспетчера пакетов -> убедитесь, что запускаете команду для проекта Events.Persistence) `Update-Database`
9. Запустите проект. Откройте страницу в браузере:
    [Swagger](https://localhost:7230/swagger/index.html)

## В приложении присутствуют пользователи по умолчанию
### Admin
**Email**: admin1@mail.com    
**Password**: Admin123!
### User 
**Email**: user1@mail.com    
**Password**: User12345!

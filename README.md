<h1 align="center">Инструкции к запуску проекта EventsApplication:</h1>
<ul>
  <li>1. Для запуска необходима .NET SDK - проект написан на .net 8 ([скачайте и установите последнюю версию, если ее нет](https://dotnet.microsoft.com/en-us/download))</li>
  <li>2. IDE - Рекомендуется использовать Visual Studio, Visual Studio Code или JetBrains Rider. Для Visual Studio выберите рабочую нагрузку "ASP.NET и веб-разработка"</li>
  <li>3. [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)</li>
  <li>4. Клонируйте репозиторий: [git clone https://github.com/martineidger/EventsApplication-ASP-.Net.git // cd EventsApplication-ASP-.Net]</li>
  <li>5. Откройте проект и восстановите зависимости [bash: dotnet restore]</li>
  <li>6. Убедитесь, что файл appsettings.json настроен правильно для вашей среды (например, строки подключения к базе данных и другие параметры конфигурации)</li>
  <li>7. Запустите миграции [bash: dotnet ef database update]</li>
  <li>8. Запустите проект. Откройте браузер [https://localhost:7230/]</li>
</ul>

## В приложении присутствуют пользователи по умолчанию:
#### Admin - admin1@mail.com / Admin123!
#### User - user1@mail.com / User12345!

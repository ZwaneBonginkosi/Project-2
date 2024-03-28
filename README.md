## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

* .NET 8 SDK
* Visual Studio 2019 or Visual Studio Code
* Mysql
* Redis

### Installing

1. Clone the repository:

```bash 
git clone https://github.com/ZwaneBonginkosi/Project-2.git
```

2. Open the solution file Project 2.sln in Visual Studio.

3. Update the appsettings.json

```json
"ConnectionStrings": {
    "RedisConnection": "127.0.0.1:6379",
    "MySqlConnection": "server=127.0.0.1;uid=root;pwd=;database=CurrencyExchange"
}
```

4. Update Migration

```bash
dotnet ef database update --project CurrencyExchange.Infrastructure/CurrencyExchange.Infrastructure.csproj --startup-project CurrencyExchange.WebAPI/CurrencyExchange.WebAPI.csproj --context CurrencyExchange.Infrastructure.Data.AppDbContext --configuration Debug 20240327161549_InitialCE
```

5. Build the solution by pressing Ctrl + Shift + B or by navigating to Build -> Build Solution.

6. Run the application by pressing F5 or by navigating to Debug -> Start Debugging.


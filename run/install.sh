sudo mkdir -p /Users/davidlong/.local/share
sudo chown -R $USER /Users/davidlong/.local

dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design

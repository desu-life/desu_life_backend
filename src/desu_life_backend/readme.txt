建表（migration）步骤：
首先直接写一个Model类，然后用ef的migration映射到数据库

先运行dotnet tool install --global dotnet-ef 安装dotnet ef
更新实体类后，使用
dotnet ef migrations add xxxx，进行Code first重新生成相关migration
检查完生成文件后，再运行
dotnet ef database update
更新到数据库
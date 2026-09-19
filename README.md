# Identity Workshop Service

ชุดโครงการทดลองสำหรับหลักสูตร 5.8.2.2 (Backend) และ 5.8.2.3 (PostgreSQL)

โครงการนี้ยกแนวคิดจาก Identity Service ของ GLO มาเหลือเฉพาะ User Directory ที่ใช้ฝึกเปิด Solution, รัน Backend, Debug, เพิ่ม Endpoint, เพิ่ม Validation, ตรวจ Log และปรับ Query/Index โดยไม่มี Keycloak, Redis, MinIO, Gateway หรือข้อมูล Production

## โครงสร้าง

```text
src/
  IdentityWorkshop.Api            HTTP API, middleware, configuration
  IdentityWorkshop.Application    DTOs, service, repository contracts
  IdentityWorkshop.Domain         WorkshopUser entity
  IdentityWorkshop.Infrastructure EF Core + PostgreSQL repository
tests/
  IdentityWorkshop.UnitTests      Unit tests ที่ไม่ต้องใช้ Database
database/                         SQL สำหรับ Database ที่ผู้สอนเตรียม
docs/                             แผนที่ Lab และการเชื่อมต่อ Environment
```

## เริ่มต้นแบบ Local Network หรือ VM

1. ติดตั้ง Visual Studio 2026 พร้อม ASP.NET and web development และ .NET 10 SDK
2. สร้าง Database ตาม `database/01_create_schema.sql` และ `database/02_seed_data.sql`
3. ตั้งค่า `ConnectionStrings__WorkshopDb` ให้ชี้ไปยัง PostgreSQL ของผู้สอน หรือแก้ `src/IdentityWorkshop.Api/appsettings.Development.json`
4. รัน `dotnet restore` และ `dotnet build -c Release`
5. รัน `dotnet run --project src/IdentityWorkshop.Api --urls http://0.0.0.0:5082`
6. จากเครื่องผู้เรียนเรียก `http://<TRAINER_HOST>:5082/health` และ API ตามเอกสาร Lab

การเปิด `0.0.0.0` ใช้เฉพาะเครือข่าย Workshop ที่ได้รับอนุญาตเท่านั้น ห้ามใช้ Production Credential, ข้อมูลบุคคลจริง หรือเปิด Port นี้ออก Internet โดยตรง

## Connection String

ตัวอย่างค่าในไฟล์เป็นค่าทดสอบเท่านั้น ไม่มี Credential จริง ให้ใช้ Environment Variable แทนเมื่อเชื่อมต่อ VM:

```powershell
$env:ConnectionStrings__WorkshopDb = "Host=<DB_HOST>;Port=5432;Database=identity_workshop;Username=<DB_USER>;Password=<DB_PASSWORD>;Search Path=workshop"
dotnet run --project src/IdentityWorkshop.Api --urls http://0.0.0.0:5082
```

รายละเอียดการเตรียม Database, Firewall และจุดที่ใช้ในแต่ละ Lab อยู่ที่ `docs/Workshop_Lab_Map.md`

## ลักษณะของชุดเริ่มต้น

ชุดนี้เป็น Workshop starter ไม่ใช่ Production service โดยมีจุดที่ติดป้าย `LAB 7`, `LAB 8` และ `LAB 11/12` ไว้ในโค้ดสำหรับให้ผู้เรียนแก้หรือปรับปรุงตามโจทย์ ผู้สอนควรแจก Release/Tag เดียวกันให้ผู้เรียนทุกคน และเก็บเฉลยไว้คนละ Branch หรือพื้นที่ที่ผู้เรียนเข้าถึงไม่ได้


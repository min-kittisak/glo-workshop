# Workshop Lab Map

ชุดนี้ตั้งใจให้ผู้สอนเตรียม PostgreSQL แยกจาก Repo และให้ผู้เรียนแก้โค้ดในชุดทดลองตามลำดับ Lab

## Backend: 5.8.2.2

| Lab | เป้าหมาย | จุดที่ผู้เรียนใช้ |
| --- | --- | --- |
| 4 | เปิด Solution และ Build | `IdentityWorkshop.slnx`, โครงสร้าง 4 Layers, Visual Studio 2026, .NET 10 |
| 5 | รัน Backend และแก้ Configuration | `appsettings.Development.json`, `ConnectionStrings__WorkshopDb`, `/health` |
| 6 | Debug และรัน Test | `UserService`, `UserServiceTests`, Breakpoint และ Output ของ Test |
| 7 | เพิ่ม Endpoint จากแม่แบบ C# | `WorkshopUsersController`, `UserService`, `IUserRepository` |
| 8 | เพิ่ม Validation และจำกัด Response | `UserSearchQuery`, `UserService.SearchAsync`, DTO ที่ส่งกลับ |
| 9 | ทดสอบ 3 กรณีและอ่าน Log | Search สำเร็จ, Validation 400, Database/Unhandled Error 500, `X-Correlation-Id` |
| 10 | ตรวจโครงสร้างและหาจุดเสี่ยง | Controller บาง, Service รับกติกา, Repository ทำ Query, ไม่มี Credential ใน Source |

## Database: 5.8.2.3

| Lab | เป้าหมาย | ไฟล์/จุดที่ใช้ |
| --- | --- | --- |
| 11 | สร้างตารางและวัดผลก่อน/หลัง Index | `database/01_create_schema.sql`, `02_seed_data.sql`, `03_lab11_before_index.sql` |
| 12 | ปรับ Query และเปรียบเทียบผล | `database/04_lab12_after_index.sql`, `UserRepository.SearchAsync` |

## สิ่งที่ผู้สอนต้องเตรียม

- PostgreSQL 16+ หรือรุ่นที่โครงการอนุมัติ พร้อมสิทธิ์สร้าง Schema/Table/Index ใน Database ทดลอง
- Network route จากเครื่องผู้เรียนไปยัง Database Host และ API Host
- Database name, username, password และ `Search Path=workshop` สำหรับชุดทดลองเท่านั้น
- Firewall อนุญาตเฉพาะ Subnet ห้องอบรม ไม่เปิด PostgreSQL ออก Internet
- Snapshot/วิธี Reset Database ก่อนเริ่มกลุ่มถัดไป
- Repository release/tag ที่ใช้สอน และค่า URL ที่ผู้เรียนต้องเรียก

## Local Network หรือ VM

ค่า Connection String ส่งผ่าน Environment Variable ได้:

```powershell
$env:ConnectionStrings__WorkshopDb = "Host=<DB_HOST>;Port=5432;Database=identity_workshop;Username=<DB_USER>;Password=<DB_PASSWORD>;Search Path=workshop"
dotnet run --project src/IdentityWorkshop.Api --urls http://0.0.0.0:5082
```

ทดสอบจากเครื่องผู้เรียน:

```powershell
Invoke-RestMethod http://<API_HOST>:5082/health
Invoke-RestMethod "http://<API_HOST>:5082/api/v1/workshop-users/search?q=9999&limit=20"
```

## ขอบเขตข้อมูล

ใช้เฉพาะข้อมูล Synthetic ที่สร้างจาก `02_seed_data.sql` ห้ามใช้ข้อมูลผู้ใช้งานจริง, Token, Password หรือ Connection String จริงใน Commit, Screenshot หรือหลักฐานส่งงาน


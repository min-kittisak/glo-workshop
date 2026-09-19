# Lab Exercise Tasks

เอกสารนี้เป็นแผนงานสำหรับชุดเริ่มต้นเท่านั้น ใช้คู่กับ Workshop Manual และไม่ใช่เฉลยฉบับเต็ม

## 5.8.2.2 Backend

### Lab 1: เปิด Solution

- เปิด `IdentityWorkshop.slnx` ด้วย Visual Studio 2026
- ตรวจว่า Target Framework เป็น .NET 10 และ Project Reference ครบ 5 โครงการ
- Build แบบ Release ให้สำเร็จ

### Lab 2: รัน Backend และแก้ Configuration

- ตรวจ `ConnectionStrings:WorkshopDb` และเปลี่ยนจากค่า Localhost เป็นค่าชุดทดลองที่ผู้สอนประกาศ
- รัน API ด้วย URL ที่ฟังบน Network Interface ที่อนุญาต
- เรียก `/health` และบันทึก URL/Environment ที่ใช้ โดยไม่แนบ Password

### Lab 3: Debug และ Test

- วาง Breakpoint ใน `UserService.SearchAsync` และ `UserRepository.SearchAsync`
- ตรวจค่า `Term`, `Limit`, Query ที่ส่งต่อ และผลลัพธ์ก่อนกลับ Controller
- รัน `dotnet test -c Release` และแนบผล Test ที่ไม่มีข้อมูลลับ

### Lab 4: เพิ่ม Endpoint

- ใช้ `WorkshopUsersController.Search` เป็นแม่แบบ
- เพิ่ม `GET /api/v1/workshop-users/{userId:guid}` ให้ไหลผ่าน Controller, Service และ Repository
- รองรับกรณีพบข้อมูลเป็น 200 และไม่พบข้อมูลเป็น 404 พร้อม Response Contract ที่อ่านได้

### Lab 5: Validation และ Response

- เพิ่ม Validation ของ `q` และ `limit` ในชั้น Application ไม่ใส่ Business Rule ไว้ใน Controller
- กำหนดเพดาน `limit` ตามที่ผู้สอนประกาศ และตอบ 400 เมื่อค่าผิด
- ตรวจว่า List Response ส่งเฉพาะฟิลด์ที่จำเป็น และไม่ส่ง `EmailAddress` ในรายการค้นหา

### Lab 6: API 3 กรณีและ Log

- กรณีสำเร็จ: ค้นหาด้วยคำที่มีข้อมูล
- กรณี Validation: ส่ง `limit` ไม่ถูกต้องหลังทำ Lab 5
- กรณี Dependency/Error: ใช้ Database ของชุดทดลองที่ตัดการเชื่อมต่อชั่วคราวตามขั้นตอนผู้สอน
- บันทึก Status Code, `X-Correlation-Id` และ Log ที่ใช้ติดตามเหตุการณ์ โดยไม่แนบ Connection String

### Lab 7: ตรวจงานและหาบริการที่มีปัญหา

- อธิบายเส้นทาง `Controller -> Application Service -> Repository -> PostgreSQL`
- ตรวจว่าการอ่านข้อมูลใช้ `AsNoTracking()` และส่ง `CancellationToken`
- ตรวจว่าไม่มี Credential, Production URL หรือ Query ที่ยิง Database ซ้ำในลูป

## 5.8.2.3 PostgreSQL

### Lab 8: Baseline และ Index

- รัน `01_create_schema.sql` และ `02_seed_data.sql`
- รัน Query ใน `03_lab8_before_index.sql` ด้วย `EXPLAIN (ANALYZE, BUFFERS)`
- บันทึก Execution Time, Scan Type, Rows และ Buffers ก่อนเพิ่ม Index
- ออกแบบ Index ที่สัมพันธ์กับ Query จริง แล้ววัดผลหลังเพิ่ม Index

### Lab 9: Query Tuning

- เปรียบเทียบ Query ที่มี Leading Wildcard กับ Prefix Search
- ใช้ `04_lab9_after_index.sql` เป็นกรณีศึกษาเรื่อง Expression Index และ Partial Index
- เปรียบเทียบแผนการทำงานและอธิบายว่าทำไม Index บางแบบไม่ช่วย Query ที่ขึ้นต้นด้วย `%`
- สรุปข้อจำกัดของผลทดสอบ เช่น จำนวนข้อมูล, Statistics, Cache และช่วงเวลาที่วัด

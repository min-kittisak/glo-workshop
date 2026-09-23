-- Migration สำหรับฐาน Workshop เดิมที่มีตาราง workshop.users อยู่แล้ว
-- รหัสผ่านเริ่มต้นของผู้ใช้ตัวอย่างทุกคนคือ P@ssw0rd

BEGIN;

ALTER TABLE workshop.users
    ADD COLUMN IF NOT EXISTS password_hash varchar(100);

UPDATE workshop.users
SET password_hash = '$2a$10$HN1PxLf/Es1Df9n/LTVLAezoFi7GttOr4LeZg3bjPXFKkJcySd3Wy'
WHERE password_hash IS NULL OR password_hash = '';

ALTER TABLE workshop.users
    ALTER COLUMN password_hash SET DEFAULT '$2a$10$HN1PxLf/Es1Df9n/LTVLAezoFi7GttOr4LeZg3bjPXFKkJcySd3Wy',
    ALTER COLUMN password_hash SET NOT NULL;

COMMENT ON TABLE workshop.users IS 'ตารางผู้ใช้ตัวอย่างสำหรับ Workshop';
COMMENT ON COLUMN workshop.users.user_id IS 'รหัสผู้ใช้สำหรับชุด Workshop';
COMMENT ON COLUMN workshop.users.password_hash IS 'รหัสผ่านแบบเข้ารหัส bcrypt สำหรับผู้ใช้ตัวอย่างของ Workshop';
COMMENT ON COLUMN workshop.users.display_name IS 'ชื่อแสดงผลสำหรับแบบฝึกหัดค้นหาใน LAB 8 และ LAB 9';

COMMIT;

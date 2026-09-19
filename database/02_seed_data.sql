-- Run after 01_create_schema.sql.
-- The data is synthetic and contains no GLO production information.
INSERT INTO workshop.users
    (user_id, username, display_name, email_address, department_code, is_active, created_at)
SELECT
    gen_random_uuid(),
    'workshop.user.' || lpad(source_id::text, 6, '0'),
    'Workshop User ' || lpad(source_id::text, 6, '0'),
    'workshop.user.' || lpad(source_id::text, 6, '0') || '@example.test',
    CASE source_id % 5
        WHEN 0 THEN 'IT'
        WHEN 1 THEN 'FIN'
        WHEN 2 THEN 'HR'
        WHEN 3 THEN 'OPS'
        ELSE 'ADM'
    END,
    source_id % 17 <> 0,
    now() - make_interval(days => (source_id % 365))
FROM generate_series(1, 50000) AS source_id
ON CONFLICT (username) DO NOTHING;

ANALYZE workshop.users;


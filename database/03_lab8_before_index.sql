-- 5.8.2.3 / LAB 8: capture the baseline plan before adding a search index.
-- Run each statement separately and keep the EXPLAIN ANALYZE output.

EXPLAIN (ANALYZE, BUFFERS)
SELECT user_id, username, display_name, department_code
FROM workshop.users
WHERE is_active = true
  AND display_name ILIKE '%9999%'
ORDER BY username
LIMIT 20;

-- Compare the row count and execution time with a term that matches many rows.
EXPLAIN (ANALYZE, BUFFERS)
SELECT count(*)
FROM workshop.users
WHERE is_active = true
  AND display_name ILIKE '%Workshop%';

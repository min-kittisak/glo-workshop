-- Lab 12: create an index for a prefix-search variant, then compare the plan.
-- The query shape must match the index expression; a leading wildcard cannot use
-- a normal B-tree index efficiently.

CREATE INDEX IF NOT EXISTS ix_workshop_users_active_display_name_prefix
    ON workshop.users (lower(display_name) text_pattern_ops)
    WHERE is_active = true;

ANALYZE workshop.users;

EXPLAIN (ANALYZE, BUFFERS)
SELECT user_id, username, display_name, department_code
FROM workshop.users
WHERE is_active = true
  AND lower(display_name) LIKE 'workshop user 9999%'
ORDER BY username
LIMIT 20;

-- This is intentionally kept as a comparison case. The leading wildcard still
-- requires a different indexing strategy (for example pg_trgm) if needed.
EXPLAIN (ANALYZE, BUFFERS)
SELECT user_id, username, display_name, department_code
FROM workshop.users
WHERE is_active = true
  AND display_name ILIKE '%9999%'
ORDER BY username
LIMIT 20;


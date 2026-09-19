-- Run as a database owner in the workshop database.
CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE SCHEMA IF NOT EXISTS workshop;

CREATE TABLE IF NOT EXISTS workshop.users
(
    user_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    username varchar(100) NOT NULL,
    display_name varchar(200) NOT NULL,
    email_address varchar(255) NOT NULL,
    department_code varchar(30),
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    CONSTRAINT uq_workshop_users_username UNIQUE (username)
);

COMMENT ON TABLE workshop.users IS 'Training-only user directory for Identity Workshop';
COMMENT ON COLUMN workshop.users.user_id IS 'Training user identifier';
COMMENT ON COLUMN workshop.users.display_name IS 'Display name used for Lab 11/12 search exercises';


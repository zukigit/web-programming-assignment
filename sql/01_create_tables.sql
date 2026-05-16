-- =====================================================
-- SchoolSystem Database - Create Tables
-- Target: PostgreSQL
-- =====================================================

CREATE TABLE Users (
    Id          SERIAL PRIMARY KEY,
    Username    VARCHAR(50)  NOT NULL UNIQUE,
    PasswordHash VARCHAR(128) NOT NULL,
    Role        VARCHAR(10)  NOT NULL CHECK (Role IN ('Admin', 'Teacher', 'Student')),
    FullName    VARCHAR(100) NOT NULL,
    Email       VARCHAR(200),
    CreatedAt   TIMESTAMP    NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS IX_Users_Username ON Users(Username);

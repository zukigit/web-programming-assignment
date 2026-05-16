-- =====================================================
-- SchoolSystem Database - Sample Data
-- Passwords are SHA256 hashes:
--   admin    / admin123
--   teacher1 / teacher123
--   student1 / student123
-- =====================================================

INSERT INTO Users (Id, Username, PasswordHash, Role, FullName, Email, CreatedAt)
VALUES
(1, 'admin',
 'JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=',
 'Admin',
 'System Administrator',
 'admin@school.edu',
 '2024-01-01T00:00:00'),

(2, 'teacher1',
 'zeOD7ujuekQArfehX3FvF5ouuXZGs34InrjW0E5mNBY=',
 'Teacher',
 'John Smith',
 'john.smith@school.edu',
 '2024-01-01T00:00:00'),

(3, 'student1',
 'cDsKPWrXW2SaKK3efYPGJR2kV1SSY7x/9F7HCbCoRIs=',
 'Student',
 'Alice Johnson',
 'alice.j@school.edu',
 '2024-01-01T00:00:00');

-- Reset sequence after explicit ID inserts
SELECT setval('users_id_seq', (SELECT MAX(Id) FROM Users));

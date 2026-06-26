# Создание БД и пользователя для Зеленый квартал — Учёт электроэнергии (PostgreSQL)
# Запуск: psql -U postgres -f scripts/create-cnt-db.sql

DO $$
BEGIN
  IF NOT EXISTS (SELECT FROM pg_roles WHERE rolname = 'cnt_gq_db_user') THEN
    CREATE ROLE cnt_gq_db_user WITH LOGIN PASSWORD 'ChangeMe_StrongPassword_123!';
  END IF;
END
$$;

SELECT 'CREATE DATABASE cnt_gq_db OWNER cnt_gq_db_user'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'cnt_gq_db')\gexec

GRANT ALL PRIVILEGES ON DATABASE cnt_gq_db TO cnt_gq_db_user;

# University Course Management API

Полноценный RESTful Web API для управления университетскими курсами Satbayev University.

## 🏛 Схема архитектуры
Client (Swagger / Postman)
↓ HTTP Request (JSON)
Controllers (Students, Teachers, Courses, Enrollments)
↓ DTO / AutoMapper
Services & Generic Repository
↓ Linq Queries
Entity Framework Core
↓ SQLite Provider
Database (university.db)

## 📌 Реализованные бизнес-правила
1. Нельзя записать студента на несуществующий курс (404 Not Found).
2. Нельзя записать несуществующего студента на курс (404 Not Found).
3. Запрещена повторная запись студента на один и тот же курс (409 Conflict).
4. Курс не может ссылаться на несуществующего преподавателя (400 Bad Request).
5. Все ответы нормализованы через универсальный контракт ReturnResult<T>.
6. Централизованная обработка исключений через ExceptionMiddleware (500 Internal Server Error).

## 🚀 Основные Endpoints
* **Students:** `GET /api/students`, `GET /api/students/{id}`, `POST /api/students`, `PUT /api/students/{id}`, `DELETE /api/students/{id}`
* **Teachers:** `GET /api/teachers`, `GET /api/teachers/{id}`, `POST /api/teachers`
* **Courses:** `GET /api/courses` (с поддержкой фильтрации ?search=...), `POST /api/courses`
* **Enrollments:** `GET /api/enrollments`, `POST /api/enrollments`, `PUT /api/enrollments/{id}/grade`, `DELETE /api/enrollments/{id}`
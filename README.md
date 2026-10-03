# Домашняя работа — Модуль 04: Book Catalog API

**CSE5032 · Разработка веб-сервисов · Модуль 04**

Требовалось сделать каталог книг с постоянным хранением и разделить приложение на слои. EF Core и миграция создают SQLite-базу. Контроллер обращается к Repository, а не напрямую к контексту. AutoMapper преобразует Entity и DTO, а `ReturnResult` задаёт общий формат ответа, не заменяя HTTP-коды. Покажу полный сценарий: создание, чтение, изменение, поиск по автору и удаление.

---

## Цель

Создать каталог книг с хранением через Entity Framework Core и показать разделение API, DTO, бизнес-доступа к данным и базы данных.

## Краткий отчёт

Проект использует SQLite и Code First миграцию. Контроллер обращается к данным через `IBookRepository`; AutoMapper преобразует сущности в DTO; ответы обёрнуты в `ReturnResult<T>`. Дополнительно реализован поиск по автору.

## Выполнение по шагам

| Шаг по заданию | Реализация | Для чего |
|---|---|---|
| 1. Модель и БД | Созданы `Book`, `BookCatalogDbContext` и строка `BookCatalog` в `appsettings.json` | Описать сущность и настроить хранилище |
| 2. Code First | Добавлена миграция `InitialCreate`; при запуске приложение применяет миграции | Создать/обновить SQLite-схему из классов |
| 3. Repository | `IBookRepository` и `BookRepository` выполняют операции EF Core | Не давать контроллеру обращаться напрямую к `DbContext` |
| 4. DTO и AutoMapper | Созданы DTO для чтения/создания/обновления и `BookMappingProfile` | Отделить API-контракт от сущности и автоматизировать преобразования |
| 5. Единый результат | `ReturnResult<T>` формирует `isSuccess`, `result`, `errorMessage` | Сделать тело ответа предсказуемым, сохраняя HTTP status code |
| 6. CRUD | Реализованы GET списка/по Id, POST, PUT, DELETE | Управлять книгами по HTTP |
| 7. Поиск | Добавлен `GET /api/books/search?author=...` | Найти книги по автору |
| 8. Проверка | Через Swagger создана книга, получена, изменена, найдена и удалена | Проверить полный сценарий работы API |

## Модель `Book`

| Поле | Тип | Ограничение / назначение |
|---|---|---|
| `Id` | `int` | Первичный ключ, генерируется базой |
| `Title` | `string` | Обязательное название, до 160 символов |
| `Author` | `string` | Обязательное имя автора, до 120 символов |
| `Year` | `int` | Год публикации, допустимый диапазон задан моделью |
| `Price` | `decimal` | Цена; точность задаётся конфигурацией модели |
| `Description` | `string` | Описание книги |

## Endpoint

| Метод | Endpoint | Назначение | Успех / основные ошибки |
|---|---|---|---|
| `GET` | `/api/books` | Получить все книги | `200 OK` |
| `GET` | `/api/books/{id}` | Получить книгу по Id | `200 OK`, `404 Not Found` |
| `POST` | `/api/books` | Создать книгу | `201 Created`, `400 Bad Request` |
| `PUT` | `/api/books/{id}` | Изменить книгу | `200 OK`, `404 Not Found` |
| `DELETE` | `/api/books/{id}` | Удалить книгу | `200 OK`, `404 Not Found` |
| `GET` | `/api/books/search?author=Bradbury` | Поиск по автору | `200 OK`; пустой параметр — `400 Bad Request` |

## Пример JSON

```json
{
  "title": "Fahrenheit 451",
  "author": "Ray Bradbury",
  "year": 1953,
  "price": 4500,
  "description": "Роман-антиутопия"
}
```

## Проверка и скриншоты

### Шаг 1. Список endpoint

![Swagger UI со списком endpoint](docs/swagger-endpoints.png)

### Шаг 2. Список книг — `GET /api/books`

![GET списка книг — 200 OK](docs/get-all.png)

### Шаг 3. Создание книги — `POST /api/books`

![POST: книга создана — 201 Created](docs/post-create.png)

### Шаг 4. Чтение созданной книги — `GET /api/books/{id}`

![GET по ID — 200 OK](docs/get-by-id.png)

### Шаг 5. Изменение — `PUT /api/books/{id}`

![PUT: данные книги изменены — 200 OK](docs/put-update.png)

### Шаг 6. Поиск — `GET /api/books/search?author=Bradbury`

![Результат поиска по автору Bradbury](docs/search-author.png)

### Шаг 7. Удаление — `DELETE /api/books/{id}`

![DELETE: книга удалена](docs/delete-book.png)

### Шаг 8. Проверка отсутствующей книги

![Повторный GET удалённой книги — 404 Not Found](docs/get-after-delete-404.png)

### Журнал EF Core

![PowerShell с журналом выполнения запросов к базе](docs/powershell-logs.png)

## Вывод

Реализована цепочка `Controller → Repository → EF Core → SQLite` и преобразование `Entity ↔ AutoMapper ↔ DTO`. Через Swagger проверены CRUD и дополнительный поиск по автору.

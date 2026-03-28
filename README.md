# 🛒 Online Store API

Проект представляет собой backend интернет-магазина с ролевой системой и уведомлениями через Telegram.

---

## 📌 Основной функционал

Система построена без корзины. Покупка происходит напрямую через создание заказа.

---

## 👥 Роли пользователей

### 🔑 Админ (Admin)
- Создается автоматически при первой регистрации
  ```
  {
      "username": "Admin",
      "passwrod": "123456"
  }
  ```
- Может:
  - Создавать продавцов
  - Управлять системой

---

### 🧑‍💼 Продавец (Seller)
- Создается только админом
- Может:
  - Просматривать заказы
  - Принимать заказ ✅
  - Отклонять заказ ❌

---

### 👤 Клиент (Client)
- Регистрируется самостоятельно
- По умолчанию получает роль клиента
- Может:
  - Просматривать товары
  - Создавать заказ

---

## 🛍️ Процесс покупки

1. Клиент выбирает товар
2. Указывает:
   - 📞 Номер телефона
   - 🎨 Цвет
   - 📏 Размер
3. Создает заказ

---

## 📩 Уведомления

После создания заказа:

- В Telegram отправляется сообщение:
  - Новый заказ
  - Номер телефона клиента

---

## 🔄 Обработка заказа

Продавец может:
- ✅ Принять заказ
- ❌ Отменить заказ

---

## ⚙️ Технологии

- ASP.NET Core
- Entity Framework Core
- PostgreSQL / SQL Server
- Telegram Bot API
- JWT Authentication
- REDIS CACHE
- Backgroud Task Hangfire

---

## 🔐 Конфигурация

В `appsettings.json`:
```json
"EmailOptions": {
    "Username": "YOUR_MAIL@gmail.com",
    "Password": "MAIL_PASSWORD",
    "Host": "smtp.gmail.com",
    "Port": 587,
    "From": "YOUR_MAIL@gmail.com"
  },
  "TelegramSettings": {
    "TelegramToken": "YOUR_TOKEN_TELEGRAMBOT",
    "TelegramChatId": "YOUR_CHAT_ID"
  }
```

---

# 🚀 Запуск проекта

## 📦 Вариант 1 — через Docker (рекомендуется)

### 1. Клонировать репозиторий

```bash
git clone <your-repo-url>
cd MarketPlace
```

---

### 2. Создать `.env` файл (если используется)

```env
POSTGRES_DB=Quality
POSTGRES_USER=postgres
POSTGRES_PASSWORD=1234
REDIS_CONNECTION=redis:6379
```

---

### 3. Запустить проект

```bash
docker-compose up --build
```

---

### 4. Открыть Swagger

```
http://localhost:8080/swagger
```
---
## 💻 Вариант 2 — запуск без Docker

### 1. Настроить `appsettings.json`

Указать:

* строку подключения к PostgreSQL
* Redis
* JWT
* Email / Telegram (если используются)

---
### 3. Запустить проект

```bash
dotnet run --project WebApp
```

---

## 🧠 Требования

* .NET 8/9 SDK
* Docker + Docker Compose
* PostgreSQL (если без Docker)
* Redis (если используется кеш)
---
## ⚠️ Важно

* Не храните секреты в `appsettings.json`
* Используйте `.env` или переменные окружения
* Убедитесь, что порты 8080, 5432, 6379 свободны

---

## 📌 Особенности

- ❌ Нет корзины (упрощенная логика)
- 📲 Интеграция с Telegram
- 🔐 Ролевая система
- ⚡ Быстрое оформление заказа

 ## 📬 Будущие улучшения
 
- Добавить корзину
- История заказов ✅
- Онлайн оплата 
- Фильтрация товаров ✅


# 👨‍💻 Автор

- Juraev Muhammadjon

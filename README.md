# GoldCube — Provably Fair Roulette

[![.NET](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-blue.svg)](https://www.docker.com/)
[![VK Bot](https://img.shields.io/badge/VK-Bot-green.svg)](https://vk.com/)

**Бот для рулетки в беседах ВКонтакте** с полностью проверяемой честностью (Provably Fair).

## ✨ Возможности

- 🎰 Полноценная рулетка (0–36)
- Ставки: Красное, Чёрное, Чётное, Нечётное, На число
- Provably Fair (SHA-256 + секретное слово)
- Независимые раунды в каждой беседе
- Реал-тайм банк ставок
- Автоматический розыгрыш
- Сохранение баланса игроков

## 🛠 Технологии

- **.NET 9**
- Entity Framework Core + PostgreSQL
- VK Long Poll API
- Docker + Docker Compose

## 🚀 Быстрый запуск

### Через Docker (рекомендуется)

Создай `.env` в корне проекта (рядом с `docker-compose.yml`):

```env
POSTGRES_DB=goldcube
POSTGRES_USER=postgres
POSTGRES_PASSWORD=your_password
VKApiKey=your_vk_group_token
```

```bash
git clone https://github.com/Romanana281/GoldCube.git
cd GoldCube
docker compose up -d --build
```

Бот ждёт готовности PostgreSQL, применяет миграции и только потом запускает Long Poll.

Локальный запуск
```bash
dotnet restore
dotnet ef database update
dotnet user-secrets set "VK:ApiKey" "vk1.a.xxxxxxxxxxxxxxxxxxxxxxxxx"
dotnet run
```

⚙️ Настройка

- Создай группу в ВКонтакте
- Получи токен сообщества с правами messages
- Добавь бота в беседу как администратора
- Настрой токен через User Secrets или .env

🎮 Как играть

- Добавь бота в беседу
- Напиши /wheel или нажми кнопку «Найти беседу»
- Делай ставки через меню
- Команда «Банк» — показывает все ставки в текущем раунде
- После розыгрыша увидишь результаты + хеш для проверки

Статус: Пет-проект в разработке

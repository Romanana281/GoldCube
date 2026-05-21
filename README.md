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

```bash
git clone https://github.com/Romanana281/GoldCube.git
cd GoldCube
docker-compose up -d
```

Локальный запуск
```bash
dotnet restore
dotnet ef database update
dotnet user-secrets set "VK:ApiKey" "vk1.a.xxxxxxxxxxxxxxxxxxxxxxxxx"
dotnet run
```

⚙️ Настройка

Создай группу в ВКонтакте
Получи токен сообщества с правами messages
Добавь бота в беседу как администратора
Настрой токен через User Secrets или .env

🎮 Как играть

Добавь бота в беседу
Напиши /wheel или нажми кнопку «Найти беседу»
Делай ставки через меню
Команда «Банк» — показывает все ставки в текущем раунде
После розыгрыша увидишь результаты + хеш для проверки

Провably Fair
Каждый раунд генерируется криптографически стойко.
После игры публикуется:

Выпавшее число
Секретное слово
SHA-256 хеш

Проверить можно командой: SHA256(число|секретное_слово)

Структура проекта
textGoldCube/
├── Database/           # DbContext и миграции
├── Models/             # Модели данных
├── Service/            # Игровая логика (Roulette, EndGame)
├── Vk/                 # VK API, команды, клавиатуры
├── Configurations/     # Настройки
├── Dockerfile
├── docker-compose.yml
├── .env.example
└── README.md

TODO

 Переход на полноценный DI + IHostedService
 Режим Dice
 Админ-команды
 Логирование
 Защита от флуда


Статус: Пет-проект в разработке

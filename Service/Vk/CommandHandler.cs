using System.Text.Json;

namespace GoldCube
{
    public class CommandHandler
    {
        private readonly UserService _userService;
        private readonly VkApiService _vk;

        public CommandHandler(UserService userService, VkApiService vkApi)
        {
            _userService = userService;
            _vk = vkApi;
        }

        public async Task Handle(JsonElement message)
        {
            Console.WriteLine(message);
            var _userId = message.GetProperty("from_id").GetInt64();
            var _message = message.GetProperty("text").GetString();
            var _peerId = message.GetProperty("peer_id").GetInt64();

            string? _command = null;
            string? _type = null;

            if (message.TryGetProperty("payload", out var payloadElement))
            {
                string? rawJson = payloadElement.GetString();

                if (!string.IsNullOrEmpty(rawJson))
                {
                    using var payload = JsonDocument.Parse(rawJson);

                    if (payload.RootElement.TryGetProperty("command", out var cmd))
                    {
                        _command = cmd.GetString();
                    }

                    if (payload.RootElement.TryGetProperty("type", out var t))
                    {
                        _type = t.GetString();
                    }
                }
            }
            using var db = new ApplicationContext();
            var user = await _userService.GetUser(db, _userId);

            var text = _message?.ToLower().Trim();

            if (user.State == UserState.WaitBetNumber)
            {
                if (int.TryParse(text, out var number))
                {
                    if (number < 0 || number > 36)
                    {
                        await _vk.SendMessage(new Message
                        {
                            UserId = _peerId,
                            Text = "Число должно быть от 0 до 36."
                        });

                        return;
                    }

                    user.State = UserState.WaitBetAmount;
                    user.PendingNumber = number;
                    user.PendingBetType = BetType.Number;
                    user.PeerId = _peerId;
                    await db.SaveChangesAsync();

                    await _vk.SendMessage(new Message
                    {
                        UserId = _peerId,
                        Text = $"{user.Name}, введи ставку на {BetType.Number.ToRussia()} {number}:"
                    });

                    return;
                }
                else
                {
                    await _vk.SendMessage(new Message
                    {
                        UserId = _peerId,
                        Text = "Некорректно указано число!"
                    });

                    user.State = UserState.None;

                    await db.SaveChangesAsync();
                    return;
                }
            }

            if (user.State == UserState.WaitBetAmount)
            {
                if (int.TryParse(text, out var amount) && amount > 0)
                {
                    if (user.PendingBetType == null)
                    {
                        user.State = UserState.None;
                        await db.SaveChangesAsync();
                        return;
                    }

                    if (amount > user.Balance)
                    {

                        await _vk.SendMessage(new Message
                        {
                            UserId = _peerId,
                            Text = "Недостаточно коинов на балансе!"
                        });

                        user.State = UserState.None;

                        await db.SaveChangesAsync();
                        return;
                    }

                    var peerId = user.PeerId ?? _peerId;
                    string betTypeRu = user.PendingBetType.Value.ToRussia();

                    await _vk.SendMessage(new Message
                    {
                        UserId = peerId,
                        Text = $"{user.Name}, ставка {amount:N0} на {(betTypeRu == "число" ? $"{betTypeRu} {user.PendingNumber}" : betTypeRu)} принята!"
                    });

                    var game = await _userService.GetOrCreateGame(db, peerId);

                    var existing = game.Bank.FirstOrDefault(x => x.UserId == user.Id && x.Type == user.PendingBetType && x.Number == user.PendingNumber);
                    if (existing != null)
                    {
                        existing.Amount += amount;
                    }
                    else
                    {
                        game.Bank.Add(new TemplateBank
                        {
                            UserId = user.Id,
                            User = user,
                            GameId = game.ConvId,
                            Amount = amount,
                            Type = user.PendingBetType.Value,
                            Number = user.PendingNumber
                        });
                    }

                    user.Balance -= amount;
                    user.State = UserState.None;
                    user.PendingBetType = null;
                    user.PendingNumber = null;
                    await db.SaveChangesAsync();
                    return;
                }
                else
                {
                    await _vk.SendMessage(new Message
                    {
                        UserId = _peerId,
                        Text = "Некорректно указана сумма!"
                    });

                    user.State = UserState.None;

                    await db.SaveChangesAsync();
                    return;
                }
            }

            if (text == "start" || text == "начать" || _command == "menu")
            {
                await _vk.SendMessage(new Message
                {
                    UserId = _userId,
                    Text = $"Привет, {user.Name}!",
                    Keyboard = TemplateKeyboards.Menu()
                });
                return;
            }

            if (_command == "FindConv")
            {
                await _vk.SendMessage(new Message
                {
                    UserId = _userId,
                    Text = "Выбирай режим и приступай к игре 👇",
                    Keyboard = TemplateKeyboards.ConvUrl()
                });
                return;
            }

            if (_command == "Balance")
            {
                await _vk.SendMessage(new Message
                {
                    UserId = _peerId,
                    Text = $"{user.Name}, у тебя: {user.Balance:N0}",
                });
                return;
            }

            if (_command == "Bet")
            {
                if (_type == "Number")
                {
                    user.State = UserState.WaitBetNumber;
                    user.PeerId = _peerId;
                    await db.SaveChangesAsync();

                    await _vk.SendMessage(new Message
                    {
                        UserId = _peerId,
                        Text = $"{user.Name}, на какое число ты ставишь? (от 0 до 36)",
                    });

                    return;
                }

                if (Enum.TryParse<BetType>(_type, out var betType))
                {
                    user.State = UserState.WaitBetAmount;
                    user.PendingNumber = null;
                    user.PendingBetType = betType;
                    user.PeerId = _peerId;
                    await db.SaveChangesAsync();

                    await _vk.SendMessage(new Message
                    {
                        UserId = _peerId,
                        Text = $"{user.Name}, введи ставку на {betType.ToRussia()}:"
                    });

                    return;
                }
            }

            if (_command == "Bank")
            {
                var game = await _userService.GetOrCreateGame(db, _peerId);

                if (game.Bank.Count == 0)
                {
                    await _vk.SendMessage(new Message
                    {
                        UserId = _peerId,
                        Text = "В этом раунде ещё никто не поставил.\n\n" +
                                $"Хэш игры (sha256): {game.Hash}\n\n" +
                                "Таймер запустится после первой ставки."
                    });
                }
                else
                {
                    var Text = $"Всего поставлено {game.Bank.Sum(x => x.Amount):N0}";
                    var BankText = game.Bank
                        .GroupBy(x => x.Type)
                        .Select(g =>
                        {
                            var title = g.Key switch
                            {
                                BetType.Red => "Ставки на красное",
                                BetType.Black => "Ставки на черное",
                                BetType.Even => "Ставки на четное",
                                BetType.Odd => "Ставки на нечетное",
                                BetType.Number => "Ставки на числа",
                                _ => "Неизвестно"
                            };

                            var lines = string.Join("\n", g.Select(x => x.FormatBet()));

                            return $"{title}:\n{lines}";
                        });

                    var timerText = game.EndAt == DateTime.MinValue
                        ? "Таймер запустится после первой ставки."
                        : FormatRoundTimer(game.EndAt);

                    var Sec_Time = $"\n\nХэш игры (sha256): {game.Hash}\n\n{timerText}";
                    await _vk.SendMessage(new Message
                    {
                        UserId = _peerId,
                        Text = $"{Text}\n\n{string.Join("\n\n", BankText)}{Sec_Time}"
                    });
                }

                return;
            }

            if (_message == "/wheel")
            {
                if (user.IsAdmin)
                {
                    await _userService.GetOrCreateGame(db, _peerId);

                    await _vk.SendMessage(new Message
                    {
                        UserId = _peerId,
                        Text = "Режим Wheel активирован!",
                        Keyboard = TemplateKeyboards.WheelMenu()
                    });

                    return;
                }
            }
        }

        private static string FormatRoundTimer(DateTime endAt)
        {
            var remaining = endAt - DateTime.UtcNow;
            if (remaining <= TimeSpan.Zero)
                return "Раунд завершается...";

            return $"До конца раунда: {remaining.Minutes}:{remaining.Seconds:D2}";
        }
    }
}
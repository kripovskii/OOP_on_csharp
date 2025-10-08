# Создайте систему уведомлений.

1. Определите интерфейс:

```csharp
public interface INotifier
```

2. Реализуйте несколько классов:

* `EmailNotifier` — отправляет сообщение по email.
* `SmsNotifier` — отправляет сообщение по SMS.
* (дополнительно) `TelegramNotifier` — отправляет сообщение в Telegram.

3. В `Main` создайте список уведомителей и отправьте одно и то же сообщение через все каналы:

```csharp
List<INotifier> notifiers = new List<INotifier>
{
    new EmailNotifier(),
    new SmsNotifier(),
    new TelegramNotifier()
};

foreach (var notifier in notifiers)
{
    notifier.Send("Интерфейсы — это круто!");
}
```


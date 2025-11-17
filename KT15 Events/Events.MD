# Делегаты в C# — **основа событий**

**Делегаты** — это **типобезопасные указатели на методы**.  
Они — фундаментальная часть событий, лямбда-выражений, LINQ, асинхронности и MVVM.

---

## 1. **Что такое делегат?**

> **Делегат** — это **объект**, который хранит **ссылку на метод** (или несколько методов) с определённой сигнатурой.

```csharp
// Объявление делегата
public delegate void MyAction(string message);
```

Это как **контракт**: любой метод, подходящий под сигнатуру, можно "вставить" в делегат.

---

## 2. **Простейший пример**

```csharp
public class Printer
{
    public void Print(string text) => Console.WriteLine("Печать: " + text);
}

class Program
{
    // 1. Объявляем делегат
    public delegate void PrintDelegate(string msg);

    static void Main()
    {
        Printer p = new Printer();

        // 2. Создаём экземпляр делегата
        PrintDelegate del = p.Print;

        // 3. Вызываем через делегат
        del("Привет от делегата!"); 
        // → Печать: Привет от делегата!
    }
}
```

---

## 3. **Встроенные делегаты .NET (не нужно объявлять!)**

| Делегат | Сигнатура | Когда использовать |
|--------|----------|-------------------|
| `Action` | `void ()` | Без параметров |
| `Action<T>` | `void (T)` | 1 параметр |
| `Action<T1,T2,...>` | `void (T1,T2,...)` | до 16 параметров |
| `Func<TResult>` | `TResult ()` | Возвращает значение |
| `Func<T, TResult>` | `TResult (T)` | 1 вход → выход |
| `Predicate<T>` | `bool (T)` | Логическое условие |

### Пример с `Action` и `Func`
```csharp
Action<string> say = msg => Console.WriteLine(msg);
say("Привет!"); // → Привет!

Func<int, int, int> sum = (a, b) => a + b;
Console.WriteLine(sum(3, 4)); // → 7
```

---

## 4. **Мультикаст: один делегат — много методов**

Делегат может ссылаться на **несколько методов**:

```csharp
Action<string> actions = null;

actions += msg => Console.WriteLine("1: " + msg);
actions += msg => Console.WriteLine("2: " + msg.ToUpper());

actions("test");
// 1: test
// 2: TEST
```

> Все методы вызываются **по порядку добавления**.

---

## 5. **Анонимные методы и лямбды**

### Анонимный метод (C# 2.0)
```csharp
del += delegate(string s) { Console.WriteLine(s); };
```

### Лямбда (C# 3.0+)
```csharp
del += s => Console.WriteLine(s);           // коротко
del += (string s) => { Console.WriteLine(s); }; // явно
```

---

## 6. **Делегаты — основа событий**

```csharp
public class Button
{
    // Событие — это приватное поле типа делегата
    private EventHandler _click;

    // public event = специальный аксессор
    public event EventHandler Click
    {
        add    => _click += value;     // подписка
        remove => _click -= value;     // отписка
    }

    public void SimulateClick()
    {
        _click?.Invoke(this, EventArgs.Empty);
    }
}
```

> `event` — это **защищённый делегат**:  
> - Подписаться/отписаться можно извне  
> - **Вызвать** (`Invoke`) — **только внутри класса**

---

## 7. **Сравнение: делегат vs событие**

| | Делегат | Событие (`event`) |
|---|--------|------------------|
| Можно вызвать извне? | Да | Нет |
| Можно перезаписать? | Да (`del = null`) | Нет |
| Безопасность | Низкая | Высокая |
| Использование | Колбэки, LINQ | Уведомления |

```csharp
// Делегат — можно перезаписать!
del = null; // опасно!

// Событие — нельзя!
Click = null; // ОШИБКА! Только += / -=
```

---

## 8. **Делегаты в WPF и MVVM**

### Команды используют делегаты!
```csharp
public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;  // делегат!
    private readonly Func<object, bool> _canExecute;

    public RelayCommand(Action<object> execute, 
                        Func<object, bool> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged; // событие!

    public bool CanExecute(object parameter) 
        => _canExecute?.Invoke(parameter) ?? true;

    public void Execute(object parameter) 
        => _execute(parameter);
}
```

---

## 9. **Полный пример: кнопка с делегатом и событием**

```csharp
public class Notifier
{
    // Делегат
    public Action<string> OnNotify;

    // Событие (на основе делегата)
    public event EventHandler<string> Notify;

    public void Send(string msg)
    {
        OnNotify?.Invoke(msg);           // делегат
        Notify?.Invoke(this, msg);       // событие
    }
}

// Использование
var n = new Notifier();

n.OnNotify = msg => Console.WriteLine("Делегат: " + msg);
n.Notify += (s, msg) => Console.WriteLine("Событие: " + msg);

n.Send("Всем привет!");
// Делегат: Всем привет!
// Событие: Всем привет!
```

---

## 10. **Ключевые моменты**

| Правило | Объяснение |
|--------|-----------|
| `?.Invoke()` | Всегда проверяй на `null` |
| `event` = безопасный делегат | Только владелец вызывает |
| `Action` / `Func` | Используй встроенные делегаты |
| Отписывайся! | `-=`, иначе утечки памяти |
| Делегаты — основа LINQ, async, событий | Всё строится на них |

---

## Мини-таблица: делегаты → события

| Уровень | Что это | Пример |
|--------|--------|--------|
| 1 | Делегат | `Action<string>` |
| 2 | Событие | `event EventHandler Click` |
| 3 | WPF Routed Event | `Button.ClickEvent` |
| 4 | MVVM Команда | `ICommand.Execute` |

# События в C# и WPF

**События** (events) — это механизм **уведомления** о том, что в объекте произошло что-то важное.  
Они реализуют паттерн **"издатель — подписчик"** (Publisher-Subscriber).

---

## 1. **Что такое событие?**

Событие — это **объект-делегат**, который хранит список методов-обработчиков.  
Когда событие **возбуждается** (`Raise`), вызываются все подписанные методы.

```csharp
public event EventHandler Click;  // объявление события
```

---

## 2. **Базовый синтаксис (классические события)**

### Объявление
```csharp
public class Button
{
    // 1. Делегат (можно опустить, используя EventHandler)
    public delegate void ClickEventHandler(object sender, EventArgs e);

    // 2. Событие
    public event ClickEventHandler Click;
    
    // 3. Метод, вызывающий событие
    protected virtual void OnClick()
    {
        Click?.Invoke(this, EventArgs.Empty);
        // или: Click?.(this, EventArgs.Empty); в C# 13+
    }

    public void SimulateClick()
    {
        OnClick(); // возбуждаем событие
    }
}
```

### Подписка и обработка
```csharp
var btn = new Button();

btn.Click += Btn_Click;           // подписка
btn.Click += (s, e) => Console.WriteLine("Лямбда!"); // анонимная

void Btn_Click(object sender, EventArgs e)
{
    Console.WriteLine("Кнопка нажата!");
}

btn.SimulateClick();
```

### Отписка (важно!)
```csharp
btn.Click -= Btn_Click; // предотвращает утечки памяти
```

---

## 3. **Стандартные делегаты .NET**

| Делегат | Когда использовать |
|--------|-------------------|
| `EventHandler` | Нет данных (`void OnEvent(object s, EventArgs e)`) |
| `EventHandler<T>` | С данными (`EventHandler<ValueChangedEventArgs>`) |

```csharp
public event EventHandler<ValueChangedEventArgs> ValueChanged;

public class ValueChangedEventArgs : EventArgs
{
    public int OldValue { get; }
    public int NewValue { get; }
    public ValueChangedEventArgs(int oldVal, int newVal) => (OldValue, NewValue) = (oldVal, newVal);
}
```

---

## 4. **События в WPF: Routed Events**

WPF расширяет события до **маршрутизируемых** — они "путешествуют" по дереву элементов.

### Типы маршрутизации
| Тип | Префикс | Направление |
|-----|--------|-----------|
| **Bubbling** | — | Вверх (от ребёнка к родителю) |
| **Tunneling** | `Preview` | Вниз (от родителя к ребёнку) |
| **Direct** | — | Только у источника |

```xml
<Button Click="Button_Click" PreviewMouseDown="Button_PreviewDown" />
```

```csharp
private void Button_Click(object sender, RoutedEventArgs e)
{
    Console.WriteLine("Bubbling: Click");
    e.Handled = true; // остановить дальнейшую маршрутизацию
}

private void Button_PreviewDown(object sender, MouseButtonEventArgs e)
{
    Console.WriteLine("Tunneling: PreviewMouseDown");
}
```

> `Preview...` срабатывает **раньше** основного события.

---

## 5. **Регистрация своего Routed Event**

```csharp
public class MyControl : Control
{
    public static readonly RoutedEvent ValueChangedEvent = 
        EventManager.RegisterRoutedEvent(
            "ValueChanged", 
            RoutingStrategy.Bubble, 
            typeof(RoutedEventHandler), 
            typeof(MyControl));

    // CLR-обёртка
    public event RoutedEventHandler ValueChanged
    {
        add => AddHandler(ValueChangedEvent, value);
        remove => RemoveHandler(ValueChangedEvent, value);
    }

    private int _value;
    public int Value
    {
        get => _value;
        set
        {
            if (_value != value)
            {
                var old = _value;
                _value = value;
                RaiseValueChanged(old, value);
            }
        }
    }

    private void RaiseValueChanged(int oldVal, int newVal)
    {
        var args = new RoutedEventArgs(ValueChangedEvent);
        RaiseEvent(args);
    }
}
```

Использование в XAML:
```xml
<local:MyControl ValueChanged="MyControl_ValueChanged" />
```

---

## 6. **События vs Команды в MVVM**

| | События | Команды |
|---|--------|--------|
| Где обрабатывать? | Code-behind | ViewModel |
| Тестируемость | Плохо | Отлично |
| Повторное использование | Нет | Да |

### Вместо события — команда:
```xml
<Button Command="{Binding SaveCommand}" Content="Сохранить" />
```

```csharp
public class MainViewModel : INotifyPropertyChanged
{
    public ICommand SaveCommand { get; }

    public MainViewModel()
    {
        SaveCommand = new RelayCommand(Save);
    }

    private void Save() => MessageBox.Show("Сохранено!");
}
```

> Используйте **поведения** (`Interaction.Triggers`) для событий, которые нельзя заменить командой.

---

## 7. **Жизненный цикл событий в WPF**

```csharp
Loaded → Initialized → (ContentRendered) → ... → Unloaded
```

```csharp
public MainWindow()
{
    InitializeComponent();
    Loaded += (s, e) => Console.WriteLine("UI готов");
    Unloaded += (s, e) => Console.WriteLine("Окно закрывается");
}
```

---

## 8. **Лучшие практики**

| Практика | Почему |
|--------|-------|
| `?.Invoke()` | Защита от `null` |
| `e.Handled = true` | Остановка нежелательной обработки |
| Отписывайтесь | Избежать утечек памяти |
| Используйте `EventHandler<T>` | Типобезопасность |
| В MVVM — **команды**, не события | Чистая архитектура |

---

## Мини-пример: счётчик с событием

```csharp
public class Counter
{
    public event EventHandler<int> ThresholdReached;

    private int _value;
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            if (_value >= 10)
                ThresholdReached?.Invoke(this, _value);
        }
    }
}

// Использование
var c = new Counter();
c.ThresholdReached += (s, val) => Console.WriteLine($"Порог! {val}");
c.Value = 15; // → "Порог! 15"
```

### Обработка событий в WPF

В WPF (Windows Presentation Foundation) обработка событий построена на **маршрутизируемых событиях** (routed events). Это позволяет событию "путешествовать" по визуальному дереву: вверх (tunneling), вниз (bubbling) или напрямую (direct). Основные преимущества:

- Централизованная обработка (например, в родительском элементе).
- Поддержка команд, MVVM и стилей.
- Автоматическая работа с составными контролами.

---

#### 1. **Типы маршрутизации событий**

| Тип | Направление | Префикс | Пример |
|-----|-------------|--------|--------|
| **Bubbling** | Вверх по дереву (от источника к корню) | — | `Button.Click` |
| **Tunneling** | Вниз по дереву (от корня к источнику) | `Preview` | `PreviewMouseDown` |
| **Direct** | Только у источника | — | `MouseEnter` |

> **Примечание**: `Preview...` события срабатывают **до** основных (bubbling).

---

#### 2. **Подписка на события**

##### В XAML:
```xml
<Button Content="Нажми" 
        Click="Button_Click"
        PreviewMouseDown="Button_PreviewMouseDown" />
```

##### В коде (code-behind):
```csharp
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        myButton.Click += MyButton_Click;
        myButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(MyButton_Click));
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Click (bubbling)");
    }

    private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        MessageBox.Show("PreviewMouseDown (tunneling)");
    }
}
```

---

#### 3. **Остановка маршрутизации**

```csharp
private void Button_Click(object sender, RoutedEventArgs e)
{
    MessageBox.Show("Обработано в кнопке");
    e.Handled = true; // Событие больше не всплывает
}
```

> Если `e.Handled = true` в `Preview...`, то основное событие **не сработает**.

---

#### 4. **Обработка на уровне родителя**

```xml
<StackPanel Button.Click="Panel_ButtonClick">
    <Button Content="1" />
    <Button Content="2" />
</StackPanel>
```

```csharp
private void Panel_ButtonClick(object sender, RoutedEventArgs e)
{
    if (e.OriginalSource is Button btn)
    {
        MessageBox.Show($"Нажата кнопка: {btn.Content}");
    }
}
```

---

#### 5. **Пример: обработка в UserControl**

```xml
<UserControl x:Class="MyApp.MyControl"
             MouseDown="MyControl_MouseDown">
    <Grid>
        <TextBox Text="Внутри UserControl" />
    </Grid>
</UserControl>
```

```csharp
private void MyControl_MouseDown(object sender, MouseButtonEventArgs e)
{
    if (e.OriginalSource is TextBox)
    {
        MessageBox.Show("Клик по TextBox внутри UserControl");
        e.Handled = true; // Блокируем всплытие
    }
}
```

---

#### 6. **События в MVVM (без code-behind)**

Используйте **поведения (behaviors)** или **триггеры**:

```xml
<Window ...
    xmlns:i="http://schemas.microsoft.com/xaml/behaviors">
    
    <Button Content="Сохранить">
        <i:Interaction.Triggers>
            <i:EventTrigger EventName="Click">
                <i:InvokeCommandAction Command="{Binding SaveCommand}" />
            </i:EventTrigger>
        </i:Interaction.Triggers>
    </Button>
</Window>
```

> Требуется пакет: `Microsoft.Xaml.Behaviors.Wpf`

---

#### 7. **Полезные встроенные события**

| Событие | Описание |
|--------|---------|
| `Loaded` / `Unloaded` | Загрузка/выгрузка элемента |
| `SizeChanged` | Изменение размера |
| `MouseLeftButtonDown` | Клик левой кнопкой |
| `KeyDown` | Нажатие клавиши |
| `TextChanged` | Изменение текста |

---

#### 8. **Создание своего маршрутизируемого события**

```csharp
public static readonly RoutedEvent ValueChangedEvent = 
    EventManager.RegisterRoutedEvent(
        "ValueChanged", RoutingStrategy.Bubble, 
        typeof(RoutedEventHandler), typeof(MyControl));

public event RoutedEventHandler ValueChanged
{
    add { AddHandler(ValueChangedEvent, value); }
    remove { RemoveHandler(ValueChangedEvent, value); }
}

private void OnValueChanged()
{
    RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
}
```

---

### Рекомендации

- Используйте `Preview...` для **предобработки** (например, валидация).
- Обрабатывайте события на **высоком уровне** (Window, UserControl) для логики приложения.
- В MVVM — **избегайте code-behind**, используйте команды и поведения.
- Всегда проверяйте `e.OriginalSource`, если обрабатываете на родителе.

---

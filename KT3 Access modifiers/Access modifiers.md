# Модификаторы доступа в C#

## Оглавление
1. [Введение](#введение)
2. [Основные модификаторы доступа](#основные-модификаторы-доступа)
   - [private](#private)
   - [protected](#protected)
   - [internal](#internal)
   - [protected internal](#protected-internal)
   - [private protected](#private-protected)
   - [public](#public)
3. [Уровни доступа](#уровни-доступа)
4. [Правила использования](#правила-использования)
5. [Примеры кода](#примеры-кода)
6. [Лучшие практики](#лучшие-практики)
7. [Заключение](#заключение)

## Введение

Модификаторы доступа в C# — это ключевые слова, которые определяют **уровень видимости** и **доступности** типов и их членов. Они являются фундаментальным аспектом инкапсуляции — одного из трех основных принципов объектно-ориентированного программирования.

**Цель модификаторов доступа:**
- Контролировать доступ к данным класса
- Предотвращать неправильное использование компонентов
- Обеспечивать безопасность данных
- Упрощать поддержку кода

## Основные модификаторы доступа

### private

**Самый ограничительный** уровень доступа. Члены, объявленные как `private`, доступны только внутри того класса или структуры, где они объявлены.

```csharp
public class BankAccount
{
    private decimal balance; // Доступен только внутри BankAccount

    private void CalculateInterest() // Приватный метод
    {
        // Логика расчета процентов
    }
}
```

### protected

Члены доступны внутри своего класса и в **производных классах** (наследниках).

```csharp
public class Vehicle
{
    protected string engineType; // Доступен в Vehicle и наследниках

    protected virtual void StartEngine() // Защищенный виртуальный метод
    {
        Console.WriteLine("Engine started");
    }
}

public class Car : Vehicle
{
    public void TestAccess()
    {
        engineType = "V8"; // Доступно, так как Car наследует Vehicle
        StartEngine();     // Также доступно
    }
}
```

### internal

Члены доступны в любой части **текущей сборки**, но не из других сборок.

```csharp
internal class Logger // Класс доступен только в текущей сборке
{
    internal void LogMessage(string message) // Метод с internal доступом
    {
        Console.WriteLine(message);
    }
}
```

### protected internal

Комбинация `protected` и `internal`. Члены доступны:
- В текущей сборке
- В производных классах, даже если они в других сборках

```csharp
public class DataAccess
{
    protected internal string ConnectionString { get; set; }

    protected internal virtual void Connect()
    {
        // Логика подключения
    }
}
```

### private protected

Доступно начиная с C# 7.2. Члены доступны внутри своего класса **и в производных классах, но только в той же сборке**.

```csharp
public class BaseClass
{
    private protected int secretValue; // Доступен в BaseClass и наследниках в той же сборке
}

public class DerivedClass : BaseClass
{
    public void Test()
    {
        secretValue = 42; // Доступно, если DerivedClass в той же сборке
    }
}
```

### public

**Наименее ограничительный** уровень доступа. Члены доступны из любого места.

```csharp
public class Calculator
{
    public double Add(double a, double b) // Публичный метод
    {
        return a + b;
    }

    public const double PI = 3.14159; // Публичная константа
}
```

## Уровни доступа

### Доступность по умолчанию

- **Члены класса**: `private`
- **Члены структуры**: `private`
- **Классы и структуры**: `internal`
- **Интерфейсы**: `public` (нельзя изменить)
- **Перечисления**: `public` (нельзя изменить)

### Сводная таблица доступности

| Модификатор      | Текущий класс | Производный класс (та же сборка) | Производный класс (другая сборка) | Непроизводный класс (та же сборка) | Непроизводный класс (другая сборка) |
|------------------|---------------|----------------------------------|-----------------------------------|-------------------------------------|--------------------------------------|
| `private`        | ✅            | ❌                               | ❌                                | ❌                                  | ❌                                   |
| `protected`      | ✅            | ✅                               | ✅                                | ❌                                  | ❌                                   |
| `internal`       | ✅            | ❌                               | ❌                                | ✅                                  | ❌                                   |
| `protected internal` | ✅         | ✅                               | ✅                                | ✅                                  | ❌                                   |
| `private protected` | ✅          | ✅                               | ❌                                | ❌                                  | ❌                                   |
| `public`         | ✅            | ✅                               | ✅                                | ✅                                  | ✅                                   |

## Правила использования

### Для классов и структур

```csharp
public class PublicClass { }          // Доступен везде
internal class InternalClass { }     // Доступен только в сборке
class DefaultClass { }               // internal по умолчанию

// private class NotAllowed { }      // Ошибка! Классы не могут быть private
```

### Для наследования

Уровень доступа производного класса не может быть **более открытым**, чем базовый класс.

```csharp
internal class BaseClass { }
public class DerivedClass : BaseClass { } // Ошибка! Нельзя сделать более открытым
```

### Для переопределения методов

Модификатор доступа переопределенного метода должен **совпадать** с модификатором базового метода.

```csharp
public class BaseClass
{
    protected virtual void Method() { }
}

public class DerivedClass : BaseClass
{
    protected override void Method() { } // Правильно - тот же модификатор
    // public override void Method() { } // Ошибка! Нельзя изменить на public
}
```

## Примеры кода

### Пример 1: Инкапсуляция с private

```csharp
public class Person
{
    private string name;
    private int age;

    public Person(string name, int age)
    {
        this.name = name;
        SetAge(age); // Используем метод для валидации
    }

    public string GetName() => name;

    public int GetAge() => age;

    private void SetAge(int value)
    {
        if (value < 0 || value > 150)
            throw new ArgumentException("Invalid age");
        age = value;
    }

    public void HaveBirthday()
    {
        SetAge(age + 1); // Контролируемое изменение возраста
    }
}
```

### Пример 2: Наследование с protected

```csharp
public abstract class Shape
{
    protected int x, y;

    protected Shape(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public abstract double Area { get; }

    protected virtual void Draw()
    {
        Console.WriteLine($"Drawing shape at ({x}, {y})");
    }
}

public class Circle : Shape
{
    private double radius;

    public Circle(int x, int y, double radius) : base(x, y)
    {
        this.radius = radius;
    }

    public override double Area => Math.PI * radius * radius;

    protected override void Draw()
    {
        base.Draw(); // Вызываем базовую реализацию
        Console.WriteLine($"Drawing circle with radius {radius}");
    }

    public void Display()
    {
        Draw(); // Доступно, так как Draw protected
        Console.WriteLine($"Area: {Area}");
    }
}
```

### Пример 3: internal для компонентов сборки

**Сборка 1: Utilities.dll**
```csharp
public class PublicUtility
{
    public void PublicMethod() { }
}

internal class InternalHelper
{
    internal void HelperMethod() { }
}

public class AnotherUtility
{
    public void UseHelper()
    {
        var helper = new InternalHelper(); // Доступно, так как в той же сборке
        helper.HelperMethod();
    }
}
```

**Сборка 2: Consumer.exe**
```csharp
// using Utilities;

public class Consumer
{
    public void Test()
    {
        var utility = new PublicUtility();
        utility.PublicMethod(); // Доступно

        // var helper = new InternalHelper(); // Ошибка! InternalHelper не виден
        // helper.HelperMethod();             // Ошибка!
    }
}
```

## Лучшие практики

1. **Принцип минимальных привилегий**: Используйте самый строгий возможный уровень доступа
2. **Поля делайте private**: Используйте свойства для контролируемого доступа
3. **Методы по умолчанию private**: Делайте public только то, что действительно нужно наружу
4. **internal для компонентов сборки**: Используйте для служебных классов, которые не должны быть публичными
5. **protected для расширяемости**: Предоставляйте protected доступ для методов, которые могут переопределяться
6. **Документируйте public API**: Тщательно документируйте все public члены

```csharp
// Хороший пример инкапсуляции
public class BankAccount
{
    private decimal balance;
    private string accountNumber;

    public BankAccount(string accountNumber, decimal initialBalance)
    {
        this.accountNumber = accountNumber;
        balance = initialBalance;
    }

    public decimal Balance => balance; // Только для чтения

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");

        balance += amount;
    }

    public bool Withdraw(decimal amount)
    {
        if (amount <= 0 || amount > balance)
            return false;

        balance -= amount;
        return true;
    }
}
```

## Заключение

Модификаторы доступа — мощный инструмент для создания надежного, безопасного и поддерживаемого кода. Правильное их использование позволяет:

- ✅ Контролировать доступ к данным
- ✅ Предотвращать неправильное использование
- ✅ Обеспечивать инкапсуляцию
- ✅ Создавать четкие API
- ✅ Упрощать рефакторинг и изменения

**Запомните:** Хорошая практика — начинать с самых строгих ограничений и ослаблять их только при необходимости. Это сделает ваш код более безопасным и предсказуемым.
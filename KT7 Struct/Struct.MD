# Лекция: Структуры в C#

## 1. Введение

В языке **C#** структуры (`struct`) — это особый тип данных, который позволяет объединить несколько переменных разных типов в одну логическую единицу.

Если классы чаще описывают **сложные объекты с поведением** (например, человек, автомобиль, база данных), то структуры чаще применяются для **простых объектов**, где важны данные, а не поведение (например, точка в пространстве, цвет, дата).

---

## 2. Отличие структур от классов

Хотя структуры и классы во многом похожи (в них могут быть поля, методы, конструкторы, свойства), есть ключевые различия:

| Характеристика        | Структура                                                                                       | Класс                             |
| --------------------- | ----------------------------------------------------------------------------------------------- | --------------------------------- |
| Тип                   | Значимый (value type)                                                                           | Ссылочный (reference type)        |
| Хранение              | В стеке                                                                                         | В куче                            |
| Наследование          | Не поддерживает наследование от других структур или классов (но может реализовывать интерфейсы) | Поддерживает наследование         |
| Значение по умолчанию | Автоматически инициализируется (например, поля `int` = 0)                                       | Нужно явно вызывать конструктор   |
| Использование         | Легковесные объекты, часто для хранения данных                                                  | Более "тяжёлые" объекты с логикой |

---

## 3. Пример из жизни

Представим, что нам нужно хранить **координаты точки на карте**.

Если использовать класс, то это будет «избыточно» — нам нужно всего лишь хранить две цифры (`X` и `Y`). Поэтому лучше подойдёт **структура**.

```csharp
public struct Point
{
    public int X;
    public int Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}
```

Использование:

```csharp
Point p1 = new Point(10, 20);
Console.WriteLine($"Точка находится в координатах X={p1.X}, Y={p1.Y}");
```

---

## 4. Когда использовать структуры

Использование структур оправдано, когда:

* Объект **маленький по размеру** (например, 2–3 числа).
* Объект имеет **короткий срок жизни**.
* Объект логически **неизменяемый** (например, цвет RGB).

###  Когда **НЕ** использовать:

* Если требуется **наследование**.
* Если объект **большой** (например, десятки полей).
* Если нужно часто передавать объект между методами (копирование может быть дорогим).

---

## 5. Примеры применения

### 5.1. Цвет RGB

Каждый цвет можно описать комбинацией трёх чисел (от 0 до 255).

```csharp
public struct Color
{
    public byte R;
    public byte G;
    public byte B;

    public Color(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }
}
```

Использование:

```csharp
Color red = new Color(255, 0, 0);
Console.WriteLine($"Красный цвет: R={red.R}, G={red.G}, B={red.B}");
```

---

### 5.2. Компактная дата

Можно сделать свою "мини-дату", если не хочется использовать `DateTime`.

```csharp
public struct SimpleDate
{
    public int Day;
    public int Month;
    public int Year;

    public SimpleDate(int day, int month, int year)
    {
        Day = day;
        Month = month;
        Year = year;
    }

    public override string ToString() => $"{Day:00}.{Month:00}.{Year}";
}
```

Использование:

```csharp
SimpleDate today = new SimpleDate(29, 09, 2025);
Console.WriteLine($"Сегодня: {today}");
```

---

### 5.3. Денежная сумма

```csharp
public struct Money
{
    public int Rubles;
    public int Kopecks;

    public Money(int rubles, int kopecks)
    {
        Rubles = rubles;
        Kopecks = kopecks;
    }

    public override string ToString() => $"{Rubles} руб. {Kopecks:D2} коп.";
}
```

Использование:

```csharp
Money price = new Money(150, 50);
Console.WriteLine($"Цена товара: {price}");
```

---

## 6. Важный момент: передача по значению

Так как структура — это **value type**, при присвоении или передаче в метод создаётся копия объекта.

Пример:

```csharp
Point p1 = new Point(5, 5);
Point p2 = p1; // копия
p2.X = 10;

Console.WriteLine(p1.X); // 5
Console.WriteLine(p2.X); // 10
```

У классов (reference type) оба объекта ссылались бы на одно и то же место в памяти.

---

## 7. Выводы

* **Структуры** — это удобный инструмент для представления маленьких и простых объектов.
* Они **работают быстрее** классов при коротком времени жизни и малом размере.
* Но их **не стоит использовать для больших и сложных объектов**.

Структуры хорошо подходят для:
- точек на карте,
- векторов в физике,
- цветов,
- небольших сущностей вроде денег, дат, координат.

# Пример 

```csharp
using System;

public struct Point
{
    public int X;
    public int Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    // Метод вычисления расстояния между двумя точками
    public double DistanceTo(Point other)
    {
        int dx = X - other.X;
        int dy = Y - other.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public override string ToString() => $"({X}, {Y})";
}

public struct Money
{
    public int Rubles;
    public int Kopecks;

    public Money(int rub, int kop)
    {
        Rubles = rub;
        Kopecks = kop;

        // Автоматическая нормализация (100 копеек = 1 рубль)
        if (Kopecks >= 100)
        {
            Rubles += Kopecks / 100;
            Kopecks %= 100;
        }
    }

    public override string ToString() => $"{Rubles} руб. {Kopecks:D2} коп.";
}

public struct Color
{
    public byte R;
    public byte G;
    public byte B;

    public Color(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }

    public string ToHex() => $"#{R:X2}{G:X2}{B:X2}";

    public override string ToString() => $"RGB({R}, {G}, {B})";
}

class Program
{
    static void Main()
    {
        // ===== Работа с точками =====
        Point p1 = new Point(0, 0);
        Point p2 = new Point(3, 4);

        Console.WriteLine($"Первая точка: {p1}");
        Console.WriteLine($"Вторая точка: {p2}");
        Console.WriteLine($"Расстояние между точками: {p1.DistanceTo(p2)}");

        Console.WriteLine();

        // ===== Работа с деньгами =====
        Money price = new Money(10, 150); // будет автоматически нормализовано в 11 руб. 50 коп.
        Console.WriteLine($"Цена товара: {price}");

        Console.WriteLine();

        // ===== Работа с цветами =====
        Color red = new Color(255, 0, 0);
        Console.WriteLine($"Красный цвет: {red} -> {red.ToHex()}");

        Color custom = new Color(128, 200, 50);
        Console.WriteLine($"Смешанный цвет: {custom} -> {custom.ToHex()}");
    }
}
```

---

## Вывод программы:

```
Первая точка: (0, 0)
Вторая точка: (3, 4)
Расстояние между точками: 5

Цена товара: 11 руб. 50 коп.

Красный цвет: RGB(255, 0, 0) -> #FF0000
Смешанный цвет: RGB(128, 200, 50) -> #80C832
```

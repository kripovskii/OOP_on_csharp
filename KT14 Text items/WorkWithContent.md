
# Работа с данными в компонентах WPF: Элементы управления содержимым (Content Controls)

## Введение в ContentControl

**Элементы управления содержимым** (content controls) — это группа элементов управления в WPF, которые предназначены для отображения **одного вложенного элемента**. Все они наследуются от базового класса:

```csharp
System.Windows.Controls.ContentControl : Control
```

К типичным элементам управления содержимым относятся:

- `Button`
- `Label`
- `CheckBox`
- `RadioButton`
- `ToggleButton`
- `ToolTip`
- `GroupBox`
- `TabItem`
- `Expander`
- `ScrollViewer`
- **`Window`** — главное окно приложения тоже является `ContentControl`!

---

## Ключевое свойство: `Content`

Отличительная черта всех `ContentControl` — **свойство `Content`**, которое определяет, **что именно будет отображаться внутри элемента**.

```xml
<Button Content="Нажми меня" />
```

### Особенности свойства `Content`

Свойство `Content` **типизировано как `object`**, то есть в него можно передать **любой объект**. Поведение зависит от типа объекта:

| Тип объекта | Что происходит |
|------------|----------------|
| **Не наследует `UIElement`** (например, `string`, `int`, `double`, `DateTime`, пользовательский класс) | Вызывается `ToString()` → результат отображается как текст |
| **Наследует `UIElement`** (например, `Button`, `TextBlock`, `StackPanel`, `Image`) | Вызывается `OnRender()` → элемент отрисовывается визуально |

---

## Примеры работы с `Content`

### 1. Простая строка

```xml
<Button Content="Hello World!" />
```

Эквивалентные варианты:

```xml
<!-- Явное указание свойства -->
<Button>
    <Button.Content>Hello World!</Button.Content>
</Button>

<!-- Неявное (сокращённое) определение -->
<Button>
    Hello World!
</Button>
```

---

### 2. Числовое значение → `ToString()`

**XAML:**
```xml
<Button x:Name="button1" />
```

**C# (MainWindow.xaml.cs):**
```csharp
public MainWindow()
{
    InitializeComponent();
    double d = 5.6;
    button1.Content = d; // → "5.6"
}
```

**Результат:** Кнопка с текстом `5.6`

---

### 3. Вложенный UI-элемент

```xml
<Button x:Name="button1">
    <Button Content="Вложенная кнопка" />
</Button>
```

Или через C#:

```csharp
button1.Content = new Button { Content = "Вложенная кнопка" };
```

> Метод `OnRender()` вложенной кнопки будет вызван для отрисовки.

---

### 4. Несколько элементов → используем контейнер

`ContentControl` принимает **только один** дочерний элемент. Чтобы вложить несколько — используем **панель компоновки**:

```xml
<Button x:Name="button1">
    <StackPanel>
        <TextBlock Text="Набор кнопок" Margin="0,0,0,5"/>
        <Button Content="Red" Background="Red" Height="25"/>
        <Button Content="Yellow" Background="Yellow" Height="25"/>
        <Button Content="Green" Background="Green" Height="25"/>
    </StackPanel>
</Button>
```

Через C#:

```csharp
var stackPanel = new StackPanel();

stackPanel.Children.Add(new TextBlock { Text = "Набор кнопок", Margin = new Thickness(0,0,0,5) });
stackPanel.Children.Add(new Button { Content = "Red", Background = Brushes.Red, Height = 25 });
stackPanel.Children.Add(new Button { Content = "Yellow", Background = Brushes.Yellow, Height = 25 });
stackPanel.Children.Add(new Button { Content = "Green", Background = Brushes.Green, Height = 25 });

button1.Content = stackPanel;
```

---

## Позиционирование содержимого

### Свойства выравнивания

- `HorizontalContentAlignment` — по горизонтали
- `VerticalContentAlignment` — по вертикали

Значения: `Left`, `Center`, `Right`, `Top`, `Bottom`, `Stretch`

```xml
<StackPanel Margin="10">
    <Button Content="Left"   Height="80" Width="300" Margin="5"
            HorizontalContentAlignment="Left" />
    <Button Content="Center" Height="80" Width="300" Margin="5"
            HorizontalContentAlignment="Center" />
    <Button Content="Right"  Height="80" Width="300" Margin="5"
            HorizontalContentAlignment="Right" />
</StackPanel>
```

---

### Свойство `Padding` — внутренние отступы

Задаёт отступы **внутри** элемента от границ до содержимого.

```xml
<Button Content="Hello World"
        Padding="50,30,0,40"
        HorizontalContentAlignment="Left" />
```

Формат: `left, top, right, bottom`

Если отступ одинаковый — одно число:

```xml
<Button Content="Hello World" Padding="20" />
```

> **Важно:** `Padding` работает **относительно точки выравнивания**!

Пример:

```xml
<Button Content="Center + Padding"
        HorizontalContentAlignment="Center"
        Padding="60,20,0,30" />
```

→ Отступ слева будет **от центра**, а не от края кнопки.

---

## Комбинация `ContentAlignment` + `Padding`

| Свойство | Эффект |
|--------|-------|
| `HorizontalContentAlignment="Left"` + `Padding="50,0,0,0"` | Текст отступает на 50px **от левого края** |
| `HorizontalContentAlignment="Center"` + `Padding="60,0,0,0"` | Текст центрируется, затем сдвигается на 60px **от центра влево** |

Это даёт **тонкую настройку расположения** содержимого.

---

## Сравнение с контейнерами компоновки

| Характеристика | ContentControl | Панели (Panel) |
|----------------|----------------|----------------|
| Количество дочерних элементов | **1** | **Много** |
| Свойство для дочерних | `Content` | `Children` |
| Примеры | `Button`, `Label` | `StackPanel`, `Grid` |
| Гибкость | Низкая | Высокая |

> Используйте `ContentControl` — когда нужен **один** элемент.
> Используйте `Panel` внутри `Content` — когда нужно **много**.

---

## Практические рекомендации

1. **Всегда используйте `UIElement`**, если нужна визуальная вложенность.
2. **Для сложного содержимого — оборачивайте в `StackPanel`/`Grid`**.
3. **Настраивайте `Padding` и `ContentAlignment`** для идеального позиционирования.
4. **Не забывайте**, что `Window` — тоже `ContentControl` → всё окно — это один большой `Content`!

---

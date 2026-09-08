# Меню и MenuItem в WPF

## Введение

В Windows Presentation Foundation (WPF) элементы управления `Menu` и `MenuItem` используются для создания стандартных строк меню в приложениях с графическим интерфейсом. Они позволяют организовать команды и действия пользователя в иерархическую структуру, что делает интерфейс интуитивно понятным и удобным.

---

## Элемент Menu

Элемент `Menu` представляет собой контейнер для пунктов меню. Он обычно размещается в верхней части окна и служит основой для отображения выпадающих списков команд.

### Основные свойства `Menu`:
- `Items` — коллекция дочерних элементов (`MenuItem`).
- `Background`, `Foreground` — цвета фона и текста.
- `VerticalAlignment`, `HorizontalAlignment` — выравнивание.

### Пример базового меню:

```xml
<Window x:Class="WpfApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="MainWindow" Height="350" Width="525">
    <DockPanel>
        <Menu DockPanel.Dock="Top">
            <MenuItem Header="_Файл">
                <MenuItem Header="_Открыть"/>
                <MenuItem Header="_Сохранить"/>
                <Separator/>
                <MenuItem Header="_Выход"/>
            </MenuItem>
            <MenuItem Header="_Правка">
                <MenuItem Header="_Отменить"/>
                <MenuItem Header="_Повторить"/>
            </MenuItem>
        </Menu>
        <!-- Остальной контент окна -->
    </DockPanel>
</Window>
```

> **Примечание:** Символ подчеркивания `_` перед буквой в заголовке (`Header`) задает горячую клавишу (Alt + буква).

---

## Элемент MenuItem

`MenuItem` — это отдельный пункт меню, который может содержать:
- Текст (через свойство `Header`);
- Иконку (через свойство `Icon`);
- Подменю (вложенные `MenuItem`);
- Разделители (`Separator`);
- Команды (`Command`);
- Горячие клавиши (`InputGestureText`).

### Основные свойства `MenuItem`:

| Свойство           | Описание |
|--------------------|----------|
| `Header`           | Текст пункта меню |
| `Icon`             | Иконка слева от текста |
| `IsCheckable`      | Может ли пункт быть "отмеченным" (галочка) |
| `IsChecked`        | Текущее состояние отметки |
| `Command`          | Привязка к команде (например, `ApplicationCommands.Open`) |
| `InputGestureText` | Отображаемая горячая клавиша (например, "Ctrl+O") |
| `Click`            | Событие при клике |

---

## Пример с иконками, горячими клавишами и командами

```xml
<Menu DockPanel.Dock="Top">
    <MenuItem Header="_Файл">
        <MenuItem Header="_Открыть"
                  Icon="{StaticResource OpenIcon}"
                  Command="Open"
                  InputGestureText="Ctrl+O"/>
        <MenuItem Header="_Сохранить"
                  Icon="{StaticResource SaveIcon}"
                  Command="Save"
                  InputGestureText="Ctrl+S"/>
        <Separator/>
        <MenuItem Header="_Выход"
                  Command="ApplicationCommands.Close"/>
    </MenuItem>
</Menu>
```

> **Важно:** Чтобы команды (`Command="Open"` и т.п.) работали, необходимо реализовать `ICommand` или использовать встроенные команды WPF и привязать их обработчики через `CommandBindings`.

---

## Работа с командами

WPF поддерживает модель команд, что позволяет отделить логику от интерфейса. Пример привязки команды в коде:

```csharp
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Open_Executed));
        CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Executed));
    }

    private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        MessageBox.Show("Открыть файл");
    }

    private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        MessageBox.Show("Сохранить файл");
    }
}
```

---

## Чекбоксы в меню

Пункты меню могут быть переключаемыми:

```xml
<MenuItem Header="_Автообновление"
          IsCheckable="True"
          IsChecked="True"
          Click="AutoRefresh_Click"/>
```

В обработчике можно проверять состояние:

```csharp
private void AutoRefresh_Click(object sender, RoutedEventArgs e)
{
    var item = sender as MenuItem;
    if (item.IsChecked)
    {
        // Включено
    }
    else
    {
        // Выключено
    }
}
```

---

## Разделители

Для визуального разделения групп команд используется элемент `Separator`:

```xml
<MenuItem Header="_Файл">
    <MenuItem Header="_Новый"/>
    <MenuItem Header="_Открыть"/>
    <Separator/>
    <MenuItem Header="_Выход"/>
</MenuItem>
```

---

## Локализация и динамическое меню

Меню можно создавать динамически в коде:

```csharp
var menu = new Menu();
var fileItem = new MenuItem { Header = "Файл" };
fileItem.Items.Add(new MenuItem { Header = "Открыть" });
menu.Items.Add(fileItem);
```

Также поддерживается привязка данных через `ItemsSource` и `ItemTemplate`, что полезно при построении меню на основе модели.

---

## Заключение

- `Menu` и `MenuItem` — стандартные элементы WPF для создания строк меню.
- Поддерживают иконки, горячие клавиши, команды, чекбоксы и подменю.
- Использование встроенных команд (`ApplicationCommands`) упрощает реализацию стандартных действий.
- Меню можно создавать как в XAML, так и программно.

Эти элементы являются неотъемлемой частью классического десктопного интерфейса и позволяют создавать удобные и функциональные приложения на WPF.

---

> **Совет:** В современных приложениях часто используют ленточный интерфейс (Ribbon), но для простых задач меню остаётся отличным решением благодаря своей простоте и знакомому пользователю поведению.

#  Введение в WPF

## 1. Что такое WPF?

**WPF (Windows Presentation Foundation)** — это современная графическая подсистема для построения настольных приложений под Windows, разработанная Microsoft и впервые выпущенная в 2006 году в составе .NET Framework 3.0.

WPF объединяет:
- **Декларативное описание интерфейса** через XAML,
- **Мощную систему привязки данных (Data Binding)**,
- **Векторную графику** на основе DirectX,
- **Поддержку анимаций, стилей, шаблонов и триггеров**,
- **Интеграцию с медиа (аудио, видео, 3D)**.

WPF построен на принципе **разделения дизайна и логики**: дизайнеры могут работать с XAML, а разработчики — с C#.

---

## 2. Исторический контекст и эволюция UI-фреймворков Microsoft

| Технология | Год | Особенности |
|-----------|-----|-------------|
| **Win32 API** | 1990-е | Низкоуровневый, ручное управление окнами и сообщениями. |
| **MFC (Microsoft Foundation Classes)** | 1992 | Обёртка над Win32 на C++. |
| **Windows Forms (WinForms)** | 2002 | Первый UI-фреймворк в .NET. Простой, но ограниченный. |
| **WPF** | 2006 | Современный, векторный, декларативный, гибкий. |
| **UWP** | 2015 | Для универсальных приложений Windows 10/11. |
| **WinUI / MAUI** | 2020+ | Современные кроссплатформенные решения. |

>  **Почему WPF всё ещё актуален?**
> - Поддерживается Microsoft (включая .NET 6/7/8).
> - Используется в промышленных и корпоративных приложениях.
> - Богатые возможности для сложных UI (например, CAD, медицинские системы, дашборды).

---

## 3. Сравнение WPF с WinForms и UWP

###  WinForms

- **Плюсы**: простота, быстрое создание форм, огромное количество примеров.
- **Минусы**:
  - UI привязан к GDI+ (растровая графика),
  - Сложно создавать адаптивные или стилизованные интерфейсы,
  - Нет встроенной поддержки привязки данных, анимаций, шаблонов.

###  WPF

- **Плюсы**:
  - Векторная графика → масштабируется без потерь,
  - Полная поддержка XAML → чёткое разделение UI и логики,
  - Мощная система привязки данных и команд,
  - Поддержка стилей, шаблонов, триггеров, анимаций,
  - Легко интегрируется с современными паттернами (MVVM).
- **Минусы**:
  - Крутая кривая обучения,
  - Только Windows (не кроссплатформенный),
  - Более высокое потребление памяти по сравнению с WinForms.

### 🔹 UWP

- **Плюсы**:
  - Современный дизайн (Fluent Design),
  - Безопасность (песочница),
  - Доступ к API Windows 10/11 (сенсоры, уведомления и т.д.).
- **Минусы**:
  - Только Windows 10/11,
  - Ограниченная гибкость по сравнению с WPF,
  - Меньше контроля над жизненным циклом приложения.

>  **Вывод**:
> Если вы создаёте **корпоративное настольное приложение под Windows** — **WPF** остаётся лучшим выбором в 2025 году.

---

## 4. Архитектура WPF-приложения

WPF-приложение состоит из:
- **App.xaml / App.xaml.cs** — точка входа, управление жизненным циклом.
- **MainWindow.xaml / MainWindow.xaml.cs** — главное окно.
- **Дополнительные окна, пользовательские элементы управления (UserControl), ресурсы, стили**.

Ключевые концепции:
- **XAML** — язык разметки для описания UI.
- **Code-behind** — файл с логикой (`.xaml.cs`).
- **Dependency Properties** — особый тип свойств, поддерживающий привязку, анимацию и наследование.
- **Routed Events** — события, "всплывающие" или "тонущие" по визуальному дереву.

---

## 5. Пошаговое создание приложения с переходом между окнами

### Шаг 1: Создание проекта

1. Откройте Visual Studio.
2. Выберите **Create a new project**.
3. Найдите **WPF App** (рекомендуется выбрать **.NET 8** или **.NET 6**).
4. Назовите проект: `WpfNavigationDemo`.

### Шаг 2: Изучение структуры проекта

- `App.xaml` — определяет ресурсы приложения и стартовое окно.
- `MainWindow.xaml` — UI главного окна.
- `MainWindow.xaml.cs` — логика главного окна.

### Шаг 3: Создание кнопки в MainWindow

**MainWindow.xaml**:

```xml
<Window x:Class="WpfNavigationDemo.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Главная форма" Height="200" Width="400"
        WindowStartupLocation="CenterScreen">
    <Grid>
        <Button x:Name="btnOpenSecond"
                Content="Перейти ко второму окну"
                Width="200"
                Height="50"
                FontSize="16"
                HorizontalAlignment="Center"
                VerticalAlignment="Center"
                Click="btnOpenSecond_Click"/>
    </Grid>
</Window>
```

>  **Пояснение**:
> - `x:Name` — задаёт имя элемента для доступа из кода.
> - `Click="btnOpenSecond_Click"` — привязка события к методу в code-behind.

**MainWindow.xaml.cs**:

```csharp
using System.Windows;

namespace WpfNavigationDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); // Обязательно! Загружает XAML.
        }

        private void btnOpenSecond_Click(object sender, RoutedEventArgs e)
        {
            // Создаём новое окно
            var secondWindow = new SecondWindow();

            // Показываем его как немодальное окно
            secondWindow.Show();

            // Опционально: скрыть текущее окно
            // this.Hide();
        }
    }
}
```

### Шаг 4: Создание второго окна

1. В **Solution Explorer** кликните правой кнопкой по проекту → **Add → Window**.
2. Назовите файл `SecondWindow.xaml`.

**SecondWindow.xaml**:

```xml
<Window x:Class="WpfNavigationDemo.SecondWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Второе окно" Height="250" Width="400"
        WindowStartupLocation="CenterOwner">
    <Grid>
        <StackPanel VerticalAlignment="Center" HorizontalAlignment="Center">
            <TextBlock Text="Вы во втором окне!"
                       FontSize="20"
                       Margin="0,0,0,20"
                       TextAlignment="Center"/>
            <Button Content="Закрыть"
                    Width="120"
                    Height="40"
                    FontSize="14"
                    Click="CloseButton_Click"/>
        </StackPanel>
    </Grid>
</Window>
```

**SecondWindow.xaml.cs**:

```csharp
using System.Windows;

namespace WpfNavigationDemo
{
    public partial class SecondWindow : Window
    {
        public SecondWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Закрывает текущее окно
        }
    }
}
```

>  **WindowStartupLocation="CenterOwner"** — окно появится по центру родительского (если оно указано). Чтобы это работало, установите `Owner`:

```csharp
var secondWindow = new SecondWindow();
secondWindow.Owner = this; // this — MainWindow
secondWindow.Show();
```

---

## 6. Модальные и немодальные окна

| Тип | Метод | Поведение |
|-----|-------|----------|
| **Немодальное** | `.Show()` | Пользователь может взаимодействовать с обоими окнами. |
| **Модальное** | `.ShowDialog()` | Блокирует доступ к родительскому окну до закрытия. Возвращает `bool?` (результат DialogResult). |

Пример модального окна:

```csharp
var dialog = new SecondWindow();
bool? result = dialog.ShowDialog();

if (result == true)
{
    MessageBox.Show("Пользователь подтвердил действие.");
}
```

Чтобы вернуть результат из модального окна:

```csharp
// Во втором окне:
this.DialogResult = true; // или false
this.Close();
```

---

## 7. Жизненный цикл окна в WPF

1. **Конструктор** → `new Window()`
2. **InitializeComponent()** → загрузка XAML
3. **Loaded** — событие, когда окно полностью загружено
4. **Activated / Deactivated** — при получении/потере фокуса
5. **Closing** — можно отменить закрытие (`e.Cancel = true`)
6. **Closed** — окно уничтожено

Пример обработки закрытия:

```csharp
private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
{
    var result = MessageBox.Show("Вы уверены, что хотите выйти?",
                                 "Подтверждение",
                                 MessageBoxButton.YesNo);
    e.Cancel = (result == MessageBoxResult.No);
}
```

---

## 8. Рекомендации по архитектуре

Хотя создание окон через `new SecondWindow()` — допустимо для обучения, в реальных проектах рекомендуется:

- Использовать **паттерн MVVM** (Model-View-ViewModel),
- Выносить логику навигации в **отдельный сервис** (INavigationService),
- Избегать прямой зависимости между окнами.

Пример простого сервиса навигации (упрощённо):

```csharp
public interface INavigationService
{
    void NavigateToSecondWindow();
}

public class NavigationService : INavigationService
{
    public void NavigateToSecondWindow()
    {
        var window = new SecondWindow();
        window.Show();
    }
}
```

---

## 9. Отладка и советы

- **Не забывайте вызывать `InitializeComponent()`** — иначе XAML не загрузится.
- **Используйте `Output` в Visual Studio** — ошибки XAML часто появляются там.
- **Binding errors** можно увидеть в окне **Output** при запуске в Debug.
- Для отладки привязок используйте:
  ```xml
  Text="{Binding MyProperty, PresentationTraceSources.TraceLevel=High}"
  ```

---

## 10. Заключение

WPF — это не просто "WinForms 2.0", а полноценная платформа для создания богатых клиентских приложений. Несмотря на возраст, она остаётся мощным инструментом благодаря:

- Гибкости XAML,
- Поддержке современных паттернов (MVVM),
- Глубокой интеграции с .NET,
- Возможности создавать профессиональные UI без ограничений WinForms.

---

##  Домашнее задание

1. Добавьте в `SecondWindow` кнопку «Вернуться», которая закрывает текущее окно и **показывает** `MainWindow`, если оно было скрыто.
2. Реализуйте передачу данных из второго окна в первое (например, имя пользователя).
3. Попробуйте открыть `SecondWindow` как **модальное** и обработать результат.

>  **Дополнительно**: изучите, как использовать `UserControl` вместо отдельных окон для более гибкой компоновки.

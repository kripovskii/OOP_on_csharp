# Лекция: Основные элементы управления в WPF — TextBox, TextBlock, Label, Button


## 🧩 1. Общие принципы WPF


Все элементы управления наследуются от `Control` или `ContentControl`, и имеют:
- **Свойства** (Properties) — определяют внешний вид
- **События** (Events) — реакция на действия пользователя
- **Шаблоны** (Templates) — возможность кастомизации внешнего вида

---

## 📝 2. TextBlock — для отображения текста (только чтение)

### 🔹 Назначение
Отображение **статического** или **динамического** текста, который **не редактируется** пользователем.

### 🔹 Пример XAML
```xml
<TextBlock Text="Привет, WPF!" 
           FontSize="18" 
           Foreground="Blue" 
           FontWeight="Bold"
           TextWrapping="Wrap"
           Margin="10"/>
```

### 🔹 Ключевые свойства
| Свойство | Описание |
|---------|----------|
| `Text` | Текстовое содержимое |
| `FontSize`, `FontWeight`, `FontFamily` | Шрифтовые настройки |
| `Foreground` | Цвет текста |
| `TextWrapping` | Перенос текста (`NoWrap`, `Wrap`) |
| `TextAlignment` | Выравнивание (`Left`, `Center`, `Right`, `Justify`) |
| `LineHeight` | Высота строки |

### 🔹 Особенности
- **Не поддерживает ввод** — только чтение.
- **Легковесный** — лучше использовать для отображения текста, чем `TextBox`.
- Поддерживает **форматирование фрагментов текста** через `Run`:
  ```xml
  <TextBlock>
      <Run Text="Обычный текст " />
      <Run Text="жирный" FontWeight="Bold" />
      <Run Text=" и курсив" FontStyle="Italic" />
  </TextBlock>
  ```

### ✅ Когда использовать?
- Подписи, заголовки, инструкции, статические сообщения.
- Всё, что **не требует редактирования**.

---

## ✏️ 3. TextBox — для ввода и редактирования текста

### 🔹 Назначение
Элемент для **ввода и редактирования текста** пользователем.

### 🔹 Пример XAML
```xml
<TextBox Width="200" 
         Height="30" 
         Text="Введите имя" 
         FontSize="14"
         Margin="10"
         PlaceholderText="Имя пользователя"
         IsReadOnly="False"
         AcceptsReturn="False"/>
```

### 🔹 Ключевые свойства
| Свойство | Описание |
|---------|----------|
| `Text` | Текущее значение (связывается с ViewModel) |
| `IsReadOnly` | Только для чтения? (`true`/`false`) |
| `IsEnabled` | Включен ли элемент? |
| `MaxLength` | Максимальное количество символов |
| `AcceptsReturn` | Разрешить ввод переноса строки (Enter) |
| `PlaceholderText` | Подсказка при пустом тексте (WPF 10+) |
| `VerticalAlignment`, `HorizontalAlignment` | Позиционирование |
| `TextWrapping` | Перенос текста (`NoWrap`, `Wrap`) |

### 🔹 События
| Событие | Описание |
|--------|----------|
| `TextChanged` | Вызывается при изменении текста (включая программное) |
| `PreviewKeyDown` / `KeyDown` | Нажатие клавиш |
| `LostFocus` / `GotFocus` | Потеря/получение фокуса |
| `PreviewTextInput` | Ввод символов (включая кириллицу) |

### 🔹 Привязка данных (Binding)
```xml
<TextBox Text="{Binding UserName, UpdateSourceTrigger=PropertyChanged}" />
```
> `UpdateSourceTrigger=PropertyChanged` — обновляет источник при каждом вводе символа (по умолчанию — при потере фокуса).

### ✅ Когда использовать?
- Формы ввода (логин, пароль, email, комментарии).
- Поля, где пользователь должен **что-то ввести**.

> ⚠️ **Важно!** Не используйте `TextBox` для отображения статического текста — он тяжелее `TextBlock` и может быть непредсказуем в поведении.

---

## 🏷️ 4. Label — метка с привязкой к другому элементу

### 🔹 Назначение
`Label` — это **элемент управления**, который **связывается с другим элементом** (например, `TextBox`) и **предназначен для подписи**.

> `Label` наследуется от `ContentControl`, а не от `TextBox` — он может содержать **любой контент** (текст, изображение, даже другой элемент).

### 🔹 Пример XAML
```xml
<StackPanel Orientation="Horizontal" Margin="10">
    <Label Target="{Binding ElementName=loginTextBox}">Логин:</Label>
    <TextBox x:Name="loginTextBox" Width="150" />
</StackPanel>
```

### 🔹 Ключевое свойство: `Target`
- `Target="{Binding ElementName=SomeControl}"` — связывает метку с другим элементом.
- При нажатии на `Label` с `Target` фокус автоматически переходит к целевому элементу.
- Особенно полезно для `TextBox`, `CheckBox`, `RadioButton`.

### 🔹 Дополнительные свойства
| Свойство | Описание |
|---------|----------|
| `Content` | Содержимое метки (может быть `TextBlock`, `Image`, `Grid`) |
| `Padding` | Отступы внутри метки |
| `HorizontalContentAlignment`, `VerticalContentAlignment` | Выравнивание содержимого |

### 🔹 Почему не просто TextBlock?
```xml
<!-- Плохо: нет связи с полем ввода -->
<TextBlock Text="Логин:" />
<TextBox x:Name="loginTextBox" />

<!-- Хорошо: нажатие на метку фокусирует поле -->
<Label Target="{Binding ElementName=loginTextBox}" Content="Логин:" />
<TextBox x:Name="loginTextBox" />
```

### ✅ Когда использовать?
- Для подписей полей ввода (`TextBox`, `ComboBox`, `CheckBox`).
- Когда нужно **удобное навигационное поведение** (нажал на метку → фокус на поле).
- Если нужно **динамически менять содержимое** (например, "Имя: Иван" → "Имя: Петр").

> 💡 **Совет**: В современных приложениях часто используют `TextBlock` для простых подписей, если нет необходимости в `Target`. `Label` — более "тяжелый" элемент, но жизненно важен для доступности.

---

## 🔘 5. Button — кнопка для инициирования действия

### 🔹 Назначение
Элемент, по нажатию на который выполняется **действие** (сохранение, отправка, закрытие окна и т.д.).

### 🔹 Пример XAML
```xml
<Button Content="Отправить" 
        Width="100" 
        Height="30" 
        Margin="10"
        Click="Button_Click" />
```

### 🔹 Ключевые свойства
| Свойство | Описание |
|---------|----------|
| `Content` | Текст, изображение, или сложный контент внутри кнопки |
| `Width`, `Height` | Размеры |
| `IsEnabled` | Включена ли кнопка (`false` — серая, неактивная) |
| `IsDefault` | Кнопка по умолчанию (Enter → нажатие) |
| `IsCancel` | Кнопка отмены (Esc → нажатие) |

### 🔹 События
| Событие | Описание |
|--------|----------|
| `Click` | Основное событие — вызывается при нажатии (ЛКМ, Enter, Space) |
| `PreviewMouseLeftButtonDown` | До клика |
| `MouseEnter` / `MouseLeave` | Ввод/вывод курсора |
| `PreviewKeyDown` | Нажатие клавиш (для кастомных сценариев) |

### 🔹 Привязка команд (MVVM)
```xml
<Button Content="Сохранить" 
        Command="{Binding SaveCommand}" 
        IsEnabled="{Binding IsSaveEnabled}" />
```

В ViewModel:
```csharp
public ICommand SaveCommand { get; }

public MainWindowViewModel()
{
    SaveCommand = new RelayCommand(Save, CanSave);
}

private void Save() => /* логика сохранения */;
private bool CanSave() => !string.IsNullOrWhiteSpace(Username);
```

> ✅ **Рекомендация**: В MVVM **всегда используйте `Command`**, а не `Click`-событие в кодеBehind.

### 🔹 Кастомизация
```xml
<Button>
    <StackPanel Orientation="Horizontal">
        <Image Source="save.png" Width="16" Height="16" />
        <TextBlock Text="Сохранить" Margin="5,0,0,0" />
    </StackPanel>
</Button>
```

### ✅ Когда использовать?
- Все действия: сохранить, удалить, отменить, найти, отправить.
- Всегда используйте `IsEnabled` для контроля доступности.
- Используйте `IsDefault` и `IsCancel` для улучшения UX.

---

## 🆚 Сравнительная таблица

| Элемент | Редактируемость | Используется для | Вес | `Target` поддержка | MVVM-совместимость |
|--------|----------------|------------------|-----|-------------------|-------------------|
| `TextBlock` | ❌ Только чтение | Отображение текста | ⚖️ Легкий | ❌ | ✅ (Binding) |
| `TextBox` | ✅ Ввод и редактирование | Формы ввода | ⚖️ Средний | ❌ | ✅ (Binding + UpdateSourceTrigger) |
| `Label` | ❌ Только чтение | Подпись к другому элементу | ⚖️ Средний | ✅ | ✅ (Binding Content) |
| `Button` | ❌ Только действие | Вызов действия | ⚖️ Средний | ❌ | ✅ (Command) |

> 💡 **Запомните правило**:  
> - **Что показать?** → `TextBlock`  
> - **Что ввести?** → `TextBox`  
> - **Что подписать?** → `Label` + `Target`  
> - **Что нажать?** → `Button`

---

## 🛠️ Практические советы

### ✅ Лучшие практики
1. **Не используйте `TextBox` как `TextBlock`** — это ресурсоемко и неэстетично.
2. **Всегда используйте `Label` с `Target`** для полей ввода — это повышает **доступность** (для пользователей с ограниченными возможностями).
3. **Для кнопок — всегда `Command` в MVVM**, а не `Click` в кодеBehind.
4. **Используйте `Margin` и `Padding`** для визуального разделения элементов.
5. **Управляйте `IsEnabled`** — не позволяйте пользователю нажимать неактивные кнопки.
6. **Добавляйте `PlaceholderText`** в `TextBox` (WPF 10+), чтобы улучшить UX.

### ❌ Распространённые ошибки
- Использование `TextBox` для отображения пароля без `PasswordBox`.
- Нет `Target` у `Label` — потеряли удобство навигации.
- Использование `Click` вместо `Command` в MVVM — нарушение архитектуры.
- Нет валидации в `TextBox` — пользователь может ввести неверные данные.

---

## 💡 Пример: Форма входа (полный XAML)

```xml
<StackPanel Width="300" HorizontalAlignment="Center" VerticalAlignment="Center" Margin="20">
    <Label Target="{Binding ElementName=txtLogin}" Content="Логин:" />
    <TextBox x:Name="txtLogin" Margin="0,5" />

    <Label Target="{Binding ElementName=txtPassword}" Content="Пароль:" />
    <PasswordBox x:Name="txtPassword" Margin="0,5" />

    <Button Content="Войти" 
            Width="100" 
            Height="35" 
            Margin="0,15,0,0"
            Click="Button_Click"
            IsDefault="True" />

    <Button Content="Отмена" 
            Width="100" 
            Height="35" 
            Margin="0,5,0,0"
            Click="CancelButton_Click"
            IsCancel="True" />
</StackPanel>
```

---

## 📚 Дополнительные ресурсы

- [Microsoft Docs: TextBox](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/textbox-overview)
- [Microsoft Docs: Label](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/label-overview)
- [Microsoft Docs: Button](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/button-overview)
- [WPF Tutorial — MVVM](https://www.wpf-tutorial.com/mvvm/)

---

## ✅ Контрольные вопросы

1. Чем `TextBlock` отличается от `TextBox`?
2. Зачем нужен `Target` в `Label`?
3. Почему `Button` в MVVM должен использовать `Command`, а не `Click`?
4. Как сделать, чтобы нажатие `Enter` активировало кнопку?
5. Можно ли в `TextBlock` вставить изображение? А в `Label`?

---

## 📌 Заключение

Эти четыре элемента — **основа любого WPF-интерфейса**.  
Правильный выбор между `TextBlock`, `Label`, `TextBox` и `Button` — ключ к созданию **удобного, доступного и производительного** приложения.

> 💬 **Помните**:  
> *«Не делайте интерфейс красивым — делайте его понятным.  
> И тогда красота придёт сама.»*

---

**Следующая лекция:** `ComboBox`, `CheckBox`, `RadioButton` и `DataTemplate`  
**Домашнее задание:** Создайте форму регистрации с 4 полями, кнопками "Зарегистрироваться" и "Отмена". Проверьте работу `IsEnabled`, `IsDefault`, `IsCancel` и `Target`.

---

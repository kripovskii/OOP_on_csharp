# Лекция: RadioButton и CheckBox в WPF

## Введение

В Windows Presentation Foundation (WPF) элементы управления `RadioButton` и `CheckBox` используются для предоставления пользователю возможности выбора. Несмотря на то, что оба элемента позволяют включать/выключать состояние, они служат разным целям и используются в различных сценариях.

---

## 1. CheckBox

### Назначение
`CheckBox` — это элемент управления, который позволяет пользователю **установить или снять галочку**, обычно для обозначения **независимого выбора** (т.е. один чекбокс не влияет на другой).

### Основные свойства
- `IsChecked` — `bool?` (nullable bool):  
  - `true` — отмечен  
  - `false` — не отмечен  
  - `null` — неопределённое состояние (если `IsThreeState = true`)
- `IsThreeState` — если `true`, то элемент может принимать **три состояния**: `true`, `false`, `null`
- `Content` — содержимое, отображаемое рядом с чекбоксом (обычно текст)

### Пример XAML

```xml
<CheckBox Content="Подписаться на рассылку" IsChecked="True" />
<CheckBox Content="Принимаю условия" IsThreeState="True" />
```

### Пример обработки в коде (C#)

```csharp
private void CheckBox_Checked(object sender, RoutedEventArgs e)
{
    var checkBox = sender as CheckBox;
    if (checkBox.IsChecked == true)
    {
        // Действие при отметке
    }
}
```

---

## 2. RadioButton

### Назначение
`RadioButton` используется, когда нужно **выбрать один вариант из нескольких взаимоисключающих**. Все `RadioButton`, находящиеся в одной группе (`GroupName`), **автоматически исключают друг друга**.

### Основные свойства
- `IsChecked` — `bool`: `true`, если выбран
- `GroupName` — строка, определяющая **группу** переключателей. Только один `RadioButton` в группе может быть выбран.
- `Content` — отображаемое содержимое

### Особенности
- Если вы не указываете `GroupName`, все `RadioButton` в одном контейнере (например, `StackPanel`) считаются одной группой.
- Если у вас несколько логических групп — **обязательно используйте разные `GroupName`**!

### Пример XAML

```xml
<StackPanel>
    <RadioButton Content="Красный" GroupName="Color" />
    <RadioButton Content="Зелёный" GroupName="Color" IsChecked="True" />
    <RadioButton Content="Синий" GroupName="Color" />

    <RadioButton Content="Маленький" GroupName="Size" />
    <RadioButton Content="Большой" GroupName="Size" />
</StackPanel>
```

### Пример обработки в коде

```csharp
private void RadioButton_Checked(object sender, RoutedEventArgs e)
{
    var rb = sender as RadioButton;
    if (rb.IsChecked == true)
    {
        MessageBox.Show($"Выбрано: {rb.Content}");
    }
}
```

---

## 3. Сравнение CheckBox и RadioButton

| Характеристика         | CheckBox                         | RadioButton                              |
|------------------------|----------------------------------|------------------------------------------|
| Число состояний        | 2 или 3 (если `IsThreeState`)   | 2 (`IsChecked` — true/false)            |
| Взаимоисключающий?     | Нет                              | Да, внутри одной группы (`GroupName`)    |
| Типичное использование | Включение/отключение опций       | Выбор одного из нескольких вариантов     |
| Состояние по умолчанию | Может быть `false` или `null`    | Один из группы может быть `true` по умолчанию |

---

## 4. Привязка данных (Data Binding)

Оба элемента отлично работают с привязкой данных через `Binding`.

### Пример для CheckBox

```xml
<CheckBox Content="Активен" IsChecked="{Binding IsActive}" />
```

### Пример для RadioButton

```xml
<RadioButton Content="Вариант A" IsChecked="{Binding SelectedOption, Converter={StaticResource OptionToBoolConverter}, ConverterParameter=A}" />
<RadioButton Content="Вариант B" IsChecked="{Binding SelectedOption, Converter={StaticResource OptionToBoolConverter}, ConverterParameter=B}" />
```

> **Примечание:** Для `RadioButton` часто используется конвертер (`IValueConverter`), чтобы связать строковое или перечислимое значение с `IsChecked`.

---

## 5. Практические советы

- Используйте `CheckBox` для **независимых** опций («включить уведомления», «запомнить меня»).
- Используйте `RadioButton` для **взаимоисключающих** вариантов («пол», «тип подписки», «уровень доступа»).
- Всегда задавайте осмысленные `GroupName` — это спасает от неожиданных багов при сложном UI.
- Старайтесь не использовать `IsThreeState` без веской причины — он может запутать пользователя.

---

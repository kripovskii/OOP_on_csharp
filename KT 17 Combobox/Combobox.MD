# Элемент ComboBox в WPF

## Что такое ComboBox?
`ComboBox` — это стандартный элемент управления в WPF, который объединяет текстовое поле и выпадающий список.  
Пользователь может:
- Выбрать готовый вариант из списка.
- (Если разрешено) ввести свой текст вручную.

Выглядит так:  
![Пример ComboBox](https://docs.microsoft.com/ru-ru/dotnet/desktop/wpf/controls/media/combobox.png)

## Основные свойства элемента ComboBox

| Свойство              | Тип            | Описание                                                                 |
|-----------------------|----------------|--------------------------------------------------------------------------|
| `ItemsSource`         | IEnumerable    | Источник данных (список элементов, например, ObservableCollection)     |
| `SelectedItem`        | object         | Текущий выбранный элемент (объект из списка)                            |
| `SelectedIndex`       | int            | Индекс выбранного элемента (начинается с 0, -1 — ничего не выбрано)      |
| `SelectedValue`       | object         | Значение по SelectedValuePath (удобно для ID)                           |
| `SelectedValuePath`   | string         | Путь к свойству объекта, которое будет в SelectedValue (например, "Id") |
| `DisplayMemberPath`   | string         | Какое свойство объекта показывать в списке (например, "Name")           |
| `IsEditable`          | bool           | Можно ли вводить свой текст (true/false, по умолчанию false)            |
| `IsReadOnly`          | bool           | Запрет ввода даже при IsEditable=true                                   |
| `Text`                | string         | Текст в поле ввода (при IsEditable=true)                                |
| `IsDropDownOpen`      | bool           | Открыт ли сейчас список                                                 |

## Простые примеры в XAML

### 1. Самый простой ComboBox со строками
```xml
<ComboBox Width="200" Height="30">
    <ComboBoxItem>Яблоко</ComboBoxItem>
    <ComboBoxItem>Банан</ComboBoxItem>
    <ComboBoxItem>Апельсин</ComboBoxItem>
</ComboBox>
```

### 2. С обычными строками (рекомендуется)
```xml
<ComboBox Width="200" Height="30" SelectedIndex="0">
    <sys:String>Яблоко</sys:String>
    <sys:String>Банан</sys:String>
    <sys:String>Апельсин</sys:String>
</ComboBox>
```
(Добавьте `xmlns:sys="clr-namespace:System;assembly=mscorlib"` в окно)

### 3. С возможностью ввода своего текста
```xml
<ComboBox Width="200" Height="30" 
          IsEditable="True" 
          Text="Введите или выберите"/>
```

### 4. Отображение выбранного значения рядом
```xml
<StackPanel>
    <ComboBox x:Name="cbFruits" Width="200" SelectedIndex="0">
        <sys:String>Яблоко</sys:String>
        <sys:String>Банан</sys:String>
        <sys:String>Апельсин</sys:String>
    </ComboBox>
    
    <TextBlock Text="{Binding SelectedItem.Content, ElementName=cbFruits}" 
               FontSize="16" Margin="0,10"/>
</StackPanel>
```

## События элемента ComboBox
- `SelectionChanged` — срабатывает при выборе элемента.
- `DropDownOpened` — когда открывается список.
- `DropDownClosed` — когда закрывается список.
- `TextChanged` — при изменении текста (только если IsEditable=true).

Пример обработки события:
```xml
<ComboBox SelectionChanged="ComboBox_SelectionChanged">
```
```csharp
private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    var cb = sender as ComboBox;
    MessageBox.Show("Выбрано: " + cb.SelectedItem);
}
```

## Полезные советы
- Чтобы ComboBox занимал всю ширину: `HorizontalAlignment="Stretch"`.
- Чтобы список открывался вниз и не обрезался: поместите ComboBox в Grid или StackPanel с достаточным пространством.
- По умолчанию высота элемента — около 25 px. Увеличьте через `Height="35"` для удобства.
- Если используете MVVM — не используйте события, а только привязки (Binding).

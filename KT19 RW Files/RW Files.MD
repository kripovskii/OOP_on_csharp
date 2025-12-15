### Чтение и запись файлов в WPF-приложениях на C#

Для работы с файлами в WPF-приложениях на языке C# используются такие же, как и в любом .NET-приложении методы, которые включены в пространства имён `System.IO`.

Мы рассмотрим:
- Диалоговые окна для выбора файлов (`OpenFileDialog` и `SaveFileDialog`).
- Простые методы чтения и записи текстовых файлов.
- Более надёжные подходы с использованием `StreamReader` и `StreamWriter`.
- Лучшие практики и обработку ошибок.

#### 1. Диалоговые окна: OpenFileDialog и SaveFileDialog

В WPF для выбора файла пользователем используются классы из пространства имён `Microsoft.Win32`.

**Пример XAML (MainWindow.xaml):**
```xml
<Window x:Class="FileIOExample.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Работа с файлами" Height="400" Width="600">
    <Grid Margin="10">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        
        <StackPanel Orientation="Horizontal" Grid.Row="0" Margin="0,0,0,10">
            <Button Content="Открыть файл" Click="OpenFile_Click" Margin="0,0,10,0"/>
            <Button Content="Сохранить файл" Click="SaveFile_Click"/>
        </StackPanel>
        
        <TextBox x:Name="TextContent" Grid.Row="1" AcceptsReturn="True" TextWrapping="Wrap"/>
        
        <TextBlock x:Name="StatusText" Grid.Row="2" Margin="0,10,0,0"/>
    </Grid>
</Window>
```

**Код-behind (MainWindow.xaml.cs):**
```csharp
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace FileIOExample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            openDialog.Title = "Выберите файл для открытия";

            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    string content = File.ReadAllText(openDialog.FileName);
                    TextContent.Text = content;
                    StatusText.Text = $"Файл открыт: {openDialog.FileName}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка чтения: {ex.Message}");
                }
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            saveDialog.Title = "Сохранить файл как";
            saveDialog.OverwritePrompt = true; // Спрашивать о перезаписи

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveDialog.FileName, TextContent.Text);
                    StatusText.Text = $"Файл сохранён: {saveDialog.FileName}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка записи: {ex.Message}");
                }
            }
        }
    }
}
```

Это базовый пример: пользователь открывает файл, текст отображается в `TextBox`, и его можно сохранить.

#### 2. Методы класса File для простых операций

Класс `System.IO.File` предоставляет статические методы:
- `File.ReadAllText(path)` — читает весь файл как строку.
- `File.ReadAllLines(path)` — читает в массив строк.
- `File.WriteAllText(path, content)` — записывает строку (перезаписывает файл).
- `File.AppendAllText(path, content)` — добавляет текст в конец.

Эти методы удобны для небольших файлов, но они загружают весь файл в память сразу.

#### 3. Потоковые классы: StreamReader и StreamWriter (рекомендуется для больших файлов)

Для построчной обработки или больших файлов лучше использовать потоки:

**Чтение построчно:**
```csharp
using (StreamReader reader = new StreamReader(openDialog.FileName))
{
    string line;
    while ((line = reader.ReadLine()) != null)
    {
        // Обработка строки
        TextContent.Text += line + "\n";
    }
}
```

**Запись:**
```csharp
using (StreamWriter writer = new StreamWriter(saveDialog.FileName, false)) // false — перезапись, true — дозапись
{
    writer.Write(TextContent.Text);
}
```

Использование `using` гарантирует автоматическое закрытие потоков.

#### 4. Лучшие практики

- **Всегда обрабатывайте исключения:** Файл может быть заблокирован, отсутствовать или быть недоступен (IOException, UnauthorizedAccessException и т.д.).
- **Используйте using для потоков:** Чтобы избежать утечек ресурсов.
- **Асинхронные операции для больших файлов:** Чтобы не блокировать UI-поток:
  ```csharp
  string content = await File.ReadAllTextAsync(path);
  await File.WriteAllTextAsync(path, content);
  ```
  Или `await Task.Run(() => /* тяжёлая операция */);`.
- **Кодировка:** По умолчанию UTF-8, но можно указать:
  ```csharp
  File.ReadAllText(path, Encoding.UTF8);
  ```
- **MVVM-подход:** В реальных приложениях не размещайте логику в code-behind. Используйте сервисы (IDialogService) для диалогов и команды в ViewModel.
- **Не храните пути жёстко:** Всегда позволяйте пользователю выбирать файл через диалог.


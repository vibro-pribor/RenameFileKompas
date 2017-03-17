using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using NLog;

namespace VP
{
    /// <summary>
	/// Логика взаимодействия для FolderBrowserDialog.xaml
    /// </summary>
    public partial class FolderBrowserDialog : Window
    {
		/// <summary>
		/// Создание диалогового окна выбора каталога
		/// </summary>
		public FolderBrowserDialog()
		{
			InitializeComponent();
			this.ShowDisk();
			// Текущая директория - корень
			this.m_CurrentFolder = string.Empty;
			// Сделать активным список директорий
			this.ListFolders.Focus();
		}

		#region Поля
		// <summary>
		/// Логирование
		/// </summary>
		private static readonly Logger m_Logger = LogManager.GetCurrentClassLogger();
		#endregion

		#region Свойства
		/// <summary>
		/// Получает или задает текущую выбранную директорию
		/// </summary>
		public string CurrentSelectedFolder { get; set;}

		/// <summary>
		/// Получает или задает текущую директорию 
		/// </summary>
		private string m_CurrentFolder { get; set; }
		#endregion

		#region Обработчики
		/// <summary>
		/// Обработчик клика левой кнопки мыши по кнопке "Вверх"
        /// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий информацию о состоянии и данные события</param>
        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
			this.TopDirectory(); 
        }

		/// <summary>
		/// Обработчик двойного клика левой кнопки мыши по списку директорий
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий данные для события</param>
		private void ListFolders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (this.ListFolders.SelectedItem != null)
            {
				this.m_CurrentFolder = this.ListFolders.SelectedItem.ToString();
				this.ShowDirectories();
			}
		}

		/// <summary>
		/// Обработчик клика левой кнопки мыши по списку директорий
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий данные для события</param>
		private void ListFolders_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.ListFolders.SelectedItem != null)
            {
				this.ListFolders.ToolTip = this.SelectedFolder.ToolTip = this.SelectedFolder.Text = this.CurrentSelectedFolder = this.ListFolders.SelectedItem.ToString();
			}
		}

		/// <summary>
		/// Обработчик клика левой кнопки мыши по кнопке "Выбрать"
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий информацию о состоянии и данные события</param>
		private void ButtonOK_Click(object sender, RoutedEventArgs e)
		{
			if (this.CurrentSelectedFolder == null || this.CurrentSelectedFolder == string.Empty)
				this.DialogResult = false;
			else
				this.DialogResult = true;
		}

		/// <summary>
		/// Обработчик клика левой кнопки мыши по кнопке "Отменить"
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий информацию о состоянии и данные события</param>
		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}

		/// <summary>
		/// Обработчик нажатия клавиш клавиатуры при активном списке директорий
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий данные для события</param>
		private void ListFolders_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key) {
				case Key.Left:
				case Key.Back: this.TopDirectory(); break;
				case Key.Right:
				case Key.Enter:
					if (this.ListFolders.SelectedItem != null) { this.m_CurrentFolder = this.ListFolders.SelectedItem.ToString(); this.ShowDirectories(); } break;
				default: break;
			}
		}
		#endregion

		#region Вспомогательные методы
		/// <summary>
		/// Вывод всех директорий, входящих в текущую
		/// </summary>
		/// <remarks>Значением текущей выбранной директории становится текущая директория</remarks>
        private void ShowDirectories()
        {
			this.SelectedFolder.ToolTip = this.SelectedFolder.Text = this.CurrentSelectedFolder = this.m_CurrentFolder;
			string[] folders;
			try {
				folders = Directory.GetDirectories(this.m_CurrentFolder);
			}
			catch (UnauthorizedAccessException) {
				m_Logger.Error("У пользователя нет прав на получение директорий из " + this.m_CurrentFolder);
				MessageBox.Show("Вы не обладаете достаточными правами доступа для этой директории. " +
								"Обратитесь к администратору или запустите программу от его имени.",
								"Отсутствуют необходимые права доступа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				this.TopDirectory();
				return;

			}
			catch (DirectoryNotFoundException exc) {
				m_Logger.Error("Путь " + this.m_CurrentFolder + " недопустим. Причина: " + exc.Message);
				MessageBox.Show("Эта директория является недопустимой. " +
								"Возможно диск был извлечен или отключен.",
								"Недопустимая директория", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				this.TopDirectory();
				return;
			}
			catch (IOException exc) {
				m_Logger.Error("Ошибка ввода/вывода при обращении к директории " + this.m_CurrentFolder + ": ", (Exception)exc);
				MessageBox.Show("Не удается получить доступ к данной директории на диске. " +
								"Возможно диск отсутствует в дисководе или был извлечен в процессе чтения.",
								"Ошибка чтения диска", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				this.TopDirectory();
				return;
			}
			catch (Exception exc) {
				m_Logger.Error("Необработанное исключение при обращении к " + this.m_CurrentFolder + ": ", exc);
				MessageBox.Show("Возникла непредвиденная ошибка приложения. Обратитесь в службу поддержки.",
								"Непредвиденная ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);

				this.TopDirectory();
				return;
			}
            this.ListFolders.Items.Clear();
			foreach (string folder in folders) {
				this.ListFolders.Items.Add(folder);
            }
        }

        /// <summary>
        /// Вывод логических дисков текущего компьютера
        /// </summary>
		/// <remarks>Значениями текущей и текущей выбранной директорий становятся пустые строки</remarks>
        private void ShowDisk()
        {
            string[] drives;
			try {
				drives = Environment.GetLogicalDrives();
			}
			catch (System.Security.SecurityException) {
				m_Logger.Error("У пользователя нет прав на получение списка логических дисков.");
				MessageBox.Show("Вы не обладаете достаточными правами доступа для этой директории. " +
								"Обратитесь к администратору или запустите программу от его имени.",
								"Отсутствуют необходимые права доступа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				this.DialogResult = false;
				return;
			}
			catch (Exception exc) {
				m_Logger.Error("Необработанное исключение при обращении к списку логических дисков: ", exc);
				MessageBox.Show("Возникла непредвиденная ошибка приложения. Обратитесь в службу поддержки.",
								"Непредвиденная ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);

				this.DialogResult = false;
				return;
			}
			this.SelectedFolder.ToolTip = this.SelectedFolder.Text = this.CurrentSelectedFolder = this.m_CurrentFolder = string.Empty;
            this.ListFolders.Items.Clear();
            foreach (string drive in drives)
            {
                this.ListFolders.Items.Add(drive);
            }
        }

		/// <summary>
		/// Перейти в каталог на уровень выше
		/// </summary>
		private void TopDirectory()
		{
			// Проверка нахождения в корне
			if (this.m_CurrentFolder == null || this.m_CurrentFolder == string.Empty) {
				this.ShowDisk();
				return;
			}
			int slash_count_folder;
			try {
				slash_count_folder = new Regex(Regex.Escape("\\")).Matches(this.m_CurrentFolder).Count;
			}
			catch (Exception exc) {
				m_Logger.Error("Необработанное исключение при обращении к " + this.m_CurrentFolder + ": ", exc);
				MessageBox.Show("Возникла непредвиденная ошибка приложения. Обратитесь в службу поддержки.",
								"Непредвиденная ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			// Если путь состоит из двух и менее каталогов
			if (slash_count_folder <= 1)
				// Если путь состоит только из имени логического диска (например C:/), то
				if (this.m_CurrentFolder.Length <= 3) 
					this.ShowDisk();
				// Иначе: путь - верхняя папка на логическом диске (например C:/Work)
				else {
					// C:/Work -> C:/
					this.m_CurrentFolder = this.m_CurrentFolder.Remove(this.m_CurrentFolder.IndexOf('\\') + 1);
					this.ShowDirectories();
				}
			// Иначе: путь состоит из трех и более каталогов
			else {
				// C:/Work/folder -> C:/Work
				this.m_CurrentFolder = this.m_CurrentFolder.Remove(this.m_CurrentFolder.LastIndexOf('\\'));
				this.ShowDirectories();
			}
		}
		#endregion	
    }
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using NLog;
using VP;
using System.IO;
using KAPITypes;
using Kompas6Constants;
using Kompas6API5;
using Kompas6API7;
using System.Runtime.InteropServices;
using Microsoft.CSharp;
using System.Text.RegularExpressions;
using IOPath = System.IO.Path;


namespace VP.RenameFileKompas
{
	/// <summary>
	/// Основные типы систематизируемых файлов КОМПАС-3D
	/// </summary>
	public enum KompasFileType { Неопознанный, Фрагмент, Спецификация, Чертеж, Текстовый, Модель };

	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		/// Создает основное окно приложения
		/// </summary>
		public MainWindow()
		{
			this.m_Worker = null;
			InitializeComponent();
			this.PathFilesKompas.Text = this.m_DefaultTextPath;
			// Изменение цвета текста на серый
			this.PathFilesKompas.Opacity = 0.5;
			string[] parts_name_file = {"Обозначение", "Наименование" };
			this.FirstPartNameFile.ItemsSource = parts_name_file;
			this.SecondPartNameFile.ItemsSource = parts_name_file;
			this.FirstPartNameFile.SelectedIndex = 0;
		}

		#region Поля
		/// <summary>
		/// Логирование
		/// </summary>
		private static readonly Logger m_Logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Получает текст, отображаемый по умолчанию в текстовом поле, содержащем путь к систематизируемым файлам КОМПАС-3D
		/// </summary>
		private readonly string m_DefaultTextPath = "Выберите путь к файлам КОМПАС-3D";
		
		/// <summary>
        /// Получает или задает ссылку на приложение КОМПАС-3D
		/// </summary>
		private KompasObject m_KompasApplication = null;

        /// <summary>
        /// Получает или задает интерфейс API версии 7 приложения КОМПАС-3D 
        /// </summary>
        private IApplication m_IKompasApplicationAPIVer7 = null;

		/// <summary>
		/// Получает или задает интерфейс штампа
		/// </summary>
		private ksStamp m_IStamp = null;
		#endregion

		#region Свойства
		/// <summary>
		/// Получает или задает контейнер для работы с фоновым потоком
		/// </summary>
		private BackgroundWorker m_Worker { get; set; }
		#endregion

		#region Обработчики событий
		/// <summary>
		/// Обработчик клика левой кнопки мыши по кнопке "Обзор..."
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий информацию о состоянии и данные события</param>
		private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog dlg = new FolderBrowserDialog();
			dlg.Owner = this;
			if (dlg.ShowDialog() == false)
				return;
			// Изменение цвета текста на чёрный
			this.PathFilesKompas.Opacity = 1;
			this.PathFilesKompas.Text = dlg.CurrentSelectedFolder;
		}

		/// <summary>
		/// Обработчик клика левой кнопки мыши по кнопке "Переименовать"
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий информацию о состоянии и данные события</param>
		private void ButtonOk_Click(object sender, RoutedEventArgs e)
		{
			// Если нет запущенных фоновых потоков, то
			if (this.m_Worker == null) {
				// Создать новый фоновый поток
				this.m_Worker = new BackgroundWorker();
				this.m_Worker.DoWork += RenameFileKompasWork;
				this.m_Worker.WorkerReportsProgress = true;
				// Градация шкалы хода выполнения процесса систематизации 0 до 100%
				ProgressTask progress_dlg = new ProgressTask(100);
				progress_dlg.Title = "Ход процесса систематизации";
				progress_dlg.Owner = this;
				this.m_Worker.ProgressChanged += progress_dlg.ProgressChanged;
				this.m_Worker.RunWorkerCompleted += progress_dlg.RunWorkerCompleted;
				this.m_Worker.WorkerSupportsCancellation = true;
				ApplicationData app_data = new ApplicationData();
				app_data.PathFilesKompas = this.PathFilesKompas.Text;
				app_data.FirstPartNameFile = this.FirstPartNameFile.Text;
				app_data.SecondPartNameFile = this.SecondPartNameFile.Text;
				this.m_Worker.RunWorkerAsync(app_data);
				// Запустить окно хода выполнения процесса как диалоговое
				progress_dlg.ShowDialog();
				// Проверка полуаварийного завершения работы приложения
				// Если это стандартное завершение операции переименования, то 
				if (this.m_Worker != null && !this.m_Worker.CancellationPending)
					// Очистить контейнер для работы с фоновым потоком
					this.m_Worker = null;
			}
		}

		/// <summary>
		/// Обработчик запуска фонового процесса систематизации файлов приложения КОМПАС-3D
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий данные для события</param>
		/// <exception cref="System.ArgumentNullException">В качестве создателя события, данных для события или их аргумента был передан null</exception>
		/// <exception cref="System.NullReferenceException">В качестве данных приложения был передан null</exception>
		/// <exception cref="System.InvalidCastException">Не существует способа преобразования аргумента данных для события в ApplicationData.
		/// Не существует способа преобразования создателя события в BackgroundWorker</exception>
		void RenameFileKompasWork(object sender, DoWorkEventArgs e)
		{
			m_Logger.Info("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
			if (sender == null || e == null || e.Argument == null) {
				m_Logger.Error("Создатель события, данные для события или их аргумент не могут иметь значение null " + Environment.StackTrace);
				throw new System.ArgumentNullException("Создатель события, данные для события или их аргумент не могут иметь значение null");
			}
			if (!(e.Argument is ApplicationData)) {
				m_Logger.Error("Не существует способа преобразования аргумента данных для события в ApplicationData " + Environment.StackTrace);
				throw new System.InvalidCastException("Не существует способа преобразования аргумента данных для события в ApplicationData");
			}
			if (!(sender is BackgroundWorker)) {
				m_Logger.Error("Не существует способа преобразования создателя события в BackgroundWorker " + Environment.StackTrace);
				throw new System.InvalidCastException("Не существует способа преобразования создателя события в BackgroundWorker");
			}
			// Извлечение данных приложения
			ApplicationData app_data = (ApplicationData)e.Argument;
			if (app_data.PathFilesKompas == null || app_data.FirstPartNameFile == null) {
				m_Logger.Error("Данные приложения не могут иметь значение null " + Environment.StackTrace);
				throw new System.NullReferenceException("Данные приложения не могут иметь значение null");
			}
			if (string.IsNullOrWhiteSpace(app_data.PathFilesKompas)) {
				MessageBox.Show("Необходимо задать путь до файлов, которые необходимо переименовать!",
								"Не указан путь до файлов", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			if (IOPath.GetPathRoot(app_data.PathFilesKompas) == null || IOPath.GetPathRoot(app_data.PathFilesKompas) == "\\" || IOPath.GetPathRoot(app_data.PathFilesKompas) == string.Empty) {
				MessageBox.Show("Необходимо задать абсолютный путь до файлов, которые необходимо переименовать!",
								"Указан относительный путь до файлов", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			if (this.m_Worker.CancellationPending == true) {
				m_Logger.Info("Фоновая операция была прервана!");
				this.m_Worker = null;
				return;
			}
			// Путь, где будут лежать исходные файлы
			string path_sources;
			try {
				path_sources = IOPath.Combine(app_data.PathFilesKompas, "Исходные файлы");
			}
			catch (ArgumentException) {
				m_Logger.Error("this.PathFilesKompas.Text=" + app_data.PathFilesKompas + ", содержит один или несколько " +
							   "недопустимых символов, определенных в GetInvalidPathChars. " + Environment.StackTrace);
				MessageBox.Show("Путь к файлам КОМПАС-3D содержит один или несколько недопустимых символов: " +
								IOPath.GetInvalidPathChars() + ".", "Недопустимые символы в пути к файлам",
								MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			double current_percent_process = 0;
			try {
				if (Directory.Exists(path_sources)) {
					Directory.Delete(path_sources, true);
					m_Logger.Info("Произведено удаление директории " + path_sources + ", а также ее подкаталогов и файлов");
				}
			}
			catch (UnauthorizedAccessException) {
				m_Logger.Error("У пользователя нет прав на удаление директории " + path_sources + " и ее подкаталогов");
				MessageBox.Show("Вы не обладаете достаточными правами доступа для этой директории. " +
								"Обратитесь к администратору или запустите программу от его имени.",
								"Отсутствуют необходимые права доступа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;

			}
			catch (PathTooLongException) {
				m_Logger.Error("Длина пути " + path_sources + " превышает установленное в системе максимальное значение");
				MessageBox.Show("Длина указанного пути превышает установленное в системе максимальное значение. " +
								"Например, для платформ на основе Windows длина пути не должна превышать 248 символов. " +
								"Попробуйте скопировать или переместить файлы в другую директорию с меньшим " +
								"количеством символов в пути.", "Превышено количество символов в пути",
								MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			catch (IOException) {
				m_Logger.Error("Файлы в каталоге " + path_sources + " используются в данный момент этим или другим приложением " +
							   "или доступны только для чтения");
				MessageBox.Show("Файлы в данном каталоге используются в данный момент этим или другим приложением " +
							   "или доступны только для чтения. Попробуйте закрыть все приложения работающие с данным каталогом",
							   "Файлы используются другим приложением", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			if (this.m_Worker.CancellationPending == true) {
				m_Logger.Info("Фоновая операция была прервана!");
				this.m_Worker = null;
				return;
			}
			try {
				Directory.CreateDirectory(path_sources);
			}
			catch (UnauthorizedAccessException) {
				m_Logger.Error("У пользователя нет прав на создание директории " + path_sources);
				MessageBox.Show("Вы не обладаете достаточными правами доступа для этой директории. " +
								"Обратитесь к администратору или запустите программу от его имени.",
								"Отсутствуют необходимые права доступа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;

			}
			catch (PathTooLongException) {
				m_Logger.Error("Длина пути " + path_sources + " превышает установленное в системе максимальное значение");
				MessageBox.Show("Длина указанного пути превышает установленное в системе максимальное значение. " +
								"Например, для платформ на основе Windows длина пути не должна превышать 248 символов. " +
								"Попробуйте скопировать или переместить файлы в другую директорию с меньшим " +
								"количеством символов в пути.", "Превышено количество символов в пути",
								MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			catch (NotSupportedException) {
				m_Logger.Error("Путь " + path_sources + " содержит двоеточие (:), которое не является частью метки диска");
				MessageBox.Show("Данный путь является недопустимым. Проверте правильность его написания. " +
								"Возможно пропущено имя диска перед \":\".", "Недопустимый путь",
								MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			catch (IOException) {
				m_Logger.Error("Путь " + path_sources + " не найден");
				MessageBox.Show("Указанный путь не найден. Возможно указан неподключенный диск.", "Путь не найден",
								MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			m_Logger.Info("Создана директория: " + path_sources);
			if (this.m_Worker.CancellationPending == true) {
				m_Logger.Info("Фоновая операция была прервана!");
				this.m_Worker = null;
				return;
			}
			string[] files_kompas;
			try {
				((BackgroundWorker)sender).ReportProgress((int)Math.Round(current_percent_process), "Получение списка файлов из " + app_data.PathFilesKompas);
                files_kompas = Directory.GetFiles(app_data.PathFilesKompas, "*.*", SearchOption.AllDirectories).Where(file_name => file_name.EndsWith(".cdw") || file_name.EndsWith(".frw") || file_name.EndsWith(".kdw") || file_name.EndsWith(".spw") || file_name.EndsWith(".m3d") || file_name.EndsWith(".a3d")).ToArray();
			}
			catch (UnauthorizedAccessException) {
				m_Logger.Error("У пользователя нет прав на чтение файлов из директории " + app_data.PathFilesKompas);
				MessageBox.Show("Вы не обладаете достаточными правами доступа для этой директории. " +
								"Обратитесь к администратору или запустите программу от его имени.",
								"Отсутствуют необходимые права доступа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;

			}
			catch (IOException) {
				m_Logger.Error("Путь " + app_data.PathFilesKompas + " в данный момент не доступен");
				MessageBox.Show("Указанный путь не найден. Возможно указан неподключенный диск или удаленный каталог.",
								"Путь не найден", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			m_Logger.Info("Найдено " + files_kompas.Length.ToString() + " файла(ов) в директории " + app_data.PathFilesKompas);
			if (this.m_Worker.CancellationPending == true) {
				m_Logger.Info("Фоновая операция была прервана!");
				this.m_Worker = null;
				return;
			}
			// Количество процентов, прибавляемых к значению шкалы хода выполнения процесса систематизации,
			// при завершении обработки одного файла
			double percent_in_one_file = (double)100 / files_kompas.Length;
			string report_all_file = IOPath.Combine(path_sources, "AllReport.txt");
			string report_error_file = IOPath.Combine(path_sources, "ErrorReport.txt");
			try {
				using (StreamWriter report_all_writer = new StreamWriter(report_all_file, false)) {
					m_Logger.Info("Создан файл \"AllReport.txt\" в директории " + path_sources);
					// Запись заголовков столбцов таблицы полного отчета
					report_all_writer.WriteLine("Тип файла\tСтарое имя файла\tНовое имя файла\tПуть к файлу");
					report_all_writer.WriteLine("\t\t\t");
					try {
						using (StreamWriter report_error_writer = new StreamWriter(report_error_file, false)) {
							m_Logger.Info("Создан файл \"ErrorReport.txt\" в директории " + path_sources);
							// Запись заголовков столбцов таблицы отчета ошибок
							report_error_writer.WriteLine("Тип файла\tСтарое имя файла\tНовое имя файла\tПуть к файлу");
							report_error_writer.WriteLine("\t\t\t");
							if (this.m_Worker.CancellationPending == true) {
								m_Logger.Info("Фоновая операция была прервана!");
								this.m_Worker = null;
								return;
							}
							int specification_counter = 0, fragment_counter = 0, drawing_counter = 0, text_counter = 0, model_counter = 0, unidentified_counter = 0, error_copy_counter = 0, ignored_counter = 0;
							foreach (string file_kompas in files_kompas) {
								KompasFileType type;
								switch (IOPath.GetExtension(file_kompas).ToLower()) {
									case ".frw": type = KompasFileType.Фрагмент; break;
									case ".spw": type = KompasFileType.Спецификация; break;
									case ".cdw": type = KompasFileType.Чертеж; break;
									case ".kdw": type = KompasFileType.Текстовый; break;
                                    case ".m3d":
                                    case ".a3d": type = KompasFileType.Модель; break;
									default:
										m_Logger.Error("Тип файла " + file_kompas + " не опознан");
										type = KompasFileType.Неопознанный;
										break;
								}
								if (this.m_Worker.CancellationPending == true) {
									m_Logger.Info("Фоновая операция была прервана!");
									this.m_Worker = null;
									try {
										if (this.m_KompasApplication != null) {
											this.m_KompasApplication.Quit();
											m_Logger.Info("Приложение КОМПАС-3D закрыто");
										}
									}
									// Игнорировать ошибки при попытке закрыть приложение КОМПАС-3D 
									// (приложение ждет ответа от пользователя или производит какие-либо операции, 
									// но после этого все равно закроется)
									catch {
										m_Logger.Info("Приложение КОМПАС-3D закрыто");
									}
									return;
								}
								string old_name_file = IOPath.GetFileNameWithoutExtension(file_kompas);
								// Новый путь к исходному файлу
								// Идея - перенести структуру каталога this.PathFilesKompas.Text в каталог "Исходные файлы"
								string new_path_old_file;
								// Если текущий файл в корне каталога this.PathFilesKompas.Text, то
								if (IOPath.GetDirectoryName(file_kompas) == app_data.PathFilesKompas)
									// Его новый путь будет каталог "Исходные файлы"
									new_path_old_file = IOPath.Combine(path_sources, IOPath.GetFileName(file_kompas));
								else
									// Иначе - собрать новый путь из каталога "Исходные файлы" и промежуточных каталогов между this.PathFilesKompas.Text и файлом
									new_path_old_file = IOPath.Combine(path_sources, IOPath.GetDirectoryName(file_kompas).Remove(0, app_data.PathFilesKompas.Length + 1), IOPath.GetFileName(file_kompas));
								// Перед копированием файла необходимо создать директорию назначения, т.к она автоматически не создается
								try {
									if (!Directory.Exists(IOPath.GetDirectoryName(new_path_old_file))) {
										Directory.CreateDirectory(IOPath.GetDirectoryName(new_path_old_file));
										m_Logger.Info("Создана директория: " + new_path_old_file);
									}
								}
								catch (UnauthorizedAccessException) {
									m_Logger.Error("У пользователя нет прав на создание директории " + new_path_old_file);
									MessageBox.Show("Вы не обладаете достаточными правами доступа для этой директории. " +
													"Обратитесь к администратору или запустите программу от его имени.",
													"Отсутствуют необходимые права доступа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
									return;

								}
								catch (IOException exc) {
									m_Logger.Error("Путь " + new_path_old_file + " не найден или длина пути " + new_path_old_file +
												   " превышает установленное в системе максимальное значение. Подробней: " + exc.Message);
									MessageBox.Show("Указанный путь не найден или длина указанного пути превышает установленное в системе " +
													"максимальное значение. " + "Например, для платформ на основе Windows длина пути " +
													"не должна превышать 248 символов. " + "Проверте наличие подключенного, готового к работе, " +
													"устройства в указанном пути или попробуйте скопировать или переместить файлы в другую " +
													"директорию с меньшим количеством символов в пути.",
													"Путь не найден или превышено количество символов в пути",
													MessageBoxButton.OK, MessageBoxImage.Exclamation);
									return;
								}
								if (this.m_Worker.CancellationPending == true) {
									m_Logger.Info("Фоновая операция была прервана!");
									this.m_Worker = null;
									try {
										if (this.m_KompasApplication != null) {
											this.m_KompasApplication.Quit();
											m_Logger.Info("Приложение КОМПАС-3D закрыто");
										}
									}
									// Игнорировать ошибки при попытке закрыть приложение КОМПАС-3D 
									// (приложение ждет ответа от пользователя или производит какие-либо операции, 
									// но после этого все равно закроется)
									catch {
										m_Logger.Info("Приложение КОМПАС-3D закрыто");
									}
									return;
								}
								try {
									((BackgroundWorker)sender).ReportProgress((int)Math.Round(current_percent_process), "Копирование файла " + file_kompas + " в " + new_path_old_file);
									File.Copy(file_kompas, new_path_old_file, true);
								}
								catch (UnauthorizedAccessException) {
									m_Logger.Error("У пользователя нет прав для копирования файла " + file_kompas + " в " + new_path_old_file);
									MessageBox.Show("Вы не обладаете достаточными правами доступа для этой директории. " +
													"Обратитесь к администратору или запустите программу от его имени.",
													"Отсутствуют необходимые права доступа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
									return;

								}
								catch (IOException exc) {
									m_Logger.Error(file_kompas + " или " + new_path_old_file + " не найден, недопустим или произошла ошибка ввода-вывода. Подробней: " + exc.Message);
									MessageBox.Show("Указанный путь не найден. Проверте наличие подключенного, готового к работе, " +
													"устройства и указанного каталога в пути к файлам.", "Каталог не найден",
													MessageBoxButton.OK, MessageBoxImage.Exclamation);
									return;
								}
								m_Logger.Info("Файл " + file_kompas + " копирован в " + new_path_old_file);
								if (this.m_Worker.CancellationPending == true) {
									m_Logger.Info("Фоновая операция была прервана!");
									try {
										if (this.m_KompasApplication != null) {
											this.m_KompasApplication.Quit();
											m_Logger.Info("Приложение КОМПАС-3D закрыто");
										}
									}
									// Игнорировать ошибки при попытке закрыть приложение КОМПАС-3D 
									// (приложение ждет ответа от пользователя или производит какие-либо операции, 
									// но после этого все равно закроется)
									catch {
										m_Logger.Info("Приложение КОМПАС-3D закрыто");
									}
									this.m_Worker = null;
									return;
								}
								if (type == KompasFileType.Неопознанный) {
									++unidentified_counter;
									continue;
								}
								string new_name_file;
								// Факт ошибки при формировании имени файла 
								bool is_error_name = false;
								if (type == KompasFileType.Фрагмент) {
									is_error_name = true;
									new_name_file = old_name_file + "(нет штампа)";
								}
								else {
									try {
										// Если данное приложение не имеет ссылки на запущенное приложение КОМПАС-3D, то
										if (this.m_KompasApplication == null) {
											((BackgroundWorker)sender).ReportProgress((int)Math.Round(current_percent_process), "Запуск приложения КОМПАС-3D");
											// Запустить КОМПАС-3D для этого приложения
											this.m_KompasApplication = (KompasObject)Activator.CreateInstance(Type.GetTypeFromProgID("KOMPAS.Application.5"));
											// Запуск приложения будет происходить в фоне (пользователь его не будет видеть)
											this.m_KompasApplication.Visible = false;
											// Запуск API КОМПАС-3D
											this.m_KompasApplication.ActivateControllerAPI();
                                            // Получить интерфейс API версии 7 приложения КОМПАС-3D
                                            this.m_IKompasApplicationAPIVer7 = (IApplication)m_KompasApplication.ksGetApplication7();
											m_Logger.Info("Приложение КОМПАС-3D запущено");
											if (this.m_Worker.CancellationPending == true) {
												m_Logger.Info("Фоновая операция была прервана!");
												this.m_Worker = null;
												try {
													this.m_KompasApplication.Quit();
												}
												// Игнорировать ошибки при попытке закрыть приложение КОМПАС-3D 
												// (приложение ждет ответа от пользователя или производит какие-либо операции, 
												// но после этого все равно закроется)
												catch {
												}
												m_Logger.Info("Приложение КОМПАС-3D закрыто");
												return;
											}
										}
									}
									catch (ArgumentNullException) {
										m_Logger.Error("Не найдена запись в реестре для КОМПАС-3D");
										MessageBox.Show("Приложение КОМПАС-3D не найдено в системе. Проверте наличие данного приложения на вашем компьютере. " +
														"Возможно были повреждены некоторые данные или файлы КОМПАС-3D. Попробуйте переустановить данное ПО. " +
														"Если проблема повториться - обратитесь в тех. поддержку.", "Приложение КОМПАС-3D не найдено",
														MessageBoxButton.OK, MessageBoxImage.Exclamation);
										return;
									}
									catch (MemberAccessException) {
										m_Logger.Error("У пользователя нет прав для запуска КОМПАС-3D");
										MessageBox.Show("Вы не обладаете достаточными правами доступа для запуска КОМПАС-3D.",
														"Отсутствуют необходимые права доступа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
										return;
									}
									catch (Exception exc) {
										m_Logger.Error("Повреждены или отсутствуют некоторые записи в реестре или файлы, связанные с КОМПАС-3D. " +
													   "Подробней: " + exc.Message + ". Объект: " + exc.Source + ". Метод: " + exc.TargetSite +
													   ". Стек вызова: " + exc.StackTrace);
										MessageBox.Show("Некоторые файлы или данные приложения КОМПАС-3D не найдены или повреждены. Попробуйте переустановить данное ПО. " +
														"Если проблема повториться - обратитесь в тех. поддержку.", "Ошибка запуска приложения КОМПАС-3D",
														MessageBoxButton.OK, MessageBoxImage.Exclamation);
										return;
									}
									try {
										ksDocument2D IDrawing = null;
										ksSpcDocument ISpecification = null;
										ksDocumentTxt IText = null;
                                        ksDocument3D IModel = null;
                                        IKompasDocument IDocAPIVer7 = null;
										switch (type) {
											case KompasFileType.Чертеж:
												// Получить указатель на интерфейс чертежа
												IDrawing = (ksDocument2D)this.m_KompasApplication.Document2D();
												if (IDrawing == null) {
													m_Logger.Error("Не удалось получить интерфейс графического документа (чертежа)");
													MessageBox.Show("Некоторые файлы или данные приложения КОМПАС-3D не найдены или повреждены. Попробуйте переустановить данное ПО. " +
																	"Если проблема повториться - обратитесь в тех. поддержку.", "Ошибка запуска приложения КОМПАС-3D",
																	MessageBoxButton.OK, MessageBoxImage.Exclamation);
													return;
												}
												// Открыть файл в фоне (пользователь его не будет видеть)
												IDrawing.ksOpenDocument(file_kompas, true);
												this.m_IStamp = (ksStamp)IDrawing.GetStamp();
                                                IDocAPIVer7 = null;
												break;
											case KompasFileType.Текстовый:
												// Получить указатель на интерфейс текстового файла
												IText = (ksDocumentTxt)this.m_KompasApplication.DocumentTxt();
												if (IText == null) {
													m_Logger.Error("Не удалось получить интерфейс текстового файла");
													MessageBox.Show("Некоторые файлы или данные приложения КОМПАС-3D не найдены или повреждены. Попробуйте переустановить данное ПО. " +
																	"Если проблема повториться - обратитесь в тех. поддержку.", "Ошибка запуска приложения КОМПАС-3D",
																	MessageBoxButton.OK, MessageBoxImage.Exclamation);
													return;
												}
												// Открыть файл в фоне (пользователь его не будет видеть)
												IText.ksOpenDocument(file_kompas, 1);
												this.m_IStamp = (ksStamp)IText.GetStamp();
                                                IDocAPIVer7 = null;
												break;
											case KompasFileType.Спецификация:
												// Получить указатель на интерфейс документа-спецификации
												ISpecification = (ksSpcDocument)this.m_KompasApplication.SpcDocument();
												if (ISpecification == null) {
													m_Logger.Error("Не удалось получить интерфейс документа-спецификации");
													MessageBox.Show("Некоторые файлы или данные приложения КОМПАС-3D не найдены или повреждены. Попробуйте переустановить данное ПО. " +
																	"Если проблема повториться - обратитесь в тех. поддержку.", "Ошибка запуска приложения КОМПАС-3D",
																	MessageBoxButton.OK, MessageBoxImage.Exclamation);
													return;
												}
												// Открыть файл в фоне (пользователь его не будет видеть)
												ISpecification.ksOpenDocument(file_kompas, 1);
												this.m_IStamp = (ksStamp)ISpecification.GetStamp();
                                                IDocAPIVer7 = null;
												break;

                                            case KompasFileType.Модель:
                                                // Получить указатель на интерфейс модели
                                                IModel = (ksDocument3D)this.m_KompasApplication.Document3D();
                                                if (IModel == null)
                                                {
                                                    m_Logger.Error("Не удалось получить интерфейс графического документа (модели)");
                                                    MessageBox.Show("Некоторые файлы или данные приложения КОМПАС-3D не найдены или повреждены. Попробуйте переустановить данное ПО. " +
                                                                    "Если проблема повториться - обратитесь в тех. поддержку.", "Ошибка запуска приложения КОМПАС-3D",
                                                                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                                    return;
                                                }
                                                // Открыть файл в фоне (пользователь его не будет видеть)
                                                IModel.Open(file_kompas, true);
                                                // Получить указатель на интерфейс текущего активного документа для API версии 7
                                                IDocAPIVer7 = this.m_IKompasApplicationAPIVer7.ActiveDocument as IKompasDocument;
												this.m_IStamp = null;
                                                break;
										}
										if (this.m_Worker.CancellationPending == true) {
											m_Logger.Info("Фоновая операция была прервана!");
											this.m_Worker = null;
											try {
												this.m_KompasApplication.Quit();
											}
											// Игнорировать ошибки при попытке закрыть приложение КОМПАС-3D 
											// (приложение ждет ответа от пользователя или производит какие-либо операции, 
											// но после этого все равно закроется)
											catch {
											}
											m_Logger.Info("Приложение КОМПАС-3D закрыто");
											return;
										}
                                        // Если интерфейс штампа получен и удалось открыть штамп документа или получен интерфейс текущего активного документа для API версии 7, то
                                        if (this.m_IStamp != null && this.m_IStamp.ksOpenStamp() == 1 || IDocAPIVer7 != null)
                                        {
											try {
                                                string name;
                                                // Если получение информации из документа идёт с помощью штампа, то
                                                if (IDocAPIVer7 == null)
												    // Получить текстовую информацию из графы штампа с номером 1 (наименование по ГОСТ 2.104-2006)
												    name = this.GetTextColumnStamp(1);
                                                else
                                                    // Получить текстовую информацию из свойства с номером 5.0 (наименование)
                                                    name = this.GetTextPropertyById(5.0, IDocAPIVer7);
												// Устраняем переходы в новую строку - если в графе было несколько строк
												name = name.Replace("\n", " ");
												if (this.m_Worker.CancellationPending == true) {
													m_Logger.Info("Фоновая операция была прервана!");
													this.m_Worker = null;
													try {
														this.m_KompasApplication.Quit();
													}
													// Игнорировать ошибки при попытке закрыть приложение КОМПАС-3D 
													// (приложение ждет ответа от пользователя или производит какие-либо операции, 
													// но после этого все равно закроется)
													catch {
													}
													m_Logger.Info("Приложение КОМПАС-3D закрыто");
													return;
												}
                                                string dec_number;
                                                 // Если получение информации из документа идёт с помощью штампа, то
                                                if (IDocAPIVer7 == null)
                                                    // Получить текстовую информацию из графы штампа с номером 2 (обозначение документа по ГОСТ 2.104-2006)
												    dec_number = this.GetTextColumnStamp(2);
                                                else
                                                    // Получить текстовую информацию из свойства с номером 4.0 (обозначение)
                                                    dec_number = this.GetTextPropertyById(4.0, IDocAPIVer7);
												// Устраняем переходы в новую строку - если в графе было несколько строк
												dec_number = dec_number.Replace("\n", " ");
												if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(dec_number))
													if (string.IsNullOrWhiteSpace(name)) {
														new_name_file = old_name_file + "(нет наименования)";
														is_error_name = true;
													}
													else {
														new_name_file = old_name_file + "(нет обозначения документа)";
														is_error_name = true;
													}
												else {
													if (app_data.FirstPartNameFile == "Обозначение документа")
														new_name_file = dec_number.Trim() + " - " + name.Trim();
													else
														new_name_file = name.Trim() + " - " + dec_number.Trim();
													// Устраняем слэши, заменяя их на дефис
													new_name_file = new_name_file.Replace('/', '-');
													new_name_file = new_name_file.Replace('\\', '-');
													// Заменяем елочки и ковычки на фигурные скобки
													new_name_file = new_name_file.Replace("<<", "{");
													new_name_file = new_name_file.Replace('<', '{');
													new_name_file = new_name_file.Replace(">>", "}");
													new_name_file = new_name_file.Replace('>', '}');
													char[] new_name_file_to_char_array = new_name_file.ToCharArray();
													while (new_name_file.IndexOf('\"') != -1) {
														new_name_file_to_char_array[new_name_file.IndexOf('\"')] = '{';
														new_name_file = new string(new_name_file_to_char_array);
														if (new_name_file.IndexOf('\"') != -1) {
															new_name_file_to_char_array[new_name_file.IndexOf('\"')] = '}';
															new_name_file = new string(new_name_file_to_char_array);
														}
														else {
															new_name_file.Remove(new_name_file.LastIndexOf("{"));
															new_name_file_to_char_array = new_name_file.ToCharArray();
														}
													}
													while (new_name_file.IndexOf('\'') != -1) {
														new_name_file_to_char_array[new_name_file.IndexOf('\'')] = '{';
														new_name_file = new string(new_name_file_to_char_array);
														if (new_name_file.IndexOf('\'') != -1) {
															new_name_file_to_char_array[new_name_file.IndexOf('\'')] = '}';
															new_name_file = new string(new_name_file_to_char_array);
														}
														else
															new_name_file.Remove(new_name_file.LastIndexOf('{'));
													}
													// Заменяем несколько подряд идущих пробелов одним
													new_name_file = new Regex(@"\s+").Replace(new_name_file, " ");
												}

											}
											catch (NullReferenceException) {
                                                // Если получение информации из документа шло с помощью штампа, то
                                                if (IDocAPIVer7 == null) {
                                                    m_Logger.Error("У файла " + file_kompas + " нет штампа");
                                                    new_name_file = old_name_file + "(нет штампа)";
                                                }
                                                else {
                                                    m_Logger.Error("У файла " + file_kompas + " нет свойств документа");
                                                    new_name_file = old_name_file + "(нет свойств)";
                                                }
												is_error_name = true;
											}
											// Если получение информации из документа шло с помощью штампа, то
											if (IDocAPIVer7 == null)
												this.m_IStamp.ksCloseStamp();
										}
										else {
											m_Logger.Error("Возникла одна или более ошибок при открытии файла " + file_kompas + " приложением КОМПАС-3D");
											new_name_file = old_name_file + "(ошибка чтения)";
											is_error_name = true;
										}
										switch (type) {
											case KompasFileType.Чертеж: IDrawing.ksCloseDocument(); break;
											case KompasFileType.Текстовый: IText.ksCloseDocument(); break;
											case KompasFileType.Спецификация: ISpecification.ksCloseDocument(); break;
                                            case KompasFileType.Модель: IModel.close(); break;
										}
										if (this.m_Worker.CancellationPending == true) {
											m_Logger.Info("Фоновая операция была прервана!");
											this.m_Worker = null;
											try {
												this.m_KompasApplication.Quit();
											}
											// Игнорировать ошибки при попытке закрыть приложение КОМПАС-3D 
											// (приложение ждет ответа от пользователя или производит какие-либо операции, 
											// но после этого все равно закроется)
											catch {
											}
											m_Logger.Info("Приложение КОМПАС-3D закрыто");
											return;
										}
									}
									catch (COMException exc) {
										m_Logger.Error("Возникла одна или более ошибок на уровне API КОМПАС-3D. Подробнее: код " + exc.ErrorCode +
													   " " + exc.Message + " в " + exc.Source + " " + exc.StackTrace);
										MessageBox.Show("Некоректная работа приложения КОМПАС-3D. Попробуйте закрыть все экземпляры КОМПАС-3D и " +
														"перезапустить данное приложение. Возможно файлы приложения КОМПАС-3D были повреждены и " +
														"требуется его переустановка. Если проблема повториться - обратитесь в тех. поддержку.",
														"Ошибка приложения КОМПАС-3D", MessageBoxButton.OK, MessageBoxImage.Exclamation);
										return;
									}
								}
								if (this.m_Worker.CancellationPending == true) {
									m_Logger.Info("Фоновая операция была прервана!");
									this.m_Worker = null;
									try {
										if (this.m_KompasApplication != null) {
											this.m_KompasApplication.Quit();
											m_Logger.Info("Приложение КОМПАС-3D закрыто");
										}
									}
									// Игнорировать ошибки при попытке закрыть приложение КОМПАС-3D 
									// (приложение ждет ответа от пользователя или производит какие-либо операции, 
									// но после этого все равно закроется)
									catch {
										m_Logger.Info("Приложение КОМПАС-3D закрыто");
									}
									return;
								}
								try {
									if (new_name_file != old_name_file) {
										((BackgroundWorker)sender).ReportProgress((int)Math.Round(current_percent_process), "Переименование " + file_kompas + " на " + IOPath.Combine(IOPath.GetDirectoryName(file_kompas), new_name_file + IOPath.GetExtension(file_kompas).ToLower()));
										// Переименование файла
										File.Move(file_kompas, IOPath.Combine(IOPath.GetDirectoryName(file_kompas), new_name_file + IOPath.GetExtension(file_kompas).ToLower()));
										switch (type) {
											case KompasFileType.Текстовый: ++text_counter; break;
											case KompasFileType.Фрагмент: ++fragment_counter; break;
											case KompasFileType.Чертеж: ++drawing_counter; break;
											case KompasFileType.Спецификация: ++specification_counter; break;
                                            case KompasFileType.Модель: ++model_counter; break;
										}
									}
									else
										++ignored_counter;

								}
								catch (ArgumentException) {
									m_Logger.Error("Новое имя " + new_name_file + " файла " + file_kompas + " содержит недопустимые знаки, определенные в InvalidPathChars");
									new_name_file = old_name_file + "(ошибка переименования)";
									is_error_name = true;
									((BackgroundWorker)sender).ReportProgress((int)Math.Round(current_percent_process), "Переименование " + file_kompas + " на " + IOPath.Combine(IOPath.GetDirectoryName(file_kompas), new_name_file + IOPath.GetExtension(file_kompas).ToLower()));
									try {
										File.Move(file_kompas, IOPath.Combine(IOPath.GetDirectoryName(file_kompas), new_name_file + IOPath.GetExtension(file_kompas).ToLower()));
									}
									catch (UnauthorizedAccessException) {
										m_Logger.Error("У пользователя нет прав для переименования файла " + file_kompas);
										MessageBox.Show("Вы не обладаете достаточными правами доступа для этой директории. " +
														"Обратитесь к администратору или запустите программу от его имени.",
														"Отсутствуют необходимые права доступа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
										return;

									}
									++error_copy_counter;
								}
								catch (UnauthorizedAccessException) {
									m_Logger.Error("У пользователя нет прав для переименования файла " + file_kompas);
									MessageBox.Show("Вы не обладаете достаточными правами доступа для этой директории. " +
													"Обратитесь к администратору или запустите программу от его имени.",
													"Отсутствуют необходимые права доступа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
									return;

								}
								catch (PathTooLongException) {
									m_Logger.Error("Длинна нового имени " + new_name_file + " файла " + file_kompas + " превышает установленное в системе максимальное значение");
									new_name_file = old_name_file + "(ошибка переименования)";
									is_error_name = true;
									((BackgroundWorker)sender).ReportProgress((int)Math.Round(current_percent_process), "Переименование " + file_kompas + " на " + IOPath.Combine(IOPath.GetDirectoryName(file_kompas), new_name_file + IOPath.GetExtension(file_kompas).ToLower()));
									try {
										string t = IOPath.Combine(IOPath.GetDirectoryName(file_kompas), new_name_file + IOPath.GetExtension(file_kompas).ToLower());
										File.Move(file_kompas, IOPath.Combine(IOPath.GetDirectoryName(file_kompas), new_name_file + IOPath.GetExtension(file_kompas).ToLower()));
									}
									catch (UnauthorizedAccessException) {
										m_Logger.Error("У пользователя нет прав для переименования файла " + file_kompas);
										MessageBox.Show("Вы не обладаете достаточными правами доступа для этой директории. " +
														"Обратитесь к администратору или запустите программу от его имени.",
														"Отсутствуют необходимые права доступа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
										return;

									}
									++error_copy_counter;
								}
								catch (IOException exc) {
									m_Logger.Error("Возникла ошибка при переименовании файла " + old_name_file + ". Причина: " + exc.Message);
									new_name_file = old_name_file + "(ошибка переименования)";
									is_error_name = true;
									((BackgroundWorker)sender).ReportProgress((int)Math.Round(current_percent_process), "Переименование " + file_kompas + " на " + IOPath.Combine(IOPath.GetDirectoryName(file_kompas), new_name_file + IOPath.GetExtension(file_kompas).ToLower()));
									try {
										File.Move(file_kompas, IOPath.Combine(IOPath.GetDirectoryName(file_kompas), new_name_file + IOPath.GetExtension(file_kompas).ToLower()));
									}
									catch (UnauthorizedAccessException) {
										m_Logger.Error("У пользователя нет прав для переименования файла " + file_kompas);
										MessageBox.Show("Вы не обладаете достаточными правами доступа для этой директории. " +
														"Обратитесь к администратору или запустите программу от его имени.",
														"Отсутствуют необходимые права доступа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
										return;

									}
									++error_copy_counter;
								}
								m_Logger.Info("Файл " + old_name_file + IOPath.GetExtension(file_kompas) + " переименован в " + new_name_file + IOPath.GetExtension(file_kompas).ToLower());
								report_all_writer.WriteLine(type.ToString() + "\t" + old_name_file + "\t" + new_name_file + "\t" + IOPath.GetDirectoryName(file_kompas));
								// Если были ошибки при формировании имени файла или его копировании, то
								if (is_error_name)
									report_error_writer.WriteLine(type.ToString() + "\t" + old_name_file + "\t" + new_name_file + "\t" + IOPath.GetDirectoryName(file_kompas));
								if (this.m_Worker.CancellationPending == true) {
									m_Logger.Info("Фоновая операция была прервана!");
									this.m_Worker = null;
									try {
										if (this.m_KompasApplication != null) {
											this.m_KompasApplication.Quit();
											m_Logger.Info("Приложение КОМПАС-3D закрыто");
										}
									}
									// Игнорировать ошибки при попытке закрыть приложение КОМПАС-3D 
									// (приложение ждет ответа от пользователя или производит какие-либо операции, 
									// но после этого все равно закроется)
									catch {
										m_Logger.Info("Приложение КОМПАС-3D закрыто");
									}
									return;
								}
								current_percent_process += percent_in_one_file;
							}
							((BackgroundWorker)sender).ReportProgress((int)Math.Round(current_percent_process), "Завершение фоновых операций");
							m_Logger.Info("Обработка завершена! Обработано " + (drawing_counter + specification_counter + model_counter + fragment_counter + text_counter).ToString() + " из " + files_kompas.Length.ToString() + " файлов: " +
										  "Чертежи - " + drawing_counter + " " +
										  "Спецификации - " + specification_counter + " " +
										  "Текстовые - " + text_counter + " " +
                                          "Модели - " + model_counter + " " +
										  "Фрагменты - " + fragment_counter + " " +
										  "Неопознанный тип файла - " + unidentified_counter + " " +
										  "Пропущено - " + ignored_counter + " " +
										  "Ошибки копирования - " + error_copy_counter);
						}
					}
					catch (System.Security.SecurityException) {
						m_Logger.Error("У пользователя нет прав для создания или изменения файла отчета ошибок " + report_all_file);
						MessageBox.Show("Вы не обладаете достаточными правами доступа для этой директории. " +
										"Обратитесь к администратору или запустите программу от его имени.",
										"Отсутствуют необходимые права доступа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
						return;
					}
					catch (DirectoryNotFoundException) {
						m_Logger.Error("Каталог " + IOPath.GetDirectoryName(report_error_file) + " не найден");
						MessageBox.Show("Указанный путь не найден. Проверте наличие подключенного, готового к работе, " +
										"устройства и указанного каталога в пути к файлам.", "Каталог не найден",
										MessageBoxButton.OK, MessageBoxImage.Exclamation);
						return;
					}
					catch (IOException exc) {
						m_Logger.Error("Произошла ошибка ввода-вывода при записи данных в отчет ошибок " + report_error_file + ". Подробней: " + exc.Message + " " + exc.StackTrace);
						MessageBox.Show("Произошла ошибка ввода-вывода. Обратитесь в тех. поддержку.", "Ошибка ввода-вывода",
										MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}
				}
			}
			catch (System.Security.SecurityException) {
				m_Logger.Error("У пользователя нет прав для создания или изменения файла полного отчета " + report_all_file);
				MessageBox.Show("Вы не обладаете достаточными правами доступа для этой директории. " +
								"Обратитесь к администратору или запустите программу от его имени.",
								"Отсутствуют необходимые права доступа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			catch (DirectoryNotFoundException) {
				m_Logger.Error("Каталог " + IOPath.GetDirectoryName(report_all_file) + " не найден");
				MessageBox.Show("Указанный путь не найден. Проверте наличие подключенного, готового к работе, " +
								"устройства и указанного каталога в пути к файлам.", "Каталог не найден",
								MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			catch (IOException exc) {
				m_Logger.Error("Произошла ошибка ввода-вывода при записи данных в полный отчет " + report_all_file + ". Подробней: " + exc.Message + " " + exc.StackTrace);
				MessageBox.Show("Произошла ошибка ввода-вывода. Обратитесь в тех. поддержку.", "Ошибка ввода-вывода",
								MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			MessageBox.Show("Систематизация файлов завершена! Исходные файлы находятся в " + path_sources +
							". Полный отчет находится в " + report_all_file + ". Отчет ошибок находится в " +
							report_error_file + ".", "Систематизация завершена",
							MessageBoxButton.OK, MessageBoxImage.Information);
		}

		/// <summary>
		/// Обработчик клика левой кнопки мыши по кнопке "Выход"
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий информацию о состоянии и данные события</param>
		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Обработчик клика левой кнопки мыши по кнопке "Очистить поле"
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий информацию о состоянии и данные события</param>
		private void ButtonClear_Click(object sender, RoutedEventArgs e)
		{
			if (this.PathFilesKompas.Text != this.m_DefaultTextPath) {
				// Изменение цвета текста на серый
				this.PathFilesKompas.Opacity = 0.5;
				this.PathFilesKompas.Text = this.m_DefaultTextPath;
			}
		}

		/// <summary>
		/// Обработчик изменения значения в списке, содержащем возможные варианты имени первой части файла
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий данные события</param>
		private void FirstPartNameFile_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.FirstPartNameFile.SelectedIndex == 0)
				this.SecondPartNameFile.SelectedIndex = 1;
			else
				this.SecondPartNameFile.SelectedIndex = 0;
		}

		/// <summary>
		/// Обработчик изменения значения в списке, содержащем возможные варианты имени второй части файла
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий данные события</param>
		private void SecondPartNameFile_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.SecondPartNameFile.SelectedIndex == 0)
				this.FirstPartNameFile.SelectedIndex = 1;
			else
				this.FirstPartNameFile.SelectedIndex = 0;
		}

		/// <summary>
		/// Обработчик события закрытия данного окна
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий данные события</param>
		private void Window_Closed(object sender, EventArgs e)
		{
			try {
				// Если запущен фоновый поток, то
				if (this.m_Worker != null) {
					// Запросить его отмену
					this.m_Worker.CancelAsync();
					// Пока запущен фоновый поток
					while (this.m_Worker != null)
						System.Threading.Thread.Sleep(100);
				}
				else
					// Если запущено приложение КОМПАС-3D
					if (this.m_KompasApplication != null) {
						// Послать ему запрос на закрытие
						this.m_KompasApplication.Quit();
						m_Logger.Info("Приложение КОМПАС-3D закрыто");
					}
			}
			// Игнорировать ошибки при попытке закрыть приложение КОМПАС-3D 
			// (приложение ждет ответа от пользователя или производит какие-либо операции, 
			// но после этого все равно закроется)
			catch {
				m_Logger.Info("Приложение КОМПАС-3D закрыто");
			}
			m_Logger.Info("Приложение закрыто");
		}

		/// <summary>
		/// Обработчик получения фокуса текстовым полем, содержащим путь к систематизируемым файлам КОМПАС-3D
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий информацию о состоянии и данные события</param>
		private void PathFilesKompas_GotFocus(object sender, RoutedEventArgs e)
		{
			if (PathFilesKompas.Text == this.m_DefaultTextPath) {
				// Изменение цвета текста на чёрный
				this.PathFilesKompas.Opacity = 1.0;
				this.PathFilesKompas.Text = string.Empty;
			}
		}

		/// <summary>
		/// Обработчик потери фокуса текстовым полем, содержащим путь к систематизируемым файлам КОМПАС-3D
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий информацию о состоянии и данные события</param>
		private void PathFilesKompas_LostFocus(object sender, RoutedEventArgs e)
		{
			if (PathFilesKompas.Text.Length == 0) {
				// Изменение цвета текста на серый
				this.PathFilesKompas.Opacity = 0.5;
				this.PathFilesKompas.Text = this.m_DefaultTextPath;
			}
		}

		/// <summary>
		/// Обработчик нажатия клавиш клавиатуры при активном главном окне приложения
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий данные для события</param>
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key) {
				case Key.F1: this.CallHelpFile(); break;
				default: break;
			}
		}
		#endregion

		#region Вспомогательные методы
		/// <summary>
		/// Возвращает текстовую информацию из указанной графы штампа
		/// </summary>
		/// <param name="ColumnNumber">Номер графы штампа, из которой необходимо извлеч текстовую информацию</param>
		/// <returns>Текстовая информация, извлеченная из указанной графы штампа</returns>
		/// <exception cref="System.ArgumentException">Указанный номер графы штампа меньше 1</exception>
        /// <exception cref="System.ArgumentNullException">Интерфейс штампа равен null</exception>
        /// <exception cref="System.NullReferenceException">Не удалось получить текстовые данные указанной графы штампа</exception>
		/// <exception cref="System.Runtime.InteropServices.COMException">Возникла одна или более ошибок на уровне API КОМПАС-3D</exception>
        /// <remarks>Данный метод не явным образом использует свойство, отвечающее за интерфейс штампа</remarks>
        private string GetTextColumnStamp(int ColumnNumber)
		{
			if (ColumnNumber < 1)
                throw new System.ArgumentException("Номер графы штампа не может быть меньше 1", "ColumnNumber");
			if (this.m_IKompasApplicationAPIVer7 == null)
				throw new System.ArgumentNullException("this.m_IStamp", "Интерфейс штампа равен null");
			// Выбрать графу штампа с указанным номером
			this.m_IStamp.ksColumnNumber(ColumnNumber);
			// Получить текстовые данные указанной графы штампа
			ksDynamicArray text_column = (ksDynamicArray)this.m_IStamp.ksGetStampColumnText(ref ColumnNumber);
			if (text_column == null) {
				m_Logger.Error("Не удалось получить текстовые данные графы штампа номер " + ColumnNumber + " " + Environment.StackTrace);
				throw new System.NullReferenceException("Не удалось получить текстовые данные графы штампа номер " + ColumnNumber);
			}
			// Получить указатель на интерфейс структуры параметров строки текстовых данных графы
			ksTextLineParam line_text_column = (ksTextLineParam)this.m_KompasApplication.GetParamStruct((short)StructType2DEnum.ko_TextLineParam);
			if (line_text_column == null) {
				m_Logger.Error("Не удалось получить интерфейс структуры параметров строки текстовых данных графы " + Environment.StackTrace);
				throw new System.Runtime.InteropServices.COMException("Не удалось получить интерфейс структуры параметров строки текстовых данных графы");
			}
			// Инициализировать параметры строки
			line_text_column.Init();
			string result = string.Empty;
			// Пройти все строки в указанной графе
			for (int i = 0; i < text_column.ksGetArrayCount(); ++i) {
				// Извлеч очередную строку из графы
				text_column.ksGetArrayItem(i, line_text_column);
				// Получить массив данных строки
				ksDynamicArray array_text_item_line = (ksDynamicArray)line_text_column.GetTextItemArr();
				// Текстовый элемент строки графы
				ksTextItemParam text_item = null;
				// Получить указатель на интерфейс структуры параметров текстового элемента строки
				text_item = (ksTextItemParam)this.m_KompasApplication.GetParamStruct((short)StructType2DEnum.ko_TextItemParam);
				if (text_item == null || array_text_item_line == null) {
					result += "\n";
					continue;
				}
				// Инициализировать параметры элемента строки
				text_item.Init();
				// Пройти все элементы строки
				for (int j = 0; j < array_text_item_line.ksGetArrayCount(); ++j) {
					// Извлеч очередной элемент строки
					array_text_item_line.ksGetArrayItem(j, text_item);
					// Добавить текст элемента к результату 
					result += text_item.s;
				}
				if (i < text_column.ksGetArrayCount()-1)
					result += "\n";
				// Очистка массива текстовых данных строки - рекомендовано в SDK
				array_text_item_line.ksDeleteArray();  
			}
			// Очистка массива текстовых данных графы - рекомендовано в SDK
			text_column.ksDeleteArray();
			return result;
		}

        /// <summary>
        /// Возвращает текстовую информацию из указанного свойства
        /// </summary>
		/// <param name="PropertyId">Идентификатор свойства, из которой необходимо извлеч текстовую информацию</param>
        /// <returns>Текстовая информация, извлеченная из указанного свойства</returns>
		/// <exception cref="System.ArgumentException">Указанный идентификатор свойства меньше 1</exception>
        /// <exception cref="System.ArgumentNullException">Интерфейс текущего активного документа для API версии 7 равен null</exception>
		/// <exception cref="System.NullReferenceException">Не удалось получить текстовые данные указанного свойства или свойства с указанным идентификатором не существует</exception>
        /// <exception cref="System.Runtime.InteropServices.COMException">Возникла одна или более ошибок на уровне API КОМПАС-3D</exception>

        private string GetTextPropertyById(double PropertyId, IKompasDocument IDocumentAPIVer7)
        {
            if (PropertyId < 1.0)
				throw new System.ArgumentException("Идентификатор свойства не может быть меньше 1", "PropertyId");

            if (IDocumentAPIVer7 == null)
                throw new System.ArgumentNullException("IDocumentAPIVer7", "Интерфейс текущего активного документа для API версии 7 равен null");

            IPropertyMng property_manager = null;
			if (IDocumentAPIVer7.Application is IPropertyMng)
				property_manager = (IPropertyMng)IDocumentAPIVer7.Application;
			else {
				m_Logger.Error("Не удалось получить интерфейс менеджера для работы со свойствами " + Environment.StackTrace);
				throw new System.Runtime.InteropServices.COMException("Не удалось получить интерфейс менеджера для работы со свойствами");
			}

			int current_num_property=0;
			IProperty property = null;
			while (current_num_property<property_manager.PropertyCount[IDocumentAPIVer7]) {
				property = property_manager.GetProperty(IDocumentAPIVer7, current_num_property);
				if (property.Id == PropertyId)
					break;
				++current_num_property;
			}

			if (current_num_property == property_manager.PropertyCount[IDocumentAPIVer7]) {
				m_Logger.Error("Cвойства с идентификатором " + PropertyId + " не существует! " + Environment.StackTrace);
				throw new System.NullReferenceException("Cвойства с идентификатором " + PropertyId + " не существует!");
			}

			IPropertyKeeper property_keeper = null;
			// Если документ - модель или сборка, то
			if (IDocumentAPIVer7 is IKompasDocument3D)
				// Получить хранитель свойств для верхнего компонента
				property_keeper = (IPropertyKeeper)(IPart7)((IKompasDocument3D)IDocumentAPIVer7).TopPart;
			else
				// Если документ - графический (чертеж)
				if (IDocumentAPIVer7 is IKompasDocument2D1)
					// Получить для него хранитель свойств
					property_keeper = (IPropertyKeeper)(IKompasDocument2D1)IDocumentAPIVer7;
				else {
					m_Logger.Error("Не удалось получить интерфейс хранителя свойств для данного документа " + Environment.StackTrace);
					throw new System.Runtime.InteropServices.COMException("Не удалось получить интерфейс хранителя свойств для данного документа");
				}
            object result;
            bool from_source;
            // Получаем значение свойства в единицах СИ (в данном случае не важно, т.к. получаем текст)
			bool not_error = property_keeper.GetPropertyValue((_Property)property, out result, true, out from_source);
            if (not_error)
                return result.ToString();
            else
            {
				m_Logger.Error("Не удалось получить текстовые данные свойства с идентификатором " + PropertyId + " " + Environment.StackTrace);
				throw new System.NullReferenceException("Не удалось получить текстовые данные свойства с идентификатором " + PropertyId);
            }
        }

		/// <summary>
		/// Выполняет вызов файла-справки
		/// </summary>
		private void CallHelpFile()
		{
			string path_help = IOPath.Combine(IOPath.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "MainHelp.pdf");
			try {
				System.Diagnostics.Process.Start(path_help);
			}
			catch (Win32Exception exc) {
				m_Logger.Error("Произошла ошибка при открытии связанного файла " + path_help + ". Подробней: " + exc.Message + " " + exc.StackTrace);
				MessageBox.Show("Произошла ошибка при чтении файла справки. Возможно он был удален или поврежден. Проверьте его наличие и целостность в " + path_help + ". Возможно у Вас отсутствует программа для чтения pdf документов.", "Ошибка чтения файла справки",
								MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			catch (FileNotFoundException) {
				m_Logger.Error("Переменная среды PATH содержит строку с кавычками. " + Environment.StackTrace);
				MessageBox.Show("Неверный формат переменной среды PATH - одна или несколько строк содержат кавычки. Обратитесь к администратору.", "Ошибка чтения файла справки",
								MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
		}
		#endregion	
		

	}		
}
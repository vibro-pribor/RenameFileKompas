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
using System.Windows.Shapes;
using System.ComponentModel;
using NLog;

namespace VP
{
	/// <summary>
	/// Логика взаимодействия для Progress.xaml
	/// </summary>
	public partial class ProgressTask : Window
	{

		#region Поля
		/// <summary>
		/// Логирование
		/// </summary>
		private static readonly Logger m_Logger = LogManager.GetCurrentClassLogger();
		#endregion

		/// <summary>
		/// Создает новое окно отображения хода выполнения процесса
		/// </summary>
		/// <param name="MaxValueProgressBar">Максимальное значение шкалы хода выполнения процесса</param>
		/// <param name="MinValueProgressBar">Минимальное значение шкалы хода выполнения процесса</param>
		/// <param name="StartValueProgressBar">Начальное значение шкалы хода выполнения процесса</param>
		public ProgressTask(double MaxValueProgressBar, double MinValueProgressBar=0, double StartValueProgressBar=0)
		{
			InitializeComponent();
			this.ProgressLine.Maximum = MaxValueProgressBar;
			this.ProgressLine.Minimum = MinValueProgressBar;
			this.ProgressLine.Value = StartValueProgressBar;
		}

		#region Обработчики событий
		/// <summary>
		/// Обработчик события ProgressChanged от BackgroundWorker
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий данные для события</param>
		/// <exception cref="System.ArgumentNullException">Данные события или пользовательское состояние имеют значение null</exception>
		/// <remarks>Выводимый текст текущей операции передается через объект состояния</remarks>
		public void ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e == null || e.UserState == null) {
				m_Logger.Error("Данные события и пользовательское состояние не могут иметь значение null " + Environment.StackTrace);
				throw new System.ArgumentNullException("Данные события и пользовательское состояние не могут иметь значение null");
			}
			this.SetValueProgressBar(e.ProgressPercentage);
			if (!(e.UserState is string)) {
				m_Logger.Error("Не существует способа преобразования пользовательского состояния в System.String " + Environment.StackTrace);
				throw new System.InvalidCastException("Не существует способа преобразования пользовательского состояния в System.String");
			}
			// Через UserState передается текст текущей выполняемой операции
			this.ProgressComment.Text = (string)e.UserState;
		}

		/// <summary>
		/// Обработчик события RunWorkerCompleted от BackgroundWorker
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий данные для события</param>
		public void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.DialogResult = true;
		}

		/// <summary>
		/// Обработчик события закрытия данного окна
		/// </summary>
		/// <param name="sender">Объект, создавший событие</param>
		/// <param name="e">Объект, содержащий данные для события</param>
		private void Window_Closing(object sender, CancelEventArgs e)
		{
			// Если результат работы данного диалогового окна отсутствует (закрытие данного окна пользователем), то
			if (this.DialogResult == null)
				// Отменить закрытие данного окна
				e.Cancel = true;
		}
		#endregion

		#region Методы
		/// <summary>
		/// Добавляет к значению шкалы ProgressBar указанное значение
		/// </summary>
		/// <param name="AddingValue">Добавляемое значение</param>
		public void AddValueProgressBar(double AddingValue) 
		{
			Dispatcher.Invoke(new Action<ProgressBar, double>((ProgressLine, Value) => ProgressLine.Value = Value), this.ProgressLine, this.ProgressLine.Value + AddingValue);
		}

		/// <summary>
		/// Устанавливает значение шкалы ProgressBar равное указанному
		/// </summary>
		/// <param name="NewValue">Устанавливаемое значение</param>
		public void SetValueProgressBar(double NewValue) 
		{
			Dispatcher.Invoke(new Action<ProgressBar, double>((ProgressLine, Value) => ProgressLine.Value = Value), this.ProgressLine, NewValue);
		}
		#endregion	
	}
}

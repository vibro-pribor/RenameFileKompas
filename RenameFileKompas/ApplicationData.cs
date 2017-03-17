using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VP.RenameFileKompas
{
	/// <summary>
	/// Класс, содержащий данные приложения
	/// </summary>
	public class ApplicationData
	{
		#region Свойства
		/// <summary>
		/// Получает или задает путь к систематизируемым файлам приложения КОМПАС-3D
		/// </summary>
		public string PathFilesKompas { get; set; }

		/// <summary>
		/// Получает или задает то, что нужно формировать в первой части нового имени файла
		/// </summary>
		public string FirstPartNameFile { get; set; }

		/// <summary>
		/// Получает или задает то, что нужно формировать во второй части нового имени файла
		/// </summary>
		public string SecondPartNameFile { get; set; }
		#endregion

		/// <summary>
		/// Создает объект, содержащий данные приложения, и инициализирует все его параметры пустыми строками
		/// </summary>
		public ApplicationData()
		{
			this.SecondPartNameFile = this.FirstPartNameFile = this.PathFilesKompas = string.Empty;
			
		}
	}
}

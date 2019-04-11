using System;
using System.Collections.Generic;

namespace InterfaceImplementationLibrary
{
	public sealed class Person
	{
		/// <summary>
		/// Персона с ФИО, датой рождения и датой смерти (опционально).
		/// </summary>
		/// <param name="fullName">Полное имя персоны (фамилия, имя и отчество, записанные через пробел).</param>
		/// <param name="birthDay">Дата и время (опционально) рождения по новому стилю.</param>
		/// <param name="deathDay">Дата и время (опционально) смерти по новому стилю.</param>
		/// <exception cref="ArgumentNullException"/>
		/// <exception cref="ArgumentOutOfRangeException"/>
		public Person(string fullName, DateTime birthDay, DateTime? deathDay = null)
		{
			FullName = fullName;
			BirthDay = birthDay;
			DeathDay = deathDay;
		}

		/// <summary>
		/// Статический метод разбора. По строковому значению получаем персона.
		/// </summary>
		/// <param name="fullName">Полное имя персоны (фамилия, имя и отчество, записанные через пробел).</param>
		/// <param name="birthDay">Строковое представление даты и времени (опционально) рождения по новому стилю.</param>
		/// <param name="deathDay">Строковое представление даты и времени (опционально) смерти по новому стилю.</param>
		/// <returns>Персона, построенная по набору строк.</returns>
		/// <exception cref="ArgumentNullException"/>
		/// <exception cref="ArgumentOutOfRangeException"/>
		public static Person Parse(string fullName, string birthDay, string deathDay = null)
		{
			DateTime? deathDate = null;
			if (deathDay != null)
				deathDate = DateTime.Parse(deathDay);

			return new Person(fullName, DateTime.Parse(birthDay), deathDate);
		}

		public string FullName
		{
			get => _fullName;
			private set
			{
				if (value is null)
					throw new ArgumentNullException(nameof(FullName));

				var trimmedValue = value.Trim();
				if (trimmedValue.Length == 0)
					throw new ArgumentOutOfRangeException(nameof(FullName));

				_fullName = trimmedValue;
			}
		}

		public DateTime BirthDay
		{
			get => _birthDay;
			private set
			{
				if (value > DateTime.Now)
					throw new ArgumentOutOfRangeException(nameof(BirthDay));

				_birthDay = value;
			}
		}

		public DateTime? DeathDay
		{
			get => _deathDay;
			private set
			{
				if (value.HasValue && value <= _birthDay)
					throw new ArgumentOutOfRangeException(nameof(BirthDay));

				_deathDay = value;
			}
		}

		public int Age => (DeathDay ?? DateTime.Now).Year - BirthDay.Year;

		public override string ToString()
		{
			return DeathDay.HasValue
				? $"{FullName} ({BirthDay.ToLongDateString()} — {DeathDay.Value.ToLongDateString()})"
				: $"{FullName} ({BirthDay.ToLongDateString()})";
		}

		private string _fullName;

		private DateTime _birthDay;

		private DateTime? _deathDay;

		public sealed class AgeComparer : IComparer<Person>
		{
			public int Compare(Person x, Person y) => x?.Age ?? 0 - y?.Age ?? 0;
		}

		public sealed class BirthDayComparer : IComparer<Person>
		{
			/// <exception cref="ArgumentNullException"></exception>
			public int Compare(Person firstPerson, Person secondPerson)
			{
				if (firstPerson is null)
					throw new ArgumentNullException(nameof(firstPerson));

				if (secondPerson is null)
					throw new ArgumentNullException(nameof(secondPerson));

				return DateTime.Compare(firstPerson.BirthDay, secondPerson.BirthDay);
			}
		}

		public sealed class FullNameComparer : IComparer<Person>
		{
			public int Compare(Person x, Person y) => string.CompareOrdinal(x?.FullName, y?.FullName);
		}
	}
}
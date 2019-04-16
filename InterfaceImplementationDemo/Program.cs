using System;
using System.Collections.Generic;

using static System.Console;
using static System.Environment;

using InterfaceImplementationLibrary;

namespace InterfaceImplementationDemo
{
	internal static class Program
	{
		public static void Main()
		{
			// IComparerDemo();
				
			var point = new Point(5);
			foreach (var coordinate in point) WriteLine(coordinate);

			// WriteLine(new Point());
			
			ReadKey(true);
		}

		private static void IComparerDemo()
		{
			var originalListOfPersons = new List<Person>
			{
				Person.Parse(
					"Бродский Иосиф Александрович",
					"1940.05.24",
					"1996.01.28"),
				Person.Parse(
					"Полозкова Вера Николаевна",
					"1986.03.05"),
				Person.Parse(
					"Цветаева Марина Ивановна",
					"1892-10-08",
					"1941-08-31"),
				Person.Parse(
					"Ахматова Анна Андреевна",
					"1889-06-23",
					"1966-03-05"),
				Person.Parse(
					"Маяковский Владимир Владимирович",
					"1893-07-19",
					"1930-04-14"),
				Person.Parse(
					"Есенин Сергей Александрович",
					"1895-10-03",
					"1925-12-28")
			};

			Print(originalListOfPersons, "Перед сортировками:");

			var sortedByAge = new List<Person>(originalListOfPersons);
			sortedByAge.Sort(new Person.AgeComparer());
			Print(sortedByAge, $"{NewLine}Упорядочены по возрасту:");

			var sortedByBirthDay = new List<Person>(originalListOfPersons);
			sortedByBirthDay.Sort(new Person.BirthDayComparer());
			Print(sortedByBirthDay, $"{NewLine}Упорядочены по дате рождения:");

			var sortedByFullName = new List<Person>(originalListOfPersons);
			sortedByFullName.Sort(new Person.FullNameComparer());
			Print(sortedByFullName, $"{NewLine}Упорядочены по имени:");

			var list = originalListOfPersons.Sort(x => x.Age, (x, y) => x - y);
		}

		private static void Print(IEnumerable<Person> persons, string message = null)
		{
			WriteLine(message);

			foreach (var person in persons) WriteLine(person);
		}
	}

	public static class MethodExtensions
	{
		public static IEnumerable<TItem> Sort<TItem, TProperty>(
			this IEnumerable<TItem> collection,
			Func<TItem, TProperty> propertySelector,
			Func<TProperty, TProperty, int> propertyComparer)
		{
			var copy = new List<TItem>(collection);
			for (var i = 0; i < copy.Count; ++i)
				for (var j = i + 1; j < copy.Count; ++j)
					if (propertyComparer(propertySelector(copy[i]), propertySelector(copy[j])) > 0)
					{
						var tmp = copy[i];
						copy[i] = copy[j];
						copy[j] = tmp;
					}

			return copy;
		}
	}
}